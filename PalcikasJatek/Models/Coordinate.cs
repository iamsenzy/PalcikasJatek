﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Palcikas_Jatek.Model
{
    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
