namespace Alten.Academy.Jumpstart.Battleship
{
    public interface IObserver
    {
        void notifyShipDown(Ship ship);
        void notifyShipHit(Ship ship, int x, int y);
        void notifyCruiseShipHit(Ship ship, int x, int y);
        void notifyMiss(int x, int y);
    }
}