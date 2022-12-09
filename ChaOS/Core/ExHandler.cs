using System;
using static System.ConsoleColor;
using static ChaOS.Core;

namespace ChaOS {
    internal class ExHandler {
        public static void Crash(Exception exc) {
            try {
                ConsoleColor OldFore = Console.ForegroundColor; ConsoleColor OldBack = Console.BackgroundColor;
                Console.CursorVisible = true; SetScreenColor(DarkBlue, White);

                #region Sheesh
                log(); log(); log(); log(); log(); log(); log(); log(); log(); log();
                #endregion // Yup

                log("              ChaOS has hit a brick wall and died in the wreckage!\n");
                Console.CursorLeft = (80 / 2) - (exc.Message.Length / 2);
                write(exc.ToString() + "\n\n");
                write("                          Press any key to continue... ");

                Console.ReadKey(true); SetScreenColor(OldBack, OldFore);
            }
            catch { Console.SetCursorPosition(0, 0); log("Mmm, ChaOS has crashed has crashed!"); }
        }
    }
}
