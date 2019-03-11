using iCreator.Graphics.Advanced;
using iCreator.Providers;
using iCreator.Utils;
using iCreator.Window;
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
        private readonly ApplicationWindow applicationWindow;
        private readonly ICursorProvider cursorProvider;
        private bool isClickable = false;
        private Vector2 size;
        private Vector2 pos;
        private bool entered = false;

        public delegate void OnClickCallback();
        private event OnClickCallback onClickEvent;

        public void AddOnClickListener(OnClickCallback callback)
        {
            onClickEvent += callback;
        }

        internal Label(ILogger logger, Text text, ApplicationWindow applicationWindow, ICursorProvider cursorProvider)
        {
            this.logger = logger;
            this.text = text;
            this.applicationWindow = applicationWindow;
            this.cursorProvider = cursorProvider;
        }

        internal void Setup(Vector2 pos, string text, Color4 textColor, int fontSize = 16, bool isClickable = false)
        {
            this.pos = pos;
            this.text.Setup(text, new System.Drawing.PointF(pos.X, pos.Y), textColor, fontSize);
            size = this.text.MeasureText(text, fontSize);
            this.isClickable = isClickable;
        }

        internal void UpdateText(string text)
        {
            this.text.SetText(text);
            size = this.text.MeasureText(text);
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
            if (e.X >= pos.X && e.X <= pos.X + size.X && 
                e.Y >= pos.Y && e.Y <= pos.Y + size.Y &&
                isClickable && onClickEvent != null)
            {
                onClickEvent();
                applicationWindow.Cursor = MouseCursor.Default;
            }
        }

        internal override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (e.X >= pos.X && e.X <= pos.X + size.X
                && e.Y >= pos.Y && e.Y <= pos.Y + size.Y && isClickable)
            {
                applicationWindow.Cursor = cursorProvider.GetHandCursor();
                entered = true;
            }
            else if (entered && isClickable)
            {
                applicationWindow.Cursor = MouseCursor.Default;

                entered = false;
            }
        }
    }
}
