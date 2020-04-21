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
            return x >= this.Left && x < this.Right && y >= this.Top && y < this.Bottom;
        }

        public Brush GetColor(bool playersTurn, bool light)
        {
            if (playersTurn)
            {
                return light ? Brushes.LightCoral : Brushes.Coral; 
            }
            else
            {
                return light ? Brushes.LightPink : Brushes.Pink;
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
                    DrawLine(this.Left, this.Top, this.Left, this.Bottom, color);
                    break;
                case Side.TOP:
                    DrawLine(this.Left, this.Top, this.Right, this.Top, color);
                    break;
                case Side.RIGHT:
                    DrawLine(this.Right, this.Top, this.Right, this.Bottom, color);
                    break;
                case Side.BOTTOM:
                    DrawLine(this.Left, this.Bottom, this.Right, this.Bottom, color);
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
            Trace.WriteLine($"distances:{distanceLeft},{distanceTop}, {distanceRight},{distanceBot} closest {closest} Mouse {x},{y}");

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
