using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Palcikas_Jatek.Model
{
    public class Square
    {
        public int W { get; set; }
        public int H { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public int SelectedNum { get; set; } = 0;
        public bool Owner { get; set; }
        public Side HighLight { get; set; } = Side.Null;
        private OneSide _left, _top, _right, _bottom;
        public bool Disable { get; set; }

        private readonly Random _random = new Random();

        public OneSide LeftSide { get { return _left; } set { _left = value; } }
        public OneSide TopSide { get { return _top; } set { _top = value; } }
        public OneSide RightSide { get { return _right; } set { _right = value; } }
        public OneSide BottomSide { get { return _bottom; } set { _bottom = value; } }

        public Square()
        {
        }

        public Square(int x, int y, int w, int h, bool rombus=false)
        {
            W = w;
            H = h;
            Left = x;
            Top = y;
            Right = x + w;
            Bottom = y + h;
            Disable = false;
            if (rombus) { DisableSquare();}
        }

        public bool Contains(int x, int y)
        {

            return x >= Left && x < Right && y >= Top && y < Bottom;
        }

        public void DisableSquare()
        {
            _left.Selected = _top.Selected = _right.Selected = _bottom.Selected = true;
            SelectedNum = 4;
            Disable = true;

        }
        public void EnableSquare()
        {
            _left.Selected = _top.Selected = _right.Selected = _bottom.Selected = false;
            SelectedNum = 0;
            Disable = false;
        }

        public Brush GetColor(bool playersTurn, bool light)
        {
            if (Disable)
            {
                return light ? Brushes.LightGray : Brushes.Gray;
            }

            if (playersTurn)
            {
                return light ? Brushes.LightPink : Brushes.Coral;
            }
            else
            {
                return light ? Brushes.SkyBlue : Brushes.DeepSkyBlue;
            }
        }

        private Line DrawLine(int x1, int y1, int x2, int y2, Brush color)
        {
            var line = new Line
            {
                Stroke = color,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            };
            return line;
        }

        public Rectangle DrawFill()
        {
            var rectangle = new Rectangle
            {
                Fill = GetColor(Owner, false),
                Width = W - W * 0.2,
                Height = H - H * 0.2
            };
            // Canvas.SetTop(rectangle, Top + 6);
            // Canvas.SetLeft(rectangle, Left + 6);
            // _canvas.Children.Add(rectangle);
            return rectangle;
        }

        public Line DrawSide(Side side, Brush color)
        {
            Line line = new Line();

            switch (side)
            {
                case Side.LEFT:
                    line = DrawLine(Left, Top, Left, Bottom, color);
                    break;
                case Side.TOP:
                    line = DrawLine(Left, Top, Right, Top, color);
                    break;
                case Side.RIGHT:
                    line = DrawLine(Right, Top, Right, Bottom, color);
                    break;
                case Side.BOTTOM:
                    line = DrawLine(Left, Bottom, Right, Bottom, color);
                    break;
                default:
                    break;
            }

            return line;
        }

        public List<Line> DrawSides(bool playersTurn)
        {
            List<Line> lines = new List<Line>();

            if (_bottom.Selected)
            {
                lines.Add(DrawSide(Side.BOTTOM, GetColor(_bottom.Owner, false)));
            }
            if (_left.Selected)
            {
                lines.Add(DrawSide(Side.LEFT, GetColor(_left.Owner, false)));
            }
            if (_top.Selected)
            {
                lines.Add(DrawSide(Side.TOP, GetColor(_top.Owner, false)));
            }
            if (_right.Selected)
            {
                lines.Add(DrawSide(Side.RIGHT, GetColor(_right.Owner, false)));
            }

            if (HighLight != Side.Null)
            {
                lines.Add(DrawSide(HighLight, GetColor(playersTurn, true)));
            }

            return lines;
        }

        public bool SelectSide(bool playersTurn)
        {
            if (HighLight == Side.Null)
            {
                return false;
            }

            switch (HighLight)
            {
                case Side.LEFT:
                    _left.Owner = playersTurn;
                    _left.Selected = true;
                    break;
                case Side.TOP:
                    _top.Owner = playersTurn;
                    _top.Selected = true;
                    break;
                case Side.RIGHT:
                    _right.Owner = playersTurn;
                    _right.Selected = true;
                    break;
                case Side.BOTTOM:
                    _bottom.Owner = playersTurn;
                    _bottom.Selected = true;
                    break;
                default:
                    break;
            }

            HighLight = Side.Null;
            SelectedNum++;
            if (SelectedNum == 4)
            {
                Owner = playersTurn;
                return true;
            }

            return false;
        }

        public Coordinate GetFreeSideCoords(Side side = Side.Null)
        {
            Coordinate cLeft = new Coordinate(Left, Top + H / 2);
            Coordinate cTop = new Coordinate(Left + W / 2, Top);
            Coordinate cRight = new Coordinate(Right - 1, Top + H / 2);
            Coordinate cBottom = new Coordinate(Left + W / 2, Bottom - 1);

            Coordinate coordinate = new Coordinate(-1, -1);
            if (side != Side.Null)
            {
                switch (side)
                {
                    case Side.LEFT:
                        coordinate = cLeft;
                        break;
                    case Side.TOP:
                        coordinate = cTop;
                        break;
                    case Side.RIGHT:
                        coordinate = cRight;
                        break;
                    case Side.BOTTOM:
                        coordinate = cBottom;
                        break;
                    case Side.Null:
                        break;
                    default:
                        break;
                }
            }
            if (side != Side.Null)
            {
                return coordinate;
            }

            var freeCoords = new List<Coordinate>();
            if (!_left.Selected)
            {
                freeCoords.Add(cLeft);
            }
            if (!_top.Selected)
            {
                freeCoords.Add(cTop);
            }
            if (!_right.Selected)
            {
                freeCoords.Add(cRight);
            }
            if (!_bottom.Selected)
            {
                freeCoords.Add(cBottom);
            }

            return freeCoords[_random.Next(freeCoords.Count)];
        }

        public Side HighLightSide(int x, int y)
        {
            int distanceRight = this.Right - x;
            int distanceTop = y - this.Top;
            int distanceLeft = x - this.Left;
            int distanceBot = this.Bottom - y;

            int[] distances = { distanceLeft, distanceTop, distanceRight, distanceBot };
            // determine closest value
            int closest = distances[0];
            for (int i = 1; i < distances.Length; i++)
            {
                if (distances[i] < closest)
                {
                    closest = distances[i];
                }
            }

            // highlight the closest if not already selected
            if (closest == distanceBot && !_bottom.Selected)
            {
                HighLight = Side.BOTTOM;
            }
            else if (closest == distanceLeft && !_left.Selected)
            {
                HighLight = Side.LEFT;
            }
            else if (closest == distanceRight && !_right.Selected)
            {
                HighLight = Side.RIGHT;
            }
            else if (closest == distanceTop && !_top.Selected)
            {
                HighLight = Side.TOP;
            }

            // return the highlighted side
            return HighLight;
        }
    }
}
