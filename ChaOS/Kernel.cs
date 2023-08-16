using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using ChaOS.Apps;

// imperium ChaOS
// Copyright (c) 2023 imperium
// All rights reserved.

namespace ChaOS
{
    public class Kernel : Sys.Kernel
    {
        public static string version = "Alpha 1.0";

        protected override void BeforeRun()
        {
            WM.Initialize();

            WM.AddWindow(new TestApp());
        }

        protected override void Run()
        {
            WM.Render();
        }
    }
}
