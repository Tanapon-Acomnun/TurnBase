using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views;

public partial class EndStoryPage : ContentPage
{
    private Character _player;
    private string _fullEndingStory;

    public EndStoryPage(Character player)
    {
        InitializeComponent();
        _player = player;

        SetBackground();
        LoadEndingStory();
    }

    private void SetBackground()
    {
        if (_player == null || string.IsNullOrEmpty(_player.Name)) return;

        switch (_player.Name.Trim())
        {
            case "Knight":
                ImgEndingBackground.Source = "endingknight.png";
                break;
            case "Wizard":
                ImgEndingBackground.Source = "endingwizard.png";
                break;
            case "Berserker":
                ImgEndingBackground.Source = "endingberserker.png";
                break;
            default:
                ImgEndingBackground.Source = "endingknight.png"; 
                break;
        }
    }

    private void LoadEndingStory()
    {
        var story = StoryService.GetEndingStory(_player.Name);
        if (story.Count > 0)
        {
            _fullEndingStory = story[0].Content;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RunFadingEnding();
    }

    private async Task RunFadingEnding()
    {
        await WhiteFadeOverlay.FadeTo(0, 4000);

        if (string.IsNullOrEmpty(_fullEndingStory)) return;

        string[] paragraphs = _fullEndingStory.Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        await Task.Delay(1000);

        foreach (var paragraph in paragraphs)
        {
            LblEndContent.Text = paragraph.Trim();

            await StoryScrollView.FadeTo(1, 1000);

            int readTime = Math.Max(3000, paragraph.Length * 50);
            await Task.Delay(readTime);

            await StoryScrollView.FadeTo(0, 1000);
            await Task.Delay(500);
        }

        TitleLayout.InputTransparent = false;
        await TitleLayout.FadeTo(1, 3000);
    }

    private async void OnBackToMenuClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}