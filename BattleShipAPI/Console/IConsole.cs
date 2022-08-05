namespace Alten.Academy.Jumpstart.Battleship.Console
{
    /// <summary>
    /// Interface for abstracting away the <see cref="System.Console"/> class in order to improve testability of the application.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Reads the next line of characters from the standard input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="int.MaxValue"/>.</exception>
        /// <seealso cref="System.Console.ReadLine"/>
        string ReadLine();

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <see cref="System.Console.WriteLine(string)"/>
        void WriteLine(string value);

        /// <summary>
        /// Clears the console buffer and corresponding console window of display information.
        /// </summary>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <seealso cref="System.Console.Clear"/>
        void Clear();
    }
}
