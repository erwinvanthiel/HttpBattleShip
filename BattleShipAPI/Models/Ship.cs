using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alten.Academy.Jumpstart.Battleship
{
    public class Ship
    {

        public Ship(string name, int x, int y, string alignment, char code, string type, int length) 
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Alignment = alignment;
            this.Length = length;
            this.Code = code;
            this.Sunk = false;
            this.Type = type;
        }

        public string Name { get; set; }
        public bool Sunk { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Alignment { get; set; }
        public char Code { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }

        public List<(int, int)> getGridPoints() { 
            List<(int, int)> points = new List<(int, int)> ();
            var myPoint = (X, Y);
            points.Add(myPoint);
            for (int i = 0; i < Length - 1; i++) 
            {
                myPoint = (myPoint.X + Convert.ToInt32(Alignment == "Horizontal"), myPoint.Y + Convert.ToInt32(Alignment == "Vertical"));
                points.Add(myPoint);
            }
            return points;
        }
    }
}
