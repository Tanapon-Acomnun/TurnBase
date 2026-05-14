using TurnBase.Model;
using TurnBase.ViewModels;
using TurnBase.Services;

namespace TurnBase.Views
{
    public partial class BattlePage : BaseGamePage
    {
        private BattleViewModel _viewModel;

        private readonly AudioService _audioService;

        public BattlePage(Character player)
        {
            InitializeComponent();

            _audioService =
                Application.Current.Handler.MauiContext.Services
                .GetService<AudioService>();

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

        // =========================
        // ATTACK
        // =========================
        private async void OnAttackClicked(object sender, EventArgs e)
        {
            // WIZARD
            if (_viewModel.Player.Name == "Wizard")
            {
                await _audioService.PlaySfx("wizard-attack.mp3");
            }

            // BERSERKER
            else if (_viewModel.Player.Name == "Berserker")
            {
                await _audioService.PlaySfx("knight-berserker-attack.mp3");
            }

            // KNIGHT
            else if (_viewModel.Player.Name == "Knight")
            {
                await _audioService.PlaySfx("knight-berserker-attack.mp3");
            }

            await Task.Delay(150);

            await _viewModel.ExecutePlayerTurn("Attack");

            if (_viewModel.Enemy.IsAlive)
            {
                await PlayEnemyAttackSound();
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
        }

        // =========================
        // SKILL
        // =========================
        private async void OnSkillClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(
                new SkillPage(
                    _viewModel.Player,
                    async selectedSkill =>
                    {
                        // WIZARD
                        if (_viewModel.Player.Name == "Wizard")
                        {
                            await _audioService.PlaySfx(
                                "wizard-fireball-skill.mp3");
                        }

                        // BERSERKER
                        else if (_viewModel.Player.Name == "Berserker")
                        {
                            await _audioService.PlaySfx(
                                "knight-berserker-attack.mp3");
                        }

                        // KNIGHT
                        else if (_viewModel.Player.Name == "Knight")
                        {
                            await _audioService.PlaySfx(
                                "knight-berserker-attack.mp3");
                        }

                        await Task.Delay(150);

                        await _viewModel.ExecuteSkillTurn(selectedSkill);

                        if (_viewModel.Enemy.IsAlive)
                        {
                            await PlayEnemyAttackSound();
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

        // =========================
        // GUARD
        // =========================
        private async void OnGuardClicked(object sender, EventArgs e)
        {
            await _audioService.PlaySfx("guard-she.mp3");

            await Task.Delay(150);

            await _viewModel.ExecutePlayerTurn("Guard");

            if (_viewModel.Enemy.IsAlive)
            {
                await PlayEnemyAttackSound();
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
        }

        // =========================
        // ITEM
        // =========================
        private async void OnItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(
                new ItemPage(
                    _viewModel.Player,
                    async selectedItem =>
                    {
                        // POTION
                        if (selectedItem == "Potion")
                        {
                            await _audioService.PlaySfx(
                                "mana-potion.mp3");

                            await Task.Delay(150);

                            await _viewModel.ExecutePlayerTurn(
                                "Item:Potion");
                        }

                        // ETHER
                        else if (selectedItem == "Ether")
                        {
                            await _audioService.PlaySfx(
                                "mana-potion.mp3");

                            await Task.Delay(150);

                            await _viewModel.ExecutePlayerTurn(
                                "Item:Ether");
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

        // =========================
        // Monster Info
        // =========================
        private async Task PlayEnemyAttackSound()
        {
            switch (_viewModel.Enemy.Name)
            {
                case "Slime":
                    await _audioService.PlaySfx("slime-attack.mp3");
                    break;

                case "Goblin":
                    await _audioService.PlaySfx("goblin-attack.mp3");
                    break;

                case "Wolf":
                    await _audioService.PlaySfx("wolf-attack.mp3");
                    break;

                case "Skeleton":
                    await _audioService.PlaySfx("skeleton-attack.mp3");
                    break;

                case "Slime King":
                    await _audioService.PlaySfx("boss-slime.mp3");
                    break;

                case "Orc Warlord":
                    await _audioService.PlaySfx("orc-boss.mp3");
                    break;

                case "Demon Lord":
                    await _audioService.PlaySfx("demon-lord.mp3");
                    break;
            }
        }
    }
}