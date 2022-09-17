using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.HAL.Drivers.PCI.Video;
using Cosmos.Core.IOGroup;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        string usr = "usr";
        readonly string ver = "Beta 1.8.1";
        VMWareSVGAII svga;
        bool gui = false;
        public uint mX = 100;
        public uint mY = 100;

        protected void LoadSVGA()
        {
            svga = new VMWareSVGAII();
            svga.SetMode(640, 480);
        }
        protected void UpdateSVGA()
        {
            svga.DoubleBufferUpdate();
        }

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("Boot successful...\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n" + ver);
            Console.WriteLine("Copyright 2022 (c) Kastle Grounds");

            //var available_space = VFSManager.GetAvailableFreeSpace(@"0:\");
            //Console.WriteLine("Available Storage: " + available_space);
            Console.WriteLine("\nType \"help\" to get started!\n");
        }

        protected override void Run()
        {
            if (gui)
            {
                svga.SetPixel(100, 100, 0xFFFFFF);
                svga.SetPixel(mX, mY, 0xFFFFFF);
                svga.SetPixel(mX + 1, mY, 0xFFFFFF);
                svga.SetPixel(mX + 2, mY, 0xFFFFFF);
                svga.SetPixel(mX, mY + 1, 0xFFFFFF);
                svga.SetPixel(mX, mY + 2, 0xFFFFFF);
                svga.SetPixel(mX + 1, mY + 1, 0xFFFFFF);
                svga.SetPixel(mX + 2, mY + 2, 0xFFFFFF);
                svga.SetPixel(mX + 3, mY + 3, 0xFFFFFF);
                UpdateSVGA();
            }
            else
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
                        Console.WriteLine(" gui - Loads user graphics interface.");

                        Console.WriteLine("");
                    }
                }

                if (input.Contains("gui"))
                {
                    gui = true;
                    LoadSVGA();
                    return;
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

                #region Color functions
                if (input.Contains("color"))
                {
                    if (input == "color")
                    {
                        var OldColor = Console.ForegroundColor;
                        var OldColorBack = Console.BackgroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nPlease do 'color list' to list colors\n");
                        Console.ForegroundColor = OldColor;
                        Console.BackgroundColor = OldColorBack;
                    }
                        if (input.Contains("list"))
                    {
                        var OldColor = Console.ForegroundColor;
                        var OldColorBack = Console.BackgroundColor;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nColor list");

                        Console.Write(" ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("black - Black with white background\n");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(" dark blue - Dark blue with black background");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(" dark green - Dark green with black background");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(" dark cyan - Dark cyan with black background");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(" dark gray - Dark gray with black background");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(" blue - Light blue with black background");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" green - Light green with black background");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(" cyan - Light blue/cyan with black background");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(" dark red - Dark red with black background");
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine(" dark magenta - Dark magenta with black background");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(" dark yellow - Orange/dark yellow/brown with black background");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(" gray - Light gray with black background");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" red - Light red with black background");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(" magenta - Light magenta with black background");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(" yellow - Light yellow with black background");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" white - Pure white with black background\n");
                        Console.ForegroundColor = OldColor;
                        Console.BackgroundColor = OldColorBack;
                    }

                    if (input.Contains("black")) //Black
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark blue")) //Dark blue
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark green")) //Dark green
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark cyan")) //Dark cyan
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark gray")) //Dark gray
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("blue")) //Blue
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            Console.WriteLine("\n" + ver);
                            Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("green")) //Green
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            Console.WriteLine("\n" + ver);
                            Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("cyan")) //Cyan
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            Console.WriteLine("\n" + ver);
                            Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (input.Contains("dark red")) //Dark red
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark magenta")) //Dark magenta
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark yellow")) //Dark yellow
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("gray")) //Gray
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            Console.WriteLine("\n" + ver);
                            Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("red")) //Red
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            Console.WriteLine("\n" + ver);
                            Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("magenta")) //Magenta
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            Console.WriteLine("\n" + ver);
                            Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("yellow")) //Yellow
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            Console.WriteLine("\n" + ver);
                            Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (input.Contains("white")) //White
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        Console.WriteLine("\n" + ver);
                        Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
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

                #region Others

                if (input.Contains("home"))
                {
                    Console.Clear();
                }

                if (input.Contains("i like goos"))
                {
                    var OldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nFuck you, ChaOS is 10 times better.\n");
                    Console.ForegroundColor = OldColor;
                }

                #endregion
            }
        }
    }
}