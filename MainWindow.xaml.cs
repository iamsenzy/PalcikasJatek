using Palcikas_Jatek.Model;
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
    public partial class MainWindow : Window
    {
        // game param
        private const int GameHeight = 500;
        private const int GameWidth = 500;
        private const int GridSize = 5;
        private const int Cell = GameWidth / (GridSize + 2);
        private const int Dot = Cell / 10;
        private const double RefreshTimerSec = 0.03;

        // game var
        private List<Square> squares;
        private List<Square> currentCells;
        private bool _playersTurn;
        private bool _isRunning;
        private readonly DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Send);


        private readonly Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(RefreshTimerSec);
            _timer.Tick += TimerTick;
            _timer.Start();
            _isRunning = true;
            NewGame();
        }


        private void TimerTick(object sender, EventArgs e)
        {
            canvas.Children.Clear();
            DrawSquares();
            DrawGrid();
        }


        private void NewGame()
        {
            _playersTurn = true;
            squares = new List<Square>();
            currentCells = new List<Square>();
            squares.Clear();
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    squares.Add(new Square(GetGridX(j), GetGridY(i), Cell, Cell, canvas));
                }
            }
        }

        private void DrawDot(int x, int y)
        {
            var dot = new Ellipse();
            dot.Fill = Brushes.Coral;
            dot.Width = dot.Height = Dot;
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
            foreach (var square in squares)
            {
                square.DrawSides(_playersTurn);
                square.DrawFill();
            }
        }



        private int GetGridX(int col)
        {
            return Cell * (col + 1);
        }
        private int GetGridY(int row)
        {
            return Cell + Cell * row;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!playersTurn)
            //{
            //    return;
            //}

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
            if(currentCells.Count == 0)
            {
                return;
            }

            foreach (var square in currentCells)
            {
                square.SelectSide();
            }
            currentCells.Clear();
        }

        private void HighLightSide(int x, int y)
        {
            // clear prev highlihting
            foreach (var square in squares)
            {
                square.HighLight = Side.Null;
            }

            currentCells.Clear();
            foreach (var square in squares)
            {
                if (square.Contains(x, y))
                {
                    square.HighLightSide(x, y);
                    square.DrawSides(_playersTurn);
                    currentCells.Add(square);
                }
            }

        }

       
    }
}
