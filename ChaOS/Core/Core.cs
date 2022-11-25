using System;
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
        public static void ilog(string text) => clog(text, ConsoleColor.Gray);
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
        public static void Stop(Exception exc) {
            Draw.ScreenColor(DarkBlue, White);
            Console.CursorVisible = false;

            log("Your copy of ChaOS has crashed!\n");
            log("STOP: 0x" + exc.GetHashCode());
            log(exc.ToString());
            log("\nYou can restart");

            while (true) Console.ReadKey(true);
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