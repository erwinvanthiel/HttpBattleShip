using Alten.Academy.Jumpstart.Battleship;
using Alten.Academy.Jumpstart.Battleship.Console;
using ConsoleWrapper = Alten.Academy.Jumpstart.Battleship.Console.Console;



namespace BattleShipAPI.Data
{
    public class GameStateService : IStateService
    {

        private BattleshipGame game;

        public GameStateService()
        {
            this.game = new BattleshipGame(new ConsoleWrapper());
            this.game.LoadLevel(AppDomain.CurrentDomain.BaseDirectory + "ships.csv");
        }

        public BattleshipGame getObject()
        {
            return game;
        }
    }
}
