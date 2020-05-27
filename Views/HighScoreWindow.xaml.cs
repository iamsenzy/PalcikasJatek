using Palcikas_Jatek.Models;
using Palcikas_Jatek.Repositories;
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
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        private IList<Score> _scores;
        public HighScoreWindow()
        {
            InitializeComponent();

            UpdateScores();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            var startGameWindow = new StartGameWindow();
            startGameWindow.Show();
            this.Close();
        }

        private void UpdateScores()
        {
            _scores = ScoresRepository.GetScores();
            ScoresListBox.ItemsSource = _scores;
        }
    }
}
