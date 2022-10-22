using System;
using System.IO;
using Cosmos.System.FileSystem;
using System.Threading;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem.VFS;
using Cosmos.HAL.Drivers.PCI.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using Cosmos.System.Graphics;
using System.Drawing;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        //Readonly 
        readonly string ver = "Release 1.0.0";
        readonly string systempath = @"0:\SYSTEM";
        readonly string langdir = @"0:\SYSTEM\LANG";
        readonly string userfile = @"0:\SYSTEM\userfile.sys";
        readonly string diskfile = @"0:\disk.hid";
        readonly string deflangfile = @"0:\SYSTEM\LANG\en_us.lang";
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
                    if (!Directory.Exists(langdir))
                    {
                        Directory.CreateDirectory(langdir);
                    }
                    if (!File.Exists(deflangfile))
                    {
                        clog("! A critical system file couldn't be found, press any key to shut down...", ConsoleColor.Red);
                        Console.ReadKey();
                        Sys.Power.Shutdown();
                    }
                    else
                    {
                        if (File.Exists(langsetting))
                        {
                            lang = File.ReadAllLines(langsetting);
                        }
                        else
                        {
                            lang = File.ReadAllLines(deflangfile);
                        }
                    }
                }
                
            }

            Sys.MouseManager.ScreenWidth = 640;
            Sys.MouseManager.ScreenHeight = 480;

            //Boot message

            clear();
            log(lang[0]);
            clog("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ", ConsoleColor.DarkGreen);
            log("\n" + ver + "\n" + lang[1] + "\n" + lang[2]);
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

                // You don't have to specify the Mode, but here we do to show that you can.
                canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));
                canvas.Clear(Color.Blue);

                Pen pen = new Pen(Color.Red);

                // A red Point
                canvas.DrawPoint(pen, 69, 69);

                // A GreenYellow horizontal line
                pen.Color = Color.GreenYellow;
                canvas.DrawLine(pen, 250, 100, 400, 100);

                // An IndianRed vertical line
                pen.Color = Color.IndianRed;
                canvas.DrawLine(pen, 350, 150, 350, 250);

                // A MintCream diagonal line
                pen.Color = Color.MintCream;
                canvas.DrawLine(pen, 250, 150, 400, 250);

                // A PaleVioletRed rectangle
                pen.Color = Color.PaleVioletRed;
                canvas.DrawRectangle(pen, 350, 350, 80, 60);

                // A LimeGreen rectangle
                pen.Color = Color.LimeGreen;
                canvas.DrawRectangle(pen, 450, 450, 80, 60);

                canvas.Display(); // Required for something to be displayed when using a double buffered driver

                Console.ReadKey();

                canvas.Disable();
                gui = false;

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

                    if (input.Contains("help") && !input.Contains("info"))
                    {
                        var diskcommandcolor = ConsoleColor.White;
                        var unavailabletext = "";
                        if (input == "help" || input.Contains("1"))
                        {
                            if (!disk)
                            {
                                diskcommandcolor = ConsoleColor.Gray;
                                unavailabletext = lang[4];
                            }
                            line();
                            clog("\n" + lang[37], ConsoleColor.DarkGreen);
                            log(lang[38]);
                            log(lang[39]);
                            log(lang[40]);
                            log(lang[41]);
                            log(lang[42]);
                            log(lang[43]);
                            log(lang[44]);
                            log(lang[45]);
                            log(lang[46]);
                            log(lang[47]);
                            log(lang[48]);
                            clog(lang[49] + unavailabletext, diskcommandcolor);
                            clog(lang[50] + unavailabletext, diskcommandcolor);
                            clog(lang[51] + unavailabletext, diskcommandcolor);
                            clog(lang[52] + unavailabletext, diskcommandcolor);
                            clog(lang[53] + unavailabletext, diskcommandcolor);
                            clog(lang[54] + unavailabletext, diskcommandcolor);
                            clog(lang[55] + unavailabletext, diskcommandcolor);
                            clog(lang[56] + unavailabletext, diskcommandcolor);
                            clog(lang[57] + unavailabletext, diskcommandcolor);
                            clog(lang[58] + unavailabletext, diskcommandcolor);
                            clog(lang[59] + unavailabletext, diskcommandcolor);
                            line();
                        }
                        else if (input.Contains("2"))
                        {
                            clog("\n" + lang[60], ConsoleColor.DarkGreen);
                            log(lang[61]);
                            log(lang[62]);
                            line();
                        }
                    }

                    //Username commands

                    else if (input.Contains("username") && !input.Contains("open"))
                    {
                        cwrite("\n" + lang[5], ConsoleColor.Gray);
                        cwrite(File.ReadAllText(userfile), ConsoleColor.Gray);
                        write("\n\n");

                        if (input.Contains("change"))
                        {
                            var text = input;
                            var start = text.IndexOf("\"") + 1; //Add one to not include quote
                            var end = text.LastIndexOf("\"") - start;
                            var nur = text.Substring(start, end);
                            usr = nur;

                            if (File.Exists(userfile))
                            {
                                try { File.WriteAllText(userfile, usr); clog(lang[6] + "\n", ConsoleColor.Gray); } catch { }
                            }
                        }
                    }

                    //CrEdItS!

                    else if (input.Contains("credits"))
                    {
                        line();
                        cwrite("C", ConsoleColor.Blue);
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

                    #region Info commands

                    else if (input.Contains("info") && !input.Contains("open") && !input.Contains("fiinfo"))
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

                    #endregion

                    #region Color functions
                    else if (input.Contains("color") && !input.Contains("open"))
                    {
                        if (input == "color")
                        {
                            clog("\n" + lang[33], ConsoleColor.Yellow);
                        }
                        if (input.Contains("list"))
                        {
                            var OldColor = Console.ForegroundColor;
                            var OldColorBack = Console.BackgroundColor;
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            log("\n" + lang[62]);

                            write(" ");
                            Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.White; write(lang[63] + "\n");
                            Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.DarkBlue;
                            log(lang[64]);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            log(lang[65]);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            log(lang[66]);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            log(lang[67]);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            log(lang[68]);
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
                            log(lang[78]);
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
                        gui = true;
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
                        clog("\n" + lang[7] + Convert.ToString(Cosmos.HAL.RTC.Hour) + ":" + Convert.ToString(Cosmos.HAL.RTC.Minute) + ":" + Convert.ToString(Cosmos.HAL.RTC.Second) + "\n", ConsoleColor.Yellow);
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
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy" || potato.Contains("."))
                        {
                            clog("\n" + lang[11] + "\n", ConsoleColor.Gray);
                        }
                        else if (!Directory.Exists(potato))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato));
                        }
                        else if (Directory.Exists(potato))
                        {
                            clog("\n" + lang[12] + "\n", ConsoleColor.Gray);
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
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                        }


                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy" || potato.Contains(".sys") || potato.Contains(".hid") || !potato.Contains("."))
                        {
                            clog("\n" + lang[11] + "\n", ConsoleColor.Gray);
                        }
                        else if (!File.Exists(potato))
                        {
                            File.Create(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato));
                        }
                        else if (File.Exists(potato))
                        {
                            clog("\n" + lang[12] + "\n", ConsoleColor.Gray);
                        }
                    }

                    else if (input.Contains("deldir") && !input.Contains("open") && disk)
                    {
                        var potato = input_beforelower;
                        if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                        try
                        {
                            potato = potato.Split("deldir ")[1];
                        }
                        catch
                        {
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy")
                        {
                            clog("\n" + lang[11] + "\n", ConsoleColor.Gray);
                        }
                        else if (Directory.Exists(potato))
                        {
                            Directory.Delete(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato), true);
                        }
                        else if (!Directory.Exists(potato))
                        {
                            clog("\n" + lang[13] + "\n", ConsoleColor.Gray);
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
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                        }

                        if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy")
                        {
                            clog("\n" + lang[11] + "\n", ConsoleColor.Gray);
                        }
                        else if (File.Exists(potato))
                        {
                            File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "\\" + potato));
                        }
                        else if (!File.Exists(potato))
                        {
                            clog("\n" + lang[13] + "\n", ConsoleColor.Gray);
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
                        clog("\n" + lang[14] + Directory.GetCurrentDirectory(), ConsoleColor.Yellow);
                        var directoryList = VFSManager.GetDirectoryListing(dir);
                        var files = 0;
                        foreach (var directoryEntry in directoryList)
                        {
                            if (!directoryEntry.mName.Contains(".hid"))
                            {
                                clog(directoryEntry.mName, ConsoleColor.Gray);
                                files += 1;
                            }
                        }
                        if (files == 0) { clog("\n" + lang[15] + "\n", ConsoleColor.Gray); }
                        else { clog("\n" + lang[16] + files + lang[17] + "\n", ConsoleColor.Yellow); }
                    }

                    else if (input.Contains("copy") && !input.Contains("open") && disk)
                    {
                        var potato = input_beforelower;
                        var potato1 = input_beforelower;
                        bool fileOK = false;

                        if (!potato.Contains(@"0:\") && !potato1.Contains(@"0:\"))
                        {
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
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
                                clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                            }


                            if (potato == "mkfile" || potato == "mkdir" || potato == "delfile" || potato == "deldir" || potato == "copy")
                            {
                                clog("\n" + lang[18] + "\n", ConsoleColor.Gray);
                            }
                            else if (potato1 == "mkfile" || potato1 == "mkdir" || potato1 == "delfile" || potato1 == "deldir" || potato1 == "copy")
                            {
                                clog("\n" + lang[19] + "\n", ConsoleColor.Gray);
                            }
                            else if (File.Exists(potato))
                            {
                                var Contents = File.ReadAllText(potato);
                                try
                                {
                                    File.Create(potato1);
                                    File.WriteAllText(potato1, Contents);
                                    clog("\n" + lang[6] + "\n", ConsoleColor.Gray);
                                }
                                catch { }
                            }
                            else if (!File.Exists(potato))
                            {
                                clog("\n" + lang[13] + "\n", ConsoleColor.Gray);
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
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                        }
                    }

                    else if (input.Contains("disk") && !input.Contains("open") && disk)
                    {
                        long availableSpace = VFSManager.GetAvailableFreeSpace(@"0:\");
                        long diskSpace = VFSManager.GetTotalSize(@"0:\");
                        string fsType = VFSManager.GetFileSystemType("0:\\");
                        clog("\n" + lang[20] + File.ReadAllText(diskfile), ConsoleColor.Yellow);
                        if (diskSpace < 1000000) //Less than 1mb
                        {
                            clog("\n" + lang[21] + availableSpace / 1000 + lang[23] + diskSpace / 1000 + lang[26], ConsoleColor.Yellow);
                        }
                        else if (diskSpace > 1000000) //More than 1mb
                        {
                            clog("\n" + lang[21] + availableSpace / 1e+6 + lang[24] + diskSpace / 1e+6 + lang[27], ConsoleColor.Yellow);
                        }
                        else if (diskSpace > 1e+9) //More than 1gb
                        {
                            clog("\n" + lang[21] + availableSpace / 1e+9 + lang[25] + diskSpace / 1e+9 + lang[28], ConsoleColor.Yellow);
                        }
                        clog("\n" + lang[22] + fsType, ConsoleColor.Yellow);
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
                            clog("\n" + lang[10] + "\n", ConsoleColor.Gray);
                        }

                        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), potato)))
                        {
                            if (input.Contains(".wav"))
                            {
                                FileInfo fi = new FileInfo(potato);
                                clog("\n" + lang[29] + potato + " (" + fi.Length + " bytes)...\n", ConsoleColor.Gray);

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
                                clog("\n" + lang[29] + potato + " (" + fi.Length + "bytes)...\n", ConsoleColor.Gray);

                                var contents = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), potato));
                                clog(lang[34] + Path.Combine(Directory.GetCurrentDirectory(), potato) + lang[35] + contents + "\n", ConsoleColor.Gray);
                            }
                            else
                            {
                                FileInfo fi = new FileInfo(potato);
                                clog("\n" + lang[29] + potato + " (" + fi.Length + " bytes)...", ConsoleColor.Gray);

                                clog("\n" + lang[30] + "\n", ConsoleColor.Gray);
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
                        Console.Beep(880, 5);
                        clog("\n" + lang[31] + "\n", ConsoleColor.Red);
                    }

                    #endregion
                }

                #region Exception catching

                catch (Exception e)
                {
                    var OldColor = Console.ForegroundColor;
                    var OldBackground = Console.BackgroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    log("\n" + lang[32] + e + "\n");
                    Console.Beep(880, 5);
                    Console.ForegroundColor = OldColor;
                    Console.BackgroundColor = OldBackground;
                }
                #endregion
            }
        }
    }
}