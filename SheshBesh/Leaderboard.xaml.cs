using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Windows;
namespace SheshBesh
{
    public partial class Leaderboard : Window
    {
       
        public Leaderboard()
        {
            InitializeComponent();
        }

        private void Leaderboard_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = Database.GetLeaderboard();
        }
    }
}