using System;
using Mosa.DeviceSystem;
using Mosa.Kernel.BareMetal;
using System.Drawing;

namespace NeoChaOS.Graphics
{
    public class Canvas
    {
        private FrameBuffer32 _buffer;

        private ConstrainedPointer _constrainedPointer;
        
        public Canvas(uint Width, uint Height)
        {
            Func<uint, uint, uint> offFuckingSet = (x, y) => x + y;
            
            uint Size = Width * Height * 4;
            
            _constrainedPointer = VirtualMemoryAllocator.AllocateMemory(Size);

            _buffer = new FrameBuffer32(_constrainedPointer, Width, Height, offFuckingSet);
        }
        
        public void DrawPoint(Color color, uint x, uint y)
        {
            _buffer.SetPixel((uint)color.ToArgb(), x, y);
        }
    }
}
