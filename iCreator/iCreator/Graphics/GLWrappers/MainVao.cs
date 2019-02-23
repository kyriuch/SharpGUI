using iCreator.Providers;
using iCreator.Utils;
using iCreator.Window;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iCreator.Graphics.GLWrappers
{
    internal sealed class MainVao
    {
        private readonly ILogger logger;
        private readonly IMathHelper mathHelper;
        private readonly GraphicsProgramProvider graphicsProgramProvider;
        private readonly ApplicationWindow ApplicationWindow;

        private readonly int[] buffersIds = new int[2];
        private readonly List<Vector2> vertexRawPositions = new List<Vector2>();
        private readonly List<Vector2> positions = new List<Vector2>();
        private readonly List<Vector4> colors = new List<Vector4>();

        private int vaoId;
        private int indicesId;

        private static readonly uint[] indices = new uint[] { 0, 1, 2, 2, 3, 0 };

        public MainVao(ILogger logger, IMathHelper mathHelper,
            GraphicsProgramProvider graphicsProgramProvider, ApplicationWindow applicationWindow)
        {
            this.logger = logger;
            this.mathHelper = mathHelper;
            this.graphicsProgramProvider = graphicsProgramProvider;
            this.ApplicationWindow = applicationWindow;
        }

        public void Initialize()
        {
            vaoId = GL.GenVertexArray();
            GL.GenBuffers(buffersIds.Length, buffersIds);
            indicesId = GL.GenBuffer();
        }

        public void OnUpdateFrame(FrameEventArgs args)
        {
            GL.BindVertexArray(vaoId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, buffersIds[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, positions.Count * Vector2.SizeInBytes,
                positions.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vaoId, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, buffersIds[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Count * Vector4.SizeInBytes,
                colors.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vaoId, 1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint),
                indices, BufferUsageHint.StaticDraw);
        }

        public void OnRenderFrame(FrameEventArgs args)
        {
            graphicsProgramProvider.MainProgram.Bind();
            GL.BindVertexArray(vaoId);
            GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void OnResize(EventArgs e)
        {
            positions.Clear();

            vertexRawPositions.ForEach(x =>
            {
                positions.Add(new Vector2(
                mathHelper.GLNormalize(x.X, 0, ApplicationWindow.Width), // normalize x to range from 0 to window width
                mathHelper.GLNormalize(ApplicationWindow.Height - x.Y, 0, ApplicationWindow.Height))); // normalize y to range from 0 to window height
            });
        }

        public void SetTopLeftCorner(Vector2 cornerPos, Vector2 size)
        {
            positions.Clear();
            vertexRawPositions.Clear();

            processPosition(cornerPos);
            processPosition(new Vector2(cornerPos.X, cornerPos.Y + size.Y));
            processPosition(new Vector2(cornerPos.X + size.X, cornerPos.Y + size.Y));
            processPosition(new Vector2(cornerPos.X + size.X, cornerPos.Y));
        }

        public void AddVertex(Vector2 vertex)
        {
            processPosition(vertex);
        }

        public void SetupColors(List<Color4> colors)
        {
            this.colors.Clear();

            colors.ForEach(color => processColor(color));
        }

        public void SetupColors(Color4[] colors)
        {
            this.colors.Clear();

            colors.ToList().ForEach(color => processColor(color));
        }

        public void AddColor(Color4 color)
        {
            processColor(color);
        }

        private void processPosition(Vector2 position)
        {
            vertexRawPositions.Add(position);

            positions.Add(new Vector2(
                mathHelper.GLNormalize(position.X, 0, ApplicationWindow.Width), // normalize x to range from 0 to window width
                mathHelper.GLNormalize(ApplicationWindow.Height - position.Y, 0, ApplicationWindow.Height))); // normalize y to range from 0 to window height
        }

        private void processColor(Color4 color)
        {
            colors.Add(new Vector4(color.R, color.G, color.B, color.A));
        }
    }
}
