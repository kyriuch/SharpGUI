using System;
using iCreator.Providers;
using iCreator.Utils;
using iCreator.Window;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace iCreator.Elements
{
    public sealed class Checkbox : Element
    {
        public override bool IsVisible { get; set; } = true;

        private ApplicationWindow applicationWindow;
        private ICursorProvider cursorProvider;
        private ILogger logger;

        private Image checkedImage;
        private Image uncheckedImage;
        private bool isChecked = false;
        private Vector2 pos;
        private bool entered = false;
        private float checkboxSize = 36;

        public delegate void OnSetCheckedCallback(bool value);
        private event OnSetCheckedCallback onSetCheckedCallback;

        public void AddOnSetCheckedListener(OnSetCheckedCallback callback)
        {
            onSetCheckedCallback += callback;
        }

        public bool IsChecked() => isChecked;

        public void SetChecked(bool value)
        {
            isChecked = value;
            onSetCheckedCallback?.Invoke(value);
        }

        internal Checkbox(ILogger logger, Image checkedImage, Image uncheckedImage, ApplicationWindow applicationWindow, ICursorProvider cursorProvider)
        {
            this.logger = logger;
            this.checkedImage = checkedImage;
            this.uncheckedImage = uncheckedImage;
            this.applicationWindow = applicationWindow;
            this.cursorProvider = cursorProvider;
        }

        internal void Setup(Vector2 pos, Color4 color)
        {
            this.pos = pos;

            checkedImage.Setup("iCreator\\Resources\\checkbox_checked.png", pos, color, 1);
            uncheckedImage.Setup("iCreator\\Resources\\checkbox_unchecked.png", pos, color, 1);
        }

        internal override void OnKeyDown(KeyboardKeyEventArgs e)
        {

        }

        internal override void OnKeyUp(KeyboardKeyEventArgs e)
        {

        }

        internal override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.X >= pos.X && e.X <= pos.X + checkboxSize &&
                e.Y >= pos.Y && e.Y <= pos.Y + checkboxSize)
            {
                SetChecked(!isChecked);
            }
        }

        internal override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (e.X >= pos.X && e.X <= pos.X + checkboxSize
                && e.Y >= pos.Y && e.Y <= pos.Y + checkboxSize)
            {
                applicationWindow.Cursor = cursorProvider.GetHandCursor();
                entered = true;
            }
            else if (entered)
            {
                applicationWindow.Cursor = MouseCursor.Default;

                entered = false;
            }
        }

        internal override void OnMouseUp(MouseButtonEventArgs e)
        {

        }

        internal override void OnRenderFrame(FrameEventArgs e)
        {
            if (IsVisible)
            {
                if (isChecked)
                {
                    checkedImage.OnRenderFrame(e);
                }
                else
                {
                    uncheckedImage.OnRenderFrame(e);
                }
            }
        }

        internal override void OnResize(EventArgs e)
        {
            uncheckedImage.OnResize(e);
            checkedImage.OnResize(e);
        }

        internal override void OnUpdateFrame(FrameEventArgs e)
        {
            checkedImage.OnUpdateFrame(e);
            uncheckedImage.OnUpdateFrame(e);
        }
    }
}
