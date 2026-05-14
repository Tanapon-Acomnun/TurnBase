using TurnBase.Model;

namespace TurnBase.Views;

public partial class CharacterSelectPage : ContentPage
{
    public CharacterSelectPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnWizardClicked(object sender, EventArgs e)
    {
        var selectedClass = new CharacterClass
        {
            Name = "Wizard",
            MaxHP = 70,
            Attack = 12,
            MP = 30,
            Sprite = "wizardback.png"
        };
        

        await SelectClass(selectedClass);
    }

    private async void OnWarriorClicked(object sender, EventArgs e)
    {
        var selectedClass = new CharacterClass
        {
            Name = "Berserker",
            MaxHP = 100,
            Attack = 15,
            MP = 10,
            Sprite = "berserkerback.png"
        };

        await SelectClass(selectedClass);
    }

    private async void OnKnightClicked(object sender, EventArgs e)
    {
        var selectedClass = new CharacterClass
        {
            Name = "Knight",
            MaxHP = 120,
            Attack = 50,
            MP = 15,
            Sprite = "knightback.png"
        };

        await SelectClass(selectedClass);
    }

    private async Task SelectClass(CharacterClass selectedClass)
    {
        var player = new Character
        {
            Name = selectedClass.Name,
            MaxHP = selectedClass.MaxHP,
            CurrentHP = selectedClass.MaxHP,
            Attack = selectedClass.Attack,
            MP = selectedClass.MP,
            MaxMP = selectedClass.MP,
            Sprite = selectedClass.Sprite
        };
        // Assign starter skills
        switch (player.Name)
        {
            case "Wizard":
                player.Skills.Add(new Skill
                {
                    Name = "Fireball",
                    SkillType = "Damage",
                    MPCost = 5,
                    Power = 10,
                    Effect = "Burn",
                    Duration = 3,
                    Description = "Deals fire damage and inflicts Burn."
                });
                player.Skills.Add(new Skill
                {
                    Name = "Arcane Shield",
                    SkillType = "Buff",
                    MPCost = 4,
                    Power = 0,
                    Duration = 3,
                    Effect = "DefenseUp",
                    Description = "Raises defense for 3 turns."
                });
                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Ice Bolt",
                    MPCost = 6,
                    Power = 14,
                    Effect = "AttackDown",
                    Duration = 2,
                    SkillType = "Damage",
                    Description =
                    "A chilling magical attack.",
                    UnlockCost = 5
                });
                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Mana Surge",
                    MPCost = 0,
                    Power = 15,
                    Effect = "RestoreMP",
                    Duration = 0,
                    SkillType = "Utility",
                    Description = "Restore magical energy.",
                    UnlockCost = 5
                });
                break;

            case "Knight":
                player.Skills.Add(new Skill
                {
                    Name = "Shield Bash",
                    MPCost = 4,
                    Power = 10,
                    Effect = "AttackDown",
                    Duration = 3,
                    SkillType = "Damage",
                    Description =
                        "A controlling shield strike."
                });

                player.Skills.Add(new Skill
                {
                    Name = "Fortify",
                    ExtraEffects = new List<string>
                    {
                      "Regen"
                    },
                    MPCost = 5,
                    Power = 0,
                    Effect = "DefenseUp",
                    Duration = 3,
                    SkillType = "Buff",
                    Description = "Increase defense and endure."
                });
                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Counter Stance",
                    MPCost = 4,
                    Effect = "CounterStance",
                    Duration = 2,
                    SkillType = "Buff",
                    Description = "Reduce and reflect damage.",
                    UnlockCost = 5
                });
                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Taunt Slam",
                    MPCost = 5,
                    Power = 14,
                    Effect = "AttackDown",
                    Duration = 3,
                    SkillType = "Damage",
                    Description = "A crushing slam that weakens enemies.",
                    UnlockCost = 5
                });

                break;

            case "Berserker":
                player.Skills.Add(new Skill
                {
                    Name = "Rage Slash",
                    MPCost = 4,
                    Power = 18,
                    Effect = "None",
                    Duration = 0,
                    SkillType = "Damage",
                    Description =
                        "A brutal heavy slash."
                });

                player.Skills.Add(new Skill
                {
                    Name = "Bloodlust",
                    MPCost = 0,
                    SelfDamage = 5,
                    Power = 0,
                    Effect = "AttackUp",
                    Duration = 3,
                    SkillType = "Buff",
                    Description = "Increase attack at the cost of HP."
                });
                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Lifesteal",
                    Description = "Passive: Heal after attacking.",
                    UnlockCost = 5
                });
                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Execute",
                    MPCost = 0,
                    Power = 25,
                    Effect = "None",
                    Duration = 0,
                    SkillType = "Damage",
                    SelfDamage = 5,
                    Description = "A devastating finishing strike.",
                    UnlockCost = 5
                });

                break;
        }

        await Navigation.PushAsync(new BattlePage(player));
    }
}