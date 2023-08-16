using IL2CPU.API.Attribs;
using PrismAPI.Graphics;
using PrismAPI.Graphics.Fonts;

namespace ChaOS
{
    public static class Resources
    {
        [ManifestResourceStream(ResourceName = "ChaOS.Resources.Font.btf")] static byte[] rawFont;
        public static Font font = new Font(rawFont, 16);

        [ManifestResourceStream(ResourceName = "ChaOS.Resources.Mouse.bmp")] static byte[] rawMouse;
        public static Canvas mouse = Image.FromBitmap(rawMouse, false);

        [ManifestResourceStream(ResourceName = "ChaOS.Resources.Close.bmp")] static byte[] rawCloseButton;
        public static Canvas closeButton = Image.FromBitmap(rawCloseButton, false);
    }
}
