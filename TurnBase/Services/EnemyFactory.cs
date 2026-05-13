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
                EnemyType = "Slime",
                Sprite = "slime2.png",
                MaxHP = 35 + (battleNumber * 5),
                CurrentHP = 35 + (battleNumber * 5),
                Attack = 8 + battleNumber
            };
        }

        if (battleNumber <= 4)
        {
            return new Enemy
            {
                Name = "Goblin",
                EnemyType = "Goblin",
                Sprite = "goblin.png",
                MaxHP = 50 + (battleNumber * 8),
                CurrentHP = 50 + (battleNumber * 8),
                Attack = 12 + battleNumber
            };
        }

        return new Enemy
        {
            Name = "Skeleton",
            EnemyType = "Skeleton",
            Sprite = "skeleton.png",
            MaxHP = 80 + (battleNumber * 10),
            CurrentHP = 80 + (battleNumber * 10),
            Attack = 18 + battleNumber
        };

    }

}
