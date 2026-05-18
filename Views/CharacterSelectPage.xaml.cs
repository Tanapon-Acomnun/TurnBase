using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views;

public partial class CharacterSelectPage : ContentPage
{
    private AudioService _audioService;

    public CharacterSelectPage()
    {
        InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);

        _audioService =
            Application.Current?.Handler?.MauiContext?.Services
            ?.GetService<AudioService>();
    }

    private async void OnWizardClicked(object sender, EventArgs e)
    {
        var selectedClass = new CharacterClass
        {
            ID = "จอมเวทนอกรีต",
            Name = "Wizard" ,
            MaxHP = 100,
            Attack = 12,
            MP = 30,
            Sprite = "mj03.png"
        };

        await SelectClass(selectedClass);
    }

    private async void OnWarriorClicked(object sender, EventArgs e)
    {
        var selectedClass = new CharacterClass
        {
            Name = "Berserker",
            ID= "นักรบคลั่ง",
            MaxHP = 120,
            Attack = 20,
            MP = 5,
            Sprite = "bs03.png"
        };

        await SelectClass(selectedClass);
    }

    private async void OnKnightClicked(object sender, EventArgs e)
    {
        var selectedClass = new CharacterClass
        {
            Name = "Knight",
            ID = "อัศวินผู้ร่วงหล่น",
            MaxHP = 150,
            Attack = 15,
            MP = 15,
            Sprite = "kn03.png"
        };

        await SelectClass(selectedClass);
    }

    private async Task SelectClass(CharacterClass selectedClass)
    {
        // =========================
        // STOP MAIN MENU BGM
        // =========================
        _audioService?.StopBgm();

        var player = new Character
        {
            Name = selectedClass.Name,
            ID = selectedClass.ID,
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
                    Dname = "ลูกไฟ",
                    SkillType = "Damage",
                    MPCost = 5,
                    Power = 20,
                    Effect = "Burn",
                    Duration = 3,
                    Description = "Deals fire damage and inflicts Burn."
                });

                player.Skills.Add(new Skill
                {
                    Name = "Arcane Shield",
                    Dname = "โล่เวทย์มนต์",
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
                    Dname = "หอกน้ำแข็ง",
                    MPCost = 6,
                    Power = 14,
                    Effect = "AttackDown",
                    Duration = 4,
                    SkillType = "Damage",
                    Description = "A chilling magical attack.",
                    UnlockCost = 5
                });

                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Mana Surge",
                    Dname = "ฟื้นฟูมานา",
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
                    Dname = "กระแทกโล่",
                    MPCost = 4,
                    Power = 10,
                    Effect = "AttackDown",
                    Duration = 3,
                    SkillType = "Damage",
                    Description = "A controlling shield strike."
                });

                player.Skills.Add(new Skill
                {
                    Name = "Fortify",
                    Dname = "ตั้งรับมั่นคง",
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
                    Dname = "ท่าตั้งรับสวนกลับ",
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
                    Dname = "ทุบยั่วยุ",
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
                    Dname = "กระหน่ำฟัน",
                    MPCost = 4,
                    Power = 18,
                    Effect = "None",
                    Duration = 0,
                    SkillType = "Damage",
                    Description = "A brutal heavy slash."
                });

                player.Skills.Add(new Skill
                {
                    Name = "Bloodlust",
                    Dname = "กระหายเลือด",
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
                    Dname = "ดูดเลือด",
                    Description = "Passive: Heal after attacking.",
                    UnlockCost = 5
                });

                player.UnlockableSkills.Add(new Skill
                {
                    Name = "Execute",
                    Dname = "พิฆาต",
                    MPCost = 0,
                    Power = 45,
                    Effect = "None",
                    Duration = 0,
                    SkillType = "Damage",
                    SelfDamage = 5,
                    Description = "A devastating strike.",
                    UnlockCost = 5
                });

                break;
        }

        await Navigation.PushAsync(new ProloguePage(player));
    }
}