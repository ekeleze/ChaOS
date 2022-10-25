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
using System.Reflection.Metadata;
using Cosmos.System.Graphics.Fonts;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        //Readonly
        readonly string ver = "1.0.0 Prerelease 7";
        readonly string systempath = @"0:\SYSTEM";
        readonly string langdir = @"0:\SYSTEM\LANG";
        readonly string userfile = @"0:\SYSTEM\userfile.sys";
        readonly string diskfile = @"0:\disk.hid";
        readonly static string root = @"0:\";

        //Not readonly
        string usr = "usr";
        public static string dir = root;
        bool gui = false;
        bool disk;
        string input;
        string input_beforelower;
        string[] lang;
        string langsetting;
        Canvas canvas;
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
                log(lang[3]);
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
	
	    #region GUI methods
	    protected void InitGUI()
	    {
            //Sys.MouseManager.ScreenWidth = 640;
            //Sys.MouseManager.ScreenHeight = 480;
            //Sys.MouseManager.MouseSensitivity = 0.5F;
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));
            canvas.Clear(Color.DimGray);
		    gui = true;
	    }
        protected void MakeWindow(int x, int y, int w, int h, string type)
        {
            Pen pen = new Pen(Color.Black);
            canvas.DrawFilledRectangle(new Pen(Color.LightGray), x, y, w, h);
            canvas.DrawRectangle(pen, x, y, w, h);
            canvas.DrawFilledRectangle(new Pen(Color.White), x, y, w, 15);
            canvas.DrawRectangle(pen, x, y, w, 15);
            PCScreenFont font = PCScreenFont.Default;
            if (type == "aboutwindow")
            {
                canvas.DrawString("About ChaOS Gui", font, pen, new Sys.Graphics.Point(x + 1, y + 1));
                canvas.DrawString("This is a demo of the Gui", font, pen, new Sys.Graphics.Point(x + 1, y + 17));
                canvas.DrawString("Press any key to close", font, pen, new Sys.Graphics.Point(x + 1, y + 33));
            }
        }
	    protected void DisableGUI()
	    {
		    canvas.Disable();
            gui = false;
	    }
	    #endregion

        protected override void Run()
        {
            if (gui)
            {
                #region GUI

                //uint x = Sys.MouseManager.X;
                //uint y = Sys.MouseManager.Y;

                //canvas.DrawPoint(new Pen(Color.Aqua), x, y);

                MakeWindow(50, 50, 200, 160, "aboutwindow");

                canvas.Display(); //Required for something to be displayed when using a double buffered driver

                Console.ReadKey();

                DisableGUI();

                #endregion
            }
            else
            {
                try
                {
                    if (disk)
                    {
                        dir = Directory.GetCurrentDirectory();
                        write(usr + " (" + dir + ")");
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
                        var diskcommandcolor = ConsoleColor.White;
                        var unavailabletext = "";
                        {
                            if (disk)
                            {
                                clog("\nFunctions:", ConsoleColor.DarkGreen);
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
                            else
                            {
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
                                //clog(" disk - Gives info about the disk" + unavailabletext, diskcommandcolor);
                                //clog(" cd - Browses to folder, works as in Windows" + unavailabletext, diskcommandcolor);
                                //clog(" cd.. - Returns to root" + unavailabletext, diskcommandcolor);
                                //clog(" dir - Lists files in the current folder" + unavailabletext, diskcommandcolor);
                                //clog(" mkdir - Makes folder, with dirname argument" + unavailabletext, diskcommandcolor);
                                //clog(" mkfile - Makes file, with filename argument" + unavailabletext, diskcommandcolor);
                                //clog(" deldir - Deletes folder, with dirname argument" + unavailabletext, diskcommandcolor);
                                //clog(" delfile - Deletes file, with filename argument" + unavailabletext, diskcommandcolor);
                                //clog(" open - Opens file. Supported formats: .txt .sys .wav" + unavailabletext, diskcommandcolor);
                                //clog(" lb - Relabels disk" + unavailabletext, diskcommandcolor);
                                //clog(" notepad - Opens MIV notepad" + unavailabletext + "\n", diskcommandcolor);
                            }
                        }
                    }

                    //Username commands

                    else if (input.Contains("username") && !input.Contains(".sys"))
                    {
                        if (!input.Contains(".txt"))
                        {
                            if (!input.Contains("open"))
                            {
                                cwrite("\n" + lang[5], ConsoleColor.Gray);
                                cwrite(File.ReadAllText(userfile), ConsoleColor.Gray);
                                write("\n\n");

                                if (input.Contains("change"))
                                {
                                    var text = input;
                                    var start = text.IndexOf(" ") + 1; //Add one to not include quote
                                    var nur = text.Substring(start);
                                    usr = nur;

                                    if (File.Exists(userfile))
                                    {
                                        try { File.WriteAllText(userfile, usr); ilog("Username changed successfully\n"); } catch { }
                                    }
                                }
                            }
                        }
                    }

                    //CrEdItS!

                    else if (input.Equals("credits"))
                    {
                        cwrite("\n\nC", ConsoleColor.Blue);
                        cwrite("r", ConsoleColor.Green);
                        cwrite("e", ConsoleColor.DarkYellow);
                        cwrite("d", ConsoleColor.Red);
                        cwrite("i", ConsoleColor.Gray);
                        cwrite("t", ConsoleColor.Cyan);
                        cwrite("s", ConsoleColor.DarkRed);
                        cwrite(":\n", ConsoleColor.Blue);
                        clog(" ekeleze - Founder of ChaOS", ConsoleColor.Yellow);
                        clog(" MrDumbrava - Contributor & spanish language writer", ConsoleColor.Yellow);
                        clog(" Retronics - German language writer", ConsoleColor.Yellow);
                        clog(" 0xRage - Portuguese language writer", ConsoleColor.Yellow);
                        clog(" Owen2k6 - Japanese language writer", ConsoleColor.Yellow);
                        clog(" mariobot128 - French language writer", ConsoleColor.Yellow);
                        line();
                    }

                    //Misc

                    #region Color functions
                    else if (input.Contains("color") && !input.Contains("open") && !input.Contains("."))
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
                            log(lang[65]);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            log(lang[66]);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            log(lang[67]);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            log(" blue - Normal blue with black background");
                            Console.ForegroundColor = ConsoleColor.Green;
                            log(lang[69]);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            log(lang[70]);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            log(lang[71]);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            log(lang[72]);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            log(lang[73]);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            log(lang[74]);
                            Console.ForegroundColor = ConsoleColor.Red;
                            log(lang[75]);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            log(lang[76]);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            log(lang[77]);
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
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark blue")) //Dark blue
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark green")) //Dark green
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark cyan")) //Dark cyan
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark gray")) //Dark gray
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("blue")) //Blue
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.BackgroundColor = ConsoleColor.Black;
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
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.BackgroundColor = ConsoleColor.Black;
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
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("dark red")) //Dark red
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark magenta")) //Dark magenta
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("dark yellow")) //Dark yellow
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (!input.Contains("dark"))
                        {
                            if (input.Contains("gray")) //Gray
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.BackgroundColor = ConsoleColor.Black;
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
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.BackgroundColor = ConsoleColor.Black;
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
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.BackgroundColor = ConsoleColor.Black;
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
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.BackgroundColor = ConsoleColor.Black;
                                clear();
                                log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                                log("\n" + ver);
                                log("Copyright 2022 (c) Kastle Grounds\n");
                            }
                        }

                        if (input.Contains("white")) //White
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            clear();
                            log("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
                            log("\n" + ver);
                            log("Copyright 2022 (c) Kastle Grounds\n");
                        }

                        if (input.Contains("test"))
                        {
                            var OldColor = Console.ForegroundColor;
                            log("");

                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            log("1");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            log("2");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            log("3");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            log("4");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            log("5");
                            Console.ForegroundColor = ConsoleColor.Green;
                            log("6");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            log("7");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            log("8");
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            log("9");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            log("10");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            log("11");
                            Console.ForegroundColor = ConsoleColor.Red;
                            log("12");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            log("13");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            log("14");

                            Console.ForegroundColor = OldColor;
                            log("\ndone\n");
                        }
                    }

                    #endregion

                    else if (input.Contains("gui") && !input.Contains("open"))
                    {
			            InitGUI();
                    }

                    else if (input.Contains("lang") && !input.Contains("open") && !input.Contains("cd"))
                    {
                        var potato = input_beforelower;
                        bool passed;
                        try
                        {
                            potato = potato.Split("lang ")[1];
                            lang = File.ReadAllLines(Path.Combine(langdir + "\\" + potato));
                            langsetting = Path.Combine(langdir + potato);
                            passed = true;
                        }
                        catch
                        {
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                            passed = false;
                        }
                        if (passed == true)
                        {
                            clog("\n" + lang[36] + "\n", ConsoleColor.Gray);
                        }
                    }

                    else if (input.Contains("clear") && !input.Contains("open") || input.Contains("cls") && !input.Contains("open"))
                    {
                        clear();
                    }

                    else if (input == "time" && !input.Contains("open") || input == "t" && !input.Contains("open"))
                    {
                        clog("\nThe time is " + Convert.ToString(Cosmos.HAL.RTC.Hour) + ":" + Convert.ToString(Cosmos.HAL.RTC.Minute) + ":" + Convert.ToString(Cosmos.HAL.RTC.Second) + "\n", ConsoleColor.Yellow);
                    }

                    //Power stuff

                    else if (input.Contains("shutdown") && !input.Contains("open") || input.Contains("sd") && !input.Contains("open"))
                    {
                        clog("\n" + lang[8], ConsoleColor.Gray);
                        Sys.Power.Shutdown();
                    }

                    else if (input.Contains("reboot") && !input.Contains("open") || input.Contains("rb") && !input.Contains("open"))
                    {
                        clog("\n" + lang[9], ConsoleColor.Gray);
                        Sys.Power.Reboot();
                    }

                    //Disk and filesystem stuff

                    else if (input.Contains("mkdir") && !input.Contains("open") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace("0:\\", ""); }

                        try
                        {
                            potato = potato.Split("mkdir ")[1];
                        }
                        catch
                        {
                            ilog("No arguments");
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy" || potato.Contains("."))
                        {
                            ilog("Name is reserved.");
                        }
                        else if (!Directory.Exists(potato))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato));
                        }
                        else if (Directory.Exists(potato))
                        {
                            ilog("Directory already exists");
                        }
                    }

                    else if (input.Contains("mkfile") && !input.Contains("open") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace("0:\\", ""); }
                        try
                        {
                            potato = potato.Split("mkfile ")[1];
                        }
                        catch
                        {
                            ilog("\nNo arguments\n");
                        }


                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy" || potato.Contains(".sys") || potato.Contains(".hid") || !potato.Contains("."))
                        {
                            ilog("\nName is reserved.\n");
                        }
                        else if (!File.Exists(potato))
                        {
                            File.Create(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato));
                        }
                        else if (File.Exists(potato))
                        {
                            ilog("\nFile already exists\n");
                        }
                    }

                    else if (input.Contains("deldir") && !input.Contains("open") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        try
                        {
                            ilog("Directory already exists");
                        }
                        catch
                        {
                            ilog("No arguments");
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy")
                        {
                            ilog("Name is reserved.");
                        }
                        else if (Directory.Exists(potato))
                        {
                            Directory.Delete(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato), true);
                        }
                        else if (!Directory.Exists(potato))
                        {
                            ilog("Directory doesn't exist");
                        }
                    }

                    else if (input.Contains("delfile") && !input.Contains("open") && disk)
                    {
                        var potato = input_beforelower;
                        var filename = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        try
                        {
                            potato = potato.Split("delfile ")[1];
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
                            File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato));
                        }
                        else if (!File.Exists(potato))
                        {
                            ilog("File doesn't exist");
                        }
                    }

                    else if (input.Contains("cd") && !input.Contains("open") && disk)
                    {
                        if (input == "cd..")
                        {
                            Directory.SetCurrentDirectory(@"0:\");
                            dir = Directory.GetCurrentDirectory();
                        }
                        else
                        {
                            var potato = input_beforelower;
                            if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                            if (!potato.Contains("\\") && potato != root) { potato = "\\" + potato; }
                            potato = potato.Split("cd ")[1];

                            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + potato)))
                            {
                                dir = Directory.GetCurrentDirectory() + potato;
                                Directory.SetCurrentDirectory(dir);
                            }
                        }
                    }

                    else if (input.Contains("dir") && !input.Contains("open") && disk)
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

                    else if (input.Contains("copy") && !input.Contains("open") && disk)
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

                    else if (input.Contains("lb") && !input.Contains("open") && disk)
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

                    else if (input.Contains("disk") && !input.Contains("open") && disk)
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

                    else if (input.Contains("open") && disk)
                    {
                        var potato = input_beforelower;
                        try
                        {
                            potato = potato.Split(" ")[1];
                        }
                        catch
                        {
                            ilog("No arguments");
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
                                clog("Contents for " + Path.Combine(Directory.GetCurrentDirectory(), potato) + " are " + contents + "", ConsoleColor.Gray);
                            }
                            else
                            {
                                FileInfo fi = new FileInfo(potato);
                                ilog("File format isn't supported, sorry!");
                            }
                        }
                    }

                    else if (input.Contains("echo") && !input.Contains("open"))
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