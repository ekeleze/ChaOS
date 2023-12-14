//using System;

using Sys = System;
using static ChaOS.Kernel;
using PrismAPI.Graphics;
using ConsoleColor = SVGAIIColor;
using static SVGAIIColor;


namespace ChaOS
{
    public class Core
    {
        public static void log(string text = null) => Console.WriteLine(text);
        public static void clog(string text, Color ForeColor)
        {
            var OldFore = Console.ForegroundColor;
            Console.ForegroundColor = ForeColor;
            Console.WriteLine(text);
            Console.ForegroundColor = OldFore;
        }
        public static void write(string text) => Console.Write(text);
        public static void cwrite(string text, Color Color)
        {
            var OldColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            write(text);
            Console.ForegroundColor = OldColor;
        }

        public static void hadd(string text)
        {
            //Kernel.HelpEntries.Add(text);
        }

        public static void SetScreenColor(Color BackColor, Color ForeColor, bool ClearScreen = true)
        {
            Console.BackgroundColor = BackColor; Console.ForegroundColor = ForeColor;
            if (ClearScreen) Console.Clear();
        }

        public static void Crash(Sys.Exception exc)
        {
            Color OldFore = Console.ForegroundColor; Color OldBack = Console.BackgroundColor;
            SetScreenColor(DarkBlue, White);
            Console.CursorY = 10; log("              ChaOS has hit a brick wall and died in the wreckage!\n");
            if (39 - ((exc.Message + new string(' ', 80)).Substring(0, 80).Length / 2) > -1) Console.CursorX = 39 - ((exc.Message + new string(' ', 80)).Substring(0, 80).Length / 2);
            write(exc + "\n\n");

            write("                          Press any key to continue... ");
            Console.ReadKey(true); SetScreenColor(OldBack, OldFore);
        }
    }
}