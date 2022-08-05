using Alten.Academy.Jumpstart.Battleship;
using System;
using System.IO;
using System.Linq;

namespace BattleShipAPI.Data
{
    /// <summary>
    /// Class responsible for parsing the file and initialising the battleship grid object.
    /// </summary>
    public class MyLevelParser : ILevelParser
    {
        public void populateBattleField(string filePath, BattleGrid grid)
        {

            foreach (string line in File.ReadLines(filePath))
            {

                // using the method
                string[] separatedString = line.Split(',');

                string name = parseName(separatedString[0]);
                string type = separatedString[1];
                int x = parseX(separatedString[2]);
                int y = parseY(separatedString[3]);
                string alignment = parseAlignment(separatedString[4]);

                switch (type)
                {
                    case "Aircraft Carrier":
                        grid.addShip(new Ship(name, x, y, alignment, 'A', "Aircraft Carrier", 5));
                        break;
                    case "Destroyer":
                        grid.addShip(new Ship(name, x, y, alignment, 'D', "Destroyer", 3));
                        break;
                    case "Cruise Ship":
                        grid.addShip(new Ship(name, x, y, alignment, 'C', "Cruise Ship", 2));
                        grid.NumberOfCruiseShips++;
                        break;
                    case "Patrol Boat":
                        grid.addShip(new Ship(name, x, y, alignment, 'P', "Patrol Boat", 2));
                        break;
                    case "Battleship":
                        grid.addShip(new Ship(name, x, y, alignment, 'B', "Battleship", 4));
                        break;
                    case "Submarine":
                        grid.addShip(new Ship(name, x, y, alignment, 'S', "Submarine", 3));
                        break;
                    default:
                        throw new Exception("Unknown ship type");
                }

                if (grid.shipsOverlap())
                {
                    throw new Exception("There are overlapping ships, which is not allowed!");
                }

            }
            grid.LocationsRemaining = grid.Ships.Select(ship => ship.Length).Sum();
        }

        private string parseAlignment(string v)
        {
            if (v != "Horizontal" && v != "Vertical")
            {
                throw new Exception("Alignment must be Horizontal or Vertical");
            }
            return v;
        }

        private int parseY(string v)
        {
            switch (v[0])
            {
                case 'A':
                    return 0;
                case 'B':
                    return 1;
                case 'C':
                    return 2;
                case 'D':
                    return 3;
                case 'E':
                    return 4;
                case 'F':
                    return 5;
                case 'G':
                    return 6;
                case 'H':
                    return 7;
                case 'I':
                    return 8;
                default:
                    throw new Exception("Invalid vertical location");
            }
        }

        private int parseX(string v)
        {
            try
            {
                int result = int.Parse(v);
                if (result < 1 || result > 9)
                {
                    throw new Exception();
                }
                return result - 1;
            }
            catch (Exception e)
            {
                throw new Exception("X was not an integer between 1 and 9", e);
            }
        }


        private string parseName(string name)
        {
            return name;
        }
    }
}