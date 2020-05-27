using Palcikas_Jatek.Model;
using Palcikas_Jatek.Models;
using Palcikas_Jatek.Repositories;
using Palcikas_Jatek.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Palcikas_Jatek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public int Dot { get; private set; }
        public int LineThickness { get; private set; }
        public int GridSize { get; private set; }
        public GameModel GameModel { get; private set; }
        private const double FPS = 30;
        private readonly DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Send);
        private readonly string _redPlayerName;
        private readonly string _bluePlayerName;
        private readonly bool _multiplayer;

        public GameWindow(string redPlayerName,string bluePlayerName, bool multiPlayer, bool rombus)
        {
            InitializeComponent();
            GameModel = new GameModel(7 , rombus);
            GridSize = GameModel.GridSize;
            Dot = GameModel.Cell / 10;
            LineThickness = Dot - 2;
            _bluePlayerName = bluePlayerName;
            _redPlayerName = redPlayerName;
            _multiplayer = multiPlayer;

            _timer.Interval = TimeSpan.FromMilliseconds(1000 / FPS);
            _timer.Tick += TimerTick;
            _timer.Start();

            NewGame();
           
        }

        private void TimerTick(object sender, EventArgs e)
        {

            canvas.Children.Clear();
            DrawSquares();
            DrawGrid();
            ComputerTurn();
           
        }

        private void NewGame()
        {
            tbBlueName.Text = _bluePlayerName;
            tbBlueName.Foreground = Brushes.SkyBlue;
            tbRedName.Text = _redPlayerName;
            tbRedName.Foreground = Brushes.Coral;
            tbBlueScore.Text = "0";
            tbRedScore.Text = "0";
            GameModel.NewGame();
        }

        public void ComputerTurn()
        {
            if (!GameModel.PlayersTurn && !_multiplayer)
            { 
                GameModel.ComputerTurn();
                SelectSide();
            }
        }

        private void DrawDot(int x, int y)
        {
            var dot = new Ellipse
            {
                Fill = Brushes.Coral ,
                Width = Dot,
                Height = Dot
            };
            Canvas.SetTop(dot, y - Dot / 2);
            Canvas.SetLeft(dot, x - Dot / 2);
            canvas.Children.Add(dot);
        }

        private void DrawGrid()
        {
            for (int i = 0; i < GridSize + 1; i++)
            {
                for (int j = 0; j < GridSize + 1; j++)
                {
                    DrawDot(GetGridX(j), GetGridY(i));
                }
            }
        }

        private void DrawSquares()
        {
            foreach (var square in GameModel.Squares)
            {
                DrawSides(square);

                if (square.SelectedNum == 4 || square.Disable)
                {
                    var rectangle = square.DrawFill();
                    Canvas.SetTop(rectangle, square.Top + square.H * 0.1);
                    Canvas.SetLeft(rectangle, square.Left + square.W *0.1);
                    canvas.Children.Add(rectangle);
                }
            }
        }
        

        private void DrawSides(Square square)
        {
            if (square.Disable)
            {
                return;
            }
            foreach (var line in square.DrawSides(GameModel.PlayersTurn))
            {
                line.StrokeThickness = LineThickness;
                canvas.Children.Add(line);
            }
        }



        private int GetGridX(int col)
        {
            return GameModel.Cell * (col + 1);
        }
        private int GetGridY(int row)
        {
            return GameModel.Cell + GameModel.Cell * row;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!GameModel.PlayersTurn && !_multiplayer)
            {
               return;
            }

            int x = (int)e.GetPosition(canvas).X;
            int y = (int)e.GetPosition(canvas).Y;

            HighLightSide(x, y);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectSide();
        }

        private void SelectSide()
        {
            if (GameModel.SelectSide())
            {
                tbRedScore.Text = GameModel.RedScore.ToString();
                tbBlueScore.Text = GameModel.BlueScore.ToString();

                if (GameOver())
                {
                    canvas.Children.Clear();
                    DrawSquares();
                    DrawGrid();

                    var score = new Score();
                    if (GameModel.RedScore > GameModel.BlueScore)
                    {
                        MessageBox.Show($"{tbRedName.Text} WIN!");
                        score.Name = tbRedName.Text;
                        score.Value = GameModel.RedScore;
                    }
                    else if (GameModel.RedScore < GameModel.BlueScore)
                    {
                        MessageBox.Show($"{tbBlueName.Text} WIN!");
                        score.Name = tbBlueName.Text;
                        score.Value = GameModel.BlueScore;
                    }
                    else
                    {
                        MessageBox.Show("It's a DRAW!");
                    }
                    score.Date = DateTime.Now;
                    score.MaxScore = GameModel.RedScore + GameModel.BlueScore;
                    score.Square = !GameModel.Rombus;

                    if(score.Name != "Computer" || _multiplayer)
                        ScoresRepository.StoreScore(score);

                    var scoreWindow = new HighScoreWindow();
                    scoreWindow.Show();
                    this.Close();

                    //NewGame();
                }
            }
        }

        private bool GameOver()
        {
            return GameModel.GameOver();
        }


        private void HighLightSide(int x, int y)
        {

            GameModel.HighLightSide(x, y);

            foreach (var square in GameModel.CurrentCells)
            {
                if (!square.Disable)
                {
                    foreach (var line in square.DrawSides(GameModel.PlayersTurn))
                    {
                        line.StrokeThickness = LineThickness;
                        canvas.Children.Add(line);
                    }
                }
            }

        }


    }
}
