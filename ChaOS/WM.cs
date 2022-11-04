using Cosmos.System.Graphics;
using System;
using Sys = Cosmos.System;
using System.Drawing;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using Cosmos.Core.Memory;

namespace ChaOS
{
    internal class WM
    {
        [ManifestResourceStream(ResourceName = "ChaOS.Resources.red_button.bmp")] 
        private static byte[] b_byte;
        private static Bitmap red_button = new Bitmap(b_byte);
        public static Canvas canvas;
        private static PCScreenFont f = PCScreenFont.Default;
        private static Pen black = new Pen(Color.Black);
        private static Pen white = new Pen(Color.White);
        private static Pen notdarkgray = new Pen(Color.DarkGray);
        private static Pen background = new Pen(Color.DimGray);
        private static String WM_text;
        public static Boolean WM_shown = true;

        public static void InitGUI()
        {
            Sys.MouseManager.MouseSensitivity = 0.5F;
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));
            canvas.Clear();
            Kernel.gui = true;
        }
        private static void DesktopUpdate()
        {
            if (!WM_shown)
                Button(0, 465, WM_text, notdarkgray, black, null);
        }
        public static void Window(int x, int y, int w, int h, string title)
        {
            WM_text = title;
            canvas.DrawFilledRectangle(new Pen(Color.LightGray), x, y, w, h);
            canvas.DrawRectangle(black, x, y, w, h);
            if (title.Length != 0)
            {
                canvas.DrawFilledRectangle(new Pen(Color.White), x, y, w, 15);
                canvas.DrawRectangle(black, x, y, w, 15);
            }
            canvas.DrawString(title, f, black, new Sys.Graphics.Point(x + 1, y + 1));
            Button(x + w - 12, y + 3, null, null, null, red_button);
        }
        public static void ChaBar()
        {
            canvas.DrawFilledRectangle(black, 0, 0, 640, 15); //ChaBar¡
            Button(0, 0, "Exit GUI", notdarkgray, white, null);
            canvas.DrawString("ChaBar (Alpha)", f, white, new Sys.Graphics.Point(264, 1));
            byte hour = Cosmos.HAL.RTC.Hour; byte minute = Cosmos.HAL.RTC.Minute;
            canvas.DrawString(Convert.ToString(hour + ":" + minute), f, white, new Sys.Graphics.Point(600, 1));
        }
        public static void Button(int x, int y, string text, Pen backgroundcolor, Pen textcolor, Bitmap image)
        {
            int w = text.Length * 8 + 1; int h = 15;
            canvas.DrawFilledRectangle(backgroundcolor, x, y, w, h);
            if (image != null) {
                canvas.DrawImageAlpha(image, x, y);
                w = (int)image.Width; h = (int)image.Height; }
            canvas.DrawString(text, f, textcolor, new Sys.Graphics.Point(x, y + 1));
            if (Sys.MouseManager.MouseState == Sys.MouseState.Left && Sys.MouseManager.X < x + w && Sys.MouseManager.X > x && Sys.MouseManager.Y < y + h && Sys.MouseManager.Y > y) {
                if (text != null)
                    ButtonPressed(text);
                else
                    WM_shown = false; }
        }
        private static void ButtonPressed(string action)
        {
            if (action == WM_text)
                WM_shown = true;
            if (action == "Exit GUI")
                Sys.Power.Reboot();
        }
        public static void Update()
        {
            Sys.MouseManager.ScreenWidth = (uint)canvas.Mode.Columns;
            Sys.MouseManager.ScreenHeight = (uint)canvas.Mode.Rows;
            DesktopUpdate();
            canvas.Display();
            ClearCanvas();
            Heap.Collect();
        }
        public static void ClearCanvas() => canvas.DrawFilledRectangle(background, new Sys.Graphics.Point(0, 0), canvas.Mode.Columns, canvas.Mode.Rows);
        public static void Text(int x, int y, int line, string text) => canvas.DrawString(text, f, black, new Sys.Graphics.Point(x + 1, y + 17 * line));
    }
}
