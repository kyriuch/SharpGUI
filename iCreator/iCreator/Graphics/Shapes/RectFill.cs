using System;
using Autofac;
using iCreator.Graphics.GLWrappers;
using iCreator.Providers;
using OpenTK;
using OpenTK.Graphics;

namespace iCreator.Graphics.Shapes
{
    internal sealed class RectFill : IShape
    {
        private readonly MainVao vao;

        public RectFill(Vector2 pos, Vector2 size, Color4 color)
        {
            vao = ContainerProvider.Scope.Resolve<MainVao>();
            vao.Initialize();

            vao.SetTopLeftCorner(new Vector2(pos.X, pos.Y), new Vector2(size.X, size.Y));
            vao.SetupColors(new Color4[] { color, color, color, color });
        }

        public void MoveLeftTopCorner(Vector2 cornerPos, Vector2 size)
        {
            vao.SetTopLeftCorner(cornerPos, size);
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            vao.OnUpdateFrame(e);
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            vao.OnRenderFrame(e);
        }

        public void OnResize(EventArgs e)
        {
            vao.OnResize(e);
        }
    }
}
