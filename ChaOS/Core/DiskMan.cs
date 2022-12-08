using System.IO;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;

namespace ChaOS {
    public class DiskManager {
        public const string systempath = @"0:\SYSTEM";
        public const string userfile = @"0:\SYSTEM\USERFILE.SYS";
        public const string rootdir = @"0:\";
        public static bool disk = true;

        public static void InitFS(CosmosVFS fs) {
            VFSManager.RegisterVFS(fs);

            try { Directory.SetCurrentDirectory(rootdir); }
            catch { disk = false; }

            if (disk) {
                if (!Directory.Exists(systempath))
                    Directory.CreateDirectory(systempath);
                else {
                    if (File.Exists(userfile))
                        Kernel.username = File.ReadAllText(userfile);
                    else if (!File.Exists(userfile)) {
                        File.Create(userfile);
                        File.WriteAllText(userfile, Kernel.username);
                    }
                }
            }
            else {
                Kernel.username = "usr";
            }
        }
    }
}
