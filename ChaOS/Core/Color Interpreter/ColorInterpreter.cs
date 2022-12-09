using System;
using static System.ConsoleColor;

namespace ChaOS.ColorInterpreter {
    public class Convert {
        public static ConsoleColor? ToConsoleColor(string Color) {
            if (Color == "Black") return Black;
            else if (Color == "DarkBlue") return DarkBlue;
            else if (Color == "DarkCyan") return DarkCyan;
            else if (Color == "DarkGreen") return DarkGreen;
            else if (Color == "DarkGray") return DarkGray;
            else if (Color == "Blue") return Blue;
            else if (Color == "Green") return Green;
            else if (Color == "Cyan") return Cyan;
            else if (Color == "DarkRed") return DarkRed;
            else if (Color == "DarkMagenta") return DarkMagenta;
            else if (Color == "DarkYellow") return DarkYellow;
            else if (Color == "Gray") return Gray;
            else if (Color == "Red") return Red;
            else if (Color == "Magenta") return Magenta;
            else if (Color == "Yellow") return Yellow;
            else if (Color == "White") return White;
            else return null;
        }
    }
}
