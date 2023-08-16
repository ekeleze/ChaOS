using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChaOS.Components;
using Cosmos.System;
using Cosmos.Core.Memory;
using PrismAPI.Hardware.GPU;
using PrismAPI.Graphics;

namespace ChaOS
{
    public static class WM
    {
        private static int framesToHeapCollect = HighResMode ? 10 : 20;

        private static int framesToUpdateLazyWindows = 50;

        public static Display Driver;

        public static bool HighResMode = false;

        public static List<Window> Windows = new List<Window>();

        public static void Initialize()
        {
            if (!HighResMode)
            {
                Driver = Display.GetDisplay(800, 600);
            }
            else
            {
                Driver = Display.GetDisplay(1920, 1080);
            }

            MouseManager.ScreenWidth = Driver.Width;
            MouseManager.ScreenHeight = Driver.Height;
        }

        public static void AddWindow(Window window)
        {
            window.Paint();
            Windows.Add(window);
        }

        public static void Render()
        {
            Driver.Clear(Color.CoolGreen);

            for (int i = 0; i < Windows.Count; i++)
            {
                if (i == Windows.Count - 1)
                {
                    Windows[i].Update();
                }
                else if (framesToUpdateLazyWindows <= 0)
                {
                    Windows[i].Update();
                }

                Driver.DrawImage(Windows[i].X, Windows[i].Y, Windows[i].Contents, false);
            }

            Driver.DrawImage((int)MouseManager.X, (int)MouseManager.Y, Resources.mouse, true);

            string fps = Driver.GetFPS() + " FPS";
            Driver.DrawString(Driver.Width - Resources.font.MeasureString(fps) - 10, Driver.Height - 16 - 10, fps, Resources.font, Color.White);

            Driver.Update();

            framesToHeapCollect--;
            framesToUpdateLazyWindows--;

            if (framesToHeapCollect <= 0)
            {
                Heap.Collect();
                framesToHeapCollect = HighResMode ? 10 : 20;
            }

            if (framesToUpdateLazyWindows <= 0)
            {
                framesToUpdateLazyWindows = 50;
            }
        }
    }
}
