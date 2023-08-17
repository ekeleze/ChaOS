using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using ChaOS.Apps;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;

// imperium ChaOS
// Copyright (c) 2023 imperium
// All rights reserved.

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        public static string Version = "Alpha 1.0";

        protected override void BeforeRun()
        {
            // Not sure why, but CreateSystemFiles is broken.
            FS.Init.InitFs();
            
            WM.Initialize();

            WM.AddWindow(new TestApp());
        }

        protected override void Run()
        {
            WM.Render();
        }
    }
}
