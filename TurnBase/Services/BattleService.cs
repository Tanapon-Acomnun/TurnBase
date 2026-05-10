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

            int damage = enemy.Attack + _rng.Next(1, 4);
            player.CurrentHP -= damage;

            if (player.CurrentHP < 0)
                player.CurrentHP = 0;

            return $"{enemy.Name} hits back for {damage} damage!";
        }
    }
}
