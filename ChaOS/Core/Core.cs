using System;
using static System.ConsoleColor;

namespace ChaOS
{
    public class Core
    {
        public static void log(string text = null) => Console.WriteLine(text);
        public static void clog(string text, ConsoleColor ForeColor)
        {
            var OldFore = Console.ForegroundColor;
            Console.ForegroundColor = ForeColor;
            Console.WriteLine(text);
            Console.ForegroundColor = OldFore;
        }
        public static void write(string text) => Console.Write(text);
        public static void cwrite(string text, ConsoleColor Color)
        {
            var OldColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            write(text);
            Console.ForegroundColor = OldColor;
        }

        public static void hadd(string text)
        {
            Kernel.HelpEntries.Add(text);
        }

        public static void SetScreenColor(ConsoleColor BackColor, ConsoleColor ForeColor, bool ClearScreen = true)
        {
            Console.BackgroundColor = BackColor; Console.ForegroundColor = ForeColor;
            if (ClearScreen) Console.Clear();
        }

        public static void Crash(Exception exc)
        {
            ConsoleColor OldFore = Console.ForegroundColor; ConsoleColor OldBack = Console.BackgroundColor;
            SetScreenColor(DarkBlue, White);
            Console.CursorTop = 10; log("              ChaOS has hit a brick wall and died in the wreckage!\n");
            if (39 - ((exc.Message + new string(' ', 80)).Substring(0, 80).Length / 2) > -1) Console.CursorLeft = 39 - ((exc.Message + new string(' ', 80)).Substring(0, 80).Length / 2);
            write(exc.ToString() + "\n\n");

            write("                          Press any key to continue... ");
            Console.ReadKey(true); SetScreenColor(OldBack, OldFore);
        }
    }
}