using iCreator.Graphics.GLWrappers;
using iCreator.Providers;
using iCreator.Utils;
using iCreator.Window;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;

namespace iCreator.Graphics.Advanced
{
    internal sealed class Text
    {
        private readonly ILogger logger;
        private readonly IFontProvider fontProvider;
        private readonly ColorTextureVao vao;
        private readonly int textureId;

        private Font font;
        private Rectangle usedRegion;
        private Bitmap bitmap;
        private Brush brush;
        private PointF point;
        private System.Drawing.Graphics graphics;

        public Text(ILogger logger, IFontProvider fontProvider, ApplicationWindow applicationWindow, ColorTextureVao vao)
        {
            this.logger = logger;
            this.fontProvider = fontProvider;
            this.vao = vao;

            vao.Initialize();

            vao.SetTopLeftCorner(new Vector2(0, 0), new Vector2(applicationWindow.Width, applicationWindow.Height));
            vao.SetupColors(new Color4[] { Color4.White, Color4.White, Color4.White, Color4.White });
            vao.SetupTextureCoords(new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) });

            bitmap = new Bitmap(applicationWindow.Width, applicationWindow.Height);
            graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, applicationWindow.Width, applicationWindow.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }

        public void Setup(string text, PointF point, Color4 color, int fontSize = 16)
        {
            graphicsClear();

            font = new Font(fontProvider.GetRobotoFontFamily(), fontSize);
            this.point = point;

            brush = new SolidBrush(Color.FromArgb(color.ToArgb()));

            drawString(text, font);
        }

        public void OnUpdateFrame(FrameEventArgs args)
        {
            vao.OnUpdateFrame(args);
            updateBitmap();
        }

        public void OnRenderFrame(FrameEventArgs args)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            vao.OnRenderFrame(args);
        }

        public void OnResize(EventArgs args)
        {
            vao.OnResize(args);
        }

        public Vector2 MeasureText(string text, int fontSize = 16)
        {
            Font temporaryFont = new Font(fontProvider.GetRobotoFontFamily(), fontSize);
            SizeF size = graphics.MeasureString(text, temporaryFont);

            return new Vector2(size.Width, size.Height);
        }

        public void SetText(string text)
        {
            graphicsClear();

            drawString(text, font);
        }

        public PointF GetPosition()
        {
            return point;
        }

        private void drawString(string text, Font font)
        {
            graphics.DrawString(text, font, brush, point);

            SizeF size = graphics.MeasureString(text, font);
            usedRegion = Rectangle.Round(RectangleF.Union(usedRegion, new RectangleF(point, size)));
            usedRegion = Rectangle.Intersect(usedRegion, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }

        private void updateBitmap()
        {
            if (usedRegion != RectangleF.Empty)
            {
                System.Drawing.Imaging.BitmapData data = bitmap.LockBits(usedRegion,
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                    usedRegion.X, usedRegion.Y, usedRegion.Width, usedRegion.Height,
                    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                usedRegion = Rectangle.Empty;
            }
        }

        private void graphicsClear()
        {
            graphics.Clear(Color.Transparent);
            usedRegion = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        }
    }
}
