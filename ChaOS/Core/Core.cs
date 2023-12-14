//using System;

using Sys = System;
using PrismAPI.Graphics;
using static ChaOS.Kernel;
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

        public static void SetScreenColor(Color BackColor, Color ForeColor, bool ClearScreen = true)
        {
            Console.BackgroundColor = BackColor; Console.ForegroundColor = ForeColor;
            if (ClearScreen) Console.Clear();
        }

        public const string CrashMessage = "ChaOS has hit a brick wall and died in the wreckage!";

        public static void Crash(Sys.Exception exc)
        {
            Color OldFore = Console.ForegroundColor; Color OldBack = Console.BackgroundColor;
            SetScreenColor(DarkBlue, White);

            string[] msg = { CrashMessage, "\n", exc.ToString(), "\n", "Press any key to continue..." };

            for (int i = 0; i < msg.Length; i++)
            {
                Console.Contents.DrawString((Console.Contents.Width / 2) - (msg[i].Length * 8 / 2), (Console.Contents.Height / 2) - (msg.Length / 2) + (i * 16), msg[i], Font_1x, Color.White);
            }

            Console.ReadKey(true);
            SetScreenColor(OldBack, OldFore);
        }
    }
}