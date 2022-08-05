using Alten.Academy.Jumpstart.Battleship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipAPI.Models
{
    /// <summary>
    /// Class responsible for constructing the textual representation of the Battleship game.
    /// </summary>
    public class MyBattleGridRenderer
    {
        private static string letters = "ABCDEFGHI";

        // Costruct the actual play field row by row.
        public static List<string> constructGridRowStrings(BattleGrid grid)
        {
            List<string> gridString = new List<string>();
            gridString.Add("   1  2  3  4  5  6  7  8  9 ");
            gridString.Add(" +---------------------------+");
            for (int i = 0; i < grid.GridWidth; i++)
            {
                gridString.Add(letters[i] + "|" + constructGridRow(grid, i) + "|");
            }
            gridString.Add(" +---------------------------+");
            return gridString;
        }

        // Costruct a row of the actual play field.
        public static string constructGridRow(BattleGrid grid, int i)
        {
            string gridRowString = "";
            for (int p = 0; p < grid.GridWidth; p++)
            {

                var gridrowpoint = grid.KnownPoints.Where(tuple => tuple.x == p && tuple.y == i);
                if (gridrowpoint.Count() == 0)
                {
                    gridRowString += " . ";
                }
                else
                {
                    try
                    {
                        gridRowString += " " + grid.getShipCodeAtLocation(gridrowpoint.First().x, gridrowpoint.First().y) + " ";
                    }
                    catch (Exception e)
                    {
                        gridRowString += " ~ ";
                    }
                }
            }
            return gridRowString;
        }

        // Construct the entire game text by concatenating individual components.
        public static List<string> constructGameText(BattleGrid grid, string msg)
        {
            var gameText = new List<string>();
            gameText.Add(constructTopLine(grid));
            gameText.Add("");
            gameText.Add(msg);
            gameText.Add("");
            gameText.AddRange(constructGridRowStrings(grid));
            gameText.Add("");
            gameText.Add(constructBottomLine(grid));
            return gameText;
        }

        private static string constructTopLine(BattleGrid grid)
        {
            return string.Format("Unsunken ships: {0}, locations remaining: {1}", grid.Ships.Count - grid.numberOfShipsDown(), grid.LocationsRemaining);
        }

        private static string constructBottomLine(BattleGrid grid)
        {
            if (!grid.MyTurn) { return "Shoot coordinate?"; }
            return string.Format("Waiting for opponent to shoot...");
        }

        // These methods comprise the messages that are the consequence of different event triggers
        public static string shipDownMessage(Ship ship)
        {
            return string.Format("HIT! You sunk {0} \"{1}\"!", ship.Type, ship.Name);
        }

        public static string cruiseShipDownMessage()
        {
            return string.Format("A cruise ship has sunk! You lose!");
        }

        public static string gameWonMessage()
        {
            return string.Format("You sunk all ships! YOU WIN!");
        }

        public static string shipHitMessage(Ship ship, int x, int y)
        {
            return string.Format("HIT! {0} on {1}{2}", ship.Type, letters[y].ToString(), x + 1);
        }

        public static string missMessage(int x, int y)
        {
            return string.Format("SPLASH! on {0}{1}", letters[y].ToString(), x + 1);
        }

        public static string cruiseShipHitMessage(Ship ship, int x, int y)
        {
            return string.Format("HIT! {0} on {1}{2}", ship.Type, letters[y].ToString(), x + 1);
        }

        public static string wrongInputMessage()
        {
            return "Input should be in the form of {A//B//C//E//F//G//H//I}{123456789}";
        }
    }
}
