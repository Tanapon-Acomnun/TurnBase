using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TurnBase.Model;
using TurnBase.Services;

public class BattleViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    void OnPropertyChanged([CallerMemberName] string name = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly BattleService _battleService = new();

    public Character Player { get; set; }
    public Enemy Enemy { get; set; }

    private string _battleLog;
    public string BattleLog
    {
        get => _battleLog;
        set { _battleLog = value; OnPropertyChanged(); }
    }

    public ICommand AttackCommand { get; }

    public BattleViewModel()
    {
        Player = new Character
        {
            Name = "Hero",
            MaxHP = 100,
            CurrentHP = 100,
            Attack = 10
        };

        Enemy = new Enemy
        {
            Name = "Slime",
            MaxHP = 50,
            CurrentHP = 50,
            Attack = 8
        };

        AttackCommand = new Command(OnAttack);
        BattleLog = "A wild Slime appears!";
    }

    private void OnAttack()
    {
        if (!Player.IsAlive || !Enemy.IsAlive)
            return;

        var log = _battleService.PlayerAttack(Player, Enemy);

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
    }
}