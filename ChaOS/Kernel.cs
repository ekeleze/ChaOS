using System;
using System.IO;
using ChaOS.Misc;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using static ChaOS.Core;
using static ChaOS.DiskManager;
using static System.ConsoleColor;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        const string ver = "Release 1.1";
        const int copyright = 2022;

        readonly string[] contributors = 
        {
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

        protected override void BeforeRun()
        {
            try
            {
                log("Starting up ChaOS...");
                StartDisk(fs);

                Clear();
                log("Boot successful!\n");
                clog("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ", DarkGreen);
                log("\n" + ver + "\nCopyright (c) " + copyright + " Kastle Grounds\nType \"help\" to get started!");
                if (!disk)
                    log("No hard drive detected, ChaOS will continue without disk support");
                log();
            }
            catch (Exception ex) { ExHandler.FatalCrash(ex); }
        }

        protected override void Run()
        {
            try
            {
                if (disk) write(username + " (" + Directory.GetCurrentDirectory() + "): ");
                else write(username + " > ");

                inputBeforeLower = Console.ReadLine();         // Input before conversion
                inputCapitalized = inputBeforeLower.ToUpper(); // Input converted to uppercase
                input = inputBeforeLower.ToLower();            // Input converted to lowercase

                if (input.Equals("help"))
                {
                    var us = string.Empty;
                    var color = White;
                    if (!disk) { us = " (unavailable)"; color = Gray; }

                    clog("\nFunctions:", DarkGreen);
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
                    clog(" cd - Browses to folder, works as in MS-DOS" + us, color);
                    clog(" cd.. - Returns to root" + us, color);
                    clog(" dir - Lists files in the current folder" + us, color);
                    clog(" mkdir - Creates folder" + us, color);
                    clog(" touch - Creates file" + us, color);
                    clog(" rd - Removes folder" + us, color);
                    clog(" del - Removes file" + us, color);
                    clog(" lb - Relabels disk" + us, color);
                    clog(" notepad - Opens MIV notepad." + us, color);
                    clog(" format - Formats the disk.\n" + us, color);
                }

                //Miscellaneous

                else if (input.Contains("username"))
                {
                    if (input.Contains("set"))
                    {
                        try { username = input.Split("username set ")[1]; }
                        catch { clog("No arguments", Red); }
                        if (disk) File.WriteAllText(userfile, username);
                        clog("Done! (" + username + ")", Gray);
                    }
                    else
                    {
                        clog("Current username: " + username, Gray);
                    }
                }

                else if (input.Equals("credits"))
                {
                    log();
                    cwrite("C", Blue); cwrite("r", Green); cwrite("e", DarkYellow); cwrite("d", Red); cwrite("i", DarkMagenta); cwrite("t", Cyan); cwrite("s:\n", DarkRed);
                    foreach (string contributor in contributors) clog(" " + contributor, Yellow);
                    log();
                }

                #region Color functions

                else if (input.StartsWith("color"))
                {
                    string ClearScreen = "ChaOS " + ver + "\nCopyright (c) " + copyright + " Kastle Grounds\n";

                    if (input.Contains("list"))
                    {
                        clog("\nColor list:", Green);
                        write(" "); SetScreenColor(White, Black, false); write("black - Pure light mode, will make you blind"); SetScreenColor(Black, White, false);
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

                    else if (input.EndsWith("black")) SetScreenColor(White, Black); //Black

                    else if (input.EndsWith("dark blue"))
                    { //Dark blue
                        SetScreenColor(Black, DarkBlue);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark green"))
                    { //Dark green
                        SetScreenColor(Black, DarkGreen);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark cyan"))
                    { //Dark cyan
                        SetScreenColor(Black, DarkCyan);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark gray"))
                    { //Dark gray
                        SetScreenColor(Black, DarkGray);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("blue"))
                    { //Blue
                        SetScreenColor(Black, Blue);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("green"))
                    { //Green
                        SetScreenColor(Black, Green);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("cyan"))
                    { //Cyan
                        SetScreenColor(Black, Cyan);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark red"))
                    { //Dark red
                        SetScreenColor(Black, DarkRed);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark magenta"))
                    { //Dark magenta
                        SetScreenColor(Black, DarkMagenta);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("dark yellow"))
                    { //Dark yellow
                        SetScreenColor(Black, DarkYellow);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("gray"))
                    { //Gray
                        SetScreenColor(Black, Gray);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("red"))
                    { //Red
                        SetScreenColor(Black, Red);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("magenta"))
                    { //Magenta
                        SetScreenColor(Black, Magenta);
                        log(ClearScreen);
                    }
                    else if (!input.Contains("dark") && input.Contains("yellow"))
                    { //Yellow
                        SetScreenColor(Black, Yellow);
                        log(ClearScreen);
                    }
                    else if (input.EndsWith("white"))
                    { //White
                        SetScreenColor(Black, White);
                        log(ClearScreen);
                    }
                    else clog("\nPlease list colors by doing \"color list\" or set a color by doing \"color yellow\" (for example)\n", Gray);
                }

                #endregion

                else if (input.Equals("clear") || input.Equals("cls"))
                    Clear();

                else if (input == "time" || input == "t")
                    log("Current time is " + GetTime());

                else if (input == "notepad" && disk)
                    MIV.StartMIV();

                else if (input.Equals("shutdown") || input.Equals("sd"))
                {
                    clog("Shutting down...", Gray);
                    Sys.Power.Shutdown();
                }

                else if (input.Equals("reboot") || input.Equals("rb"))
                {
                    clog("Restarting...", Gray);
                    Sys.Power.Reboot();
                }

                else if (input.StartsWith("echo"))
                    log("\n" + inputBeforeLower.Split("echo ")[1] + "\n");

                #region File management

                else if (input.StartsWith("mkdir"))
                {
                    if (input.Contains("0:\\")) { input.Replace("0:\\", ""); }
                    try { input = input.Split("mkdir ")[1]; }
                    catch { clog("No arguments", Red); }
                    if (!Directory.Exists(input))
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\" + input);
                    else
                        clog("Directory already existing", Red);
                }

                else if (input.StartsWith("touch"))
                {
                    if (input.Contains("0:\\")) { input.Replace("0:\\", ""); }
                    try { input = input.Split("touch ")[1]; }
                    catch { clog("No arguments", Red); }
                    if (!File.Exists(input))
                        File.Create(Directory.GetCurrentDirectory() + @"\" + input);
                    else
                        clog("File already existing!", Red);
                }

                else if (input.StartsWith("rd"))
                {
                    if (input.Contains("0:\\")) { input.Replace(@"0:\", ""); }
                    try { input = input.Split("rd ")[1]; }
                    catch { clog("No arguments", Red); }
                    if (Directory.Exists(Directory.GetCurrentDirectory() + @"\" + input))
                        Directory.Delete(Directory.GetCurrentDirectory() + @"\" + input, true);
                    else if (!Directory.Exists(input))
                        clog("Directory not found!", Red);
                }

                else if (input.StartsWith("del"))
                {
                    if (input.Contains("0:\\")) { input.Replace(@"0:\", ""); }
                    try { input = input.Split("del ")[1]; }
                    catch { clog("No arguments", Red); }
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\" + input))
                        File.Delete(Directory.GetCurrentDirectory() + @"\" + input);
                    else if (!File.Exists(input))
                        clog("File not found!", Red);
                }

                else if (input.Contains("cd") && disk)
                {
                    if (input == "cd..") Directory.SetCurrentDirectory(root);

                    else
                    {
                        inputCapitalized = inputCapitalized.Split("CD ")[1];
                        if (inputCapitalized.Contains(@"0:\")) { inputCapitalized.Replace(@"0:\", ""); }
                        if (Directory.GetCurrentDirectory() != root) { inputCapitalized = @"\" + inputCapitalized; }
                        if (Directory.Exists(Directory.GetCurrentDirectory() + @"\" + inputCapitalized))
                            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + inputCapitalized);
                        else clog("Directory not found!", Red);
                    }
                }

                else if (input.Equals("dir") && disk)
                {
                    clog("\nDirectory listing at " + Directory.GetCurrentDirectory(), Yellow);
                    var directoryList = VFSManager.GetDirectoryListing(Directory.GetCurrentDirectory());
                    var files = 0;
                    foreach (var directoryEntry in directoryList)
                    {
                        if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + directoryEntry.mName))
                            clog("<Dir> " + directoryEntry.mName, Gray);
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\" + directoryEntry.mName))
                            clog("<Fil> " + directoryEntry.mName, Gray);
                        files += 1;
                    }
                    clog("\nFound " + files + " elements\n", Yellow);
                }

                else if (input.StartsWith("copy"))
                {
                    var potato = inputBeforeLower.Split(" ")[1];
                    var potato1 = inputBeforeLower.Split(" ")[2];
                    var Contents = File.ReadAllText(potato);
                    File.Create(potato1);
                    File.WriteAllText(potato1, Contents);
                    clog("\nAction successful\n", Blue);
                }

                else if (input.StartsWith("lb") && disk)
                    fs.SetFileSystemLabel(root, inputBeforeLower.Split("lb ")[1]);

                else if (input.Equals("diskinfo") && disk)
                {
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

                else if (input.Equals("format") && disk)
                    Directory.Delete(@"0:\", true);

                #endregion

                else clog("\nUnknown command.\n", Red);
            }

            catch (Exception ex) { ExHandler.Crash(ex); }
        }
    }
}