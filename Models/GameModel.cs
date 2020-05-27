using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Palcikas_Jatek.Model
{
    public class GameModel
    {
        public int GameWidth { get; private set; }
        public int GameHeight { get; private set; }
        public int GridSize { get; private set; }
        public int Cell { get; private set; }


        private List<Square> _currentCells;
        private bool _playersTurn;
        private int _redScore;
        private int _blueScore;

        private readonly Random _random = new Random();

        public List<Square> Squares { get; set; }
        public bool PlayersTurn { get => _playersTurn; set => _playersTurn = value; }
        public int RedScore { get => _redScore; private set => _redScore = value; }
        public int BlueScore { get => _blueScore; private set => _blueScore = value; }
        public List<Square> CurrentCells { get => _currentCells; set => _currentCells = value; }

        public bool Rombus { get; set; }

        public GameModel(int gridSize, bool rombus)
        {
            GameWidth = 500;
            GameHeight = 500;
            Rombus = rombus;
            GridSize = gridSize;
            Cell = GameWidth / (GridSize + 2);
        }

        public void NewGame()
        {
            BlueScore = 0;
            RedScore = 0;
            PlayersTurn = _random.Next() % 2 == 0;
            Squares = new List<Square>();
            CurrentCells = new List<Square>();
            Squares.Clear();
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (Rombus)
                    {
                        Squares.Add(new Square(GetGridX(j), GetGridY(i), Cell, Cell, true));
                    }
                    else
                    {
                        Squares.Add(new Square(GetGridX(j), GetGridY(i), Cell, Cell));
                    }
                    
                }
            }

            if (Rombus)
            {
                int half = GridSize / 2;
                int k = 1;
                bool l = true;
                for (int i = 0; i < GridSize; i++)
                {
                    for (int j = 0; j < GridSize; j++)
                    {
                        if (j == half)
                        {
                            Squares[i * GridSize + j].EnableSquare();
                            for (int d = 0; d < k; d++)
                            {
                                Squares[i * GridSize + j+d].EnableSquare();
                                Squares[i * GridSize + j-d].EnableSquare();
                            }

                        }
                    }

                    if (l)
                    {
                        k++;
                    }
                    else
                    {
                        k--;
                    }

                    if (k > half)
                    {
                        l = false;
                    }


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

        public List<List<(Square S, List<Side> Sides)>> NextMoveOptions()
        {
            var options = new List<List<(Square, List<Side>)>>();
            for (int i = 0; i < 3; i++)
            {
                var opt = new List<(Square, List<Side>)>();

                options.Add(opt);
            }
            var empty_sides = new List<Side>();

            for (int i = 0; i < Squares.Count; i++)
            {
                switch (Squares[i].SelectedNum)
                {
                    case 3:
                        options[0].Add((Squares[i], empty_sides));
                        break;
                    case 0:
                    case 1:
                        var goodSides = CheckNeighbrous(i);
                        var priority = goodSides.Count > 0 ? 1 : 2;
                        options[priority].Add((Squares[i], goodSides));
                        break;
                    case 2:
                        options[2].Add((Squares[i], empty_sides));
                        break;
                }
            }

            return options;
        }

        public List<Side> CheckNeighbrous(int ind)
        {
            var goodSides = new List<Side>();
            var square = Squares[ind];
            int row = ind / GridSize;

            if (!square.LeftSide.Selected)
            {
                if (ind % GridSize == 0 || Squares[ind - 1].Disable || Squares[ind - 1].SelectedNum < 2)
                {
                    goodSides.Add(Side.LEFT);

                }
            }
            if (!square.RightSide.Selected)
            {
                if (ind % GridSize == GridSize - 1 || Squares[ind + 1].Disable || Squares[ind + 1].SelectedNum < 2)
                {
                    goodSides.Add(Side.RIGHT);
                }
            }

            if (!square.TopSide.Selected)
            {
                if (row == 0 || Squares[ind - GridSize].Disable || Squares[ind - GridSize].SelectedNum < 2)
                {
                    goodSides.Add(Side.TOP);
                }
            }

            if (!square.BottomSide.Selected)
            {
                if (row == GridSize - 1 || Squares[ind + GridSize].Disable || Squares[ind + GridSize].SelectedNum < 2)
                {
                    goodSides.Add(Side.BOTTOM);
                }
            }

            return goodSides;
        }

        public (Square,Side) ComputerTurn()
        {
            if (_playersTurn)
            {
                return (new Square(),new Side());
            }
            var options = NextMoveOptions();

            Square option = new Square();
            var sides = new List<Side>();
            var side = new Side();
            if (options[0].Count > 0) // 3 side selected
            {
                option = options[0][_random.Next(options[0].Count)].S;
            }
            else if (options[1].Count > 0) // 0 or 1 side selected
            {
                var (S, Sides) = options[1][_random.Next(options[1].Count)];
                option = S;
                sides = Sides;
            }
            else if (options[2].Count > 0) // 2 side selected
            {
                option = options[2][_random.Next(options[2].Count)].S;
            }

            Coordinate coordinate;
            if (sides.Count > 0)
            {
                side = sides[_random.Next(sides.Count)];
                coordinate = option.GetFreeSideCoords(side);
            }
            else
            {
                coordinate = option.GetFreeSideCoords();
            }

            HighLightSide(coordinate.X, coordinate.Y);
            return (option,side);
        }

        public bool SelectSide()
        {
            if (CurrentCells.Count == 0)
            {
                return false;
            }
            bool filledSquare = false;
            foreach (var square in CurrentCells)
            {
                if (square.SelectSide(PlayersTurn))
                {
                    filledSquare = true;
                    if (_playersTurn)
                    {
                        RedScore++;
                    }
                    else
                    {
                        BlueScore++;
                    }
                }
            }
            if (filledSquare)
            {
                
                return true;
            }

            _playersTurn = !_playersTurn;
            CurrentCells.Clear();
            return true;
        }

        public void HighLightSide(int x, int y)
        {
            // clear prev highlihting
            foreach (var square in Squares)
            {
                square.HighLight = Side.Null;
            }

            CurrentCells.Clear();
            for (int i = 0; i < Squares.Count; i++)
            {
                Side side;
                // highlight current
                if (Squares[i].Contains(x, y))
                {
                    side = Squares[i].HighLightSide(x, y);
                    if (side != Side.Null)
                    {
                        CurrentCells.Add(Squares[i]);
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
                        Squares[k].HighLight = highlight;
                        CurrentCells.Add(Squares[k]);
                    }

                    break;
                }
            }

        }

        public bool GameOver()
        {
            foreach (var square in Squares)
            {
                if (square.SelectedNum != 4 && !square.Disable)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
