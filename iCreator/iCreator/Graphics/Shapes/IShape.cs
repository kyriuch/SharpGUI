using OpenTK;
using System;

namespace iCreator.Graphics.Shapes
{
    internal interface IShape
    {
        void OnUpdateFrame(FrameEventArgs e);
        void OnRenderFrame(FrameEventArgs e);
        void OnResize(EventArgs e);
    }
}
