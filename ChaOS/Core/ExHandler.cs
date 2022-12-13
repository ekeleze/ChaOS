using System;
using static System.ConsoleColor;
using static ChaOS.Core;

namespace ChaOS {
    internal class ExHandler {
        public static void Crash(Exception exc) {
            ConsoleColor OldFore = Console.ForegroundColor; ConsoleColor OldBack = Console.BackgroundColor;
            Console.CursorVisible = true; SetScreenColor(DarkBlue, White);
            Console.CursorTop = 10; log("              ChaOS has hit a brick wall and died in the wreckage!\n");
            try { Console.CursorLeft = 40 - (exc.Message.Length / 2); } catch { Console.CursorLeft = 0; } write(exc.ToString() + "\n\n");
            write("                          Press any key to continue... ");
            Console.ReadKey(true); SetScreenColor(OldBack, OldFore);
        }
    }
}