using System.IO;
using System.Linq;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;

namespace ChaOS.FS;

public static class Init
{
    // Ladies and Gentleman, it is my honor to present to you, the root directory: 
    public static string RootDir = @"0:\";
    
    // The file system, not sure what to comment here
    public static CosmosVFS Fs = new CosmosVFS();

    public static void InitFs()
    {
        // Register the FS
        VFSManager.RegisterVFS(Fs);
        
        // Make sure there is a disk
        try
        {
            Directory.GetFiles(RootDir);
        }
        catch
        {
            Format.FormatDisk();
        }
        
        // Get the directories in the root directory, put them in an array
        string[] rootFiles = Directory.GetDirectories(RootDir);

        // Check for the *System directory
        // */Asterisk hides the files. sorta like . in windows or linux.
        // Seems random but i like it this way personally
        if (!rootFiles.Contains("System"))
        {
            //CreateSystemFiles();
        }
    }
    
    // As the comment in Kernel.cs points out, this seems to not work.
    private static void CreateSystemFiles()
    {
        // Check for and make system directories. Im not going to comment each one individually.
        string[] rootFiles = Directory.GetDirectories(RootDir);
        
        string[] systemFiles = Directory.GetDirectories(RootDir + "\\System");
        
        if (!rootFiles.Contains("System"))
        {
            Directory.CreateDirectory(RootDir + "\\System");
        }
        
        if (!systemFiles.Contains("appData"))
        {
            Directory.CreateDirectory(RootDir + "\\System\\appdata");
        }
        
        if (!systemFiles.Contains("Config"))
        {
            Directory.CreateDirectory(RootDir + "\\System\\Config");
        }
    }
}