using System;
using System.Collections.Generic;
using System.Text;
using TurnBase.Model;
using TurnBase.ViewModels;

namespace TurnBase.Views
{
    public partial class BattlePage : BaseGamePage
    {
        private BattleViewModel _viewModel;

        public BattlePage(Character player)
        {

            InitializeComponent();

            player.CurrentHP = player.MaxHP;

            _viewModel = new BattleViewModel(player);

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Optional future use
        }

        private async void OnAttackClicked(object sender, EventArgs e)
        {
            await _viewModel.ExecutePlayerTurn("Attack");

            if (!_viewModel.Enemy.IsAlive)
            {
                await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
            }
            else if (!_viewModel.Player.IsAlive)
            {
                await Navigation.PopToRootAsync();
            }
        }
        private async void OnSkillClicked(object sender, EventArgs e)
        {
            await _viewModel.ExecutePlayerTurn("Skill");

            if (!_viewModel.Enemy.IsAlive)
            {
                await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
            }
            else if (!_viewModel.Player.IsAlive)
            {
                await Navigation.PopToRootAsync();
            }
        }

        private async void OnGuardClicked(object sender, EventArgs e)
        {
            await _viewModel.ExecutePlayerTurn("Guard");

            if (!_viewModel.Enemy.IsAlive)
            {
                await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
            }
            else if (!_viewModel.Player.IsAlive)
            {
                await Navigation.PopToRootAsync();
            }
        }

        private async void OnItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemPage(_viewModel.Player));
        }

    }
}
