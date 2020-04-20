using System;
using System.Collections.Generic;
using System.Text;

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

        private bool highlight;

        public Square(int x, int y, int w, int h)
        {
            W = w;
            H = h;
            Left = x;
            Top = y;
            Right = x + w;
            Bottom = y;
        }

        public bool Contains(int x, int y)
        {
            return x >= this.Left && x < this.Right && y >= this.Top && y < this.Bottom;
        }

        public void DrawFill()
        {
            // TODO
        }

        public void DrawSides()
        {
            // TODO
        }

        public void DrawSide()
        {

            // TODO
        }
    }
}
