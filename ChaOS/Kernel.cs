using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.Core.IOGroup;
using Cosmos.System;
using System.Threading;
using System.Threading.Tasks.Sources;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        string usr = "usr";
        readonly string ver = "Beta 1.9.3";
        int r;
        bool gs;
        int item = 0;
        ConsoleKeyInfo cki;

        protected override void BeforeRun()
        {
            #region Tip randomness
            Random rnd = new Random();
            r = rnd.Next(0, 4);
            r = rnd.Next(0, 4);
            r = rnd.Next(0, 4);
            #endregion

            clear();
            print("Boot successful...\n");
            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
            System.Console.ForegroundColor = ConsoleColor.White;
            print("\n" + ver);
            print("Dev build 13\n");
            print("Copyright 2022 (c) Kastle Grounds");

            //var available_space = VFSManager.GetAvailableFreeSpace(@"0:\");
            //Console.WriteLine("Available Storage: " + available_space);
            print("\nType \"help\" to get started!\n");
            ShowTips();
        }

        #region Tips & Tricks
        protected void ShowTips()
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            print("/// Tips & Tricks ///\n");
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            if (r < 2)
            {
                print("ChaOS is all loaded into RAM when booted, that means you can take the CD out of the computer and ChaOS will still run!\n");
            }
            else if (r == 2)
            {
                print("You can boot from other disks while ChaOS is loaded!\n");
            }
            else if (r > 2)
            {
                print("ChaOS is all loaded into RAM when booted, that means you can take the CD out of the computer and ChaOS will still run!\n");
            }
            System.Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion

        #region ChaOS Core
        protected void print(string text)
        {
            System.Console.WriteLine(text);
        }
        protected void clear()
        {
            System.Console.Clear();
        }
        #endregion

        protected void RewriteMenu(string ItemText)
        {
            System.Console.BackgroundColor = ConsoleColor.Cyan;
            System.Console.ForegroundColor = ConsoleColor.White;
            clear();
            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write(ItemText);
            System.Console.CursorLeft = ItemText.Length;
            System.Console.Write("                                                                                           ");
            System.Console.CursorLeft = 72;
            System.Console.CursorTop = 0;
            System.Console.Write("Press ESC To Exit");
            System.Console.CursorLeft = 0;
            System.Console.BackgroundColor = ConsoleColor.Cyan;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.CursorTop = 1;
            System.Console.Write("                                                                                         ");
            System.Console.CursorLeft = 0;
            System.Console.CursorTop = 1;
        }
        protected void InitializeGS()
        {
            System.Console.SetWindowSize(90, 30);
            RewriteMenu("System Info");
            SystemInfo();
            while (true) 
            {
                cki = System.Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Tab)
                {
                    print("IT WORKS");
                    item += 1;
                    if (item == 2)
                        item = 0;

                    if (item == 0)
                    {
                        RewriteMenu("Help");
                        Help();
                    }
                    if (item == 1)
                    {
                        RewriteMenu("System Info");
                        SystemInfo();
                    }
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    Power.Reboot();
                }
            }
        }

        protected void SystemInfo()
        {
            print("\n------SYSTEM INFO------");
            print("   ChaOS Beta 1.9.2    ");
            print(" Developement build 13 ");
            print("(c) Kastle Grounds 2022");
        }

        protected void Help()
        {
            print("\nFunctions:");
            print(" help - Shows all functions.");
            print(" username - Allows you to use usernames");
            print(" info  - Shows more detail about commands even tho it's broken");
            print(" credits - Shows all of the wonderful people that make ChaOS work.");
            print(" clear - Clears the screen.");
            print(" color - Changes text color, do 'color list' to list all colors.");
            print(" shutdown - Shuts down ChaOS");
            print(" boot - Reboots the system");
            print(" gs - See a test!");
            print("");
        }
        protected override void Run()
        {
            if (gs == true) {}

            else
            { 
                try
                {
                    //Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
                    //Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

                    string syspath = @"0:\system";
                    string usrfile = "usrnam.txt";

                    System.Console.Write(usr);
                    System.Console.Write(" > ");
                    var input_beforelower = System.Console.ReadLine();
                    var input = input_beforelower.ToLower();

                    if (input.Contains("help"))
                    {
                        if (!input.Contains("info")) Help();
                    }

                    else if (input.Contains("username"))
                    {
                        if (!input.Contains("info"))
                        {
                            if (input.Contains("current"))
                            {
                                System.Console.Write("\nCurrent username: ");
                                System.Console.Write(usr);
                                System.Console.Write("\n\n");
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
                                    print("\nusername SubFunctions:\n current - shows current username.\n change \"username-here\" - changes current username.\n");
                                }
                            }
                        }
                    }

                    else if (input.Contains("gs"))
                    {
                        gs = true;
                        InitializeGS();
                    }

                    else if (input.Contains("clear"))
                    {
                        clear();
                    }

                    else if (input.Contains("shutdown"))
                    {
                        Power.Shutdown();
                    }

                    else if (input.Contains("boot"))
                    {
                        if (input == "boot")
                        {
                            print("\nPlease insert the OS disc you want to boot and hit any key.");
                            System.Console.ReadKey();
                            Power.Reboot();
                        }
                    }

                    #region Info commands

                    else if (input.Contains("info"))
                    {
                        if (input.Contains("help"))
                        {
                            print("\nFunction: help\nShows all functions.\n");
                        }

                        if (input.Contains("username"))
                        {
                            print("\nFunction: username\nSubFunctions: current, change");
                            print("Allows you to change, or view the current username by using the SubFunctions.\n");
                        }

                        if (input.Contains("info info"))
                        {
                            print("\nFunction: info\nGives info on functions.\n");
                        }

                        if (input == "info")
                        {
                            print("\nPlease specify the function at the end of the \"info\" command.\n");
                        }

                        if (input.Contains("credits"))
                        {
                            print("\nFunction: credits\nShows all of the wonderful people that make ChaOS work.\n");
                        }
                    }

                    else if (input.Contains("credits"))
                    {
                        if (!input.Contains("info"))
                        {
                            print("\nCredits:\nekeleze - Owner\nMrDumbrava - Contributor\n");
                        }
                    }

                    #endregion

                    #region Color functions
                    else if (input.Contains("color"))
                    {
                        if (input == "color")
                        {
                            var OldColor = System.Console.ForegroundColor;
                            var OldColorBack = System.Console.BackgroundColor;
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            print("\nPlease do 'color list' to list colors\n");
                            System.Console.ForegroundColor = OldColor;
                            System.Console.BackgroundColor = OldColorBack;
                        }
                        if (input.Contains("list"))
                        {
                            var OldColor = System.Console.ForegroundColor;
                            var OldColorBack = System.Console.BackgroundColor;
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            print("\nColor list");

                            System.Console.Write(" ");
                            System.Console.ForegroundColor = ConsoleColor.Black;
                            System.Console.BackgroundColor = ConsoleColor.White;
                            System.Console.Write("black - Black with white background\n");
                            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            print(" dark blue - Dark blue with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            print(" dark green - Dark green with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                            print(" dark cyan - Dark cyan with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkGray;
                            print(" dark gray - Dark gray with black background");
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                            print(" blue - Light blue with black background");
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            print(" green - Light green with black background");
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            print(" cyan - Light blue/cyan with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            print(" dark red - Dark red with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            print(" dark magenta - Dark magenta with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                            print(" dark yellow - Orange/dark yellow/brown with black background");
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            print(" gray - Light gray with black background");
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            print(" red - Light red with black background");
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                            print(" magenta - Light magenta with black background");
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            print(" yellow - Light yellow with black background");
                            System.Console.ForegroundColor = ConsoleColor.White;
                            print(" white - Pure white with black background");
                            Thread.Sleep(100);
                            System.Console.ForegroundColor = OldColor;
                            System.Console.BackgroundColor = OldColorBack;
                            Thread.Sleep(100);
                            print("");
                            Thread.Sleep(100);
                        }

                        if (input.Contains("black")) //Black
                        {
                            System.Console.ForegroundColor = ConsoleColor.Black;
                            System.Console.BackgroundColor = ConsoleColor.White;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark blue")) //Dark blue
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark green")) //Dark green
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark cyan")) //Dark cyan
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark gray")) //Dark gray
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkGray;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("blue")) //Blue
                            {
                                System.Console.ForegroundColor = ConsoleColor.Blue;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                print("\n" + ver);
                                print("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("green")) //Green
                            {
                                System.Console.ForegroundColor = ConsoleColor.Green;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                print("\n" + ver);
                                print("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("cyan")) //Cyan
                            {
                                System.Console.ForegroundColor = ConsoleColor.Cyan;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                print("\n" + ver);
                                print("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("dark red")) //Dark red
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark magenta")) //Dark magenta
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark yellow")) //Dark yellow
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("gray")) //Gray
                            {
                                System.Console.ForegroundColor = ConsoleColor.Gray;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                print("\n" + ver);
                                print("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("red")) //Red
                            {
                                System.Console.ForegroundColor = ConsoleColor.Red;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                print("\n" + ver);
                                print("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("magenta")) //Magenta
                            {
                                System.Console.ForegroundColor = ConsoleColor.Magenta;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                print("\n" + ver);
                                print("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("yellow")) //Yellow
                            {
                                System.Console.ForegroundColor = ConsoleColor.Yellow;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                print("\n" + ver);
                                print("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("white")) //White
                        {
                            System.Console.ForegroundColor = ConsoleColor.White;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            print("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            print("\n" + ver);
                            print("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("test"))
                        {
                            var OldColor = System.Console.ForegroundColor;
                            print("");

                            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                            print("1");
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            print("2");
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                            print("3");
                            System.Console.ForegroundColor = ConsoleColor.DarkGray;
                            print("4");
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                            print("5");
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            print("6");
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            print("7");
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            print("8");
                            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            print("9");
                            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                            print("10");
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            print("11");
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            print("12");
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                            print("13");
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            print("14");

                            System.Console.ForegroundColor = OldColor;
                            print("\ndone\n");
                        }
                    }

                    #endregion

                    #region Others
                    else if (input.Contains("i like goos"))
                    {
                        var OldColor = System.Console.ForegroundColor;
                        var OldBackground = System.Console.BackgroundColor;
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;
                        print("\nFuck you, ChaOS is 10 times better.\n");
                        System.Console.ForegroundColor = OldColor;
                        System.Console.BackgroundColor = OldBackground;
                    }
                    #endregion

                    #region Unknown command handling

                    else
                    {
                        var OldColor = System.Console.ForegroundColor;
                        var OldBackground = System.Console.BackgroundColor;
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Beep(880, 5);
                        print("\nUnknown command.\n");
                        System.Console.ForegroundColor = OldColor;
                        System.Console.BackgroundColor = OldBackground;
                    }

                    #endregion
                }

                #region Exception catching

                catch (Exception e)
                {
                    var OldColor = System.Console.ForegroundColor;
                    var OldBackground = System.Console.BackgroundColor;
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    print("\nSorry, an error occured while running command.\n" + e + "\n");
                    System.Console.Beep(880, 5);
                    System.Console.ForegroundColor = OldColor;
                    System.Console.BackgroundColor = OldBackground;
                }
                #endregion
            }
        }
    }
}