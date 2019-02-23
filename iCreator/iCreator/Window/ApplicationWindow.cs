using System;
using System.Drawing;
using Autofac;
using iCreator.Graphics.Shaders;
using iCreator.Models;
using iCreator.Providers;
using iCreator.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace iCreator.Window
{
    internal sealed class ApplicationWindow : GameWindow
    {
        private readonly ILogger logger;
        private readonly IShaderLoader shaderLoader;
        private readonly ISceneManager sceneManager;

        public ApplicationWindow(ILogger logger, IShaderLoader shaderLoader, 
            ISceneManager sceneManager) : base(1280, 720, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 0, 0, 8), "")
        {
            this.logger = logger;
            this.shaderLoader = shaderLoader;
            this.sceneManager = sceneManager;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            setupGL();
            shaderLoader.LoadShaders();
            setupScene();
            
            Icon = new Icon("iCreator\\Resources\\app_icon.ico");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            sceneManager.GetCurrentScene().OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            sceneManager.GetCurrentScene().OnRenderFrame(e);

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            sceneManager.GetCurrentScene().OnResize(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            sceneManager.GetCurrentScene().OnKeyUp(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            sceneManager.GetCurrentScene().OnKeyDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            sceneManager.GetCurrentScene().OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            sceneManager.GetCurrentScene().OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            sceneManager.GetCurrentScene().OnMouseMove(e);
        }

        private void setupGL()
        {
            VSync = VSyncMode.On;
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.DepthTest);
        }

        private void setupScene()
        {
            View view = ContainerProvider.Scope.Resolve<View>();

            sceneManager.ChangeScene(view);
        }
    }
}
