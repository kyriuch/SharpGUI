using iCreator.Graphics.Advanced;
using iCreator.Graphics.GLWrappers;
using iCreator.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;

namespace iCreator.Elements
{
    public sealed class Image : Element
    {
        public override bool IsVisible { get; set; } = true;

        private readonly ILogger logger;
        private readonly Texture texture;
        private readonly ColorTextureVao vao;

        private Vector2 pos;
        private Vector2 size;

        internal Image(ILogger logger, Texture texture, ColorTextureVao vao)
        {
            this.logger = logger;
            this.texture = texture;
            this.vao = vao;
        }

        internal void Setup(string fileName, Vector2 pos, Color4 color, float scale = 1.0f)
        {
            this.pos = pos;

            texture.BindFilename(fileName);

            size = new Vector2(scale * texture.Width, scale * texture.Height);

            vao.Initialize();

            vao.SetTopLeftCorner(new Vector2(pos.X, pos.Y), new Vector2(size.X, size.Y));
            vao.SetupColors(new Color4[] { color, color, color, color });
            vao.SetupTextureCoords(new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) });
        }

        internal override void OnUpdateFrame(FrameEventArgs args)
        {
            vao.OnUpdateFrame(args);
        }

        internal override void OnRenderFrame(FrameEventArgs args)
        {
            texture.Bind(TextureUnit.Texture0);
            vao.OnRenderFrame(args);
        }

        internal override void OnResize(EventArgs args)
        {
            vao.OnResize(args);
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

        internal override void OnMouseDown(MouseButtonEventArgs args)
        {
            if (args.X >= pos.X && args.X <= pos.X + size.X
                && args.Y >= pos.Y && args.Y <= pos.Y + size.Y)
            {
                // Image clicked
            }
        }

        internal override void OnMouseMove(MouseMoveEventArgs e)
        {

        }
    }
}
