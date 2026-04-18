using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBase.Views
{
    public partial class BattlePage : ContentPage
    {
        public BattlePage()
        {
            InitializeComponent();
            BindingContext = new BattleViewModel();
        }
    }
}
