using Alten.Academy.Jumpstart.Battleship;
using System.IO;

namespace BattleShipAPI.Data
{
    public interface ILevelParser
    {
        void populateBattleField(string filePath, BattleGrid grid);
    }
}