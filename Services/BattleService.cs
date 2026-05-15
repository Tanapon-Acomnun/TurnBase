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
            int damage =
            player.Attack +
            player.AttackModifier +
            _rng.Next(1, 5);

            damage -= enemy.DefenseModifier;

            if (damage < 1)
            {
                damage = 1;
            }

            if (enemy.IsGuarding)
            {
                damage /= 2;
                enemy.IsGuarding = false;
            }

            enemy.CurrentHP -= damage;

            if (enemy.CurrentHP < 0)
                enemy.CurrentHP = 0;

            string message =
                $"{player.Name} attacks {enemy.Name} for {damage} damage!";

            // Lifesteal passive
            if (player.HasLifesteal)
            {
                int healAmount = damage / 3;

                player.CurrentHP += healAmount;

                if (player.CurrentHP > player.MaxHP)
                {
                    player.CurrentHP = player.MaxHP;
                }

                message +=
                    $" {player.Name} heals {healAmount} HP!";
            }

            return message;
        }

        public string EnemyTurn(Character player, Enemy enemy)
        {
            switch (enemy.EnemyType)
            {
                case "Goblin":
                    return GoblinTurn(player, enemy);

                case "Skeleton":
                    return SkeletonTurn(player, enemy);

                case "Boss":
                    return BossTurn(player, enemy);

                case "DarkKnight":
                    return DarkKnightTurn(player, enemy);

                case "Wolf":
                    return WolfTurn(player, enemy);

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

        public string UseSkill(Character player, Enemy enemy, Skill skill)
        {
            if (player.MP < skill.MPCost)
            {
                return "Not enough MP!";
            }

            player.MP -= skill.MPCost;

            switch (skill.SkillType)
            {
                case "Damage":
                    {
                        int damage =
                        player.Attack +
                        skill.Power +
                        player.AttackModifier;

                        damage -= enemy.DefenseModifier;

                        if (damage < 1)
                        {
                            damage = 1;
                        }

                        enemy.CurrentHP -= damage;

                        if (skill.Effect != "None")
                        {
                            enemy.StatusEffects[skill.Effect] =
                                skill.Duration;

                            switch (skill.Effect)
                            {
                                case "AttackDown":
                                    enemy.AttackModifier = enemy.AttackModifier /2;
                                    break;
                            }
                        }

                        if (enemy.CurrentHP < 0)
                        {
                            enemy.CurrentHP = 0;
                        }

                        string damageMessage =
                            $"{player.Name} uses {skill.Name} " +
                            $"for {damage} damage!";

                        if (player.HasLifesteal)
                        {
                            int healAmount = damage / 3;

                            player.CurrentHP += healAmount;

                            if (player.CurrentHP > player.MaxHP)
                            {
                                player.CurrentHP = player.MaxHP;
                            }

                            damageMessage +=
                                $" {player.Name} heals {healAmount} HP!";
                        }

                        return damageMessage;
                    }

                case "Utility":
                    {
                        switch (skill.Effect)
                        {
                            case "RestoreMP":

                                player.MP += skill.Power;

                                if (player.MP > player.MaxMP)
                                {
                                    player.MP = player.MaxMP;
                                }

                                return
                                    $"{player.Name} restores {skill.Power} MP!";
                        }

                        return "Utility skill failed.";
                    }

                case "Buff":
                    {
                        player.StatusEffects[skill.Effect] =
                            skill.Duration;

                        foreach (var extraEffect in skill.ExtraEffects)
                        {
                            player.StatusEffects[extraEffect] =
                                skill.Duration;
                        }

                        switch (skill.Effect)
                        {
                            case "Shielded":
                                player.DefenseModifier = 5;
                                break;

                            case "AttackUp":
                                player.AttackModifier = 5;
                                break;

                            case "CounterStance":
                                player.CounterStanceActive = true;
                                break;
                        }

                        if (skill.SelfDamage > 0)
                        {
                            player.CurrentHP -= skill.SelfDamage;

                            if (player.CurrentHP < 1)
                            {
                                player.CurrentHP = 1;
                            }
                        }

                        string message =
                            $"{player.Name} uses {skill.Name}!";

                        if (skill.SelfDamage > 0)
                        {
                            message +=
                                $" {player.Name} loses {skill.SelfDamage} HP!";
                        }

                        return message;
                    }

                default:
                    return "Skill failed.";
            }
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
            int damage =
            enemy.Attack +
            enemy.AttackModifier +
            _rng.Next(1, 5);

            damage -= player.DefenseModifier;

            if (damage < 1)
            {
                damage = 1;
            }

            string message = "";

            // Counter stance
            if (player.CounterStanceActive)
            {
                int reflectedDamage = damage / 2;

                damage /= 2;

                enemy.CurrentHP -= reflectedDamage;

                if (enemy.CurrentHP < 0)
                {
                    enemy.CurrentHP = 0;
                }

                message +=
                    $"{enemy.Name} takes {reflectedDamage} reflected damage! ";
            }

            // Guard
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

            message +=
                $"{enemy.Name} attacks for {damage} damage!";

            return message;
        }

        private string GoblinTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

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

                return $"{enemy.Name} lands a CRITICAL HIT for {critDamage} damage!";
            }

            if (roll >= 20 && roll < 40)
            {
                if (!player.StatusEffects.ContainsKey("Poison"))
                {
                    player.StatusEffects["Poison"] = 5;
                }

                return $"{enemy.Name} poisons {player.Name}!";
            }

            return BasicEnemyAttack(player, enemy);
        }

        private string SkeletonTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            if (roll < 20)
            {
                enemy.IsGuarding = true;

                return $"{enemy.Name} raises its shield!";
            }

            return BasicEnemyAttack(player, enemy);
        }
        private string BossTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            // =========================================
            // SLIME KING
            // =========================================
            if (enemy.Name == "Slime King")
            {
                // 35% poison slam
                if (roll < 35)
                {
                    player.StatusEffects["Poison"] = 4;

                    return $"{enemy.Name} uses Toxic Slam! {player.Name} is poisoned!";
                }

                return BasicEnemyAttack(player, enemy);
            }

            // =========================================
            // ORC WARLORD
            // =========================================
            if (enemy.Name == "Orc Warlord")
            {
                // Rage attack
                if (roll < 30)
                {
                    int damage = enemy.Attack * 2;

                    player.CurrentHP -= damage;

                    if (player.CurrentHP < 0)
                    {
                        player.CurrentHP = 0;
                    }

                    return $"{enemy.Name} uses Berserker Smash for {damage} damage!";
                }

                return BasicEnemyAttack(player, enemy);
            }

            // =========================================
            // DEMON LORD
            // =========================================
            if (enemy.Name == "Demon Lord")
            {
                // Burn attack
                if (roll < 40)
                {
                    player.StatusEffects["Burn"] = 4;

                    return $"{enemy.Name} casts Hellfire! {player.Name} is burned!";
                }

                // Curse attack
                if (roll >= 40 && roll < 70)
                {
                    player.StatusEffects["Poison"] = 5;

                    return $"{enemy.Name} casts Dark Curse! {player.Name} is poisoned!";
                }

                return BasicEnemyAttack(player, enemy);
            }

            return BasicEnemyAttack(player, enemy);
        }

        private string WolfTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            // Fast bite
            if (roll < 30)
            {
                int damage = enemy.Attack + 10;

                player.CurrentHP -= damage;

                if (player.CurrentHP < 0)
                {
                    player.CurrentHP = 0;
                }

                return $"{enemy.Name} uses Savage Bite for {damage} damage!";
            }

            return BasicEnemyAttack(player, enemy);
        }

        private string DarkKnightTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            // Defense stance
            if (roll < 25)
            {
                enemy.DefenseModifier = 10;

                return $"{enemy.Name} enters Defensive Stance!";
            }

            // Heavy slash
            if (roll >= 25 && roll < 50)
            {
                int damage = enemy.Attack + 15;

                player.CurrentHP -= damage;

                if (player.CurrentHP < 0)
                {
                    player.CurrentHP = 0;
                }

                return $"{enemy.Name} uses Dark Slash for {damage} damage!";
            }

            return BasicEnemyAttack(player, enemy);
        }

        public string ProcessPlayerStatusEffects(Character player)
        {
            player.AttackModifier = 0;
            player.DefenseModifier = 0;

            string log = "";

            List<string> expiredStatuses = new();

            foreach (var status in player.StatusEffects.Keys.ToList())
            {
                switch (status)
                {
                    case "Poison":

                        player.CurrentHP -= 5;

                        log +=
                            $"{player.Name} suffers 5 poison damage!\n";

                        break;

                    case "Burn":

                        player.CurrentHP -= 7;

                        log +=
                            $"{player.Name} is burned for 7 damage!\n";

                        break;

                    case "DefenseUp":

                        player.DefenseModifier = 5;

                        log +=
                            $"{player.Name}'s defense is increased!\n";

                        break;

                    case "AttackUp":

                        player.AttackModifier = 5;

                        log +=
                            $"{player.Name}'s attack is increased!\n";

                        break;

                    case "Regen":

                        player.CurrentHP += 8;

                        if (player.CurrentHP > player.MaxHP)
                        {
                            player.CurrentHP = player.MaxHP;
                        }

                        log +=
                            $"{player.Name} regenerates 8 HP!\n";

                        break;

                    case "CounterStance":

                        player.CounterStanceActive = true;

                        log +=
                            $"{player.Name} prepares to counter attacks!\n";

                        break;
                }

                player.StatusEffects[status]--;

                if (player.StatusEffects[status] <= 0)
                {
                    expiredStatuses.Add(status);
                }
            }

            foreach (var expired in expiredStatuses)
            {
                player.StatusEffects.Remove(expired);

                if (expired == "CounterStance")
                {
                    player.CounterStanceActive = false;
                }

                log +=
                    $"{player.Name} is no longer affected by {expired}.\n";
            }

            if (player.CurrentHP < 0)
            {
                player.CurrentHP = 0;
            }

            return log;
        }

        public string ProcessEnemyStatusEffects(Enemy enemy)
        {
            enemy.AttackModifier = 0;
            enemy.DefenseModifier = 0;

            string log = "";

            List<string> expiredStatuses = new();

            foreach (var status in enemy.StatusEffects.Keys.ToList())
            {
                switch (status)
                {
                    case "Poison":

                        enemy.CurrentHP -= 5;

                        log +=
                            $"{enemy.Name} suffers 5 poison damage!\n";

                        break;

                    case "Burn":

                        enemy.CurrentHP -= 7;

                        log +=
                            $"{enemy.Name} is burned for 7 damage!\n";

                        break;

                    case "AttackDown":

                        enemy.AttackModifier = -5;

                        log +=
                            $"{enemy.Name}'s attack is weakened!\n";

                        break;
                }

                enemy.StatusEffects[status]--;

                if (enemy.StatusEffects[status] <= 0)
                {
                    expiredStatuses.Add(status);
                }
            }

            foreach (var expired in expiredStatuses)
            {
                enemy.StatusEffects.Remove(expired);

                log +=
                    $"{enemy.Name} is no longer affected by {expired}.\n";
            }

            if (enemy.CurrentHP < 0)
            {
                enemy.CurrentHP = 0;
            }

            return log;
        }

        public void RefreshPlayerBuffs(Character player)
        {
            player.AttackModifier = 0;
            player.DefenseModifier = 0;

            foreach (var status in player.StatusEffects.Keys)
            {
                switch (status)
                {
                    case "DefenseUp":
                        player.DefenseModifier = 10;
                        break;

                    case "AttackUp":
                        player.AttackModifier = 10;
                        break;

                    case "CounterStance":
                        player.CounterStanceActive = true;
                        break;
                }
            }
        }

        public void RefreshEnemyBuffs(Enemy enemy)
        {
            enemy.AttackModifier = 0;
            enemy.DefenseModifier = 0;

            foreach (var status in enemy.StatusEffects.Keys)
            {
                switch (status)
                {
                    case "AttackDown":
                        enemy.AttackModifier = -5;
                        break;

                    case "DefenseUp":
                        enemy.DefenseModifier = 10;
                        break;
                }
            }
        }
    }
}