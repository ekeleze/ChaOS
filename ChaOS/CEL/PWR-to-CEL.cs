using System.Collections.Generic;
using System.IO;
using Cosmos.System;
using Console = System.Console;

namespace ChaOS.CEL;

public class PWR_to_CEL
{
    public static void PTC(string main)
    {
        string cdir = Directory.GetCurrentDirectory();
        string mainFile = Directory.GetCurrentDirectory() + "/" + main;
        
        if (File.Exists(mainFile))
        {
            string[] lines = File.ReadAllLines(mainFile);
            List<string> newLines = new List<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith("let")) // Variable translating!
                {
                    string varNameAndContents = line.Remove(4);
                    string name = varNameAndContents.Split(' ')[0];
                    string contents = varNameAndContents.Split(' ')[2];
                    string contentsLower = contents.ToLower();

                    string newType = "l";
                    
                    if (contents.Contains('"'))
                    {
                        newType = "s";
                    }

                    else if (contentsLower.Contains("true") || contentsLower.Contains("false"))
                    {
                        newType = "b";
                    }

                    else if (int.TryParse(contents, out var n))
                    {
                        newType = "i";
                    }

                    else if (contents.Contains(','))
                    {
                        newType = "l";
                    }

                    string translatedLine = newType + ":" + name + ":" + contents;
                    newLines.Add(translatedLine);
                }
                else // The rest of the code. AAAAAAA
                {
                    
                }
            }
        }
    }
}