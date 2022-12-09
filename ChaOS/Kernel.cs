using System;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using static ChaOS.Core;
using static ChaOS.DiskManager;
using static System.ConsoleColor;


namespace ChaOS {
    public class Kernel : Sys.Kernel {
        public const string ver = "Release 1.1_02";
        public const int copyright = 2022;

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

        public static string username = "usr";

        public static string input;
        public static string inputBeforeLower;
        public static string inputCapitalized;

        public static CosmosVFS fs = new CosmosVFS();

        protected override void BeforeRun() {
            try {
                log("Starting up ChaOS...\n");
                InitFS(fs);

                Console.Clear();
                log("Welcome back, " + username + ", to...\n");
                clog("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ", DarkGreen); // ChaOS logo! but its all one line xD
                log("\n" + ver + "\nCopyright (c) " + copyright + " Kastle Grounds\nType \"help\" to get started!");
                if (!disk)
                    log("No hard drive detected, ChaOS will continue without disk support.");
                log();
            }
            catch (Exception ex) { ExHandler.Crash(ex); }
        }

        protected override void Run() {
            var CanContinue = true;

            try {
                if (!Directory.GetCurrentDirectory().StartsWith(rootdir)) Directory.SetCurrentDirectory(rootdir); // Directory error correction

                if (disk) write(username + " (" + Directory.GetCurrentDirectory() + "): ");
                else write(username + " > ");

                inputBeforeLower = Console.ReadLine();         // Input before conversion
                inputCapitalized = inputBeforeLower.ToUpper(); // Input converted to uppercase
                input = inputBeforeLower.ToLower().Trim();     // Input converted to lowercase

                log(); // Do not remove!

                if (input == "help") {
                    var us = string.Empty;
                    var color = Console.ForegroundColor;
                    if (!disk) { us = " (unavailable)"; color = Gray; }

                    clog("Functions:", DarkGreen);
                    log(" help - Shows all functions");
                    log(" username - Allows you to use usernames");
                    log(" credits - Shows all of the wonderful people that make ChaOS work");
                    log(" cls/clear - Clears the screen");
                    log(" color - Changes text color, do 'color list' to list all colors");
                    log(" t/time - Tells you the time");
                    log(" echo - Echoes what you say");
                    log(" sd/shutdown - Shuts down ChaOS");
                    log(" rb/reboot - Reboots the system");
                    clog(" diskinfo - Gives info about the disk" + us, color);
                    clog(" cd - Browses to directory" + us, color);
                    clog(" cd.. - Browses to last directory" + us, color);
                    clog(" dir - Lists files in the current directory" + us, color);
                    clog(" mkdir - Creates a directory" + us, color);
                    clog(" mkfile - Creates a file" + us, color);
                    clog(" deldir - Deletes a directory" + us, color);
                    clog(" delfile - Removes file" + us, color);
                    clog(" lb - Relabels disk" + us, color);
                    clog(" miv - Opens MIV (Minimalistic Vi)." + us, color);
                    clog(" format - Formats the disk.\n" + us, color);
                }

                // Miscellaneous

                else if (input.Contains("username")) {
                    if (input.Contains("set")) {
                        try { username = input.Split("set ")[1].Trim(); }
                        catch { clog("No arguments\n", Red); }
                        clog("Done! (" + username + ")\n", Yellow);
                    }

                    else if (input.EndsWith("current")) clog("\nCurrent username: " + username + "\n", Yellow);

                    else {
                        clog("Username subfunctions:", DarkGreen);
                        log(" username set (username) - Changes the username\n" +
                            " username current - Displays current username\n");
                    }
                }

                else if (input == "credits") {
                    log(); cwrite("C", Blue); cwrite("r", Green); cwrite("e", DarkYellow); cwrite("d", Red); cwrite("i", DarkMagenta); cwrite("t", Cyan); cwrite("s:\n", DarkRed);
                    foreach (string contributor in contributors) clog(" " + contributor, Yellow); log();
                }

                #region Color functions

                else if (input.StartsWith("color")) {
                    string ClearScreen = "ChaOS " + ver + "\nCopyright (c) " + copyright + " Kastle Grounds\n";

                    if (input.Contains("list")) {
                        var OldBack = Console.BackgroundColor; var OldFore = Console.ForegroundColor;
                        clog("Color list:", Green);
                        write(" "); SetScreenColor(White, Black, false); write("black - Pure light mode, will make you blind"); SetScreenColor(OldBack, OldFore, false);
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
                        clog(" white - Pure white with black background\n", White);
                    }

                    else if (input.EndsWith("black")) SetScreenColor(White, Black); //Black

                    else if (input.EndsWith("dark blue")) { //Dark blue
                        SetScreenColor(Black, DarkBlue);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark green")) { //Dark green
                        SetScreenColor(Black, DarkGreen);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark cyan")) { //Dark cyan
                        SetScreenColor(Black, DarkCyan);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark gray")) { //Dark gray
                        SetScreenColor(Black, DarkGray);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("blue")) { //Blue
                        SetScreenColor(Black, Blue);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("green")) { //Green
                        SetScreenColor(Black, Green);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("cyan")) { //Cyan
                        SetScreenColor(Black, Cyan);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark red")) { //Dark red
                        SetScreenColor(Black, DarkRed);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark magenta")) { //Dark magenta
                        SetScreenColor(Black, DarkMagenta);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark yellow")) { //Dark yellow
                        SetScreenColor(Black, DarkYellow);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("gray")) { //Gray
                        SetScreenColor(Black, Gray);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("red")) { //Red
                        SetScreenColor(Black, Red);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("magenta")) { //Magenta
                        SetScreenColor(Black, Magenta);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("yellow")) { //Yellow
                        SetScreenColor(Black, Yellow);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("white")) { //White
                        SetScreenColor(Black, White);
                        log(ClearScreen);
                    }
                    else clog("\nPlease list colors by doing \"color list\" or set a color by doing \"color yellow\" (for example)\n", Gray);
                }

                #endregion

                else if (input == "clear" || input == "cls")
                    Console.Clear();

                else if (input == "time" || input == "t") {
                    /* Now shows up properly:
                       Ex: 7:6 -> 07:06 */

                    string Hour = DateTime.Now.Hour.ToString(); string Minute = DateTime.Now.Minute.ToString();
                    if (Hour.Length < 2) Hour = "0" + Hour; if (Minute.Length < 2) Minute = "0" + Minute;
                    clog("Current time is " + Hour + ":" + Minute + "\n", Yellow);
                }

                else if (input == "miv" && disk)
                    MIV.StartMIV();

                else if (input == "shutdown" || input == "sd") {
                    if (disk) SaveChangesToDisk();
                    clog("Shutting down...", Gray);
                    Sys.Power.Shutdown();
                }

                else if (input == "reboot" || input == "rb") {
                    if (disk) SaveChangesToDisk();
                    clog("Rebooting...", Gray);
                    Sys.Power.Reboot();
                }

                else if (input.StartsWith("echo"))
                    clog(inputBeforeLower.Split("echo ")[1] + "\n", Gray);

                #region File management

                else if (input.StartsWith("mkdir")) {
                    try { inputCapitalized = inputCapitalized.Split("MKDIR ")[1]; }
                    catch { clog("No arguments\n", Red); CanContinue = false; }
                    if (inputCapitalized.Contains("0:\\")) { inputCapitalized.Replace("0:\\", ""); }
                    if (inputCapitalized.Contains(" ")) { clog("Directory name cannot contain spaces!\n", Red); CanContinue = false; }
                    if (CanContinue) {
                        if (!Directory.Exists(inputCapitalized))
                            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\" + inputCapitalized);
                        else
                            clog("Directory already exists!\n", Red);
                    }
                }

                else if (input.StartsWith("mkfile")) {
                    try { inputCapitalized = inputCapitalized.Split("MKFILE ")[1]; }
                    catch { clog("No arguments\n", Red); CanContinue = false; }
                    if (inputCapitalized.Contains("0:\\")) { input.Replace("0:\\", ""); }
                    if (inputCapitalized.Contains(" ")) { clog("Filename cannot contain spaces!\n", Red); CanContinue = false; }
                    if (CanContinue) {
                        if (!File.Exists(inputCapitalized))
                            File.Create(Directory.GetCurrentDirectory() + @"\" + inputCapitalized);
                        else
                            clog("File already exists!\n", Red);
                    }
                }

                else if (input.StartsWith("deldir")) {
                    if (inputCapitalized.Contains("0:\\")) { input.Replace(@"0:\", ""); }
                    try { inputCapitalized = inputCapitalized.Split("DELDIR ")[1]; }
                    catch { clog("No arguments\n", Red); CanContinue = false; }
                    if (CanContinue) {
                        if (Directory.Exists(Directory.GetCurrentDirectory() + @"\" + inputCapitalized))
                            Directory.Delete(Directory.GetCurrentDirectory() + @"\" + inputCapitalized, true);
                        else if (!Directory.Exists(inputCapitalized))
                            clog("Directory not found!\n", Red);
                    }
                }

                else if (input.StartsWith("delfile")) {
                    if (inputCapitalized.Contains("0:\\")) { inputCapitalized.Replace(@"0:\", ""); }
                    try { inputCapitalized = inputCapitalized.Split("DELFILE ")[1]; }
                    catch { clog("No arguments\n", Red); CanContinue = false; }
                    if (CanContinue) {
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\" + inputCapitalized))
                            File.Delete(Directory.GetCurrentDirectory() + @"\" + inputCapitalized);
                        else if (!File.Exists(inputCapitalized))
                            clog("File not found!\n", Red);
                    }
                }

                else if (input.StartsWith("cd") && disk) {
                    if (input == "cd..") {
                        try {
                            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory().TrimEnd('\\').Remove(Directory.GetCurrentDirectory().LastIndexOf('\\') + 1));
                            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length - 1));
                        }
                        catch { } // Error correction
                    }

                    else if (input.StartsWith("cd ")) {
                        try { inputCapitalized = inputCapitalized.Split("CD ")[1]; }
                        catch { clog("No arguments\n", Red); CanContinue = false; }
                        if (inputCapitalized.Trim() != string.Empty) CanContinue = true;

                        if (CanContinue) {
                            if (inputCapitalized.Contains(@"0:\")) { inputCapitalized.Replace(@"0:\", ""); }
                            if (Directory.GetCurrentDirectory() != rootdir) { inputCapitalized = @"\" + inputCapitalized; }

                            if (Directory.Exists(Directory.GetCurrentDirectory() + inputCapitalized))
                                Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + inputCapitalized);
                            else
                                clog("Directory not found!\n", Red);
                        }
                    }

                    else {
                        clog("Cd subfunctions:", DarkGreen);
                        log(" cd (path) - Browses to directory\n" +
                            " cd.. - Browses to last directory\n");
                    }
                }

                else if (input == "dir" && disk) {
                    clog("Directory listing at " + Directory.GetCurrentDirectory(), Yellow);
                    var directoryList = VFSManager.GetDirectoryListing(Directory.GetCurrentDirectory());
                    var files = 0; var dirs = 0;
                    foreach (var directoryEntry in directoryList) {
                        if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + directoryEntry.mName))
                            clog("<Dir> " + directoryEntry.mName, Gray); dirs += 1;
                    }
                    foreach (var directoryEntry in directoryList) {
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\" + directoryEntry.mName))
                            clog("<File> " + directoryEntry.mName, Gray); files += 1;
                    }
                    clog("\nFound " + files + " files and " + dirs + " directories.\n", Yellow);
                }

                else if (input.StartsWith("copy")) {
                    var potato = string.Empty;
                    var potato1 = string.Empty;
                    try { potato = inputBeforeLower.Split(" ")[1]; potato1 = inputBeforeLower.Split(" ")[2]; }
                    catch { clog("No arguments\n", Red); CanContinue = false; }

                    if (CanContinue) {
                        var Contents = File.ReadAllText(potato);
                        File.Create(potato1);
                        File.WriteAllText(potato1, Contents);
                        clog("Copy process was successful!\n", Gray);
                    }
                }

                else if (input.StartsWith("lb") && disk)
                    fs.SetFileSystemLabel(rootdir, inputBeforeLower.Split("lb ")[1]);

                else if (input == "diskinfo" && disk) {
                    long availableSpace = VFSManager.GetAvailableFreeSpace(@"0:\");
                    long diskSpace = VFSManager.GetTotalSize(@"0:\");
                    string fsType = VFSManager.GetFileSystemType("0:\\");
                    clog("Disk info for " + fs.GetFileSystemLabel(rootdir), Yellow);
                    if (diskSpace < 1000000) //Less than 1mb
                        clog("\nDisk space: " + availableSpace / 1000 + " KB free out of " + diskSpace / 1000 + " KB total", Yellow);
                    else if (diskSpace > 1000000) //More than 1mb
                        clog("\nDisk space: " + availableSpace / 1e+6 + " MB free out of " + diskSpace / 1e+6 + " MB total", Yellow);
                    else if (diskSpace > 1e+9) //More than 1gb
                        clog("\nDisk space: " + availableSpace / 1e+9 + " GB free out of " + diskSpace / 1e+9 + " GB total", Yellow);
                    clog("\nFilesystem type: " + fsType + "\n", Yellow);
                }

                else if (input == "format" && disk)
                    Directory.Delete(@"0:\", true);

                #endregion

                else if (input == "i like goos")
                    clog("Fuck you, ChaOS is 10 times better.\n", Red);

                else clog("Unknown command.\n", Red);
            }

            catch (Exception ex) { ExHandler.Crash(ex); }
        }
    }
}