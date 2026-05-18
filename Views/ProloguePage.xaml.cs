using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views;

public partial class ProloguePage : ContentPage
{
    private Character _player;
    private int _step = 0;
    private List<StoryPart> _prologueList;
    private CancellationTokenSource _cts;

    private bool _isProcessingTitle = false;
    private bool _isTyping = false;

    public ProloguePage(Character player)
    {
        InitializeComponent();
        _player = player;

        SetBackground();

        _prologueList = StoryService.GetCharacterPrologue(_player.Name);

        StartCurrentStep();
    }

    private void SetBackground()
    {
        if (_player == null || string.IsNullOrEmpty(_player.Name)) return;

        switch (_player.Name)
        {
            case "Knight":
                ImgBackground.Source = "prologueknight.png";
                break;
            case "Wizard":
                ImgBackground.Source = "prologuewizard.png";
                break;
            case "Berserker":
                ImgBackground.Source = "prologueberserker.png";
                break;
            default:
                ImgBackground.Source = "";
                break;
        }
    }

    private async void StartCurrentStep()
    {
        if (_prologueList == null || _step >= _prologueList.Count) return;

        _isProcessingTitle = true; 
        BtnNext.IsVisible = false;
        _cts = new CancellationTokenSource();

        var current = _prologueList[_step];
        ImgScene.Source = current.ImageSource; 
        LblStory.Text = "";

        if (!string.IsNullOrWhiteSpace(current.Title))
        {
            LblTitle.Text = current.Title;
            TitleLayout.Opacity = 0;
            await TitleLayout.FadeTo(1, 1000);
            await Task.Delay(2000);
            await TitleLayout.FadeTo(0, 800);
        }
        else
        {
            TitleLayout.Opacity = 0;
        }
        
        _isProcessingTitle = false;
        await TypeText(current.Content);
    }

    private async Task TypeText(string text)
    {
        _isTyping = true;
        BtnNext.Text = "ข้าม ►"; 
        BtnNext.IsVisible = true;

        try
        {
            foreach (char c in text)
            {
                if (_cts.Token.IsCancellationRequested) break;
                LblStory.Text += c;
                await Task.Delay(40, _cts.Token); 
            }
        }
        catch (TaskCanceledException) { }
        finally
        {
           
            FinalizeTextDisplay(text);
        }
    }

    private void FinalizeTextDisplay(string fullText)
    {
        LblStory.Text = fullText;
        _isTyping = false; 

        BtnNext.Text = (_step == _prologueList.Count - 1) ? "เข้าสู่การต่อสู้ ⚔️" : "ถัดไป ►";
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (_isProcessingTitle) return;

        if (_isTyping)
        {
            _cts?.Cancel();
        }
        else
        {
            if (_step < _prologueList.Count - 1)
            {
                _step++;
                StartCurrentStep();
            }
            else
            {
                await Navigation.PushAsync(new VillagerDialoguePage(_player));
                Navigation.RemovePage(this);
            }
        }
    }
}