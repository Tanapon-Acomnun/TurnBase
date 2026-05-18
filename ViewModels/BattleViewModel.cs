using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.ViewModels
{
    public class BattleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void NotifyBattleChanged()
        {
            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(Enemy));
            OnPropertyChanged(nameof(EnemyHPBarWidth));
            OnPropertyChanged(nameof(PlayerHPBarWidth));
            OnPropertyChanged(nameof(PlayerMPBarWidth));
            OnPropertyChanged(nameof(PlayerStatusText));
            OnPropertyChanged(nameof(EnemyStatusText));
            OnPropertyChanged(nameof(BattleLog));
        }

        private readonly BattleService _battleService = new();
        private bool _isProcessingTurn;
        private bool _itemUsedThisTurn;

        private Character _player;
        public Character Player
        {
            get => _player;
            set { _player = value; OnPropertyChanged(); }
        }

        private Enemy _enemy;
        public Enemy Enemy
        {
            get => _enemy;
            set { _enemy = value; OnPropertyChanged(); }
        }

        private string _battleLog;
        public string BattleLog
        {
            get => _battleLog;
            set { _battleLog = value; OnPropertyChanged(); }
        }

        // ── HP/MP Bar widths ──────────────────────────
        private const double BarMaxWidth = 240;

        public double EnemyHPBarWidth =>
            Enemy.MaxHP <= 0 ? 0
            : Math.Max(0, (double)Enemy.CurrentHP / Enemy.MaxHP * BarMaxWidth);

        public double PlayerHPBarWidth =>
            Player.MaxHP <= 0 ? 0
            : Math.Max(0, (double)Player.CurrentHP / Player.MaxHP * BarMaxWidth);

        public double PlayerMPBarWidth =>
            Player.MaxMP <= 0 ? 0
            : Math.Max(0, (double)Player.MP / Player.MaxMP * BarMaxWidth);

        // ─────────────────────────────────────────────

        public ICommand AttackCommand { get; }

        public bool IsBattleOver =>
            !Player.IsAlive || !Enemy.IsAlive;

        public bool IsPlayerTurnReady =>
            !_isProcessingTurn &&
            Player.IsAlive &&
            Enemy.IsAlive;

        public string PlayerStatusText =>
            string.Join(", ",
                Player.StatusEffects
                .Select(s => $"{s.Key}({s.Value})"));

        public string EnemyStatusText =>
            string.Join(", ",
                Enemy.StatusEffects
                .Select(s => $"{s.Key}({s.Value})"));

        public BattleViewModel(Character selectedPlayer)
        {
            Player = selectedPlayer;

            // Reset status effects between battles
            Player.StatusEffects.Clear();

            Player.AttackModifier = 0;
            Player.DefenseModifier = 0;

            Player.IsShielded = false;
            Player.CounterStanceActive = false;
            Player.IsGuarding = false;

            Enemy =
                EnemyFactory.CreateEnemy(Player.BattleNumber);

            AttackCommand =
                new Command(OnAttack);

            BattleLog =
                $"A wild {Enemy.Name} appears!";
        }

        // ============================================
        // HELPER METHODS
        // ============================================

        private void AppendBattleLog(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (string.IsNullOrWhiteSpace(BattleLog))
            {
                BattleLog = text;
            }
            else
            {
                BattleLog += "\n" + text;
            }
        }

        private void StartPlayerTurn()
        {
            string statusLog =
                _battleService.ProcessPlayerStatusEffects(Player);

            _battleService.RefreshPlayerBuffs(Player);

            AppendBattleLog(statusLog);

            NotifyBattleChanged();
        }

        private void StartEnemyTurn()
        {
            string statusLog =
                _battleService.ProcessEnemyStatusEffects(Enemy);

            _battleService.RefreshEnemyBuffs(Enemy);

            AppendBattleLog(statusLog);

            NotifyBattleChanged();
        }

        private bool CheckBattleEnd()
        {
            if (!Enemy.IsAlive)
            {
                AppendBattleLog("Enemy defeated!");
                NotifyBattleChanged();
                return true;
            }

            if (!Player.IsAlive)
            {
                AppendBattleLog("You were defeated...");
                NotifyBattleChanged();
                return true;
            }

            return false;
        }

        private async Task ExecuteEnemyPhase()
        {
            if (!Enemy.IsAlive)
                return;

            await Task.Delay(700);

            StartEnemyTurn();

            if (!Enemy.IsAlive)
                return;

            await Task.Delay(500);

            string enemyAction =
                _battleService.EnemyTurn(Player, Enemy);

            AppendBattleLog(enemyAction);

            NotifyBattleChanged();

            _itemUsedThisTurn = false;
        }

        // ============================================
        // BASIC ATTACK
        // ============================================

        private async void OnAttack()
        {
            await ExecutePlayerTurn("Attack");
        }

        // ============================================
        // PLAYER TURN
        // ============================================

        public async Task ExecutePlayerTurn(string actionType)
        {
            if (_isProcessingTurn || !Player.IsAlive || !Enemy.IsAlive)
                return;

            _isProcessingTurn = true;

            BattleLog = "";

            // ----------------------------
            // START PLAYER TURN
            // ----------------------------

            StartPlayerTurn();

            if (CheckBattleEnd())
            {
                _isProcessingTurn = false;
                return;
            }

            // ----------------------------
            // PLAYER ACTION
            // ----------------------------

            string actionLog = "";

            if (actionType.StartsWith("Item:"))
            {
                if (_itemUsedThisTurn)
                {
                    AppendBattleLog(
                        "You already used an item this turn!");

                    NotifyBattleChanged();

                    _isProcessingTurn = false;
                    return;
                }

                _itemUsedThisTurn = true;
            }

            switch (actionType)
            {
                case "Attack":
                    actionLog =
                        _battleService.PlayerAttack(Player, Enemy);
                    break;

                case "Guard":
                    actionLog =
                        _battleService.Guard(Player);
                    break;

                case "Item:Potion":
                    actionLog =
                        _battleService.UsePotion(Player);
                    break;

                case "Item:Ether":
                    actionLog =
                        _battleService.UseEther(Player);
                    break;
            }

            AppendBattleLog(actionLog);

            NotifyBattleChanged();

            // Items do not consume turn
            if (actionType.StartsWith("Item:"))
            {
                _isProcessingTurn = false;
                return;
            }

            // Invalid action
            if (actionLog.Contains("No ") ||
                actionLog.Contains("already full") ||
                actionLog.Contains("Not enough MP") ||
                actionLog.Contains("failed"))
            {
                _isProcessingTurn = false;
                return;
            }

            // ----------------------------
            // CHECK AFTER PLAYER ACTION
            // ----------------------------

            if (CheckBattleEnd())
            {
                _isProcessingTurn = false;
                return;
            }

            // ----------------------------
            // ENEMY TURN
            // ----------------------------

            await ExecuteEnemyPhase();

            // ----------------------------
            // FINAL CHECK
            // ----------------------------

            CheckBattleEnd();

            NotifyBattleChanged();

            _isProcessingTurn = false;
        }

        // ============================================
        // SKILL TURN
        // ============================================

        public async Task ExecuteSkillTurn(Skill skill)
        {
            if (_isProcessingTurn || !Player.IsAlive || !Enemy.IsAlive)
                return;

            _isProcessingTurn = true;

            BattleLog = "";

            if (Player.MP < skill.MPCost)
{
    AppendBattleLog("Not enough MP!");

    NotifyBattleChanged();

    _isProcessingTurn = false;
    return;
}

            // ----------------------------
            // START PLAYER TURN
            // ----------------------------

            StartPlayerTurn();

            if (CheckBattleEnd())
            {
                _isProcessingTurn = false;
                return;
            }

            // ----------------------------
            // PLAYER SKILL
            // ----------------------------

            string actionLog = _battleService.UseSkill(Player, Enemy, skill);

            _battleService.RefreshPlayerBuffs(Player);
            _battleService.RefreshEnemyBuffs(Enemy);

            AppendBattleLog(actionLog);

            NotifyBattleChanged();

            // Invalid skill
            if (actionLog.Contains("failed"))
            {
                _isProcessingTurn = false;
                return;
            }

            // ----------------------------
            // CHECK AFTER PLAYER ACTION
            // ----------------------------

            if (CheckBattleEnd())
            {
                _isProcessingTurn = false;
                return;
            }

            // ----------------------------
            // ENEMY TURN
            // ----------------------------

            await ExecuteEnemyPhase();

            // ----------------------------
            // FINAL CHECK
            // ----------------------------

            CheckBattleEnd();

            NotifyBattleChanged();

            _isProcessingTurn = false;
        }
    }
}