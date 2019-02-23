using Autofac;
using iCreator.Providers;
using iCreator.Utils;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace iCreator.Graphics.Shapes
{
    internal sealed class RectNoFill : IShape
    {
        private readonly List<RectFill> sides;

        public RectNoFill(Vector2 pos, Vector2 size, Color4 color)
        {
            ILogger logger = ContainerProvider.Scope.Resolve<ILogger>();

            sides = new List<RectFill>();

            createSides(pos, size, color);
        }

        public void OnUpdateFrame(FrameEventArgs args)
        {
            sides.ForEach(rect => rect.OnUpdateFrame(args));
        }

        public void OnRenderFrame(FrameEventArgs args)
        {
            sides.ForEach(rect => rect.OnRenderFrame(args));
        }

        public void OnResize(EventArgs args)
        {
            sides.ForEach(rect => rect.OnResize(args));
        }

        private void createSides(Vector2 pos, Vector2 size, Color4 color)
        {
            RectFill topSide = new RectFill(
                new Vector2(pos.X, pos.Y),
                new Vector2(size.X, 1),
                color);

            RectFill leftSide = new RectFill(
                new Vector2(pos.X, pos.Y),
                new Vector2(1, size.Y),
                color);

            RectFill bottomSide = new RectFill(
                new Vector2(pos.X, pos.Y + size.Y),
                new Vector2(size.X, 1),
                color);

            RectFill rightSide = new RectFill(
                new Vector2(pos.X + size.X - 1, pos.Y),
                new Vector2(1, size.Y),
                color);


            sides.Add(topSide);
            sides.Add(leftSide);
            sides.Add(bottomSide);
            sides.Add(rightSide);
        }
    }
}
