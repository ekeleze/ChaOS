using System;

namespace NeoChaOS
{
    public static class Logger
    {
        public static void InfoLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.LightBlue;
            Console.Write("[ INFO ] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(str);
        }

        public static void SuccessLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[  OK  ] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(str);
        }

        public static void WarnLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[ WARN ] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(str);
        }

        public static void ErrorLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ FAIL ] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(str);

            for (;;);
        }
    }
}
