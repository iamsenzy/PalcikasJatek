using System;
using System.Collections.Generic;
using System.Text;

namespace Palcikas_Jatek.Models
{
    public class Score
    {
        public String Name { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }

        public int MaxScore { get; set; }

        public Boolean Square { get; set; }

        public override string ToString()
        {
            var map = Square ? "Square" : "Rombus";
            return $"{Name} with: {Value} score of {MaxScore}, on {Date}, map:{map}";
        }
    }
}
