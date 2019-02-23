
namespace iCreator.Utils
{
    internal interface IColorHelper
    {
        OpenTK.Graphics.Color4 ToTKColor(System.Drawing.Color color);
        OpenTK.Graphics.Color4 ToTKColor(string color);
        System.Drawing.Color ToSystemColor(OpenTK.Graphics.Color4 color);
        System.Drawing.Color ToSystemColor(string color);
    }

    internal sealed class ColorHelper : IColorHelper
    {
        public OpenTK.Graphics.Color4 ToTKColor(System.Drawing.Color color) => new OpenTK.Graphics.Color4(color.R, color.G, color.B, color.A);
        public OpenTK.Graphics.Color4 ToTKColor(string color) => ToTKColor(ToSystemColor(color));
        public System.Drawing.Color ToSystemColor(OpenTK.Graphics.Color4 color) => System.Drawing.Color.FromArgb(color.ToArgb());
        public System.Drawing.Color ToSystemColor(string color) => System.Drawing.ColorTranslator.FromHtml(color);
    }
}
