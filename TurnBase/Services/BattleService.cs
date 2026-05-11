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
            enemy.CurrentHP -= damage;

            if (enemy.CurrentHP < 0)
                enemy.CurrentHP = 0;

            return $"{player.Name} attacks {enemy.Name} for {damage} damage!";
        }

        public string EnemyTurn(Character player, Enemy enemy)
        {
            if (!enemy.IsAlive) return "";

            int damage = enemy.Attack;

            if (player.IsGuarding)
            {
                damage /= 2;
                player.IsGuarding = false;
            }
            player.CurrentHP -= damage;

            if (player.CurrentHP < 0)
                player.CurrentHP = 0;

            return $"{enemy.Name} hits back for {damage} damage!";
        }

        public string Guard(Character player)
        {
            player.IsGuarding = true;

            player.MP += 3;

            return $"{player.Name} guards, reducing incoming damage and gaining 3 MP!";
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

            int damage = player.Attack + 10;

            enemy.CurrentHP -= damage;

            if (enemy.CurrentHP < 0)
                enemy.CurrentHP = 0;

            return $"{player.Name} casts Fireball for {damage} damage!";
        }

        private string KnightSkill(Character player, Enemy enemy)
        {
            return $"{player.Name} skill is not implemented yet!";
        }

        private string BerserkerSkill(Character player, Enemy enemy)
        {
            return $"{player.Name} skill is not implemented yet!";
        }
    }
}
