using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alten.Academy.Jumpstart.Battleship
{
    /// <summary>
    /// Class responsible maintaining track of the state of the Battlegrid, this class notifies its observers of evenets specified in the interface.
    /// </summary>
    public class BattleGrid : IObservable
    {
        private List<IObserver> observers;

        public List<(int x, int y)> KnownPoints { get; }
        public List<Ship> Ships { get; }
        public int GridWidth { get; }
        public int NumberOfCruiseShips { get; set; }

        public int LocationsRemaining { get; set; }
        public bool MyTurn { get; set; }

        public BattleGrid(int width, int height)
        {
            this.GridWidth = width;
            KnownPoints = new List<(int, int)> ();
            this.Ships = new List<Ship>();
            this.NumberOfCruiseShips = 0;
            this.observers = new List<IObserver>();
        }

        // We add new points to our known gridpoints.
        // If a ship was hit we decrement locationsRemaining.
        // We notify the observers whether it was a hit, and if so, what was hit.
        public void addStruckLocation(int x, int y)
        {
            var alreadyHitThisPoint = KnownPoints.Contains((x, y));

            if (!alreadyHitThisPoint)
            {
                KnownPoints.Add((x, y));
            }

            foreach (var ship in Ships) 
            {
                if (ship.getGridPoints().Contains((x, y))) 
                {
                    if (ship.Code == 'C')
                    {
                        foreach (var observer in this.observers) 
                        {
                            observer.notifyCruiseShipHit(ship, x, y);
                        }
                        LocationsRemaining -= (1 - Convert.ToInt32(alreadyHitThisPoint));
                        return;
                    }
                    else
                    {
                        LocationsRemaining -= (1 - Convert.ToInt32(alreadyHitThisPoint));
                        foreach (var observer in this.observers)
                        {
                            observer.notifyShipHit(ship, x, y);
                        }
                        return;
                    }
                }
            }
            foreach (var observer in this.observers)
            {
                observer.notifyMiss(x, y);
            }
        }

        public void addShip(Ship ship)
        {
            Ships.Add(ship);
        }

        // Check if there is a known ship at the passed location (x,y).
        public char getShipCodeAtLocation(int x1, int x2)
        {
            foreach (var ship in Ships) 
            {
                if (ship.getGridPoints().Contains((x1, x2)))
                {
                    return ship.Code;                
                }
            }
            throw new Exception("No ship found at this location!");
        }

        // Check if there are overlapping ships.
        public bool shipsOverlap()
        {
            foreach (var ship1 in Ships)
            {
                foreach (var ship2 in Ships)
                {
                    foreach (var pointShip1 in ship1.getGridPoints())
                    {
                        foreach (var pointShip2 in ship2.getGridPoints())
                        {
                            if (pointShip1.Equals(pointShip2) && !ship1.Name.Equals(ship2.Name))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        // Check wheter a ship has sunk.
        public bool hasSunk(Ship ship)
        {
            int pointsStruck = 0;
            foreach (var location in ship.getGridPoints()) 
            {
                if (KnownPoints.Contains(location))
                { 
                    pointsStruck++;
                }
            }
            var sunk = pointsStruck == ship.Length;
            return (sunk);
        }

        // Check how many ships are down, if a we notice a ship is down that wasnt down before, we acivate the event trigger.
        public int numberOfShipsDown()
        {
            int numberOfShipsDown = 0;
            // Add all locations that contain a ship to a collection.
            foreach (var ship in Ships) 
            {
                // We do not include cruise ships.
                if (ship.Code != 'C')
                {
                    if (hasSunk(ship)) 
                    {
                        numberOfShipsDown++;
                    }
                }
            }
            return numberOfShipsDown;
        }

        // Check if any ships are down, if a we notice a cruise ship is down, we acivate the event trigger.
        public bool cruiseShipStillSafe()
        {
            foreach (var ship in Ships)
            {
                // We type has to be cruise ship.
                if (ship.Code == 'C')
                { 
                    if (hasSunk(ship)) 
                    {
                        return false;    
                    }
                }
            }
            return true;     
        }

        public void subscribe(IObserver observer)
        {
            this.observers.Add(observer);
        }
    }
}
