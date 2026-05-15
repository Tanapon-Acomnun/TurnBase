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
        }

        private readonly BattleService _battleService = new();
        private bool _isProcessingTurn;

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
        public bool IsBattleOver => !Player.IsAlive || !Enemy.IsAlive;
        public bool IsPlayerTurnReady => !_isProcessingTurn && Player.IsAlive && Enemy.IsAlive;

        public string PlayerStatusText =>
            string.Join(", ", Player.StatusEffects.Select(s => $"{s.Key}({s.Value})"));

        public string EnemyStatusText =>
            string.Join(", ", Enemy.StatusEffects.Select(s => $"{s.Key}({s.Value})"));

        public BattleViewModel(Character selectedPlayer)
        {
            Player = selectedPlayer;
            Enemy = EnemyFactory.CreateEnemy(Player.BattleNumber);
            AttackCommand = new Command(OnAttack);
            BattleLog = $"A wild {Enemy.Name} appears!";
        }

        private async void OnAttack()
        {
            if (_isProcessingTurn || !Player.IsAlive || !Enemy.IsAlive)
                return;

            _isProcessingTurn = true;

            var log = _battleService.PlayerAttack(Player, Enemy);
            BattleLog = log;
            NotifyBattleChanged();

            await Task.Delay(1000);

            if (Enemy.IsAlive)
                log += "\n" + _battleService.EnemyTurn(Player, Enemy);

            if (!Enemy.IsAlive)
                log += "\nEnemy defeated!";
            else if (!Player.IsAlive)
                log += "\nYou were defeated...";

            BattleLog = log;
            NotifyBattleChanged();

            _isProcessingTurn = false;
        }

        public async Task ExecutePlayerTurn(string actionType)
        {
            if (_isProcessingTurn || !Player.IsAlive || !Enemy.IsAlive)
                return;

            _isProcessingTurn = true;

            string statusLog = _battleService.ProcessPlayerStatusEffects(Player);
            string log = "";

            switch (actionType)
            {
                case "Attack":
                    log = _battleService.PlayerAttack(Player, Enemy); break;
                case "Guard":
                    log = _battleService.Guard(Player); break;
                case "Item:Potion":
                    log = _battleService.UsePotion(Player); break;
                case "Item:Ether":
                    log = _battleService.UseEther(Player); break;
            }

            BattleLog = statusLog + log;

            if (log.Contains("No ") || log.Contains("already full") ||
                log.Contains("Not enough MP") || log.Contains("not implemented"))
            {
                NotifyBattleChanged();
                _isProcessingTurn = false;
                return;
            }

            NotifyBattleChanged();

            await Task.Delay(1000);

            if (Enemy.IsAlive)
            {
                string enemyStatusLog = _battleService.ProcessEnemyStatusEffects(Enemy);
                if (!string.IsNullOrWhiteSpace(enemyStatusLog))
                    log += "\n" + enemyStatusLog;

                _battleService.RefreshPlayerBuffs(Player);

                if (Enemy.IsAlive)
                {
                    _battleService.RefreshEnemyBuffs(Enemy);
                    log += "\n" + _battleService.EnemyTurn(Player, Enemy);
                }
            }

            BattleLog = !Enemy.IsAlive ? log + "\nEnemy defeated!"
                      : !Player.IsAlive ? log + "\nYou were defeated..."
                      : log;

            NotifyBattleChanged();
            _isProcessingTurn = false;
        }

        public async Task ExecuteSkillTurn(Skill skill)
        {
            if (_isProcessingTurn || !Player.IsAlive || !Enemy.IsAlive)
                return;

            _isProcessingTurn = true;

            string statusLog = _battleService.ProcessPlayerStatusEffects(Player);
            string log = _battleService.UseSkill(Player, Enemy, skill);

            BattleLog = statusLog + log;
            NotifyBattleChanged();

            await Task.Delay(1000);

            if (Enemy.IsAlive)
            {
                string enemyStatusLog = _battleService.ProcessEnemyStatusEffects(Enemy);
                if (!string.IsNullOrWhiteSpace(enemyStatusLog))
                    log += "\n" + enemyStatusLog;

                _battleService.RefreshPlayerBuffs(Player);

                if (Enemy.IsAlive)
                {
                    _battleService.RefreshEnemyBuffs(Enemy);
                    log += "\n" + _battleService.EnemyTurn(Player, Enemy);
                }
            }

            BattleLog = statusLog + log;
            NotifyBattleChanged();

            _isProcessingTurn = false;
        }
    }
}