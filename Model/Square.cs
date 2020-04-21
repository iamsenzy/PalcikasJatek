using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

        public Side HighLight { get; set; } = Side.Null;
        private bool _left, _top, _right, _bottom;
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
                return light ? Brushes.LightCoral : Brushes.Coral; 
            }
            else
            {
                return light ? Brushes.LightGreen : Brushes.Green;
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
            //var rectangle = new Rectangle();
            //rectangle.Fill = Brushes.Gray;
            //rectangle.Stroke = Brushes.Green;
            //rectangle.Width = this.W;
            //rectangle.Height = this.H;
            //Canvas.SetTop(rectangle, this.Top);
            //Canvas.SetLeft(rectangle, this.Left);
            //_canvas.Children.Add(rectangle);
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

            if (_bottom)
            {
                DrawSide(Side.BOTTOM, GetColor(playersTurn, false));
            }
            if (_left)
            {
                DrawSide(Side.LEFT, GetColor(playersTurn, false));
            }
            if (_top)
            {
                DrawSide(Side.TOP, GetColor(playersTurn, false));
            }
            if (_right)
            {
                DrawSide(Side.RIGHT, GetColor(playersTurn, false));
            }

        }

        public void SelectSide()
        {
            if (HighLight == Side.Null)
            {
                return;
            }

            switch (HighLight)
            {
                case Side.LEFT:
                    _left = true;
                    break;
                case Side.TOP:
                    _top = true;
                    break;
                case Side.RIGHT:
                    _right = true;
                    break;
                case Side.BOTTOM:
                    _bottom = true;
                    break;
                default:
                    break;
            }

            HighLight = Side.Null;
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
            if (closest == distanceBot && !this._bottom)
            {
                HighLight = Side.BOTTOM;
            }
            else if (closest == distanceLeft && !this._left)
            {
                HighLight = Side.LEFT;
            }
            else if (closest == distanceRight && !this._right)
            {
                HighLight = Side.RIGHT;
            }
            else if (closest == distanceTop && !this._top)
            {
                HighLight = Side.TOP;
            }

            // return the highlighted side
            return HighLight;
        }
    }
}
