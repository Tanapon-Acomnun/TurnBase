using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views;

public partial class FinalBossDialoguePage : ContentPage
{
    private Character _player;
    private List<DialoguePart> _dialogues;
    private int _index = 0;

    public FinalBossDialoguePage(Character player)
    {
        InitializeComponent();
        _player = player;

        SetPlayerImage();

        _dialogues = StoryService.GetFinalBossDialogue(_player.Name);

        ShowCurrent();
    }

    private void SetPlayerImage()
    {
        switch (_player.Name)
        {
            case "Knight":
                ImgLeft.Source = "knight.png";
                break;
            case "Berserker":
                ImgLeft.Source = "berserker.png";
                break;
            case "Wizard":
                ImgLeft.Source = "wizard.png";
                break;
            default:
                ImgLeft.Source = _player.Sprite; 
                break;
        }
    }

    private void ShowCurrent()
    {
        if (_index < _dialogues.Count)
        {
            var d = _dialogues[_index];
            LblName.Text = d.SpeakerName;
            LblMessage.Text = d.Message;

            ImgLeft.Opacity = (d.Side == "Left") ? 1.0 : 0.3;
            ImgRight.Opacity = (d.Side == "Right") ? 1.0 : 0.3;
        }
        else
        {
            ProceedToFinalBattle();
        }
    }

    private void OnNextClicked(object sender, EventArgs e) => ShowCurrent(++_index);

    private void ShowCurrent(int newIndex) { _index = newIndex; ShowCurrent(); }

    private async void ProceedToFinalBattle()
    {
        await Navigation.PushAsync(new BattlePage(_player));

        Navigation.RemovePage(this);
    }
}