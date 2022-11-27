using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using System.IO;
using Sys = Cosmos.System;
using ChaOS.Misc;
using ChaOS.System;
using static ChaOS.Core;
using static System.ConsoleColor;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        //Variables
        readonly string[] contributors = {
            "ekeleze - Creator",
            "MrDumbrava - Contributor & spanish language writer",
            "Retronics - German language writer",
            "0xRage - Portuguese language writer",
            "Owen2k6 - Japanese language writer",
            "mariobot128 - French language writer",
            "RaphMar2019 - GUI helper",
            "BiltzWolf007 - GUI helper"
        };

        const string ver = "Release 1.1";
        const int copyright = 2022;
        const string systempath = @"0:\SYSTEM";
        public const string userfile = @"0:\SYSTEM\USERFILE.SYS";
        const string root = @"0:\";
        public static string usr = "usr";
        public static bool disk;
        string input;
        string inputBeforeLower;
        string inputCapitalized;
        CosmosVFS fs = new CosmosVFS();
        protected override void BeforeRun() {
            try {
                // Initialization

                VFSManager.RegisterVFS(fs);

                try {
                    var temp = fs.GetTotalSize(root);
                    Directory.SetCurrentDirectory(root);
                    disk = true;
                }
                catch { disk = false; }

                if (disk) {
                    if (!Directory.Exists(systempath))
                        Directory.CreateDirectory(systempath);
                    else {
                        if (File.Exists(userfile))
                            usr = File.ReadAllText(userfile);
                        else if (!File.Exists(userfile)) {
                            File.Create(userfile);
                            File.WriteAllText(userfile, usr);
                        }
                    }
                }

                // Login system

                // Commented this out because there is no obvoius way to make a accound and thus breaks the os
                //Clear();
                //log("Boot successful!");
                //Security.Login();

                // Boot message

                Clear();
                clog("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ", DarkGreen);
                log("\n" + ver + "\nCopyright (c) " + copyright + " Kastle Grounds\nType \"help\" to get started!");
                if (!disk) {
                    log("No internal hard drive detected, ChaOS will continue in ClassiChaOS mode!");
                }
                log();
            }
            catch (Exception exc) { Core.Stop(exc); }
        }

        protected override void Run() {
            try {
                if (disk) {
                    write(usr + " (" + Directory.GetCurrentDirectory() + ")");
                    write(": "); }
                else if (!disk) {
                    write(usr + " > ");
                }

                inputBeforeLower = Console.ReadLine(); // Input
                inputCapitalized = inputBeforeLower.ToUpper(); // Input converted to uppercase
                input = inputBeforeLower.ToLower(); // Input converted to lowercase

                if (input.Equals("help")) {
                    if (disk)
                        clog("\nFunctions:", DarkGreen);
                    else
                        clog("\nFunctions (ClassiChaOS Mode):", DarkGreen);
                    log(" help - Shows all functions");
                    log(" user - User functions (username, password...)");
                    log(" about - Shows all of the wonderful people that make ChaOS work");
                    log(" cls/clear - Clears the screen");
                    log(" color - Changes text color, do 'color list' to list all colors");
                    log(" t/time - Tells you the time");
                    log(" echo - Echoes what you say");
                    log(" sd/shutdown - Shuts down ChaOS");
                    log(" rb/reboot - Reboots the system");
                    if (disk) {
                        log(" disk - Gives info about the disk");
                        log(" cd - Browses to folder, works as in MS-DOS");
                        log(" cd.. - Returns to root");
                        log(" dir - Lists files in the current folder");
                        log(" mkdir - Creates folder");
                        log(" touch - Creates file");
                        log(" rd - Removes folder");
                        log(" del - Removes file");
                        log(" lb - Relabels disk");
                        log(" notepad - Opens MIV notepad.");
                    } log();
                }

                //Miscellaneous

                else if (input.Equals("user"))
                    MIV.StartMIV(userfile);

                else if (input.Equals("about")) {
                    log();
                    cwrite("C", Blue); cwrite("r", Green); cwrite("e", DarkYellow); cwrite("d", Red); cwrite("i", DarkMagenta); cwrite("t", Cyan); cwrite("s:\n", DarkRed);
                    foreach (string contributor in contributors) clog(" " + contributor, Yellow);
                    log();
                }

                #region Color functions

                else if (input.StartsWith("color"))  {
                    string ClearScreen = "ChaOS " + ver + "\nCopyright (c) " + copyright + " Kastle Grounds\n";

                    if (input.Contains("list")) {
                        clog("\nColor list:", Green);
                        write(" "); Draw.ScreenColor(White, Black, false); write("black - Pure light mode, will make you blind"); Draw.ScreenColor(Black, White, false);
                        clog("\n dark blue - Dark blue with black background", DarkBlue);
                        clog(" dark green - Dark green with black background", DarkGreen);
                        clog(" dark cyan - Dark cyan with black background", DarkCyan);
                        clog(" dark gray - Dark gray with black background", DarkGray);
                        clog(" blue - Normal blue with black background", Blue);
                        clog(" green - Green with black background", Green);
                        clog(" cyan - Cyan with black background", Cyan);
                        clog(" dark red - Dark red with black background", DarkRed);
                        clog(" dark magenta - Dark magenta with black background", DarkMagenta);
                        clog(" dark yellow - Dark yellow/orange with black background", DarkYellow);
                        clog(" gray - Gray with black background", Gray);
                        clog(" red - Red with black background", Red);
                        clog(" magenta - Magenta with black background", Magenta);
                        clog(" yellow - Light yellow with black background", Yellow);
                        clog(" white - Dark mode B)\n", White);
                    }

                    else if (input.EndsWith("black")) Draw.ScreenColor(White, Black); //Black

                    else if (input.EndsWith("dark blue")) { //Dark blue
                        Draw.ScreenColor(Black, DarkBlue);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark green")) { //Dark green
                        Draw.ScreenColor(Black, DarkGreen);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark cyan")) { //Dark cyan
                        Draw.ScreenColor(Black, DarkCyan);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark gray")) { //Dark gray
                        Draw.ScreenColor(Black, DarkGray);
                        log(ClearScreen); }
                    else if (!input.Contains("dark") && input.Contains("blue")) { //Blue
                        Draw.ScreenColor(Black, Blue);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("green")) { //Green
                        Draw.ScreenColor(Black, Green);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("cyan")) { //Cyan
                        Draw.ScreenColor(Black, Cyan);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark red")) { //Dark red
                        Draw.ScreenColor(Black, DarkRed);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark magenta")) { //Dark magenta
                        Draw.ScreenColor(Black, DarkMagenta);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark yellow")) { //Dark yellow
                        Draw.ScreenColor(Black, DarkYellow);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("gray")) { //Gray
                        Draw.ScreenColor(Black, Gray);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("red")) { //Red
                        Draw.ScreenColor(Black, Red);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("magenta")) { //Magenta
                        Draw.ScreenColor(Black, Magenta);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("yellow")) { //Yellow
                        Draw.ScreenColor(Black, Yellow);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("white")) { //White
                        Draw.ScreenColor(Black, White);
                        log(ClearScreen);
                    }
                    else log("\nPlease list colors by doing \"color list\" or set a color by doing \"color yellow\" (for example)\n");
                }

                #endregion

                else if (input.Equals("clear") || input.Equals("cls"))
                    Clear();

                else if (input == "time" || input == "t")
                    log("\nCurrent time is " + GetTime() + "\n");

                else if (input.StartsWith("notepad") && disk)
                    MIV.StartMIV();

                else if (input.Equals("shutdown") || input.Equals("sd")) {
                    log("\nShutting down...");
                    Sys.Power.Shutdown();
                }

                else if (input.Equals("reboot") || input.Equals("rb")) {
                    log("\nRestarting...");
                    Sys.Power.Reboot();
                }

                else if (input.StartsWith("echo"))
                    log("\n" + inputBeforeLower.Split("echo ")[1] + "\n");

                #region File management

                else if (input.StartsWith("mkdir")) {
                    string potato = input;
                    if (potato.Contains("0:\\")) { potato.Replace("0:\\", ""); }
                    potato = potato.Split("mkdir ")[1];
                    if (!Directory.Exists(potato))
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\" + potato);
                }

                else if (input.StartsWith("touch")) {
                    string potato = input;
                    if (potato.Contains("0:\\")) { potato.Replace("0:\\", ""); }
                    potato = potato.Split("touch ")[1];
                    if (!File.Exists(potato))
                        File.Create(Directory.GetCurrentDirectory() + @"\" + potato);
                }

                else if (input.StartsWith("rd")) {
                    string potato = input;
                    if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                    potato = potato.Split("rd ")[1];
                    potato = "\\" + potato;
                    if (Directory.Exists(Directory.GetCurrentDirectory() + @"\" + potato))
                        Directory.Delete(Directory.GetCurrentDirectory() + @"\" + potato, true);
                    else if (!Directory.Exists(potato))
                        throw new DirectoryNotFoundException();
                }

                else if (input.StartsWith("del")) {
                    string potato = input;
                    if (potato.Contains("0:\\")) { potato.Replace(@"0:\", ""); }
                    potato = potato.Split("del ")[1];
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\" + potato))
                        File.Delete(Directory.GetCurrentDirectory() + @"\" + potato);
                    else if (!File.Exists(potato))
                        throw new FileNotFoundException();
                }

                else if (input.Contains("cd") && disk) {
                    if (input == "cd..")
                        Directory.SetCurrentDirectory(root);
                    else {
                        if (inputCapitalized.Contains(@"0:\")) { inputCapitalized.Replace(@"0:\", ""); }
                        if (!inputCapitalized.Contains("\\") && inputCapitalized != root) { inputCapitalized = "\\" + inputCapitalized; }
                        inputCapitalized = inputCapitalized.Split("CD ")[1];
                        if (Directory.Exists(Directory.GetCurrentDirectory() + inputCapitalized))
                            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + inputCapitalized);
                        else throw new DirectoryNotFoundException();
                    }
                }

                else if (input.Equals("dir") && disk) {
                    clog("\nDirectory listing at " + Directory.GetCurrentDirectory(), Yellow);
                    var directoryList = VFSManager.GetDirectoryListing(Directory.GetCurrentDirectory());
                    var files = 0;
                    foreach (var directoryEntry in directoryList) {
                        if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + directoryEntry.mName))
                            clog("<Dir> " + directoryEntry.mName, Gray);
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\" + directoryEntry.mName))
                            clog("<File> " + directoryEntry.mName, Gray);
                        files += 1;
                    }
                    clog("\nFound " + files + " elements\n", Yellow);
                }

                else if (input.StartsWith("copy")) {
                    var potato = inputBeforeLower.Split(" ")[1];
                    var potato1 = inputBeforeLower.Split(" ")[2];
                    var Contents = File.ReadAllText(potato);
                    File.Create(potato1);
                    File.WriteAllText(potato1, Contents);
                    clog("\nAction successful\n", Blue);
                }

                else if (input.StartsWith("lb") && disk)
                    fs.SetFileSystemLabel(root, inputBeforeLower.Split("lb ")[1]);

                else if (input.Equals("disk") && disk) {
                    long availableSpace = VFSManager.GetAvailableFreeSpace(@"0:\");
                    long diskSpace = VFSManager.GetTotalSize(@"0:\");
                    string fsType = VFSManager.GetFileSystemType("0:\\");
                    clog("\nDisk info for " + fs.GetFileSystemLabel(root), Yellow);
                    if (diskSpace < 1000000) //Less than 1mb
                        clog("\nDisk space: " + availableSpace / 1000 + " KB free out of " + diskSpace / 1000 + " KB total", Yellow);
                    else if (diskSpace > 1000000) //More than 1mb
                        clog("\nDisk space: " + availableSpace / 1e+6 + " MB free out of " + diskSpace / 1e+6 + " MB total", Yellow);
                    else if (diskSpace > 1e+9) //More than 1gb
                        clog("\nDisk space: " + availableSpace / 1e+9 + " GB free out of " + diskSpace / 1e+9 + " GB total", Yellow);
                    clog("\nFilesystem type: " + fsType, Yellow);
                    log();
                }

                #endregion

                else clog("\nUnknown command.\n", Red);
            }

            #region Error handling

            catch (ArgumentNullException) { clog("\nArgument(s) cannot be null!\n", Red); }
            catch (IndexOutOfRangeException) { clog("\nIndex is out of range! Check if arguments are null.\n", Red); }

            catch (UnauthorizedAccessException) { clog("\nUnauthorized access.\n", Red); }
            catch (DirectoryNotFoundException) { clog("\nDirectory not found or not existing\n", Red); }
            catch (FileNotFoundException) { clog("\nFile not found or not existing.\n", Red); }

            catch (Exception ex) {
                Core.Stop(ex); }

            #endregion
        }
    }
}