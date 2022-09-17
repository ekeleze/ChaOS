using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        string usr = "usr";

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("Boot successful...\n\n");
            Console.WriteLine("Welcome to...");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Private beta 1.6");
            //var available_space = VFSManager.GetAvailableFreeSpace(@"0:\");
            //Console.WriteLine("Available Storage: " + available_space);
            Console.WriteLine("\nType \"help\" to get started!\n\n");
        }

        protected override void Run()
        {
            //Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
            //Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            string syspath = @"0:\system";
            string usrfile = "usrnam.txt";

            Console.Write(usr);
            Console.Write(" > ");
            var input_beforelower = Console.ReadLine();
            var input = input_beforelower.ToLower();

            if (input.Contains("help"))
            {
                if (!input.Contains("info"))
                {
                    Console.WriteLine("\nFunctions:");
                    Console.WriteLine(" help - Shows all functions.");
                    Console.WriteLine(" username - Allows you to use usernames, use the info command for more info.");
                    Console.WriteLine(" info function-here - Shows more detail about commands.");
                    Console.WriteLine(" credits - Shows all of the wonderful people that make ChaOS work.");
                    Console.WriteLine(" clear - Clears the screen.");
                    Console.WriteLine(" color - Changes text color, do 'color list' to list all colors.");
                    
                    Console.WriteLine("");
                }
            }

            if (input.Contains("username"))
            {
                if (!input.Contains("info"))
                {
                    if (input.Contains("current"))
                    {
                        Console.Write("\nCurrent username: ");
                        Console.Write(usr);
                        Console.Write("\n\n");
                    }

                    else
                    {
                        if (input.Contains("change"))
                        {
                            var text = input;
                            var start = text.IndexOf("\"") + 1;//add one to not include quote
                            var end = text.LastIndexOf("\"") - start;
                            var nur = text.Substring(start, end);
                            usr = nur;
                        }
                        else
                        {
                            Console.WriteLine("\nusername SubFunctions:\n current - shows current username.\n change \"username-here\" - changes current username.\n");
                        }
                    }
                }
            }

            if (input.Contains("info"))
            {
                if (input.Contains("help"))
                {
                    Console.WriteLine("\nFunction: help\nShows all functions.\n");
                }

                if (input.Contains("username"))
                {
                    Console.WriteLine("\nFunction: username\nSubFunctions: current, change");
                    Console.WriteLine("Allows you to change, or view the current username by using the SubFunctions.\n");
                }

                if (input.Contains("info info"))
                {
                    Console.WriteLine("\nFunction: info\nGives info on functions.\n");
                }

                if (input == "info")
                {
                    Console.WriteLine("\nPlease specify the function at the end of the \"info\" command.\n");
                }

                if (input.Contains("credits"))
                {
                    Console.WriteLine("\nFunction: credits\nShows all of the wonderful people that make ChaOS work.\n");
                }
            }

            if (input.Contains("credits"))
            {
                if (!input.Contains("info"))
                {
                    Console.WriteLine("\nCredits:\nekeleze - Owner\nMrDumbrava - Contributor\n");
                }
            }

            if (input.Contains("clear"))
            {
                Console.Clear();
            }

            #region color
            if (input.Contains("color"))
            {
                if (input.Contains("list"))
                {
                    Console.WriteLine("\nColor list\n black - Black with white background.\n");
                }

                if (input.Contains("black")) //Black
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Clear();
                }

                if (input.Contains("dark blue")) //Dark blue
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("dark green")) //Dark green
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("dark cyan")) //Dark cyan
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("dark gray")) //Dark gray
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("blue")) //Blue
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("green")) //Green
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("cyan")) //Cyan
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("dark red")) //Dark red
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("dark magenta")) //Dark magenta
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("dark yellow")) //Dark yellow
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("gray")) //Gray
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("red")) //red
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("magenta")) //Magenta
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("yellow")) //Yellow
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("white")) //White
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }

                if (input.Contains("test"))
                {
                    var OldColor = Console.ForegroundColor;
                    Console.WriteLine("");

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("1");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("2");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("3");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("4");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("5");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("6");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("7");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("8");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("9");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("10");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("11");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("12");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("13");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("14");

                    Console.ForegroundColor = OldColor;
                    Console.WriteLine("\ndone\n");
                }
            }

            
            #endregion

            if (input.Contains("i like goos"))
            {
                var OldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nFuck you, ChaOS is 10 times better.\n");
                Console.ForegroundColor = OldColor;
            }

            if (input.Contains("color goos"))
            {
                var OldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nLet's color GoOS red, shall we?\n");
                Console.ForegroundColor = OldColor;
            }
        }
    }
}