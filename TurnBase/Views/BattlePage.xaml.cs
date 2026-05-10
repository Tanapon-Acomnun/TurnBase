using System;
using System.Collections.Generic;
using System.Text;
using TurnBase.Model;
using TurnBase.ViewModels;

namespace TurnBase.Views
{
    public partial class BattlePage : ContentPage
    {
        public BattlePage(Character player)
        {
            InitializeComponent();
            BindingContext = new BattleViewModel(player);
        }
    }
}
