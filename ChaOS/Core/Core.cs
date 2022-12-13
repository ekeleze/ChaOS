using System;
using System.Threading;

namespace ChaOS {
    public class Core {
        // ChaOS Core
        public static void log(string text = null) => Console.WriteLine(text);
        public static void clog(string text, ConsoleColor ForeColor) {
            var OldFore = Console.ForegroundColor;
            Console.ForegroundColor = ForeColor;
            Console.WriteLine(text);
            Console.ForegroundColor = OldFore;
        }
        public static void write(string text) => Console.Write(text);
        public static void cwrite(string text, ConsoleColor Color) {
            var OldColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            write(text);
            Console.ForegroundColor = OldColor;
        }

        public static void SetScreenColor(ConsoleColor BackColor, ConsoleColor ForeColor, bool ClearScreen = true) {
            Console.BackgroundColor = BackColor; Console.ForegroundColor = ForeColor;
            if (ClearScreen) Console.Clear();
        }
    }
}