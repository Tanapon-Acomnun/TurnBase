using System.Collections.Generic;

namespace TurnBase.Model;

public static class SkillDatabase
{
    // ฟังก์ชันนี้จะเป็นคนจ่ายข้อมูลสกิลให้กับทุกระบบในเกม!
    public static Skill GetSkill(string skillName)
    {
        switch (skillName)
        {
            // === สกิล Wizard ===
            case "Fireball":
                return new Skill { Name = "Fireball", SkillType = "Damage", MPCost = 5, Power = 10, Effect = "Burn", Duration = 3, Description = "Deals fire damage and inflicts Burn." };
            case "Arcane Shield":
                return new Skill { Name = "Arcane Shield", SkillType = "Buff", MPCost = 4, Power = 0, Duration = 3, Effect = "DefenseUp", Description = "Raises defense for 3 turns." };
            case "Ice Bolt":
                return new Skill { Name = "Ice Bolt", SkillType = "Damage", MPCost = 6, Power = 14, Effect = "AttackDown", Duration = 2, Description = "A chilling magical attack.", UnlockCost = 5 };
            case "Mana Surge":
                return new Skill { Name = "Mana Surge", SkillType = "Utility", MPCost = 0, Power = 15, Effect = "RestoreMP", Duration = 0, Description = "Restore magical energy.", UnlockCost = 5 };

            // === สกิล Knight ===
            case "Shield Bash":
                return new Skill { Name = "Shield Bash", SkillType = "Damage", MPCost = 4, Power = 10, Effect = "AttackDown", Duration = 3, Description = "A controlling shield strike." };
            case "Fortify":
                return new Skill { Name = "Fortify", SkillType = "Buff", ExtraEffects = new List<string> { "Regen" }, MPCost = 5, Power = 0, Effect = "DefenseUp", Duration = 3, Description = "Increase defense and endure." };
            case "Counter Stance":
                return new Skill { Name = "Counter Stance", SkillType = "Buff", MPCost = 4, Effect = "CounterStance", Duration = 2, Description = "Reduce and reflect damage.", UnlockCost = 5 };
            case "Taunt Slam":
                return new Skill { Name = "Taunt Slam", SkillType = "Damage", MPCost = 5, Power = 14, Effect = "AttackDown", Duration = 3, Description = "A crushing slam that weakens enemies.", UnlockCost = 5 };

            // === สกิล Berserker ===
            case "Rage Slash":
                return new Skill { Name = "Rage Slash", SkillType = "Damage", MPCost = 4, Power = 18, Effect = "None", Duration = 0, Description = "A brutal heavy slash." };
            case "Bloodlust":
                return new Skill { Name = "Bloodlust", SkillType = "Buff", MPCost = 0, SelfDamage = 5, Power = 0, Effect = "AttackUp", Duration = 3, Description = "Increase attack at the cost of HP." };
            case "Lifesteal":
                return new Skill { Name = "Lifesteal", Description = "Passive: Heal after attacking.", UnlockCost = 5 };
            case "Execute":
                return new Skill { Name = "Execute", SkillType = "Damage", MPCost = 0, Power = 25, SelfDamage = 5, Effect = "None", Duration = 0, Description = "A devastating finishing strike.", UnlockCost = 5 };

            default:
                return new Skill { Name = skillName, SkillType = "Normal", Power = 10, MPCost = 5, Description = "Unknown Skill" };
        }
    }
}