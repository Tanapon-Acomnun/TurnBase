using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views;

public partial class MiniBossDialoguePage : ContentPage
{
    private Character _player;
    private int _bossIndex;
    private List<DialoguePart> _dialogues;
    private int _index = 0;

    public MiniBossDialoguePage(Character player, int bossIndex, string bossImage = "demonlord.png")
    {
        NavigationPage.SetHasNavigationBar(this, false);
        InitializeComponent();
        _player = player;
        _bossIndex = bossIndex;

        SetPlayerImage();

        ImgRight.Source = bossImage;

        _dialogues = StoryService.GetMiniBossDialogue(_bossIndex, _player.Name);
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
        if (_dialogues == null || _dialogues.Count == 0)
        {
            ProceedToBattle();
            return;
        }

        if (_index < _dialogues.Count)
        {
            var d = _dialogues[_index];
            LblName.Text = d.SpeakerName;
            LblMessage.Text = d.Message;

            ImgLeft.Opacity = (d.Side == "Left") ? 1.0 : 0.4;
            ImgRight.Opacity = (d.Side == "Right") ? 1.0 : 0.4;
        }
        else
        {
            ProceedToBattle();
        }
    }

    private void OnNextTapped(object sender, EventArgs e)
    {
        _index++;
        ShowCurrent();
    }

    private async void ProceedToBattle()
    {
        await Navigation.PushAsync(new BattlePage(_player));
        Navigation.RemovePage(this);
    }
}