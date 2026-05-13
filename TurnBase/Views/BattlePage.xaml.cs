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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;
            BindingContext = _viewModel;
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
            await Navigation.PushAsync(
                new SkillPage(
                    _viewModel.Player,
                    async selectedSkill =>
                    {
                        await _viewModel.ExecuteSkillTurn(selectedSkill);

                        if (!_viewModel.Enemy.IsAlive)
                        {
                            await Navigation.PushAsync(
                                new VictoryPage(_viewModel.Player));
                        }
                        else if (!_viewModel.Player.IsAlive)
                        {
                            await Navigation.PopToRootAsync();
                        }
                    }));
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
            await Navigation.PushAsync(
                new ItemPage(
                    _viewModel.Player,
                    async selectedItem =>
                    {
                        switch (selectedItem)
                        {
                            case "Potion":
                                await _viewModel.ExecutePlayerTurn("Item:Potion");
                                break;

                            case "Ether":
                                await _viewModel.ExecutePlayerTurn("Item:Ether");
                                break;
                        }

                        if (!_viewModel.Enemy.IsAlive)
                        {
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
