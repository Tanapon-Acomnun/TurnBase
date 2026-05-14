using TurnBase.Model;

namespace TurnBase.Views;

public partial class CharacterSelectPage : ContentPage
{
    public CharacterSelectPage()
    {
        InitializeComponent();
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
            Attack = 10,
            MP = 5,
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

        await Navigation.PushAsync(new BattlePage(player));
    }
}