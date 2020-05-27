using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Palcikas_Jatek.Views
{
    /// <summary>
    /// Interaction logic for StartGameWindow.xaml
    /// </summary>
    public partial class StartGameWindow : Window
    {
        public StartGameWindow()
        {
            InitializeComponent();

        }


        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (tbPlayerRedName.Text != "" && ((tbPlayerBlueName.Text != "" && cbMultiplayer.IsChecked.Value) || !cbMultiplayer.IsChecked.Value))
            {
                var GameWindow = new GameWindow(tbPlayerRedName.Text, tbPlayerBlueName.Text,
                                                cbMultiplayer.IsChecked.Value, rbRombus.IsChecked.Value);
                GameWindow.Show();
                this.Close();
            }
        }

        private void TbPlayerRedName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPlayerRedName.Text = "";
        }

        private void CbMultiplayer_Checked(object sender, RoutedEventArgs e)
        {

            tbPlayerBlueName.IsEnabled = true;

        }

        private void CbMultiplayer_Unchecked(object sender, RoutedEventArgs e)
        {
            tbPlayerBlueName.Text = "Computer";
            tbPlayerBlueName.IsEnabled = false;
        }

        private void HighScores_Click(object sender, RoutedEventArgs e)
        {
            var scoresView = new HighScoreWindow();
            scoresView.Show();
            this.Close();
        }

        private void TbPlayerBlueName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPlayerBlueName.Text = "";
        }
    }
}
