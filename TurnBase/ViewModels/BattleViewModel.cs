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
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private readonly BattleService _battleService = new();

        public Character Player { get; set; }
        public Enemy Enemy { get; set; }

        private string _battleLog;
        public string BattleLog
        {
            get => _battleLog;
            set
            {
                _battleLog = value;
                OnPropertyChanged();
            }
        }

        public ICommand AttackCommand { get; }

        public bool IsBattleOver => !Player.IsAlive || !Enemy.IsAlive;

        public BattleViewModel(Character selectedPlayer)
        {
            Player = selectedPlayer;

            Enemy = new Enemy
            {
                Name = "Slime",
                MaxHP = 50,
                CurrentHP = 50,
                Attack = 8
            };

            AttackCommand = new Command(OnAttack);

            BattleLog = $"A wild {Enemy.Name} appears!";
        }

        private bool _isProcessingTurn;

        private async void OnAttack()
        {
            if (_isProcessingTurn || !Player.IsAlive || !Enemy.IsAlive)
                return;

            _isProcessingTurn = true;

            var log = _battleService.PlayerAttack(Player, Enemy);
            BattleLog = log;

            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(Enemy));

            await Task.Delay(1000);

            if (Enemy.IsAlive)
            {
                log += "\n" + _battleService.EnemyTurn(Player, Enemy);
            }

            if (!Enemy.IsAlive)
                log += "\nEnemy defeated!";
            else if (!Player.IsAlive)
                log += "\nYou were defeated...";

            BattleLog = log;

            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(Enemy));

            _isProcessingTurn = false;
        }
        public async Task ExecutePlayerTurn(string actionType)
        {
            if (_isProcessingTurn || !Player.IsAlive || !Enemy.IsAlive)
                return;

            _isProcessingTurn = true;

            string log = "";

            switch (actionType)
            {
                case "Attack":
                    log = _battleService.PlayerAttack(Player, Enemy);
                    break;

                case "Skill":
                    log = _battleService.UseSkill(Player, Enemy);
                    break;

                case "Guard":
                    log = _battleService.Guard(Player);
                    break;
            }

            BattleLog = log;

            if (log.Contains("Not enough MP") ||
            log.Contains("has no skill available") ||
            log.Contains("not implemented yet"))
            {
                _isProcessingTurn = false;
                return;
            }

            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(Enemy));

            // Delay before enemy turn
            await Task.Delay(1000);

            if (Enemy.IsAlive)
            {
                log += "\n" + _battleService.EnemyTurn(Player, Enemy);
            }

            if (!Enemy.IsAlive)
            {
                BattleLog = log + "\nEnemy defeated!";
            }
            else if (!Player.IsAlive)
            {
                BattleLog = log + "\nYou were defeated...";
            }
            else
            {
                BattleLog = log;
            }

            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(Enemy));

            _isProcessingTurn = false;
        }
        public bool IsPlayerTurnReady => !_isProcessingTurn && Player.IsAlive && Enemy.IsAlive;
    }
}