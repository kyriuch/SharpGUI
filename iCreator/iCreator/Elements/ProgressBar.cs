using System;
using iCreator.Graphics.Shapes;
using iCreator.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace iCreator.Elements
{
    public sealed class ProgressBar : Element
    {
        public override bool IsVisible { get; set; } = true;

        private readonly ILogger logger;
        private readonly IMathHelper mathHelper;

        public delegate void OnUpdateCallback();
        private event OnUpdateCallback onUpdateCallback;

        private float progress;

        private Vector2 pos;
        private Vector2 size;
        private Color4 color;
        private Color4 fillColor;

        private RoundRectNoFill rectNoFill;
        private RoundRectFill rectFill;

        internal ProgressBar(ILogger logger, IMathHelper mathHelper)
        {
            this.logger = logger;
            this.mathHelper = mathHelper;
            progress = 0;
        }

        internal void Setup(Vector2 pos, Vector2 size, Color4 color, Color4 fillColor)
        {
            this.pos = pos;
            this.size = size;
            this.color = color;
            this.fillColor = fillColor;

            rectNoFill = new RoundRectNoFill(pos, size, color);

            updateProgressBar();
        }

        public void AddOnUpdateListener(OnUpdateCallback callback)
        {
            onUpdateCallback += callback;
        }

        public void SetPercentageProgress(float progress)
        {
            if (progress < 0 || progress > 100)
                throw new ArgumentException($"Progress can be from 0 to 100 but was { progress }");

            updateProgress(progress);
            updateProgressBar();
        }

        private void updateProgress(float newProgress)
        {
            progress = newProgress;
        }

        private void updateProgressBar()
        {
            Vector2 fillPos = pos;
            fillPos.X += 3;
            fillPos.Y += 3;
            Vector2 fillSize = size;
            fillSize.X = mathHelper.Lerp(30, size.X - 6, progress);
            fillSize.Y -= 6;

            rectFill = new RoundRectFill(fillPos, fillSize, fillColor);
        }

        internal override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            
        }

        internal override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            
        }

        internal override void OnMouseDown(MouseButtonEventArgs e)
        {
            
        }

        internal override void OnMouseMove(MouseMoveEventArgs e)
        {
            
        }

        internal override void OnMouseUp(MouseButtonEventArgs e)
        {
            
        }

        internal override void OnRenderFrame(FrameEventArgs e)
        {
            rectNoFill.OnRenderFrame(e);
            rectFill.OnRenderFrame(e);
        }

        internal override void OnResize(EventArgs e)
        {
            rectNoFill.OnResize(e);
            rectFill.OnResize(e);
        }

        internal override void OnUpdateFrame(FrameEventArgs e)
        {
            onUpdateCallback?.Invoke();
            rectNoFill.OnUpdateFrame(e);
            rectFill.OnUpdateFrame(e);
        }
    }
}
