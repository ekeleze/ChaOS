using System;
using System.IO;
using Cosmos.System.FileSystem;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem.VFS;
using Cosmos.HAL.Drivers.PCI.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.Core.Memory;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        //GUI variables

        //Readonly
        readonly string ver = "1.0.0 Prerelease 8";
        readonly string systempath = @"0:\SYSTEM";
        readonly string userfile = @"0:\SYSTEM\userfile.sys";
        readonly string diskfile = @"0:\disk.hid";
        readonly static string root = @"0:";

        //Not readonly
        string usr = "usr";
        public static string dir = root;
        public static bool gui = false;
        bool disk;
        string input;
        string input_beforelower;
        protected override void BeforeRun()
        {
            //Early initialization

            log(Cosmos.Core.CPU.GetAmountOfRAM() + "MB System RAM OK");

            CosmosVFS fs = new CosmosVFS();
            VFSManager.RegisterVFS(fs);

            try
            {
                var temp = fs.GetTotalSize(root);
                Directory.SetCurrentDirectory(dir);
                disk = true;
            }
            catch
            {
                disk = false;
            }

            //Initialization

            if (disk)
            {
                if (!Directory.Exists(systempath))
                {
                    Directory.CreateDirectory(systempath);
                }
                else
                {
                    if (File.Exists(userfile))
                    {
                        usr = File.ReadAllText(userfile);
                    }
                    else if (!File.Exists(userfile))
                    {
                        File.Create(userfile);
                        File.WriteAllText(userfile, usr);
                    }
                    if (!File.Exists(diskfile))
                    {
                        File.Create(diskfile);
                        File.WriteAllText(diskfile, "Default");
                    }
                }
            }

            Sys.MouseManager.ScreenWidth = 640;
            Sys.MouseManager.ScreenHeight = 480;

            //Boot message

            clear();
            log("Boot successful!");
            clog("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ", ConsoleColor.DarkGreen);
            log("\n" + ver + "\nCopyright 2022 (c) Kastle Grounds\nType \"help\" to get started!");
            if (!disk)
            {
                log("No internal hard drive detected, ChaOS will continue in ClassiChaOS mode!");
            }
            line();
        }

        #region ChaOS Core
        protected void log(string text) //Log commmand for writing line
        {
            Console.WriteLine(text);
        }
        protected void clog(string text, ConsoleColor color) //Color log for writing color line
        {
            var OldColor = Console.ForegroundColor;
            var OldColorBack = Console.BackgroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = OldColor;
            Console.BackgroundColor = OldColorBack;
        }
        protected void ilog(string text) //ilog
        {
            clog("! " + text, ConsoleColor.Gray);
        }
        protected void write(string text) //Log commmand for writing line
        {
            Console.Write(text);
        }
        protected void cwrite(string text, ConsoleColor color) //Log command for only writing
        {
            var OldColor = Console.ForegroundColor;
            var OldColorBack = Console.BackgroundColor;
            Console.ForegroundColor = color;
            write(text);
            Console.ForegroundColor = OldColor;
            Console.BackgroundColor = OldColorBack;
        }

        protected void clear()
        {
            Console.Clear();
        }
        protected void line()
        {
            Console.WriteLine("");
        }
        #endregion

        protected override void Run()
        {
            if (gui)
            {
                #region GUI
                int x = 50; int y = 50;

                WM.ChaBar();

                WM.Window(x, y, 200, 160, "Test Window - ChaOS");
                WM.Text(x, y, 1, "This is the current stage");
                WM.Text(x, y, 2, "of ChaOS's GUI!");
                WM.Text(x, y, 3, "Note: it's very, very");
                WM.Text(x, y, 4, "glitchy!");

                Pen pen = new Pen(Color.Black);
                WM.canvas.DrawFilledRectangle(pen, (int)Sys.MouseManager.X, (int)Sys.MouseManager.Y, 2, 2);

                WM.canvas.Display();
                WM.ClearCanvas();
                Heap.Collect();
                #endregion
            }
            else
            {
                try
                {
                    if (disk)
                    {
                        write(usr + " (" + Directory.GetCurrentDirectory() + "\\" + ")");
                        write(": ");
                    }
                    else if (!disk)
                    {
                        write(usr + " > ");
                    }
                    input_beforelower = Console.ReadLine();
                    input = input_beforelower.ToLower();

                    if (input.Equals("help"))
                    {
                        if (disk)
                            clog("\nFunctions:", ConsoleColor.DarkGreen);
                        else
                            clog("\nFunctions (ClassiChaOS Mode):", ConsoleColor.DarkGreen);
                        log(" help - Shows all functions");
                        log(" username - Allows you to use usernames");
                        log(" info  - Shows more detail about commands");
                        log(" credits - Shows all of the wonderful people that make ChaOS work");
                        log(" cls/clear - Clears the screen");
                        log(" color - Changes text color, do 'color list' to list all colors");
                        log(" gui - See a test!");
                        log(" t/time - Tells you the time");
                        log(" echo - Echoes what you say");
                        log(" sd/shutdown - Shuts down ChaOS");
                        log(" rb/reboot - Reboots the system");
                        if (disk)
                        {
                            log(" disk - Gives info about the disk");
                            log(" cd - Browses to folder, works as in MS-DOS");
                            log(" cd.. - Returns to root");
                            log(" dir - Lists files in the current folder");
                            log(" mkdir - Makes folder, with dirname argument");
                            log(" mkfile - Makes file, with filename argument");
                            log(" deldir - Deletes folder, with dirname argument");
                            log(" delfile - Deletes file, with filename argument");
                            log(" open - Opens file. Supported formats: .txt .sys .wav");
                            log(" lb - Relabels disk");
                            log(" notepad - Opens MIV notepad.\n");
                        }
                    }

                    //Username commands

                    else if (input.Contains("username") && !input.Contains(".sys"))
                    {
                        cwrite("\nCurrent username:", ConsoleColor.Gray);
                        cwrite(File.ReadAllText(userfile), ConsoleColor.Gray);
                        write("\n\n");

                        if (input.Contains("change"))
                        {
                            var text = input;
                            var start = text.IndexOf(" ") + 1; //Add one to not include quote
                            var nur = text.Substring(start);
                            usr = nur;

                            if (disk && File.Exists(userfile))
                            {
                                try { File.WriteAllText(userfile, usr); ilog("Username changed successfully\n"); } catch { }
                            }
                            else
                            {
                                ilog("Username changed successfully\n");
                            }
                        }
                    }

                    //CrEdItS!

                    else if (input.Equals("credits"))
                    {
                        line();
                        cwrite("C", ConsoleColor.Blue);
                        cwrite("r", ConsoleColor.Green);
                        cwrite("e", ConsoleColor.DarkYellow);
                        cwrite("d", ConsoleColor.Red);
                        cwrite("i", ConsoleColor.DarkMagenta);
                        cwrite("t", ConsoleColor.Cyan);
                        cwrite("s", ConsoleColor.DarkRed);
                        cwrite(":\n", ConsoleColor.Blue);
                        clog(" ekeleze - Creator of ChaOS", ConsoleColor.Yellow);
                        clog(" MrDumbrava - Contributor & spanish language writer", ConsoleColor.Yellow);
                        clog(" Retronics - German language writer", ConsoleColor.Yellow);
                        clog(" 0xRage - Portuguese language writer", ConsoleColor.Yellow);
                        clog(" Owen2k6 - Japanese language writer", ConsoleColor.Yellow);
                        clog(" mariobot128 - French language writer", ConsoleColor.Yellow);
                        line();
                    }

                    //Misc

                    #region Color functions
                    else if (input.Contains("color") && !input.Contains("."))
                    {
                        if (input == "color")
                        {
                            ilog("Please write \"color list\" to list colors");
                        }
                        if (input.Contains("list"))
                        {
                            var OldColor = Console.ForegroundColor;
                            var OldColorBack = Console.BackgroundColor;
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            clog("\nPlease write \"color list\" to list colors", ConsoleColor.Green);

                            write(" ");
                            Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.White; write("\nblack - Black with white background");
                            Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.DarkBlue;
                            log(" dark blue - Dark blue with black background");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            log(" dark green - Dark green with black background");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            log(" dark cyan - Dark cyan with black background");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            log(" dark gray - Dark gray with black background");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            log(" blue - Normal blue with black background");
                            Console.ForegroundColor = ConsoleColor.Green;
                            log(" green - Green with black background");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            log(" cyan - Cyan with black background");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            log(" dark red - Dark red with black background");
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            log(" dark magenta - Dark magenta with black background");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            log(" dark yellow - Dark yellow/orange with black background");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            log(" gray - Gray with black background");
                            Console.ForegroundColor = ConsoleColor.Red;
                            log(" red - Red with black background"); ;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            log(" magenta - Magenta with black background");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            log(" yellow - Light yellow with black background");
                            Console.ForegroundColor = ConsoleColor.White;
                            log(" white - Pure white with black background");
                            Console.ForegroundColor = OldColor;
                            Console.BackgroundColor = OldColorBack;
                            line();
                        }

                        if (input.Contains("black")) //Black
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark blue")) //Dark blue
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark green")) //Dark green
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark cyan")) //Dark cyan
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark gray")) //Dark gray
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("blue")) //Blue
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("ChaOS v" + ver);
                                
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("green")) //Green
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("ChaOS v" + ver);
                                
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("cyan")) //Cyan
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("ChaOS v" + ver);
                                
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("dark red")) //Dark red
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark magenta")) //Dark magenta
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark yellow")) //Dark yellow
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("gray")) //Gray
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("ChaOS v" + ver);
                                
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("red")) //Red
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("ChaOS v" + ver);
                                
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("magenta")) //Magenta
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("ChaOS v" + ver);
                                
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("yellow")) //Yellow
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("ChaOS v" + ver);
                                
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("white")) //White
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("ChaOS v" + ver);
                            
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }
                    }

                    #endregion

                    else if (input.Contains("gui"))
                        WM.InitGUI();

                    else if (input.Contains("clear") || input.Contains("cls"))
                        clear();

                    else if (input == "time" || input == "t")
                        clog("\nCurrent time is " + Convert.ToString(Cosmos.HAL.RTC.Hour) + ":" + Convert.ToString(Cosmos.HAL.RTC.Minute) + ":" + Convert.ToString(Cosmos.HAL.RTC.Second) + "\n", ConsoleColor.Yellow);

                    //Power stuff

                    else if (input.Contains("shutdown") || input.Contains("sd"))
                    {
                        clog("\nShutting down...", ConsoleColor.Gray);
                        Sys.Power.Shutdown();
                    }

                    else if (input.Contains("reboot") || input.Contains("rb"))
                    {
                        clog("\nRestarting...", ConsoleColor.Gray);
                        Sys.Power.Reboot();
                    }

                    //Disk and filesystem stuff

                    else if (input.Contains("mkdir") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace("0:\\", ""); }

                        try
                        {
                            potato = potato.Split("mkdir ")[1];
                            potato = "\\" + potato;
                        }
                        catch
                        {
                            ilog("No arguments");
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy" || potato.Contains("."))
                        {
                            ilog("Name is reserved or contains file extension.");
                        }
                        else if (!Directory.Exists(potato))
                        {
                            Directory.CreateDirectory(Directory.GetCurrentDirectory() + potato);
                        }
                        else if (Directory.Exists(potato))
                        {
                            ilog("Directory already exists");
                        }
                    }

                    else if (input.Contains("mkfile") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace("0:\\", ""); }
                        try
                        {
                            potato = potato.Split("mkfile ")[1];
                            potato = "\\" + potato;
                        }
                        catch
                        {
                            ilog("\nNo arguments\n");
                        }


                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy" || potato.Contains(".sys") || potato.Contains(".hid") || !potato.Contains("."))
                        {
                            ilog("Name is reserved or doesn't contain file extension.");
                        }
                        else if (!File.Exists(potato))
                        {
                            File.Create(Directory.GetCurrentDirectory() + potato);
                        }
                        else if (File.Exists(potato))
                        {
                            ilog("File already exists");
                        }
                    }

                    else if (input.Contains("deldir") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        try
                        {
                            potato = potato.Split("deldir ")[1];
                            potato = "\\" + potato;
                        }
                        catch
                        {
                            ilog("No arguments");
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy")
                        {
                            ilog("Name is reserved");
                        }
                        else if (Directory.Exists(potato))
                        {
                            ilog("Do you want to delete this folder? (Y/N):");
                            if (Console.ReadKey(true).Key == ConsoleKey.Y)
                                Directory.Delete(Directory.GetCurrentDirectory() + potato, true);
                            if (Console.ReadKey(true).Key == ConsoleKey.N)
                                ilog("Operation canceled");
                        }
                        else if (!Directory.Exists(potato))
                        {
                            ilog("Directory doesn't exist");
                        }
                    }

                    else if (input.Contains("delfile") && disk)
                    {
                        var potato = input_beforelower;
                        var filename = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        try
                        {
                            potato = potato.Split("delfile ")[1];
                            potato = "\\" + potato;
                        }
                        catch
                        {
                            ilog("No arguments");
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy")
                        {
                            ilog("Name is reserved.");
                        }
                        else if (File.Exists(potato))
                        {
                            ilog("Do you want to delete this file? (Y/N):");
                            if (Console.ReadKey(true).Key == ConsoleKey.Y)
                                File.Delete(Directory.GetCurrentDirectory() + potato);
                            if (Console.ReadKey(true).Key == ConsoleKey.N)
                                ilog("Operation canceled");
                        }
                        else if (!File.Exists(potato))
                        {
                            ilog("File doesn't exist");
                        }
                    }

                    else if (input.Contains("cd") && disk)
                    {
                        if (input == "cd..")
                        {
                            Directory.SetCurrentDirectory(@"0:\");
                            dir = Directory.GetCurrentDirectory();
                        }
                        else
                        {
                            var potato = input_beforelower;
                            potato = potato.Split("cd ")[1];
                            if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                            if (!potato.Contains("\\") && potato != root) { potato = "\\" + potato; }

                            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + potato)))
                            {
                                dir = Directory.GetCurrentDirectory() + potato;
                                Directory.SetCurrentDirectory(dir);
                            }
                        }
                    }

                    else if (input.Contains("dir") && disk)
                    {
                        clog("\nDirectory listing at " + Directory.GetCurrentDirectory(), ConsoleColor.Yellow);
                        var directoryList = VFSManager.GetDirectoryListing(dir);
                        var files = 0;
                        foreach (var directoryEntry in directoryList)
                        {
                            if (!directoryEntry.mName.Contains(".hid") || !directoryEntry.mName.Contains(".HID"))
                            {
                                if (Directory.Exists(dir + "\\" + directoryEntry.mName))
                                {
                                    clog("<Dir> " + directoryEntry.mName, ConsoleColor.Gray);
                                }
                                if (File.Exists(dir + "\\" + directoryEntry.mName))
                                {
                                    clog("<File> " + directoryEntry.mName, ConsoleColor.Gray);
                                }
                                files += 1;
                            }
                        }
                        if (files == 0) { clog("\nFound nothing :(\n", ConsoleColor.Gray); }
                        else { clog("\nFound " + files + " elements\n", ConsoleColor.Yellow); }
                    }

                    else if (input.Contains("copy") && disk)
                    {
                        var potato = input_beforelower;
                        var potato1 = input_beforelower;
                        bool fileOK = false;

                        if (!potato.Contains(@"0:\") && !potato1.Contains(@"0:\"))
                        {
                            ilog("Path not specified");
                        }
                        else
                        {
                            fileOK = true;
                        }

                        if (fileOK)
                        {
                            try
                            {
                                potato = potato.Split(" ")[1];
                                potato1 = potato1.Split(" ")[2];
                            }
                            catch
                            {
                                ilog("No arguments");
                            }


                            if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy")
                            {
                                clog("\nName is reserved\n", ConsoleColor.Gray);
                            }
                            else if (potato1 == "mkfile" || potato1 == "mkdir" || potato1 == "delfile" || potato1 == "deldir" || potato1 == "copy")
                            {
                                clog("\nName is reserved.\n", ConsoleColor.Gray);
                            }
                            else if (File.Exists(potato))
                            {
                                var Contents = File.ReadAllText(potato);
                                try
                                {
                                    File.Create(potato1);
                                    File.WriteAllText(potato1, Contents);
                                    ilog("Action successful");
                                }
                                catch { }
                            }
                            else if (!File.Exists(potato))
                            {
                                ilog("File doesn't exist");
                            }
                        }
                    }

                    else if (input.Contains("lb") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        try
                        {
                            potato = potato.Split("lb ")[1];
                            File.WriteAllText(diskfile, potato);
                        }
                        catch
                        {
                            ilog("Couldn't relablel disk, name argument?");
                        }
                    }

                    else if (input.Contains("disk") && disk)
                    {
                        long availableSpace = VFSManager.GetAvailableFreeSpace(@"0:\");
                        long diskSpace = VFSManager.GetTotalSize(@"0:\");
                        string fsType = VFSManager.GetFileSystemType("0:\\");
                        clog("\nDisk info for " + File.ReadAllText(diskfile), ConsoleColor.Yellow);
                        if (diskSpace < 1000000) //Less than 1mb
                        {
                            clog("\nDisk space: " + availableSpace / 1000 + "KB free out of" + diskSpace / 1000 + "KB total", ConsoleColor.Yellow);
                        }
                        else if (diskSpace > 1000000) //More than 1mb
                        {
                            clog("\nDisk space: " + availableSpace / 1e+6 + "MB free out of" + diskSpace / 1e+6 + "MB total", ConsoleColor.Yellow);
                        }
                        else if (diskSpace > 1e+9) //More than 1gb
                        {
                            clog("\nDisk space: " + availableSpace / 1e+9 + "GB free out of" + diskSpace / 1e+9 + "GB total", ConsoleColor.Yellow);
                        }
                        clog("\nFilesystem type: " + fsType, ConsoleColor.Yellow);
                        line();
                    }

                    else if (input.Contains("echo"))
                    {
                        var thingtosay = input_beforelower;
                        thingtosay = thingtosay.Split("echo ")[1];
                        clog(thingtosay, ConsoleColor.Gray);
                    }

                    else if (input == "notepad" && disk)
                    {
                        MIVNotepad.StartMIV();
                    }

                    #region Unknown command handling

                    else
                    {
                        if (disk)
                        {
                            var potato = input_beforelower;
                            try
                            {
                                potato = potato.Split(": ")[1];
                            }
                            catch
                            {
                                Console.Beep(880, 5);
                                clog("\n! Unknown command.\n", ConsoleColor.Red);
                            }

                            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), potato)))
                            {
                                if (input.Contains(".wav"))
                                {
                                    FileInfo fi = new FileInfo(potato);
                                    ilog("Open contents for " + potato + " (" + fi.Length + " bytes)...");

                                    var mixer = new AudioMixer();
                                    byte[] bytes = File.ReadAllBytes(potato);
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                    var driver = AC97.Initialize(bufferSize: 4096);
                                    mixer.Streams.Add(wavAudioStream);

                                    var audioManager = new AudioManager()
                                    {
                                        Stream = mixer,
                                        Output = driver
                                    };
                                    audioManager.Enable();
                                }
                                else if (input.Contains(".sys") || input.Contains(".txt") || input.Contains(".hid") || input.Contains(".lang"))
                                {
                                    FileInfo fi = new FileInfo(potato);
                                    ilog("Opening contents for " + potato + " (" + fi.Length + "bytes)...");

                                    var contents = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), potato));
                                    clog("Contents for " + Path.Combine(Directory.GetCurrentDirectory(), potato) + " are " + contents + "\n", ConsoleColor.Gray);
                                }
                                else
                                {
                                    FileInfo fi = new FileInfo(potato);
                                    ilog("File format isn't supported, sorry!");
                                }
                            }
                        }
                        else
                        {
                            Console.Beep(880, 5);
                            clog("\n! Unknown command.\n", ConsoleColor.Red);
                        }
                    }

                    #endregion
                }

                #region Exception catching

                catch (Exception e)
                {
                    clog("\nAn exception occurred." + e + "\n", ConsoleColor.Red);
                    Console.Beep(880, 5);
                }
                #endregion
            }
        }
    }
}