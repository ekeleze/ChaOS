using System.Drawing;
using Mosa.DeviceSystem;

namespace NeoChaOS.Graphics
{
    public class Display
    {
        public SimpleBitFont DefaultFont { get; private set; }
        private FrameBuffer32 DisplayFrame { get; set; }
        public FrameBuffer32 BackFrame { get; set; }

        public IGraphicsDevice Driver { get; private set; }

        public uint Width { get; private set; }
        public uint Height { get; private set; }

        public Display(uint Width, uint Height)
        {
            this.Width = Width;
            this.Height = Height;

            Driver = Program.DeviceService.GetFirstDevice<IGraphicsDevice>().DeviceDriver as IGraphicsDevice;
            if (Driver == null) throw new System.NotSupportedException();

            Driver.SetMode((ushort)Width, (ushort)Height);

            DisplayFrame = Driver.FrameBuffer;
            BackFrame = DisplayFrame.Clone();
        }

        public void setFont(byte[] rawMeatyBytes, uint size, uint width, uint height, string charset)
        {
            if (charset != null)
            {
                this.DefaultFont = new SimpleBitFont("font", width, height, height, charset, rawMeatyBytes);
            }
            else
            {
                string DefaultCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
                this.DefaultFont = new SimpleBitFont("font", width, height, height, DefaultCharset, rawMeatyBytes);
                
                
            }

        }

        public void DrawPoint(uint x, uint y, Color color)
        {
            BackFrame.SetPixel((uint)color.ToArgb(), x, y);
        }

        public void DrawBuffer(uint x, uint y, FrameBuffer32 buffer, bool drawWithAlpha = false)
        {
            BackFrame.DrawBuffer(buffer, x, y, drawWithAlpha);
        }

        public void DrawString(uint x, uint y, string text, ISimpleFont font, Color color)
        {
            font.DrawString(BackFrame, (uint)color.ToArgb(), x, y, text);
        }

        public void DrawRectangle(uint x, uint y, uint width, uint height, Color color, bool fill)
        {
            if (fill) BackFrame.FillRectangle((uint)color.ToArgb(), x, y, width, height);
            else BackFrame.DrawRectangle((uint)color.ToArgb(), x, y, width, height, 1);
        }

        public bool IsInBounds(uint x1, uint x2, uint y1, uint y2, uint width, uint height)
        {
            return x1 >= x2 && x1 <= x2 + width && y1 >= y2 && y1 <= y2 + height;
        }

        public void Clear(Color color)
        {
            BackFrame.ClearScreen((uint)color.ToArgb());
        }

        public void Update()
        {
            DisplayFrame.CopyFrame(BackFrame);
            Driver.Update(0, 0, Width, Height);
        }
    }
}
