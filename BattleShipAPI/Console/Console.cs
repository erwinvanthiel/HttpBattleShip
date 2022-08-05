using NetConsole = System.Console;

namespace Alten.Academy.Jumpstart.Battleship.Console
{
    public class Console : IConsole
    {
        public void Clear()
        {
            NetConsole.Clear();
        }

        public string ReadLine()
        {
            return NetConsole.ReadLine();
        }

        public void WriteLine(string value)
        {
            NetConsole.WriteLine(value);
        }
    }
}
