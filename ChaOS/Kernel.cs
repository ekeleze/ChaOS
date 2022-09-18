using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.HAL.Drivers.PCI.Video;
using Cosmos.Core.IOGroup;
using Cosmos.System;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        string usr = "usr";
        readonly string ver = "Beta 1.8.1";
        VMWareSVGAII svga;
        bool gui = false;

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
            System.Console.Clear();
            System.Console.WriteLine("Boot successful...\n");
            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("\n" + ver);
            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds");

            //var available_space = VFSManager.GetAvailableFreeSpace(@"0:\");
            //Console.WriteLine("Available Storage: " + available_space);
            System.Console.WriteLine("\nType \"help\" to get started!\n");
        }

        protected override void Run()
        {
            if (gui)
            {
                UpdateSVGA();
                svga.SetPixel(MouseManager.X, MouseManager.Y, 0xFFFFFF);
                svga.SetPixel(MouseManager.X + 1, MouseManager.Y, 0xFFFFFF);
                svga.SetPixel(MouseManager.X + 2, MouseManager.Y, 0xFFFFFF);
                svga.SetPixel(MouseManager.X, MouseManager.Y + 1, 0xFFFFFF);
                svga.SetPixel(MouseManager.X, MouseManager.Y + 2, 0xFFFFFF);
                svga.SetPixel(MouseManager.X + 1, MouseManager.Y + 1, 0xFFFFFF);
                svga.SetPixel(MouseManager.X + 2, MouseManager.Y + 2, 0xFFFFFF);
                svga.SetPixel(MouseManager.X + 3, MouseManager.Y + 3, 0xFFFFFF);
                UpdateSVGA();
            }
            else
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
                    if (!input.Contains("info"))
                    {
                        System.Console.WriteLine("\nFunctions:");
                        System.Console.WriteLine(" help - Shows all functions.");
                        System.Console.WriteLine(" username - Allows you to use usernames, use the info command for more info.");
                        System.Console.WriteLine(" info function-here - Shows more detail about commands.");
                        System.Console.WriteLine(" credits - Shows all of the wonderful people that make ChaOS work.");
                        System.Console.WriteLine(" clear - Clears the screen.");
                        System.Console.WriteLine(" color - Changes text color, do 'color list' to list all colors.");
                        System.Console.WriteLine(" gui - Loads user graphics interface.");

                        System.Console.WriteLine("");
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
                                System.Console.WriteLine("\nusername SubFunctions:\n current - shows current username.\n change \"username-here\" - changes current username.\n");
                            }
                        }
                    }
                }

                if (input.Contains("info"))
                {
                    if (input.Contains("help"))
                    {
                        System.Console.WriteLine("\nFunction: help\nShows all functions.\n");
                    }

                    if (input.Contains("username"))
                    {
                        System.Console.WriteLine("\nFunction: username\nSubFunctions: current, change");
                        System.Console.WriteLine("Allows you to change, or view the current username by using the SubFunctions.\n");
                    }

                    if (input.Contains("info info"))
                    {
                        System.Console.WriteLine("\nFunction: info\nGives info on functions.\n");
                    }

                    if (input == "info")
                    {
                        System.Console.WriteLine("\nPlease specify the function at the end of the \"info\" command.\n");
                    }

                    if (input.Contains("credits"))
                    {
                        System.Console.WriteLine("\nFunction: credits\nShows all of the wonderful people that make ChaOS work.\n");
                    }
                }

                if (input.Contains("credits"))
                {
                    if (!input.Contains("info"))
                    {
                        System.Console.WriteLine("\nCredits:\nekeleze - Owner\nMrDumbrava - Contributor\n");
                    }
                }

                #region Color functions
                if (input.Contains("color"))
                {
                    if (input == "color")
                    {
                        var OldColor = System.Console.ForegroundColor;
                        var OldColorBack = System.Console.BackgroundColor;
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        System.Console.WriteLine("\nPlease do 'color list' to list colors\n");
                        System.Console.ForegroundColor = OldColor;
                        System.Console.BackgroundColor = OldColorBack;
                    }
                        if (input.Contains("list"))
                    {
                        var OldColor = System.Console.ForegroundColor;
                        var OldColorBack = System.Console.BackgroundColor;
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        System.Console.WriteLine("\nColor list");

                        System.Console.Write(" ");
                        System.Console.ForegroundColor = ConsoleColor.Black;
                        System.Console.BackgroundColor = ConsoleColor.White;
                        System.Console.Write("black - Black with white background\n");
                        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.WriteLine(" dark blue - Dark blue with black background");
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        System.Console.WriteLine(" dark green - Dark green with black background");
                        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Console.WriteLine(" dark cyan - Dark cyan with black background");
                        System.Console.ForegroundColor = ConsoleColor.DarkGray;
                        System.Console.WriteLine(" dark gray - Dark gray with black background");
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                        System.Console.WriteLine(" blue - Light blue with black background");
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.WriteLine(" green - Light green with black background");
                        System.Console.ForegroundColor = ConsoleColor.Cyan;
                        System.Console.WriteLine(" cyan - Light blue/cyan with black background");
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;
                        System.Console.WriteLine(" dark red - Dark red with black background");
                        System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        System.Console.WriteLine(" dark magenta - Dark magenta with black background");
                        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                        System.Console.WriteLine(" dark yellow - Orange/dark yellow/brown with black background");
                        System.Console.ForegroundColor = ConsoleColor.Gray;
                        System.Console.WriteLine(" gray - Light gray with black background");
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine(" red - Light red with black background");
                        System.Console.ForegroundColor = ConsoleColor.Magenta;
                        System.Console.WriteLine(" magenta - Light magenta with black background");
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        System.Console.WriteLine(" yellow - Light yellow with black background");
                        System.Console.ForegroundColor = ConsoleColor.White;
                        System.Console.WriteLine(" white - Pure white with black background\n");
                        System.Console.ForegroundColor = OldColor;
                        System.Console.BackgroundColor = OldColorBack;
                    }

                    if (input.Contains("black")) //Black
                    {
                        System.Console.ForegroundColor = ConsoleColor.Black;
                        System.Console.BackgroundColor = ConsoleColor.White;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark blue")) //Dark blue
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark green")) //Dark green
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark cyan")) //Dark cyan
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark gray")) //Dark gray
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkGray;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("blue")) //Blue
                        {
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            System.Console.Clear();
                            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            System.Console.WriteLine("\n" + ver);
                            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("green")) //Green
                        {
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            System.Console.Clear();
                            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            System.Console.WriteLine("\n" + ver);
                            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("cyan")) //Cyan
                        {
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            System.Console.Clear();
                            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            System.Console.WriteLine("\n" + ver);
                            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (input.Contains("dark red")) //Dark red
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark magenta")) //Dark magenta
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("dark yellow")) //Dark yellow
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("gray")) //Gray
                        {
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            System.Console.Clear();
                            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            System.Console.WriteLine("\n" + ver);
                            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("red")) //Red
                        {
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            System.Console.Clear();
                            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            System.Console.WriteLine("\n" + ver);
                            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("magenta")) //Magenta
                        {
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            System.Console.Clear();
                            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            System.Console.WriteLine("\n" + ver);
                            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (!input.Contains("dark"))
                    {
                        if (input.Contains("yellow")) //Yellow
                        {
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            System.Console.Clear();
                            System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            System.Console.WriteLine("\n" + ver);
                            System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    if (input.Contains("white")) //White
                    {
                        System.Console.ForegroundColor = ConsoleColor.White;
                        System.Console.BackgroundColor = ConsoleColor.Black;
                        System.Console.Clear();
                        System.Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                        System.Console.WriteLine("\n" + ver);
                        System.Console.WriteLine("Copyright 2022 (c) Kastle Grounds\n");
                    }

                    if (input.Contains("test"))
                    {
                        var OldColor = System.Console.ForegroundColor;
                        System.Console.WriteLine("");

                        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                        System.Console.WriteLine("1");
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        System.Console.WriteLine("2");
                        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Console.WriteLine("3");
                        System.Console.ForegroundColor = ConsoleColor.DarkGray;
                        System.Console.WriteLine("4");
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                        System.Console.WriteLine("5");
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.WriteLine("6");
                        System.Console.ForegroundColor = ConsoleColor.Cyan;
                        System.Console.WriteLine("7");
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;
                        System.Console.WriteLine("8");
                        System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        System.Console.WriteLine("9");
                        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                        System.Console.WriteLine("10");
                        System.Console.ForegroundColor = ConsoleColor.Gray;
                        System.Console.WriteLine("11");
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("12");
                        System.Console.ForegroundColor = ConsoleColor.Magenta;
                        System.Console.WriteLine("13");
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        System.Console.WriteLine("14");

                        System.Console.ForegroundColor = OldColor;
                        System.Console.WriteLine("\ndone\n");
                    }
                }
                #endregion

                #region Others

                if (input.Contains("clear"))
                {
                    System.Console.Clear();
                }

                if (input.Contains("i like goos"))
                {
                    var OldColor = System.Console.ForegroundColor;
                    System.Console.ForegroundColor = ConsoleColor.DarkRed;
                    System.Console.WriteLine("\nFuck you, ChaOS is 10 times better.\n");
                    System.Console.ForegroundColor = OldColor;
                }

                #endregion
            }
        }
    }
}