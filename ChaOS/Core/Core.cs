using System;
using System.Runtime.CompilerServices;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using static System.ConsoleColor;

namespace ChaOS {
    public class Core {
        //ChaOS Core
        public static void log(string text = null) => Console.WriteLine(text);
        public static void clog(string text, ConsoleColor color) {
            var OldColor = Console.ForegroundColor;
            var OldColorBack = Console.BackgroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = OldColor;
            Console.BackgroundColor = OldColorBack;
        }
        public static void warn(string text, short warnLevel = 0)
        {
            log(); ConsoleColor OldFore = Console.ForegroundColor; ConsoleColor OldBack = Console.BackgroundColor;
            if (warnLevel.Equals(0))
            {
                clog(text, Gray);
            }
            else if (warnLevel.Equals(1))
            {
                clog(text, DarkBlue);
            }
            else if (warnLevel.Equals(2))
            {
                clog(text, DarkYellow);
            }
            else if (warnLevel.Equals(3))
            {
                clog(text, DarkRed);
            }
            else if (warnLevel.Equals(4))
            {
                log(text);
            }
            else if (warnLevel > 5)
            {
                Stop(new Exception(text), warnLevel);
            }
            Draw.ScreenColor(OldBack, OldFore, false); log();
        }
        public static void write(string text) => Console.Write(text);
        public static void cwrite(string text, ConsoleColor color) {
            var OldColor = Console.ForegroundColor;
            var OldColorBack = Console.BackgroundColor;
            Console.ForegroundColor = color;
            write(text);
            Console.ForegroundColor = OldColor;
            Console.BackgroundColor = OldColorBack;
        }
        public static void Clear() => Console.Clear();
        public static void Stop(Exception exc, int code = 0) {
            ConsoleColor OldFore = Console.ForegroundColor; ConsoleColor OldBack = Console.BackgroundColor;

            Draw.ScreenColor(DarkBlue, White);
            Console.CursorVisible = false;

            log("Your copy of ChaOS has crashed!\n");
            log("*** STOP: 0x" + code + " ***");
            log("\nPress any key to continue");

            Console.ReadKey(true);

            Draw.ScreenColor(OldBack, OldFore);
            Console.CursorVisible = true;
        }
        public static string GetTime() { 
            return Convert.ToString(DateTime.Now.Hour) + ":" + Convert.ToString(DateTime.Now.Minute); }
        public static class Draw {
            public static void ScreenColor(ConsoleColor BackColor, ConsoleColor ForeColor, bool ClearScreen = true) {
                Console.ForegroundColor = ForeColor; Console.BackgroundColor = BackColor;
                if (ClearScreen) Clear();
            }
        }
    }
}