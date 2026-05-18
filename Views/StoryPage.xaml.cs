using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TurnBase.Model;
using TurnBase.Services;

namespace TurnBase.Views
{
    public partial class StoryPage : ContentPage
    {
        private int _step = 0;
        private bool _isTyping = false;
        private string _currentFullText = "";
        private CancellationTokenSource _cts;

        private List<StoryPart> _storyList = new List<StoryPart>();

        public StoryPage()
        {
            InitializeComponent();
            SetupStoryData();

            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(300); 
                UpdatePage();
            });
        }

        private void SetupStoryData()
        {
            _storyList = StoryService.GetOpeningStory();
        }

        private async void UpdatePage()
        {
            if (_storyList == null || _storyList.Count == 0) return;

            var currentStory = _storyList[_step];

            ImgScene.Source = currentStory.ImageSource;
            BtnBack.IsVisible = (_step > 0);

            _cts?.Cancel();

            LblStory.Text = "";
            if (StoryScrollView != null)
            {
                await StoryScrollView.ScrollToAsync(0, 0, false);
            }

            await TypeText(currentStory.Content);
        }

        private async Task TypeText(string text)
        {
            _isTyping = true;
            _currentFullText = text;
            LblStory.Text = "";

            BtnNext.Text = "ข้าม ►";
            BtnNext.IsVisible = true;
            BtnStartGame.IsVisible = false;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                foreach (char c in text)
                {
                    if (token.IsCancellationRequested) break;

                    LblStory.Text += c;

                    if (StoryScrollView != null)
                    {
                        await StoryScrollView.ScrollToAsync(0, StoryScrollView.ContentSize.Height, false);
                    }

                    await Task.Delay(50, token);
                }
            }
            catch (TaskCanceledException) { }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    FinalizeTextDisplay();
                }
            }
        }

        private async void FinalizeTextDisplay()
        {
            LblStory.Text = _currentFullText;
            _isTyping = false;

            if (StoryScrollView != null)
            {
                await Task.Delay(1000);
                await StoryScrollView.ScrollToAsync(0, StoryScrollView.ContentSize.Height, false);
            }

            bool isLastStep = (_step == _storyList.Count - 1);
            if (isLastStep)
            {
                BtnNext.IsVisible = false;
                BtnStartGame.IsVisible = true;
            }
            else
            {
                BtnNext.Text = "ถัดไป ►";
                BtnNext.IsVisible = true;
            }
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            if (_isTyping)
            {
                _cts?.Cancel();
                FinalizeTextDisplay();
            }
            else
            {
                if (_step < _storyList.Count - 1)
                {
                    _step++;
                    UpdatePage();
                }
            }
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            if (_step > 0)
            {
                _cts?.Cancel();
                _step--;
                UpdatePage();
            }
        }

        private async void OnStartGameClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharacterSelectPage());
            Navigation.RemovePage(this);
        }
    }
}