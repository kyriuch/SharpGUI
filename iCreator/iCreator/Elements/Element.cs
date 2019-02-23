using OpenTK;
using OpenTK.Input;
using System;

namespace iCreator.Elements
{
    public abstract class Element
    {
        public abstract bool IsVisible { get; set; }
        internal abstract void OnUpdateFrame(FrameEventArgs e);
        internal abstract void OnRenderFrame(FrameEventArgs e);
        internal abstract void OnResize(EventArgs e);
        internal abstract void OnKeyUp(KeyboardKeyEventArgs e);
        internal abstract void OnKeyDown(KeyboardKeyEventArgs e);
        internal abstract void OnMouseUp(MouseButtonEventArgs e);
        internal abstract void OnMouseDown(MouseButtonEventArgs e);
        internal abstract void OnMouseMove(MouseMoveEventArgs e);
    }
}
