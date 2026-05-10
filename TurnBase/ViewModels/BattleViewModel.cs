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
    }
}