using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System;
using PrismAPI.Graphics;

namespace ChaOS.Components
{
    public class Window
    {
        /* TODO:
         * 1. Implement close, maximize and minimize buttons.
         * 2. Implement moving windows.
         */

        public int X; // X loaction of the window.
        public int Y; // Y loaction of the window.
        public int Width; // Width of the window.
        public int Height; // Height of the window.
        public string Title; // Title of the window.
        public Canvas Contents; //Graphical contents of the window.

        public static Color BackgroundColor = new Color(232, 255, 255); // Default window background color.
        public static Color BorderColor = new Color(63, 73, 73); // Default window border color.

        // Window constructor.
        public Window(int X, int Y, int Width, int Height, string Title)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Title = Title;
            Contents = new Canvas((ushort)Width, (ushort)Height);
        }
        
        // Paint the window.
        public virtual void Paint()
        {
            Contents.Clear(BackgroundColor); // Clear the canvas.
            Contents.DrawRectangle(0, 21, Convert.ToUInt16(Contents.Width - 1), Convert.ToUInt16(Contents.Height - 22), 0, BorderColor); // Render the border.
            Contents.DrawFilledRectangle(0, 0, Contents.Width, 21, 0, BorderColor); // Render the title bar.
            Contents.DrawString(2, 2, Title, Resources.font, Color.White); // Title.
            Contents.DrawImage(Contents.Width - Resources.closeButton.Width - 2, 2, Resources.closeButton, false); // Close button. NOT WORKING CAUSES OS TO CRASH wait it doesnt anymore what
        }

        // Handle the window, runs every frame.
        public virtual void Update()
        {
            
        }

        /*private bool IsMouseOverTitleBar
        {
            get
            {
                return MouseManager.X > X && MouseManager.X 
            }
        }*/
    }
}
