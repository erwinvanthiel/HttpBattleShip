namespace Alten.Academy.Jumpstart.Battleship
{
    public interface IObservable
    {
        void subscribe(IObserver observer);
    }
}