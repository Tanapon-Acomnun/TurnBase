using System;
using System.Collections.Generic;
using System.Text;
using TurnBase.Model;

namespace TurnBase.Services
{
    public class BattleService
    {
        private Random _rng = new();

        public string PlayerAttack(Character player, Enemy enemy)
        {
            int damage = player.Attack + _rng.Next(1, 5);
            if (enemy.IsGuarding)
            {
                damage /= 2;
                enemy.IsGuarding = false;
            }
            enemy.CurrentHP -= damage;

            if (enemy.CurrentHP < 0)
                enemy.CurrentHP = 0;

            return $"{player.Name} attacks {enemy.Name} for {damage} damage!";
        }

        public string EnemyTurn(Character player, Enemy enemy)
        {
            switch (enemy.EnemyType)
            {
                case "Goblin":
                    return GoblinTurn(player, enemy);

                case "Skeleton":
                    return SkeletonTurn(player, enemy);

                default:
                    return BasicEnemyAttack(player, enemy);
            }
        }

        public string Guard(Character player)
        {
            player.IsGuarding = true;

            player.MP += 3;

            return $"{player.Name} uses GUARD";
        }

        public string UseSkill(Character player, Enemy enemy)
        {
            switch (player.Name)
            {
                case "Wizard":
                    return WizardSkill(player, enemy);

                case "Knight":
                    return KnightSkill(player, enemy);

                case "Berserker":
                    return BerserkerSkill(player, enemy);

                default:
                    return $"{player.Name} has no skill available!";
            }
        }
        private string WizardSkill(Character player, Enemy enemy)
        {
            const int manaCost = 5;

            if (player.MP < manaCost)
            {
                return "Not enough MP!";
            }

            player.MP -= manaCost;

            if (player.MP < 0)
            {
                player.MP = 0;
            }

            int damage = player.Attack + 10;
            if (enemy.IsGuarding)
            {
                damage /= 2;
                enemy.IsGuarding = false;
            }

            enemy.CurrentHP -= damage;

            if (enemy.CurrentHP < 0)
                enemy.CurrentHP = 0;

            if (!enemy.StatusEffects.Contains("Burn"))
            {
                enemy.StatusEffects.Add("Burn");
            }

            return $"{player.Name} casts Fireball for {damage} damage!";
        }

        private string KnightSkill(Character player, Enemy enemy)
        {
            const int manaCost = 4;

            if (player.MP < manaCost)
            {
                return "Not enough MP!";
            }

            player.MP -= manaCost;

            int damage = player.Attack + 6;

            enemy.CurrentHP -= damage;

            if (enemy.CurrentHP < 0)
                enemy.CurrentHP = 0;

            enemy.IsGuarding = true;

            return $"{player.Name} uses Shield Bash for {damage} damage and stuns the enemy!";
        }

        private string BerserkerSkill(Character player, Enemy enemy)
        {
            const int manaCost = 3;

            if (player.MP < manaCost)
            {
                return "Not enough MP!";
            }

            player.MP -= manaCost;

            int selfDamage = 5;

            player.CurrentHP -= selfDamage;

            if (player.CurrentHP < 1)
                player.CurrentHP = 1;

            int damage = player.Attack + 15;

            enemy.CurrentHP -= damage;

            if (enemy.CurrentHP < 0)
                enemy.CurrentHP = 0;

            return $"{player.Name} uses Rage Slash! Deals {damage} damage but loses {selfDamage} HP!";
        }
        public string UsePotion(Character player)
        {
            if (player.Potions <= 0)
            {
                return "No Potions left!";
            }

            if (player.CurrentHP >= player.MaxHP)
            {
                return "HP is already full!";
            }

            player.Potions--;

            int healAmount = 20;

            player.CurrentHP += healAmount;

            if (player.CurrentHP > player.MaxHP)
            {
                player.CurrentHP = player.MaxHP;
            }

            return $"{player.Name} used a Potion and restored {healAmount} HP!";
        }

        public string UseEther(Character player)
        {
            if (player.Ethers <= 0)
            {
                return "No Ethers left!";
            }

            if (player.MP >= player.MaxMP)
            {
                return "MP is already full!";
            }

            player.Ethers--;

            int manaAmount = 10;

            player.MP += manaAmount;

            if (player.MP > player.MaxMP)
            {
                player.MP = player.MaxMP;
            }

            return $"{player.Name} used an Ether and restored {manaAmount} MP!";
        }

        private string BasicEnemyAttack(Character player, Enemy enemy)
        {
            int damage = enemy.Attack;

            if (player.IsGuarding)
            {
                damage /= 2;
                player.IsGuarding = false;
            }

            player.CurrentHP -= damage;

            if (player.CurrentHP < 0)
            {
                player.CurrentHP = 0;
            }

            return $"{enemy.Name} attacks for {damage} damage!";
        }

        private string GoblinTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            // 20% critical hit
            if (roll < 20)
            {
                int critDamage = enemy.Attack * 2;

                if (player.IsGuarding)
                {
                    critDamage /= 2;
                    player.IsGuarding = false;
                }

                player.CurrentHP -= critDamage;

                if (player.CurrentHP < 0)
                {
                    player.CurrentHP = 0;
                }

            if (roll >= 20 && roll < 40)
                {
                    if (!player.StatusEffects.Contains("Poison"))
                    {
                        player.StatusEffects.Add("Poison");
                    }

                    return $"{enemy.Name} poisons {player.Name}!";
                }

                return $"{enemy.Name} lands a CRITICAL HIT for {critDamage} damage!";
            }

            return BasicEnemyAttack(player, enemy);
        }

        private string SkeletonTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            // 30% chance to guard
            if (roll < 30)
            {
                enemy.IsGuarding = true;

                return $"{enemy.Name} raises its shield!";
            }

            return BasicEnemyAttack(player, enemy);
        }
        public string ProcessPlayerStatusEffects(Character player)
        {
            string log = "";

            // Poison
            if (player.StatusEffects.Contains("Poison"))
            {
                int poisonDamage = 5;

                player.CurrentHP -= poisonDamage;

                if (player.CurrentHP < 0)
                {
                    player.CurrentHP = 0;
                }

                log += $"{player.Name} suffers {poisonDamage} poison damage!\n";
            }

            // Burn
            if (player.StatusEffects.Contains("Burn"))
            {
                int burnDamage = 7;

                player.CurrentHP -= burnDamage;

                if (player.CurrentHP < 0)
                {
                    player.CurrentHP = 0;
                }

                log += $"{player.Name} is burned for {burnDamage} damage!\n";
            }

            return log;
        }

        public string ProcessEnemyStatusEffects(Enemy enemy)
        {
            string log = "";

            // Poison
            if (enemy.StatusEffects.Contains("Poison"))
            {
                int poisonDamage = 5;

                enemy.CurrentHP -= poisonDamage;

                if (enemy.CurrentHP < 0)
                {
                    enemy.CurrentHP = 0;
                }

                log += $"{enemy.Name} suffers {poisonDamage} poison damage!\n";
            }

            // Burn
            if (enemy.StatusEffects.Contains("Burn"))
            {
                int burnDamage = 7;

                enemy.CurrentHP -= burnDamage;

                if (enemy.CurrentHP < 0)
                {
                    enemy.CurrentHP = 0;
                }

                log += $"{enemy.Name} is burned for {burnDamage} damage!\n";
            }

            return log;
        }
    }

}
