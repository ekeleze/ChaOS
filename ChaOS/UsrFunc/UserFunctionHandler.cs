using Cosmos.System;
using Console = System.Console;
using static ChaOS.Kernel;

namespace ChaOS.UsrCmds;

public class UserFunctionHandler
{
    // Please, for other developer's sake, make your functions' code in the UserCSfiles directory and just call it here.
    // This file should only be used for allowing your function to run.
    // Functions should always be only lowercase in this file's code, as i have set it so the command is set as lowercase only to allow for no case sensitivity

    public void Try(string Command)
    {
        // User created function example
        if (Command.StartsWith("userfunc")) // The string here is the function to type in the OS
        {
            Console.WriteLine("UserFunc is working.\n"); // This will be printed when the function is run

            Kernel.UserCMDreturn = "Known"; // Return weather the command is known or not.
            
            // in your case the code (lines 17-19) should be in their own .CS file in the UserCSfiles project directory.
        }
        // Put your commands below!
        
        
        
        else
        {
            Kernel.UserCMDreturn = "Unknown";
        }
    }
}