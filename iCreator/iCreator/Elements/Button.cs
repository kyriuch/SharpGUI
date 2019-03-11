using iCreator.Graphics.Advanced;
using iCreator.Graphics.Shapes;
using iCreator.Providers;
using iCreator.Utils;
using iCreator.Window;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;

namespace iCreator.Elements
{
    internal enum ButtonType
    {
        RectangleFill,
        RectangleNoFill,
        RoundedRectangleFill,
        RoundedRectangleNoFill
    }

    public sealed class Button : Element
    {
        public override bool IsVisible { get; set; } = true;

        private readonly ILogger logger;
        private readonly ICursorProvider cursorProvider;
        private readonly ApplicationWindow applicationWindow;
        private readonly Text textElement;

        private Vector2 pos;
        private Vector2 size;
        private IShape shape;

        private bool entered = false;

        public delegate void OnClickCallback();
        private event OnClickCallback onClickEvent;

        public void AddOnClickListener(OnClickCallback callback)
        {
            onClickEvent += callback;
        }

        internal Button(ILogger logger, ICursorProvider cursorProvider, ApplicationWindow applicationWindow, Text textElement)
        {
            this.logger = logger;
            this.cursorProvider = cursorProvider;
            this.applicationWindow = applicationWindow;
            this.textElement = textElement;
        }

        

        internal void Setup(Vector2 pos, string text, Vector2 size, Color4 color, Color4 textColor, int fontSize = 16, 
            ButtonType type = ButtonType.RoundedRectangleNoFill)
        {
            setupShape(type, pos, size, color);
            setupText(pos, text, textColor, fontSize);
        }

        internal override void OnUpdateFrame(FrameEventArgs args)
        {
            shape.OnUpdateFrame(args);
            textElement.OnUpdateFrame(args);
        }

        internal override void OnRenderFrame(FrameEventArgs args)
        {
            shape.OnRenderFrame(args);
            textElement.OnRenderFrame(args);
        }

        internal override void OnResize(EventArgs args)
        {
            shape.OnResize(args);
            textElement.OnResize(args);
        }

        internal override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            
        }

        internal override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            
        }

        internal override void OnMouseUp(MouseButtonEventArgs e)
        {
            
        }

        internal override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.X >= pos.X && e.X <= pos.X + size.X && 
                e.Y >= pos.Y && e.Y <= pos.Y + size.Y && onClickEvent != null)
            {
                onClickEvent();
                applicationWindow.Cursor = MouseCursor.Default;
            }
        }

        internal override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (e.X >= pos.X && e.X <= pos.X + size.X
                && e.Y >= pos.Y && e.Y <= pos.Y + size.Y)
            {
                applicationWindow.Cursor = cursorProvider.GetHandCursor();
                entered = true;
            }
            else if(entered)
            {
                applicationWindow.Cursor = MouseCursor.Default;

                entered = false;
            }
                
        }

        private void setupShape(ButtonType type, Vector2 pos, Vector2 size, Color4 color)
        {
            this.pos = pos;
            this.size = size;

            switch (type)
            {
                case ButtonType.RectangleFill:
                    {
                        shape = new RectFill(pos, size, color);

                        break;
                    }
                case ButtonType.RoundedRectangleFill:
                    {
                        shape = new RoundRectFill(pos, size, color);

                        break;
                    }
                case ButtonType.RoundedRectangleNoFill:
                    {
                        shape = new RoundRectNoFill(pos, size, color);

                        break;
                    }
                case ButtonType.RectangleNoFill:
                    {
                        shape = new RectNoFill(pos, size, color);

                        break;
                    }
            }
        }

        private void setupText(Vector2 pos, string text, Color4 textColor, int fontSize = 16)
        {
            Vector2 textSize = textElement.MeasureText(text, fontSize);
            float xOffset = (size.X - textSize.X) / 2f;
            float yOffset = (size.Y - textSize.Y) / 2f;

            textElement.Setup(text, new System.Drawing.PointF(pos.X + xOffset, pos.Y + yOffset + 2f), textColor, fontSize);
        }
    }
}
