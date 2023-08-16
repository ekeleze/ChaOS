using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChaOS.Components;
using PrismAPI.Graphics;

namespace ChaOS.Apps
{
    public class TestApp : Window
    {
        public TestApp() : base(50, 50, 200, 200, "Test App") { }

        public override void Paint()
        {
            base.Paint();

            Contents.DrawString(10, 32, "Hello, world!", Resources.font, Color.Black);
        }
    }
}
