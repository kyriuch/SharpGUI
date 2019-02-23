using System.Collections.Generic;

namespace iCreator.Models
{
    internal sealed class Theme
    {
        public string BackgroundColor { get; set; }
        public string PrimaryColor { get; set; }
        public string TextColor { get; set; }
        public string TooltipColor { get; set; }
    }

    internal sealed class View
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Theme Theme { get; set; }
        public string ThemeName { get; set; } // ThemeName is ignored when Theme was provided
        public List<Element> Elements { get; set; }
    }
}
