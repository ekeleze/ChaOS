using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using System.IO;
using static ChaOS.Core;
using static System.ConsoleColor;

namespace ChaOS
{
    public class DiskManager
    {
        public const string systempath = @"0:\SYSTEM";
        public const string rootdir = @"0:\";
        public static bool disk = true;

        public class Files
        {
            public const string userfile = @"0:\SYSTEM\USERFILE.SYS";
            public const string colorfile = @"0:\SYSTEM\COLOR.SYS";
        };

        public static void InitFS(CosmosVFS fs)
        {
            VFSManager.RegisterVFS(fs);
            try { Directory.GetFiles(rootdir); }
            catch { disk = false; }
        }

        public static void LoadSettings()
        {
            if (disk)
            {
                if (Directory.Exists(systempath))
                {
                    if (File.Exists(Files.userfile)) Kernel.username = File.ReadAllText(Files.userfile);
                    if (File.Exists(Files.colorfile)) SetScreenColor((ConsoleColor)File.ReadAllBytes(Files.colorfile)[0], (ConsoleColor)File.ReadAllBytes(Files.colorfile)[1], false);
                }
                else
                {
                    FirstTimeSetup();
                }
            }
        }

        public static void FirstTimeSetup()
        {
            if (disk)
            {
                clog("Welcome to ChaOS!", DarkGreen);
                write("Please type the username you are going to use: ");
                Kernel.username = Console.ReadLine();
                SaveChangesToDisk(true);
            }
        }

        public static void SaveChangesToDisk(bool isFirstTimeSetup = false)
        {
            clog("Saving settings to disk...", Gray);
            Directory.CreateDirectory(systempath);
            File.WriteAllText(Files.userfile, Kernel.username);
            File.WriteAllBytes(Files.colorfile, new byte[] { (byte)Console.BackgroundColor, (byte)Console.ForegroundColor });
        }
    }
}
