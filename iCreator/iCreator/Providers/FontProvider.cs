using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace iCreator.Providers
{
    internal interface IFontProvider
    {
        FontFamily GetRobotoFontFamily();
    }

    internal sealed class FontProvider : IFontProvider
    {
        private readonly PrivateFontCollection collection;
        private readonly FontFamily robotoFontFamily;

        public FontProvider()
        {
            collection = new PrivateFontCollection();
            collection.AddFontFile($"{ Directory.GetCurrentDirectory() }\\iCreator\\Resources\\Roboto-Regular.ttf");
            robotoFontFamily = new FontFamily("Roboto", collection);
        }

        public FontFamily GetRobotoFontFamily() => robotoFontFamily;
    }
}
