namespace Alten.Academy.Jumpstart.Battleship.Console

{
using System.Collections.Generic;

public interface IBattleGridRenderer
    {
        List<string> constructGameText(string msg);
        string shipDownMessage(Ship ship);
        string cruiseShipDownMessage();
        string gameWonMessage();
        string shipHitMessage(Ship ship, int x, int y);
        string wrongInputMessage();
        string cruiseShipHitMessage(Ship ship, int x, int y);
        string missMessage(int x, int y);
    }
}