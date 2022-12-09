using System;
using System.IO;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using static ChaOS.Core;
using static System.ConsoleColor;

namespace ChaOS {
    public class DiskManager {
        public const string systempath = @"0:\SYSTEM";
        public const string userfile = @"0:\SYSTEM\USERFILE.SYS";
        public const string colorfile = @"0:\SYSTEM\COLOR.SYS";
        public const string rootdir = @"0:\";
        public static bool disk = true;

        public static void InitFS(CosmosVFS fs) {
            VFSManager.RegisterVFS(fs);

            try { Directory.SetCurrentDirectory(rootdir); }
            catch { disk = false; }

            if (disk) LoadSettings();
        }

        private static void LoadSettings() {
            if (Directory.Exists(systempath)) {
                if (File.Exists(userfile)) Kernel.username = File.ReadAllText(userfile);
                if (File.Exists(colorfile)) SetScreenColor((ConsoleColor)File.ReadAllBytes(colorfile)[0], (ConsoleColor)File.ReadAllBytes(colorfile)[1]);
            }
        }

        public static void SaveChangesToDisk() {
            clog("Writing unsaved changes to disk...", Gray);
            Directory.CreateDirectory(systempath);
            File.WriteAllText(userfile, Kernel.username);
            File.WriteAllBytes(colorfile, new byte[] { (byte)Console.BackgroundColor, (byte)Console.ForegroundColor } );
        }
    }
}
