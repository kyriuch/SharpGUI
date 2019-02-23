namespace iCreator.Models
{
    internal struct Vector2
    {
        public float X, Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    internal sealed class Element
    {
        public string Name { get; set; } // Element name that is checked inside Controller
        public string Type { get; set; } // Button, Image, Label, TextField for now
        public string Text { get; set; }
        public Vector2 Position { get; set; } // Top left corner
        public Vector2 Size { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public int FontSize { get; set; }
        public string Tooltip { get; set; } // Tooltip tip for TextField
        public string TooltipColor { get; set; }
        public float Scale { get; set; } // Scale for image
        public string FileName { get; set; } // Filename of an image
        public int ShapeType { get; set; } // Type of a shape for element
        public int TextType { get; set; } // 0 - Text   1 - Password
    }
}
