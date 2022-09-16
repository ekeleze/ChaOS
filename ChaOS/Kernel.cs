using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        string usr = "usr";

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("Boot successful...\n\n");
            Console.WriteLine("Welcome to...");
            Console.WriteLine("  ______   __                   ______    ______  \n /      \\ |  \\                 /      \\  /      \\ \n|  $$$$$$\\| $$____    ______  |  $$$$$$\\|  $$$$$$\\\n| $$   \\$$| $$    \\  |      \\ | $$  | $$| $$___\\$$\n| $$      | $$$$$$$\\  \\$$$$$$\\| $$  | $$ \\$$    \\ \n| $$   __ | $$  | $$ /      $$| $$  | $$ _\\$$$$$$\\\n| $$__/  \\| $$  | $$|  $$$$$$$| $$__/ $$|  \\__| $$\n \\$$    $$| $$  | $$ \\$$    $$ \\$$    $$ \\$$    $$\n  \\$$$$$$  \\$$   \\$$  \\$$$$$$$  \\$$$$$$   \\$$$$$$ ");
            Console.WriteLine("Private beta 1.4");
            //var available_space = VFSManager.GetAvailableFreeSpace(@"0:\");
            //Console.WriteLine("Available Storage: " + available_space);
            Console.WriteLine("\nType \"help\" to get started!\n\n");
        }

        protected override void Run()
        {
            //Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
            //Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            
            string syspath = @"0:\system";
            string usrfile = "usrnam.txt";

            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            

            Console.Write(usr);
            Console.Write(" > ");
            var input = Console.ReadLine();

            if (input.Contains("help", comp))
            {
                if (!input.Contains("info"))
                {
                    Console.WriteLine("\nFunctions:");
                    Console.WriteLine(" help - Shows all functions.");
                    Console.WriteLine(" username - Allows you to use usernames, use the info command for more info.");
                    Console.WriteLine(" info function-here - Shows more detail about commands.");
                    Console.WriteLine(" credits - Shows all of the wonderful people that make ChaOS work.");
                    Console.WriteLine(" clear - Clears the screen.");
                    //
                    Console.WriteLine("");
                }
            }

            if (input.Contains("username", comp))
            {
                if (!input.Contains("info"))
                {
                    if (input.Contains("current"))
                    {
                        Console.Write("\nCurrent username: ");
                        Console.Write(usr);
                        Console.Write("\n\n");
                    }

                    else
                    {
                        if (input.Contains("change"))
                        {
                            var text = input;
                            var start = text.IndexOf("\"") + 1;//add one to not include quote
                            var end = text.LastIndexOf("\"") - start;
                            var nur = text.Substring(start, end);
                            usr = nur;
                        }
                        else
                        {
                            Console.WriteLine("\nusername SubFunctions:\n current - shows current username.\n change \"username-here\" - changes current username.\n");
                        }
                    }
                }
            }

            if (input.Contains("info"))
            {
                if (input.Contains("help"))
                {
                    Console.WriteLine("\nFunction: help\nShows all functions.\n");
                }

                if (input.Contains("username"))
                {
                    Console.WriteLine("\nFunction: username\nSubFunctions: current, change");
                    Console.WriteLine("Allows you to change, or view the current username by using the SubFunctions.\n");
                }
                
                if (input.Contains("info info"))
                {
                    Console.WriteLine("\nFunction: info\nGives info on functions.\n");
                }

                if (input == "info")
                {
                    Console.WriteLine("\nPlease specify the function at the end of the \"info\" command.\n");
                }

                if (input.Contains("credits"))
                {
                    Console.WriteLine("\nFunction: credits\nShows all of the wonderful people that make ChaOS work.\n");
                }
            }

            if (input.Contains("credits"))
            {
                if (!input.Contains("info"))
                {
                    Console.WriteLine("\nCredits:\nekeleze - Owner\nMrDumbrava - Contributor\n");
                }
            }

            if (input.Contains("clear"))
            {
                Console.Clear();
            }
        }
    }
}