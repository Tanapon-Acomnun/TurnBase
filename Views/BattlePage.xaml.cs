using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Maui.Audio;
using TurnBase.Model;
using TurnBase.Services;
using TurnBase.ViewModels;

namespace TurnBase.Views
{
    public partial class BattlePage : BaseGamePage
    {
        private BattleViewModel _viewModel;

        // Audio Service
        private readonly AudioService _audioService;

        public BattlePage(Character player)
        {
            InitializeComponent();

            player.CurrentHP = player.MaxHP;

            _viewModel = new BattleViewModel(player);

            BindingContext = _viewModel;

            // Create Audio Service
            _audioService = new AudioService(AudioManager.Current);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;
            BindingContext = _viewModel;
        }

        private async void OnAttackClicked(object sender, EventArgs e)
        {
            // Play attack sound
            await _audioService.PlaySfx("swordattack.mp3");

            await _viewModel.ExecutePlayerTurn("Attack");

            if (!_viewModel.Enemy.IsAlive)
            {
                // Victory sound
                await _audioService.PlaySfx("victory.mp3");

                await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
            }
            else if (!_viewModel.Player.IsAlive)
            {
                await Navigation.PopToRootAsync();
            }
        }

        private async void OnSkillClicked(object sender, EventArgs e)
        {
            // Open skill menu sound
            await _audioService.PlaySfx("click.mp3");

            await Navigation.PushAsync(
                new SkillPage(
                    _viewModel.Player,
                    async selectedSkill =>
                    {
                        switch (selectedSkill)
                        {
                            case "Fireball":

                                await _audioService.PlaySfx("fireballspell.mp3");

                                await _viewModel.ExecutePlayerTurn("Skill:Fireball");
                                break;

                            case "Shield Bash":

                                await _audioService.PlaySfx("swordattack.mp3");

                                await _viewModel.ExecutePlayerTurn("Skill:ShieldBash");
                                break;

                            case "Rage Slash":

                                await _audioService.PlaySfx("Rage-slash.mp3");

                                await _viewModel.ExecutePlayerTurn("Skill:RageSlash");
                                break;
                        }
                    }));
        }

        private async void OnGuardClicked(object sender, EventArgs e)
        {
            // Guard sound
            await _audioService.PlaySfx("guard.mp3");

            await _viewModel.ExecutePlayerTurn("Guard");

            if (!_viewModel.Enemy.IsAlive)
            {
                await _audioService.PlaySfx("victory.mp3");

                await Navigation.PushAsync(
                    new VictoryPage(_viewModel.Player));
            }
            else if (!_viewModel.Player.IsAlive)
            {
                await Navigation.PopToRootAsync();
            }
        }

        private async void OnItemClicked(object sender, EventArgs e)
        {
            // Open inventory sound
            await _audioService.PlaySfx("click.mp3");

            await Navigation.PushAsync(
                new ItemPage(
                    _viewModel.Player,
                    async selectedItem =>
                    {
                        switch (selectedItem)
                        {
                            case "Potion":

                                await _audioService.PlaySfx("healpotion.mp3");

                                await _viewModel.ExecutePlayerTurn("Item:Potion");
                                break;

                            case "Ether":

                                await _audioService.PlaySfx("manapotion.mp3");

                                await _viewModel.ExecutePlayerTurn("Item:Ether");
                                break;
                        }

                        if (!_viewModel.Enemy.IsAlive)
                        {
                            await _audioService.PlaySfx("victory.mp3");

                            await Navigation.PushAsync(
                                new VictoryPage(_viewModel.Player));
                        }
                        else if (!_viewModel.Player.IsAlive)
                        {
                            await Navigation.PopToRootAsync();
                        }
                    }));
        }
    }
}