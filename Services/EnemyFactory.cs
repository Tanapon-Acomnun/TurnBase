using TurnBase.Model;

namespace TurnBase.Services;

public class EnemyFactory : ContentPage
{
    public static Enemy CreateEnemy(int battleNumber)
    {
        if (battleNumber <= 2)
        {
            return new Enemy
            {
                Name = "Slime",
                Sprite = "slime2.png",
                MaxHP = 50 + (battleNumber * 5),
                CurrentHP = 50 + (battleNumber * 5),
                Attack = 8 + battleNumber
            };
        }

        if (battleNumber <= 4)
        {
            return new Enemy
            {
                Name = "Goblin",
                Sprite = "goblin.png",
                MaxHP = 80 + (battleNumber * 8),
                CurrentHP = 80 + (battleNumber * 8),
                Attack = 12 + battleNumber
            };
        }

        return new Enemy
        {
            Name = "Skeleton",
            Sprite = "skeleton.png",
            MaxHP = 120 + (battleNumber * 10),
            CurrentHP = 120 + (battleNumber * 10),
            Attack = 18 + battleNumber
        };

    }

}
