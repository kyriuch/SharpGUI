using iCreator.Graphics.Advanced;
using iCreator.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;

namespace iCreator.Elements
{
    public sealed class Label : Element
    {
        public override bool IsVisible { get; set; } = true;

        private readonly ILogger logger;
        private readonly Text text;

        internal Label(ILogger logger, Text text)
        {
            this.logger = logger;
            this.text = text;
        }

        internal void Setup(Vector2 pos, string text, Color4 textColor, int fontSize = 16)
        {
            this.text.Setup(text, new System.Drawing.PointF(pos.X, pos.Y), textColor, fontSize);
        }

        internal void UpdateText(string text)
        {
            this.text.SetText(text);
        }

        internal override void OnUpdateFrame(FrameEventArgs args)
        {
            text.OnUpdateFrame(args);
        }

        internal override void OnRenderFrame(FrameEventArgs args)
        {
            text.OnRenderFrame(args);
        }

        internal override void OnResize(EventArgs args)
        {
            text.OnResize(args);
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
        }

        internal override void OnMouseMove(MouseMoveEventArgs e)
        {
        }
    }
}
