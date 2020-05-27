using Palcikas_Jatek.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Palcikas_Jatek.Model
{
    public struct OneSide
    {
        public bool Owner { get; set; }
        public bool Selected { get; set; }

        public OneSide(bool owner, bool selected)
        {
            Owner = owner;
            Selected = selected;
        }
    }
}
