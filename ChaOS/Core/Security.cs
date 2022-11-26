using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.ConsoleColor;
using static ChaOS.Core;

namespace ChaOS.System {
    public class Security {
        public static string UsernameFromUser;
        public static string PasswordFromUser;
        public static string Username;
        public static string Password;
        public static void Login() {
            clog("***ChaOS Login***\n", Green);
            write("Username: "); UsernameFromUser = Console.ReadLine();
            write("\nPassword: "); PasswordFromUser = Console.ReadLine();

            string[] Userfile = File.ReadAllLines(Kernel.userfile);
            for (int i = 0; i < Userfile.Length + 1; i++) {
                if (Userfile[i].StartsWith("Username=")) {
                    Username = Userfile[i].Split("=")[1];
                    if (Userfile[i + 1].StartsWith("Password=")) {
                        Password = Userfile[i + 1].Split("=")[1];
                        if (UsernameFromUser == Username && PasswordFromUser == Password) {
                            Kernel.usr = Username;
                            Clear();
                            log("\nWelcome back to ChaOS!");
                            return;
                        }
                        else log("Invalid username or password.");
                    }
                    else throw new Exception("Userfile is invalid or corrupted!");
                }
                else if (Userfile[i].StartsWith("//")) { }

                else if (!Userfile.Contains("Username=")) throw new Exception("Userfile is invalid or corrupted!");
            }
        }
    }
}
