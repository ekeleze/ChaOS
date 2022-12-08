using System;

namespace ChaOS {
    public class Core {
        // ChaOS Core
        public static void log(string TEXT = null) => Console.WriteLine(TEXT);
        public static void clog(string TEXT, ConsoleColor ForeColor) {
            var OldFore = Console.ForegroundColor;
            Console.ForegroundColor = ForeColor;
            Console.WriteLine(TEXT);
            Console.ForegroundColor = OldFore;
        }
        public static void write(string TEXT) => Console.Write(TEXT);
        public static void cwrite(string TEXT, ConsoleColor COLOR) {
            var OldColor = Console.ForegroundColor;
            Console.ForegroundColor = COLOR;
            write(TEXT);
            Console.ForegroundColor = OldColor;
        }

        public static void SetScreenColor(ConsoleColor BackColor, ConsoleColor ForeColor, bool ClearScreen = true) {
            Console.BackgroundColor = BackColor; Console.ForegroundColor = ForeColor;
            if (ClearScreen) Console.Clear();
        }
    }
}