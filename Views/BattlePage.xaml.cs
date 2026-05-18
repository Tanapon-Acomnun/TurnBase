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
            player.MP = player.MaxMP;
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
        // ANIMATIONS
        // =========================

        private async Task PlayPlayerAttackAsync()
        {
            // Player พุ่งไปข้างหน้า
            await PlayerSprite.TranslateTo(80, -10, 100, Easing.CubicOut);

            // Slash + flash
            SlashLabel.Opacity = 1;
            SlashLabel.Scale = 0.5;
            EnemyFlash.Opacity = 0.55;

            await Task.WhenAll(
                SlashLabel.ScaleTo(1.4, 120, Easing.CubicOut),
                EnemySprite.TranslateTo(14, 0, 60),
                EnemySprite.ScaleTo(0.92, 60)
            );

            await Task.WhenAll(
                EnemySprite.TranslateTo(0, 0, 80),
                EnemySprite.ScaleTo(1.0, 80),
                SlashLabel.FadeTo(0, 150),
                EnemyFlash.FadeTo(0, 180)
            );

            await PlayerSprite.TranslateTo(0, 0, 120, Easing.CubicIn);
        }

        private async Task PlayPlayerSkillAsync()
        {
            // Player charge
            await PlayerSprite.ScaleTo(1.15, 150, Easing.CubicOut);

            // Skill orb บินไปหา enemy
            SkillLabel.Opacity = 1;
            SkillLabel.Scale = 0.3;
            SkillLabel.TranslationX = -200;

            await Task.WhenAll(
                SkillLabel.ScaleTo(1.2, 250, Easing.CubicOut),
                SkillLabel.TranslateTo(0, 0, 250, Easing.CubicOut)
            );

            // Explosion
            EnemyFlash.Color = Colors.Cyan;
            EnemyFlash.Opacity = 0.6;

            await Task.WhenAll(
                SkillLabel.ScaleTo(2.0, 150, Easing.CubicOut),
                SkillLabel.FadeTo(0, 150),
                EnemySprite.RotateTo(8, 80),
                EnemySprite.ScaleTo(0.88, 80)
            );

            await Task.WhenAll(
                EnemySprite.RotateTo(0, 100),
                EnemySprite.ScaleTo(1.0, 100),
                EnemyFlash.FadeTo(0, 200)
            );

            EnemyFlash.Color = Colors.Red;
            await PlayerSprite.ScaleTo(1.0, 120, Easing.CubicIn);
        }

        private async Task PlayEnemyAttackAsync()
        {
            // Enemy พุ่งเข้าหา player
            await EnemySprite.TranslateTo(-70, 10, 100, Easing.CubicOut);

            // Flash ที่ player
            PlayerFlash.Opacity = 0.5;

            await Task.WhenAll(
                PlayerSprite.TranslateTo(-10, 0, 60),
                PlayerSprite.ScaleTo(0.93, 60)
            );

            await Task.WhenAll(
                PlayerSprite.TranslateTo(0, 0, 80),
                PlayerSprite.ScaleTo(1.0, 80),
                PlayerFlash.FadeTo(0, 200)
            );

            await EnemySprite.TranslateTo(0, 0, 120, Easing.CubicIn);
        }

        private async Task PlayGuardAsync()
        {
            await PlayerSprite.ScaleTo(0.88, 100, Easing.CubicOut);
            await Task.Delay(300);
            await PlayerSprite.ScaleTo(1.0, 150, Easing.BounceOut);
        }

        private async Task PlayItemAsync()
        {
            // วงแหวนสีเขียวกระพริบที่ player
            PlayerFlash.Color = Colors.LimeGreen;
            PlayerFlash.Opacity = 0.45;

            await Task.WhenAll(
                PlayerSprite.ScaleTo(1.1, 150, Easing.CubicOut),
                PlayerFlash.FadeTo(0, 400)
            );

            await PlayerSprite.ScaleTo(1.0, 150, Easing.BounceOut);
            PlayerFlash.Color = Colors.MediumPurple;
        }

        // =========================
        // ATTACK
        // =========================
        private async void OnAttackClicked(object sender, EventArgs e)
        {
            if (_viewModel.Player.Name == "Wizard")
                await _audioService.PlaySfx("wizard-attack.mp3");
            else if (_viewModel.Player.Name == "Berserker")
                await _audioService.PlaySfx("knight-berserker-attack.mp3");
            else if (_viewModel.Player.Name == "Knight")
                await _audioService.PlaySfx("knight-berserker-attack.mp3");

            // Animation player โจมตี
            await PlayPlayerAttackAsync();

            await _viewModel.ExecutePlayerTurn("Attack");

            if (_viewModel.Enemy.IsAlive)
            {
                await PlayEnemyAttackSound();
                await PlayEnemyAttackAsync();
            }

            if (!_viewModel.Enemy.IsAlive)
                await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
            else if (!_viewModel.Player.IsAlive)
                await Navigation.PopToRootAsync();
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
                        if (_viewModel.Player.Name == "Wizard")
                            await _audioService.PlaySfx("wizard-fireball-skill.mp3");
                        else if (_viewModel.Player.Name == "Berserker")
                            await _audioService.PlaySfx("knight-berserker-attack.mp3");
                        else if (_viewModel.Player.Name == "Knight")
                            await _audioService.PlaySfx("knight-berserker-attack.mp3");

                        // Animation skill
                        await PlayPlayerSkillAsync();

                        await _viewModel.ExecuteSkillTurn(selectedSkill);

                        if (_viewModel.Enemy.IsAlive)
                        {
                            await PlayEnemyAttackSound();
                            await PlayEnemyAttackAsync();
                        }

                        if (!_viewModel.Enemy.IsAlive)
                            await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
                        else if (!_viewModel.Player.IsAlive)
                            await Navigation.PopToRootAsync();
                    }));
        }

        // =========================
        // GUARD
        // =========================
        private async void OnGuardClicked(object sender, EventArgs e)
        {
            await _audioService.PlaySfx("guard-she.mp3");

            // Animation guard
            await PlayGuardAsync();

            await _viewModel.ExecutePlayerTurn("Guard");

            if (_viewModel.Enemy.IsAlive)
            {
                await PlayEnemyAttackSound();
                await PlayEnemyAttackAsync();
            }

            if (!_viewModel.Enemy.IsAlive)
                await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
            else if (!_viewModel.Player.IsAlive)
                await Navigation.PopToRootAsync();
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
                        if (selectedItem == "Potion")
                        {
                            await _audioService.PlaySfx("mana-potion.mp3");
                            await PlayItemAsync();
                            await _viewModel.ExecutePlayerTurn("Item:Potion");
                        }
                        else if (selectedItem == "Ether")
                        {
                            await _audioService.PlaySfx("mana-potion.mp3");
                            await PlayItemAsync();
                            await _viewModel.ExecutePlayerTurn("Item:Ether");
                        }

                        if (!_viewModel.Enemy.IsAlive)
                            await Navigation.PushAsync(new VictoryPage(_viewModel.Player));
                        else if (!_viewModel.Player.IsAlive)
                            await Navigation.PopToRootAsync();
                    }));
        }

        // =========================
        // ENEMY ATTACK SOUND
        // =========================
        private async Task PlayEnemyAttackSound()
        {
            switch (_viewModel.Enemy.Name)
            {
                case "สไลม์":
                    await _audioService.PlaySfx("slime-attack.mp3"); break;
                case "ก็อบลิน":
                    await _audioService.PlaySfx("goblin-attack.mp3"); break;
                case "ฝูงหมาป่า":
                    await _audioService.PlaySfx("wolf-attack.mp3"); break;
                case "โครงกระดูก":
                    await _audioService.PlaySfx("skeleton-attack.mp3"); break;
                case "ราชาสไลม์":
                    await _audioService.PlaySfx("boss-slime.mp3"); break;
                case "แม่ทัพออร์ค":
                    await _audioService.PlaySfx("orc-boss.mp3"); break;
                case "จอมมาร":
                    await _audioService.PlaySfx("demon-lord.mp3"); break;
                case "อัศวินทมิฬ":
                    await _audioService.PlaySfx("dark k.mp3"); break;
            }
        }
    }
}