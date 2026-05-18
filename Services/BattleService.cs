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
                $"{player.ID} โจมตี {enemy.Name} สร้างความเสียหาย {damage} หน่วย !";

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
                    $" {player.ID} ฟื้นฟู HP {healAmount} หน่วย!";
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

            return $"{player.ID} ใช้ป้องกัน";
        }

        public string UseSkill(Character player, Enemy enemy, Skill skill)
        {
            if (player.MP < skill.MPCost)
            {
                return "MP ไม่เพียงพอ!";
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

                        if (enemy.IsGuarding)
                        {
                            damage /= 2;
                            enemy.IsGuarding = false;
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
                            $"{player.ID} ใช้ {skill.Name} " +
                            $"สร้างความเสียหาย {damage} หน่วย!";

                        if (player.HasLifesteal)
                        {
                            int healAmount = damage / 3;

                            player.CurrentHP += healAmount;

                            if (player.CurrentHP > player.MaxHP)
                            {
                                player.CurrentHP = player.MaxHP;
                            }

                            damageMessage +=
                                $" {player.ID} ฟื้นฟู HP {healAmount} หน่วย!";
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
                                    $"{player.ID} ฟื้นฟู MP {skill.Power} หน่วย!";
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
                                player.IsShielded = true;
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
                            $"{player.ID} ใช้ {skill.Name}!";

                        if (skill.SelfDamage > 0)
                        {
                            message +=
                                $" {player.ID} สูญเสีย HP {skill.SelfDamage} หน่วย!";
                        }

                        return message;
                    }

                default:
                    return "ใช้สกิลไม่สำเร็จ";
            }
        }

        public string UsePotion(Character player)
        {
            if (player.Potions <= 0)
            {
                return "ไม่มีโพชั่นเหลือแล้ว!";
            }

            if (player.CurrentHP >= player.MaxHP)
            {
                return "HP เต็มอยู่แล้ว!";
            }

            player.Potions--;

            int healAmount = 35;

            player.CurrentHP += healAmount;

            if (player.CurrentHP > player.MaxHP)
            {
                player.CurrentHP = player.MaxHP;
            }

            return $"{player.ID}  ใช้โพชั่นและฟื้นฟู HP {healAmount} หน่วย!";
        }

        public string UseEther(Character player)
        {
            if (player.Ethers <= 0)
            {
                return "ไม่มีอีเธอร์เหลือแล้ว!";
            }

            if (player.MP >= player.MaxMP)
            {
                return "MP เต็มอยู่แล้ว!";
            }

            player.Ethers--;

            int manaAmount = 15;

            player.MP += manaAmount;

            if (player.MP > player.MaxMP)
            {
                player.MP = player.MaxMP;
            }

            return $"{player.ID} ใช้อีเธอร์และฟื้นฟู MP {manaAmount} MP!";
        }

        private string BasicEnemyAttack(Character player, Enemy enemy)
        {
            int damage =
            enemy.Attack +
            enemy.AttackModifier +
            _rng.Next(1, 5);      

            if (damage < 1)
            {
                damage = 1;
            }

            string message = "";

            // Shielded
            if (player.IsShielded)
            {
                damage /= 2;
            }

            // Arcane Shield
            if (player.ArcaneShieldActive)
            {
                damage = (int)(damage * 0.7);
            }

            damage -= player.DefenseModifier;

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
                    $"{enemy.Name} ได้รับความเสียหายสะท้อนกลับ {reflectedDamage} หน่วย ! ";
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
                $"{enemy.Name}  โจมตีสร้างความเสียหาย {damage} หน่วย !";

            return message;
        }

        private string DealEnemyDamage(
            Character player,
            Enemy enemy,
            int damage,
            string attackMessage)
        {
            damage -= player.DefenseModifier;

            if (damage < 1)
            {
                damage = 1;
            }

            string message = "";

            // Shielded
            if (player.IsShielded)
            {
                damage /= 2;
            }

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
                    $"{enemy.Name} ได้รับความเสียหายสะท้อนกลับ {reflectedDamage} หน่วย! ";
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
                $"{attackMessage} โจมตีสร้างความเสียหาย {damage} หน่วย !";

            return message;
        }

        private string GoblinTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            if (roll <= 20)
            {
                int damage =
                   (enemy.Attack * 2) +
                   enemy.AttackModifier +
                   _rng.Next(1, 5);

                return DealEnemyDamage(
                    player,
                    enemy,
                    damage,
                    $"{enemy.Name} โจมตีคริติคอล !");
            }

            if (roll > 20 && roll <= 40)
            {
                if (!player.StatusEffects.ContainsKey("Poison"))
                {
                    player.StatusEffects["Poison"] = 5;
                }

                return $"{enemy.Name} ทำให้ {player.ID} ติดพิษ !";
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

                return $"{enemy.Name} ยกโล่ตั้งรับ !";
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
                // 25% poison slam
                if (roll < 25)
                {
                    player.StatusEffects["Poison"] = 4;

                    return $"{enemy.Name} ใช้พิษ {player.ID} ติดพิษ !";
                }

                return BasicEnemyAttack(player, enemy);
            }

            // =========================================
            // ORC WARLORD
            // =========================================
            if (enemy.Name == "Orc Warlord")
            {
                // Rage attack
                if (roll < 20)
                {
                    int damage =
                        (enemy.Attack * 2) +
                        enemy.AttackModifier +
                        _rng.Next(1, 5);

                    return DealEnemyDamage(
                        player,
                        enemy,
                        damage,
                        $"{enemy.Name} ใช้เบอร์เซอร์เกอร์ สแมช");
                }

                return BasicEnemyAttack(player, enemy);
            }

            // =========================================
            // DEMON LORD
            // =========================================
            if (enemy.Name == "Demon Lord")
            {
                // Burn attack
                if (roll < 20)
                {
                    player.StatusEffects["Burn"] = 4;

                    return $"{enemy.Name} ใช้ไฟนรก! {player.ID} โดนเผาไหม้!";
                }

                // Curse attack
                if (roll >= 20 && roll < 30)
                {
                    player.StatusEffects["Poison"] = 5;

                    return $"{enemy.Name} ใช้คำสาปแห่งความมืด! {player.ID} โดนคำสาบ!";
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
                int damage =
                    enemy.Attack +
                    enemy.AttackModifier +
                    10 +
                    _rng.Next(1, 5);

                return DealEnemyDamage(
                    player,
                    enemy,
                    damage,
                    $"{enemy.Name} ใช้คมเขี้ยวอำมหิต");
            }

            return BasicEnemyAttack(player, enemy);
        }

        private string DarkKnightTurn(Character player, Enemy enemy)
        {
            Random rng = new();

            int roll = rng.Next(100);

            // Defense stance
            if (roll < 20)
            {
                enemy.StatusEffects["DefenseUp"] = 2;

                return $"{enemy.Name} เข้าสู่ท่าตั้งรับ!";
            }

            // Heavy slash
            if (roll >= 20 && roll < 30)
            {
                int damage =
                    enemy.Attack +
                    enemy.AttackModifier +
                    15 +
                    _rng.Next(1, 5);

                return DealEnemyDamage(
                    player,
                    enemy,
                    damage,
                    $"{enemy.Name} ใช้ดาร์กสแลช!");
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
                            $"{player.ID} ได้รับความเสียหายพิษ 5 หน่วย\n";

                        break;

                    case "Burn":

                        player.CurrentHP -= 7;

                        log +=
                            $"{player.ID} ถูกเผาไหม้ได้รับความเสียหาย 7หน่วย\n";

                        break;

                    case "DefenseUp":

                        player.DefenseModifier = 5;
                        player.ArcaneShieldActive = true;

                        log +=
                            $"พลังป้องกันของ{player.ID}เพิ่มขึ้น\n";

                        break;

                    case "AttackUp":

                        player.AttackModifier = 5;

                        log +=
                            $"พลังโจมตีของ{player.ID}เพิ่มขึ้น\n";

                        break;

                    case "Regen":

                        player.CurrentHP += 8;

                        if (player.CurrentHP > player.MaxHP)
                        {
                            player.CurrentHP = player.MaxHP;
                        }

                        log +=
                            $"{player.ID} ฟื้นฟู HP 8 หน่วย!\n";

                        break;

                    case "CounterStance":

                        player.CounterStanceActive = true;

                        log +=
                            $"{player.ID} เตรียมสวนกลับการโจมตี!\n";

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
                    $"{player.ID} ไม่ได้รับผลของ {expired} อีกต่อไป.\n";
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
                            $"{enemy.Name} ได้รับความเสียหายพิษ 5 หน่วย \n";

                        break;

                    case "Burn":

                        int burnDamage = (int)(enemy.MaxHP * 0.05);

                        if (burnDamage < 1)
                        {
                            burnDamage = 1;
                        }

                        enemy.CurrentHP -= burnDamage;

                        log +=
                            $"{enemy.Name} ถูกเผาไหม้ได้รับความเสียหาย {burnDamage} !\n";

                        break;

                    case "AttackDown":

                        enemy.AttackModifier = -(enemy.Attack / 2);

                        log +=
                            $"พลังโจมตีของ {enemy.Name} ลดลง\n";

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
                    $"{enemy.Name} ไม่ได้รับผลของ {expired} อีกต่อไป.\n";
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

            player.ArcaneShieldActive = false;
            player.IsShielded = false;
            player.CounterStanceActive = false;

            foreach (var status in player.StatusEffects.Keys)
            {
                switch (status)
                {
                    case "DefenseUp":
                        player.DefenseModifier = 5;
                        player.ArcaneShieldActive = true;
                        break;

                    case "AttackUp":
                        player.AttackModifier = 10;
                        break;

                    case "Shielded":
                        player.IsShielded = true;
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
                        enemy.AttackModifier =- (enemy.Attack / 2);
                        break;

                    case "DefenseUp":
                        enemy.DefenseModifier = 10;
                        break;
                }
            }
        }
    }
}