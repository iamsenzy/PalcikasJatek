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
        private const double RefreshTimerSec = 0.05;

        // game var
        private List<Square> squares;
        private List<Square> currentCells;
        private bool _playersTurn;
        private bool _computer; // if false no computer player
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
            ComputerTurn();
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

        public void ComputerTurn()
        {
            if(_playersTurn)
            {
                return;
            }

            var options = new List<List<Square>>();
            for (int i = 0; i < 3; i++)
            {
                List<Square> opt = new List<Square>();
                options.Add(opt);
            }

            for (int i=0;i<squares.Count;i++)
            {
                switch (squares[i].SelectedNum)
                {
                    case 3:
                        options[0].Add(squares[i]);
                        break;
                    case 0:
                    case 1:
                        options[1].Add(squares[i]);
                        break;
                    case 2: 
                        options[2].Add(squares[i]);
                        break;
                }
            }

            Square option = new Square();

            if (options[0].Count > 0)
            {
                option = options[0][_random.Next(options[0].Count)];
            }
            else if (options[1].Count > 0)
            {
                option = options[1][_random.Next(options[1].Count)];
            }
            else if (options[2].Count > 0)
            {
                option = options[2][_random.Next(options[2].Count)];
            }

            Coordinate coordinate = option.GetFreeSideCoords();
           
            HighLightSide(coordinate.X,coordinate.Y);
            SelectSide();
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
                if (square.SelectedNum == 4)
                {
                    square.DrawFill();
                }
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
            if (!_playersTurn)
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
            if (currentCells.Count == 0)
            {
                return;
            }
            bool filledSquare = false;
            foreach (var square in currentCells)
            {

                if (square.SelectSide(_playersTurn))
                {
                    filledSquare = true;
                }
            }
            currentCells.Clear();
            if (filledSquare)
            {
                if (GameOver())
                {
                    NewGame();
                }
            }
            else
            {
                _playersTurn = !_playersTurn;
            }
        }

        private bool GameOver()
        {
            foreach (var square in squares)
            {
                if(square.SelectedNum != 4)
                {
                    return false;
                }
            }
            return true;
        }


        private void HighLightSide(int x, int y)
        {
            // clear prev highlihting
            foreach (var square in squares)
            {
                square.HighLight = Side.Null;
            }

            currentCells.Clear();
            for (int i = 0; i < squares.Count; i++)
            {
                Side side = Side.Null;
                // highlight current
                if (squares[i].Contains(x, y))
                {
                    side = squares[i].HighLightSide(x, y);
                    if (side != Side.Null)
                    {
                        squares[i].DrawSides(_playersTurn);
                        currentCells.Add(squares[i]);
                    }

                }

                bool neighbour = true;
                Side highlight = Side.Null;
                int row = i / GridSize;
                int k = 0;
                if (side == Side.LEFT && i % GridSize > 0)
                {
                    k = i - 1;
                    highlight = Side.RIGHT;
                }
                else if (side == Side.TOP && row - 1 >= 0)
                {
                    k = i - GridSize;
                    highlight = Side.BOTTOM;
                }
                else if (side == Side.RIGHT && i % GridSize + 1 < GridSize)
                {
                    k = i + 1;
                    highlight = Side.LEFT;
                }
                else if (side == Side.BOTTOM && row + 1 < GridSize)
                {
                    k = i + GridSize;
                    highlight = Side.TOP;
                }
                else
                {
                    neighbour = false;
                }

                if (neighbour)
                {
                    squares[k].HighLight = highlight;
                    currentCells.Add(squares[k]);
                }


            }

        }


    }
}
