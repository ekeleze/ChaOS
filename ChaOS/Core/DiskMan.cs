using System.IO;
using Cosmos.System.FileSystem.VFS;

namespace ChaOS
{
    public class DiskManager 
    {
        public const string systempath = @"0:\SYSTEM";
        public const string userfile = @"0:\SYSTEM\USERFILE.SYS";
        public const string root = @"0:\";
        public static bool disk;

        public static void Initialize()
        {
            VFSManager.RegisterVFS(Kernel.fs);

            try
            {
                Directory.SetCurrentDirectory(root);
                disk = true;
            }
            catch { disk = false; }

            if (disk)
            {
                if (!Directory.Exists(systempath))
                    Directory.CreateDirectory(systempath);
                else
                {
                    if (File.Exists(userfile))
                        Kernel.CurrentUser = File.ReadAllText(userfile);
                    else if (!File.Exists(userfile))
                    {
                        File.Create(userfile);
                        File.WriteAllText(userfile, Kernel.CurrentUser);
                    }
                }
            }
        }
    }
}
