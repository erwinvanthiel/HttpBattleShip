using Alten.Academy.Jumpstart.Battleship.Console;
using BattleShipAPI.Data;
using BattleShipAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;


namespace Alten.Academy.Jumpstart.Battleship
{
    /// <summary>
    /// Class responsible for orchastrating a game of Battleship. This class observes the observable battle grid.
    /// </summary>
    public class BattleshipGame : IObserver { 

        // Game grid that is observed by this observer class.
        private BattleGrid grid;
        public string OpponentGrid { get; set; }
        private bool myTurn;

        // We use dependency inversion for the renderer, console and parser so that more sophisticated versions could be plugged in without too much code refactoring.
        private IConsole console;
        private ILevelParser parser;
        private string msg;
        public bool MyTurn { get { return myTurn; }  set { this.myTurn = value;  this.grid.MyTurn = value; } }
        public bool MustNotifyOpponent;

        public void renderMyTurn()
        {
            throw new NotImplementedException();
        }

        public BattleshipGame(IConsole console)
        {
            this.console = console;
            this.grid = new BattleGrid(9,9);
            this.parser = new MyLevelParser();
            this.msg = "";
            this.OpponentGrid = "";
            grid.subscribe(this);
            IsGameCrashed = false;
            IsGameWon = false;
            IsGameLost = false;
        }

        // Print the game string to the console.
        public void printGame()
        {
            console.Clear();
            console.WriteLine(this.OpponentGrid);
        }

        public void printFreshGrid()
        {
            console.Clear();
            MyTurn = !MyTurn;
            console.WriteLine(stringListToString(MyBattleGridRenderer.constructGameText(this.grid, "")));
            MyTurn = !MyTurn;
        }

        public bool IsGameWon { get; set; }

        public bool IsGameLost { get; set; }

        public bool IsGameCrashed { get; set; }

        // Load level from comma delimited textfile (csv)
        public void LoadLevel(string args)
        {
            try {
                parser.populateBattleField(args, grid);
            }
            catch (Exception e) {
                console.WriteLine(e.Message);
                IsGameCrashed = true;
            }
        }

        public string getMygrid()
        { 
            return stringListToString(MyBattleGridRenderer.constructGameText(this.grid, this.msg));
        }

        private string stringListToString(List<string> list) 
        {
            string results = "";
            foreach(var str in list) 
            {
                results += str + "\n";
            }
            return results;
        }

        // Parse input, update gamestate and return game in string representation.
        public string PlayTurn(string targetedGridPointString)
        {
            // Clear game message.
            this.msg = "";

            // Parse input and update grid state.
            (int, int) gridPoint = parseGridPoint(targetedGridPointString);
            grid.addStruckLocation(gridPoint.Item1, gridPoint.Item2);

            // for drawing the correct string.
            grid.MyTurn = true;

            // Check winning/losing conditions first.
            if (grid.numberOfShipsDown() == grid.Ships.Count - grid.NumberOfCruiseShips)
            {
                var endGameText = MyBattleGridRenderer.constructGameText(this.grid, MyBattleGridRenderer.gameWonMessage());
                endGameText.RemoveAt(endGameText.Count - 1);
                MustNotifyOpponent = true;
                MyTurn = false;
                IsGameLost = true;
                return stringListToString(endGameText);
            }
            if (!grid.cruiseShipStillSafe())
            {
                var endGameText = MyBattleGridRenderer.constructGameText(this.grid, MyBattleGridRenderer.cruiseShipDownMessage());
                endGameText.RemoveAt(endGameText.Count - 1);
                MustNotifyOpponent = true;
                IsGameWon = true;
                MyTurn = false;
                return stringListToString(endGameText);
            }

            // for enabling the gameLoop thread to parse new input
            MyTurn = true;
            // return grid string to opponent.
            return stringListToString(MyBattleGridRenderer.constructGameText(grid, this.msg));
        }

        // Parse the user input and return the location in the form of int, char
        private (int, int) parseGridPoint(string targetedGridPointString)
        {
            string letters = "ABCDEFGHI";
            int x = Int32.Parse(targetedGridPointString[1].ToString()) - 1;
            int y = letters.IndexOf(Char.ToUpper((targetedGridPointString[0])));
            return (x, y);
        }

        // These methods are event triggers that are called by the observable object, grid in this case.
        public void notifyShipDown(Ship ship)
        {
            this.msg = MyBattleGridRenderer.shipDownMessage(ship);
        }

        public void notifyShipHit(Ship ship, int x, int y)
        {
            this.msg = MyBattleGridRenderer.shipHitMessage(ship, x, y);
        }
        public void notifyCruiseShipHit(Ship ship, int x, int y)
        {
            this.msg = MyBattleGridRenderer.cruiseShipHitMessage(ship, x, y);
        }

        public void notifyMiss(int x, int y)
        {
            this.msg = MyBattleGridRenderer.missMessage(x, y);
        }

    }
}
