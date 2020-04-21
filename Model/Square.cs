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
    class Square
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
        private Canvas _canvas;

        public Square(int x, int y, int w, int h, Canvas canvas)
        {
            W = w;
            H = h;
            Left = x;
            Top = y;
            Right = x + w;
            Bottom = y + h;
            _canvas = canvas;
        }

        public bool Contains(int x, int y)
        {
            return x >= Left && x < Right && y >= Top && y < Bottom;
        }

        public Brush GetColor(bool playersTurn, bool light)
        {
            if (playersTurn)
            {
                return light ? Brushes.LightPink : Brushes.Coral; 
            }
            else
            {
                return light ? Brushes.LimeGreen : Brushes.Lime;
            }
        }

        private void DrawLine(int x1, int y1, int x2, int y2, Brush color)
        {
            var line = new Line();
            line.Stroke = color;
            line.StrokeThickness = 6;
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            _canvas.Children.Add(line);
        }

        public void DrawFill()
        {
            var rectangle = new Rectangle();
            rectangle.Fill = GetColor(Owner, false);
            rectangle.Width = W / 2;
            rectangle.Height = H / 2;
            Canvas.SetTop(rectangle, Top + H/2);
            Canvas.SetLeft(rectangle, Left + W/2);
            _canvas.Children.Add(rectangle);
        }

        public void DrawSide(Side side, Brush color)
        {
            switch (side)
            {
                case Side.LEFT:
                    DrawLine(Left, Top, Left, Bottom, color);
                    break;
                case Side.TOP:
                    DrawLine(Left, Top, Right, Top, color);
                    break;
                case Side.RIGHT:
                    DrawLine(Right, Top, Right, Bottom, color);
                    break;
                case Side.BOTTOM:
                    DrawLine(Left, Bottom, Right, Bottom, color);
                    break;
                default:
                    break;
            }

        }

        public void DrawSides(bool playersTurn)
        {
            if (HighLight!=Side.Null)
            {
                DrawSide(HighLight, GetColor(playersTurn, true));
            }

            if (_bottom.Selected)
            {
                DrawSide(Side.BOTTOM, GetColor(_bottom.Owner, false));
            }
            if (_left.Selected)
            {
                DrawSide(Side.LEFT, GetColor(_left.Owner, false));
            }
            if (_top.Selected)
            {
                DrawSide(Side.TOP, GetColor(_top.Owner, false));
            }
            if (_right.Selected)
            {
                DrawSide(Side.RIGHT, GetColor(_right.Owner, false));
            }

        }

        public bool SelectSide(bool playersTurn)
        {
            if (HighLight == Side.Null )
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
                if(distances[i] < closest)
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
