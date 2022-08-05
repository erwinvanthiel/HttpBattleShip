using System.Text;

namespace BattleShipAPI.Data
{
    public class LevelFileCreator
    {
        private static List<string> types = new List<string> {
        "Aircraft Carrier","Cruise Ship","Patrol Boat","Submarine","Battleship","Destroyer"
        };

        private static string vertical_locations = "ABCDEFGHI";

        private static string horizontal_locations = "123456789";

        public static void CreateLevelFile()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "ships.csv";

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Create a new file     
            using (FileStream fs = File.Create(fileName))
            {

                // Parse 5 ships and write them to the csv
                int numberOfShips = 0;
                while (numberOfShips < 1)
                {

                    // Parse a ship
                    System.Console.WriteLine("Whats the name of the ship");
                    var name = System.Console.ReadLine();

                    var type = "#";
                    while (!types.Contains(type))
                    {
                        System.Console.WriteLine("Whats the ship type? Choose from: Aircraft Carrier-Cruise Ship-Patrol Boat-Submarine-Battleship-Destroyer");
                        type = System.Console.ReadLine();
                    }

                    var x = "#";
                    while (!horizontal_locations.Contains(x))
                    {
                        System.Console.WriteLine("Whats horizontal location of the ship? Choose from: 1 2 3 4 5 6 7 8 9");
                        x = System.Console.ReadLine();
                    }

                    var y = "#";
                    while (!vertical_locations.Contains(y))
                    {
                        System.Console.WriteLine("Whats vertical location of the ship? Choose from: A B C D E F G H I");
                        y = System.Console.ReadLine();
                    }

                    var alignment = "#";
                    while (!(alignment == "Horizontal" || alignment == "Vertical"))
                    {
                        System.Console.WriteLine("Whats alignment of the ship? Choose from: Vertical-Horizontal");
                        alignment = System.Console.ReadLine();
                    }

                    // Write the ship to csv
                    var comma_delimited_ship = name + "," + type + "," + x + "," + y + "," + alignment+"\n";
                    Byte[] ship = new UTF8Encoding(true).GetBytes(comma_delimited_ship);
                    fs.Write(ship, 0, ship.Length);

                    numberOfShips++;
                }
            }
        }
    }
}
