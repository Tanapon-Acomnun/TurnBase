using TurnBase.Model;

namespace TurnBase.Services;

public class EnemyFactory : ContentPage
{
    public static Enemy CreateEnemy(int battleNumber)
    {
        // =========================================
        // BOSS 1
        // =========================================
        if (battleNumber == 5)
        {
            return new Enemy
            {
                Name = "Slime King",
                EnemyType = "Boss",
                Sprite = "slimeking.png",

                MaxHP = 180,
                CurrentHP = 180,

                Attack = 22
            };
        }

        // =========================================
        // BOSS 2
        // =========================================
        if (battleNumber == 10)
        {
            return new Enemy
            {
                Name = "Orc Warlord",
                EnemyType = "Boss",
                Sprite = "orcboss.png",

                MaxHP = 320,
                CurrentHP = 320,

                Attack = 35
            };
        }

        // =========================================
        // FINAL BOSS
        // =========================================
        if (battleNumber == 15)
        {
            return new Enemy
            {
                Name = "Demon Lord",
                EnemyType = "Boss",
                Sprite = "demonlord.png",

                MaxHP = 500,
                CurrentHP = 500,

                Attack = 48
            };
        }

        // =========================================
        // NORMAL ENEMIES
        // =========================================

        // SLIME
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

        // GOBLIN
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

        // AFTER BOSS 1
        if (battleNumber <= 7)
        {
            return new Enemy
            {
                Name = "Wolf",
                EnemyType = "Wolf",
                Sprite = "wolf.png",

                MaxHP = 90 + (battleNumber * 10),
                CurrentHP = 90 + (battleNumber * 10),

                Attack = 20 + battleNumber
            };
        }

        // AFTER BOSS 2
        if (battleNumber <= 12)
        {
            return new Enemy
            {
                Name = "Dark Knight",
                EnemyType = "DarkKnight",
                Sprite = "darkknight.png",

                MaxHP = 140 + (battleNumber * 12),
                CurrentHP = 140 + (battleNumber * 12),

                Attack = 28 + battleNumber
            };
        }

        // END GAME ENEMY
        return new Enemy
        {
            Name = "Skeleton",
            EnemyType = "Skeleton",
            Sprite = "skeleton.png",

            MaxHP = 180 + (battleNumber * 15),
            CurrentHP = 180 + (battleNumber * 15),

            Attack = 35 + battleNumber
        };
    }
}