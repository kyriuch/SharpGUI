using Autofac;
using iCreator.Models;
using iCreator.Providers;
using iCreator.Utils;

namespace iCreator.Window
{
    internal interface ISceneManager
    {
        void ChangeScene(View newView);
        IScene GetCurrentScene();
    }

    internal sealed class SceneManager : ISceneManager
    {
        private readonly ILogger logger;

        private IScene currentScene;

        public SceneManager(ILogger logger)
        {
            this.logger = logger;
        }

        public void ChangeScene(View newView)
        {
            if (currentScene != null)
                currentScene.Unload();

            currentScene = ContainerProvider.Scope.Resolve<IScene>();

            currentScene.Load(newView);
        }

        public IScene GetCurrentScene()
        {
            return currentScene;
        }
    }
}
