using Autofac;
using iCreator.Graphics.Advanced;
using iCreator.Graphics.GLWrappers;
using iCreator.Providers;
using iCreator.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace iCreator.Graphics.Shapes
{
    internal sealed class RoundRectNoFill : IShape
    {
        private readonly List<ColorTextureVao> edges;
        private readonly List<RectFill> fill;
        private static Texture edgeTexture;

        public RoundRectNoFill(Vector2 pos, Vector2 size, Color4 color)
        {
            if (edgeTexture == null)
            {
                edgeTexture = ContainerProvider.Scope.Resolve<Texture>();
                edgeTexture.BindFilename("iCreator\\Resources\\edge_border.png");
            }

            if (size.X < 12 || size.Y < 12)
            {
                ILogger logger = ContainerProvider.Scope.Resolve<ILogger>();

                logger.Warning("Better provide size larger or equal to 11x11.");
            }

            edges = new List<ColorTextureVao>();
            fill = new List<RectFill>();

            createEdges(pos, size, color);
            createFill(pos, size, color);
        }

        public void OnUpdateFrame(FrameEventArgs args)
        {
            edges.ForEach(vao => vao.OnUpdateFrame(args));
            fill.ForEach(rect => rect.OnUpdateFrame(args));
        }

        public void OnRenderFrame(FrameEventArgs args)
        {
            edgeTexture.Bind(TextureUnit.Texture0);
            edges.ForEach(vao => vao.OnRenderFrame(args));
            fill.ForEach(rect => rect.OnRenderFrame(args));
        }

        public void OnResize(EventArgs args)
        {
            edges.ForEach(vao => vao.OnResize(args));
            fill.ForEach(rect => rect.OnResize(args));
        }

        private void createEdges(Vector2 pos, Vector2 size, Color4 color)
        {
            ColorTextureVao leftTop = ContainerProvider.Scope.Resolve<ColorTextureVao>();
            ColorTextureVao leftBottom = ContainerProvider.Scope.Resolve<ColorTextureVao>();
            ColorTextureVao rightBottom = ContainerProvider.Scope.Resolve<ColorTextureVao>();
            ColorTextureVao rightTop = ContainerProvider.Scope.Resolve<ColorTextureVao>();

            leftTop.Initialize();
            leftBottom.Initialize();
            rightBottom.Initialize();
            rightTop.Initialize();

            leftTop.SetTopLeftCorner(new Vector2(pos.X, pos.Y), new Vector2(13, 13));
            leftBottom.SetTopLeftCorner(new Vector2(pos.X, pos.Y + size.Y - 11), new Vector2(13, 13));
            rightBottom.SetTopLeftCorner(new Vector2(pos.X + size.X - 11, pos.Y + size.Y - 11), new Vector2(13, 13));
            rightTop.SetTopLeftCorner(new Vector2(pos.X + size.X - 11, pos.Y), new Vector2(13, 13));

            Color4[] colors = new Color4[] { color, color, color, color };

            leftTop.SetupColors(colors);
            leftBottom.SetupColors(colors);
            rightBottom.SetupColors(colors);
            rightTop.SetupColors(colors);

            leftTop.SetupTextureCoords(new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) });
            leftBottom.SetupTextureCoords(new Vector2[] { new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) });
            rightBottom.SetupTextureCoords(new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1) });
            rightTop.SetupTextureCoords(new Vector2[] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, 0) });

            edges.Add(leftTop);
            edges.Add(leftBottom);
            edges.Add(rightBottom);
            edges.Add(rightTop);
        }

        private void createFill(Vector2 pos, Vector2 size, Color4 color)
        {
            RectFill leftRectangle = new RectFill(
                new Vector2(pos.X + 1, pos.Y + 12),
                new Vector2(1, size.Y - 22),
                color);

            RectFill topRectangle = new RectFill(
                new Vector2(pos.X + 12, pos.Y + 1),
                new Vector2(size.X - 22, 1),
                color);

            RectFill rightRectangle = new RectFill(
                new Vector2(pos.X + size.X, pos.Y + 12),
                new Vector2(1, size.Y - 22),
                color);

            RectFill bottomRectangle = new RectFill(
                new Vector2(pos.X + 12, pos.Y + size.Y),
                new Vector2(size.X - 22, 1),
                color);


            fill.Add(leftRectangle);
            fill.Add(topRectangle);
            fill.Add(rightRectangle);
            fill.Add(bottomRectangle);
        }
    }
}
