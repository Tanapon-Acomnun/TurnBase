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
                Name = "ราชาสไลม์",
                EnemyType = "Boss",
                Sprite = "king.png",

                MaxHP = 180,
                CurrentHP = 180,

                Attack = 18
            };
        }

        // =========================================
        // BOSS 2
        // =========================================
        if (battleNumber == 10)
        {
            return new Enemy
            {
                Name = "แม่ทัพออร์ค",
                EnemyType = "Boss",
                Sprite = "orc.png",

                MaxHP = 250,
                CurrentHP = 250,

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
                Name = "จอมมาร",
                EnemyType = "Boss",
                Sprite = "demonlord.png",

                MaxHP = 400,
                CurrentHP = 400,

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
                Name = "สไลม์",
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
                Name = "ก็อบลิน",
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
                Name = "ฝูงหมาป่า",
                EnemyType = "Wolf",
                Sprite = "wolf01.png",

                MaxHP = 55 + (battleNumber * 10),
                CurrentHP = 55 + (battleNumber * 10),

                Attack = 14 + battleNumber
            };
        }

        // AFTER BOSS 2
        if (battleNumber <= 12)
        {
            return new Enemy
            {
                Name = "อัศวินทมิฬ",
                EnemyType = "DarkKnight",
                Sprite = "darkknight.png",

                MaxHP = 60 + (battleNumber * 12),
                CurrentHP = 60 + (battleNumber * 12),

                Attack = 20 + battleNumber
            };
        }

        // END GAME ENEMY
        return new Enemy
        {
            Name = "โครงกระดูก",
            EnemyType = "Skeleton",
            Sprite = "skeleton.png",

            MaxHP = 60 + (battleNumber * 13),
            CurrentHP = 60 + (battleNumber * 13),

            Attack = 24 + battleNumber
        };
    }
}