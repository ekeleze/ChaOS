using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.Core.IOGroup;
using Cosmos.System;
using System.Threading;
using System.Threading.Tasks.Sources;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using Cosmos.HAL.BlockDevice.Registers;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        readonly string ver = "1.0.0 Prerelease 1";
        string usr = "usr";
        int r;
        bool gs;
        int item = 0;
        int command = 1;
        string dir = "0:\\";
        ConsoleKeyInfo cki;

        protected override void BeforeRun()
        {
            clear();

            #region Tip randomness
            Random rnd = new Random();
            r = rnd.Next(0, 4);
            r = rnd.Next(0, 4);
            r = rnd.Next(0, 4);
            #endregion

            log("Boot successful...\n");
            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
            System.Console.ForegroundColor = ConsoleColor.White;
            log("\n" + ver + "\nBuild 14\n" + "\nCopyright 2022 (c) Kastle Grounds" + "\nType \"help\" to get started!\n");

            CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            Directory.SetCurrentDirectory(dir);

            if (!File.Exists(@"0:\"))
            {
                //File.Create(@"0:\");
            }

            //var available_space = VFSManager.GetAvailableFreeSpace(@"0:\");
            //Console.WriteLine("Available Storage: " + available_space);
        }

        #region ChaOS Core
        protected void log(string text)
        {
            System.Console.WriteLine(text);
        }
        protected void colorlog(string text, ConsoleColor color)
        {
            var OldColor = System.Console.ForegroundColor;
            var OldColorBack = System.Console.BackgroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(text);
            System.Console.ForegroundColor = OldColor;
            System.Console.BackgroundColor = OldColorBack;
        }
        protected void clear()
        {
            System.Console.Clear();
        }
        protected void line()
        {
            System.Console.WriteLine("");
        }
        #endregion

        #region Abandoned GUI

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
            log("\n------SYSTEM INFO------");
            log("   ChaOS Beta 1.9.2    ");
            log(" Developement build 13 ");
            log("(c) Kastle Grounds 2022");
        }

        protected void Help()
        {
            log("\nFunctions:");
            log(" help - Shows all functions.");
            log(" username - Allows you to use usernames");
            log(" info  - Shows more detail about commands even tho it's broken");
            log(" credits - Shows all of the wonderful people that make ChaOS work.");
            log(" clear - Clears the screen.");
            log(" color - Changes text color, do 'color list' to list all colors.");
            log(" shutdown - Shuts down ChaOS");
            log(" boot - Reboots the system");
            log(" gs - See a test!");
            log("");
        }
        #endregion

        protected override void Run()
        {
            if (!gs)
            { 
                try
                {
                    //Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
                    //Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

                    dir = Directory.GetCurrentDirectory();
                    string userfile = "userfile.txt";

                    System.Console.Write(usr + " (" + dir + ")");
                    System.Console.Write(": ");
                    var input_beforelower = System.Console.ReadLine();
                    var input = input_beforelower.ToLower();

                    if (input.Contains("help"))
                    {
                        if (!input.Contains("info"))
                        {
                            colorlog("\nFunctions:", ConsoleColor.DarkGreen);

                            log(" help - Shows all functions.");
                            log(" username - Allows you to use usernames");
                            log(" info  - Shows more detail about commands even tho it's broken");
                            log(" credits - Shows all of the wonderful people that make ChaOS work.");
                            log(" clear - Clears the screen.");
                            log(" color - Changes text color, do 'color list' to list all colors.");
                            log(" gs - See a test!");
                            log(" tips - Get some tips");


                            colorlog("\nPower Functions:", ConsoleColor.DarkGreen);
                            log(" shutdown - Shuts down ChaOS");
                            log(" reboot - Reboots the system");

                            colorlog("\nDisk Functions:", ConsoleColor.DarkGreen);
                            log(" disk - Lists disk subcommands");
                            log(" diskinfo - Gives info about the disk");
                            log(" cd - Browses to folder, works as in Windows");
                            log(" dir - Lists files in current folder");
                            log(" mkdir - Makes folder, with dirname argument");
                            log(" mkfile - Makes file, with filename argument");
                            log(" deldir - Makes folder, with dirname argument");
                            log(" delfile - Makes folder, with filename argument");

                            line();
                        }
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
                                    log("\nusername SubFunctions:\n current - shows current username.\n change \"username-here\" - changes current username.\n");
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

                    else if (input.Contains("reboot"))
                    {
                        if (input == "reboot")
                        {
                            log("\nPlease insert the OS disc you want to boot and hit any key.");
                            System.Console.ReadKey();
                            Power.Reboot();
                        }
                    }

                    else if (input.Contains("tips"))
                    {
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        log("/// Tips & Tricks ///\n");
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        if (r < 2)
                        {
                            log("ChaOS is all loaded into RAM when booted, that means you can take the CD out of the computer and ChaOS will still run!\n");
                        }
                        else if (r == 2)
                        {
                            log("You can boot from other disks while ChaOS is loaded!\n");
                        }
                        else if (r > 2)
                        {
                            log("ChaOS is all loaded into RAM when booted, that means you can take the CD out of the computer and ChaOS will still run!\n");
                        }
                        System.Console.ForegroundColor = ConsoleColor.White;
                    }

                    else if (input.Contains("mkdir"))
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        potato = potato.Split("md ")[1];

                        if (!Directory.Exists(potato))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + potato));
                        }
                    }

                    else if (input.Contains("mkfile"))
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        potato = potato.Split("md ")[1];

                        if (!Directory.Exists(potato))
                        {
                            File.Create(Path.Combine(Directory.GetCurrentDirectory() + potato));
                        }
                    }

                    else if (input.Contains("deldir"))
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        potato = potato.Split("md ")[1];

                        if (!Directory.Exists(potato))
                        {
                            Directory.Delete(Path.Combine(Directory.GetCurrentDirectory() + potato));
                        }
                    }

                    else if (input.Contains("delfile"))
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        potato = potato.Split("md ")[1];

                        if (!Directory.Exists(potato))
                        {
                            File.Delete(Path.Combine(Directory.GetCurrentDirectory() + potato));
                        }
                    }

                    else if (input.Contains("cd"))
                    {
                        if (input == "cd..")
                        {
                            
                        }
                        else
                        {
                            var potato = input_beforelower;
                            if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                            potato = potato.Split("cd ")[1];

                            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + potato)))
                            {
                                dir = Path.Combine(Directory.GetCurrentDirectory(), potato);
                                Directory.SetCurrentDirectory(dir);
                            }
                        }
                    }

                    else if (input.Contains("dir"))
                    {
                        colorlog("\nDirectory listing at " + Directory.GetCurrentDirectory(), ConsoleColor.Yellow);
                        var directoryList = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(dir);
                        var files = 0;
                        foreach (var directoryEntry in directoryList)
                        {
                            colorlog(directoryEntry.mName, ConsoleColor.Gray);
                            files += 1;
                        }
                        colorlog("\nFound " + files + " files & directories", ConsoleColor.Yellow);
                    }

                    else if (input.Contains("disk"))
                    {
                        if (input.Contains("diskinfo"))
                        {
                            long availableSpace = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                            string fsType = Sys.FileSystem.VFS.VFSManager.GetFileSystemType("0:\\");
                            colorlog("\nAvailable Free Space: " + availableSpace / 1e+6 + " MB free", ConsoleColor.Yellow);
                            colorlog("\nFilesystem Type: " + fsType + "\n", ConsoleColor.Yellow);
                        }
                        else
                        {
                            log("\n? Invalid command syntax, do command 'help' for more info.\n");
                        }
                    }

                    #region Info commands

                    else if (input.Contains("info"))
                    {
                        if (input.Contains("help"))
                        {
                            log("\nFunction: help\nShows all functions.\n");
                        }

                        if (input.Contains("username"))
                        {
                            log("\nFunction: username\nSubFunctions: current, change");
                            log("Allows you to change, or view the current username by using the SubFunctions.\n");
                        }

                        if (input.Contains("info info"))
                        {
                            log("\nFunction: info\nGives info on functions.\n");
                        }

                        if (input == "info")
                        {
                            log("\nPlease specify the function at the end of the \"info\" command.\n");
                        }

                        if (input.Contains("credits"))
                        {
                            log("\nFunction: credits\nShows all of the wonderful people that make ChaOS work.\n");
                        }
                    }

                    else if (input.Contains("credits"))
                    {
                        if (!input.Contains("info"))
                        {
                            log("\nCredits:\nekeleze - Owner\nMrDumbrava - Contributor\n");
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
                            log("\nPlease do 'color list' to list colors\n");
                            System.Console.ForegroundColor = OldColor;
                            System.Console.BackgroundColor = OldColorBack;
                        }
                        if (input.Contains("list"))
                        {
                            var OldColor = System.Console.ForegroundColor;
                            var OldColorBack = System.Console.BackgroundColor;
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            log("\nColor list");

                            System.Console.Write(" ");
                            System.Console.ForegroundColor = ConsoleColor.Black;
                            System.Console.BackgroundColor = ConsoleColor.White;
                            System.Console.Write("black - Black with white background\n");
                            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            log(" dark blue - Dark blue with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            log(" dark green - Dark green with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                            log(" dark cyan - Dark cyan with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkGray;
                            log(" dark gray - Dark gray with black background");
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                            log(" blue - Light blue with black background");
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            log(" green - Light green with black background");
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            log(" cyan - Light blue/cyan with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            log(" dark red - Dark red with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            log(" dark magenta - Dark magenta with black background");
                            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                            log(" dark yellow - Orange/dark yellow/brown with black background");
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            log(" gray - Light gray with black background");
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            log(" red - Light red with black background");
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                            log(" magenta - Light magenta with black background");
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            log(" yellow - Light yellow with black background");
                            System.Console.ForegroundColor = ConsoleColor.White;
                            log(" white - Pure white with black background");
                            Thread.Sleep(100);
                            System.Console.ForegroundColor = OldColor;
                            System.Console.BackgroundColor = OldColorBack;
                            Thread.Sleep(100);
                            log("");
                            Thread.Sleep(100);
                        }

                        if (input.Contains("black")) //Black
                        {
                            System.Console.ForegroundColor = ConsoleColor.Black;
                            System.Console.BackgroundColor = ConsoleColor.White;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark blue")) //Dark blue
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark green")) //Dark green
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark cyan")) //Dark cyan
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark gray")) //Dark gray
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkGray;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("blue")) //Blue
                            {
                                System.Console.ForegroundColor = ConsoleColor.Blue;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("green")) //Green
                            {
                                System.Console.ForegroundColor = ConsoleColor.Green;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("cyan")) //Cyan
                            {
                                System.Console.ForegroundColor = ConsoleColor.Cyan;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("dark red")) //Dark red
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark magenta")) //Dark magenta
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark yellow")) //Dark yellow
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("gray")) //Gray
                            {
                                System.Console.ForegroundColor = ConsoleColor.Gray;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("red")) //Red
                            {
                                System.Console.ForegroundColor = ConsoleColor.Red;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("magenta")) //Magenta
                            {
                                System.Console.ForegroundColor = ConsoleColor.Magenta;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("yellow")) //Yellow
                            {
                                System.Console.ForegroundColor = ConsoleColor.Yellow;
                                System.Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("white")) //White
                        {
                            System.Console.ForegroundColor = ConsoleColor.White;
                            System.Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("test"))
                        {
                            var OldColor = System.Console.ForegroundColor;
                            log("");

                            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                            log("1");
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            log("2");
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                            log("3");
                            System.Console.ForegroundColor = ConsoleColor.DarkGray;
                            log("4");
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                            log("5");
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            log("6");
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            log("7");
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            log("8");
                            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            log("9");
                            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                            log("10");
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            log("11");
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            log("12");
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                            log("13");
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            log("14");

                            System.Console.ForegroundColor = OldColor;
                            log("\ndone\n");
                        }
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
                        log("\n? Unknown command.\n");
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
                    log("\n! An exception occured. " + e + command + "\n");
                    System.Console.Beep(880, 5);
                    System.Console.ForegroundColor = OldColor;
                    System.Console.BackgroundColor = OldBackground;
                }
                #endregion
            }
        }
    }
}