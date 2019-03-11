
using Autofac;
using iCreator.Models;
using iCreator.Providers;
using iCreator.Utils;
using iCreator.Window;
using Newtonsoft.Json;
using System.IO;

namespace iCreator.External
{
    public class SceneChanger
    {
        private SceneChanger()
        {

        }

        public static void ChangeScene(string sceneName)
        {
            ILogger logger = ContainerProvider.Scope.Resolve<ILogger>();
            IDirectoryHelper directoryHelper = ContainerProvider.Scope.Resolve<IDirectoryHelper>();

            string viewFilePath = $"{ Directory.GetCurrentDirectory() }\\iCreator\\Views\\{ sceneName }.json";
            string controllerFilePath = $"{ directoryHelper.GetRootProjectDirectory() }iCreator\\Controllers\\{ sceneName }Controller.cs";

            if (viewExists(viewFilePath, controllerFilePath))
            {
                switchScene(viewFilePath);
            }
            else
            {
                logger.Info($"{ sceneName } does not exist.");
            }
        }

        private static bool viewExists(string viewFilePath, string controllerFilePath)
        {
            return File.Exists(viewFilePath) && File.Exists(controllerFilePath);
        }

        private static void switchScene(string viewFilePath)
        {
            View newView = JsonConvert.DeserializeObject<View>(
                    File.ReadAllText(viewFilePath));

            ISceneManager sceneManager = ContainerProvider.Scope.Resolve<ISceneManager>();

            if (newView.Elements == null)
                newView.Elements = new System.Collections.Generic.List<Element>();

            if (newView.Theme == null)
            {
                if (!string.IsNullOrEmpty(newView.ThemeName))
                {
                    switch (newView.ThemeName)
                    {
                        case "Light":
                            newView.Theme = new Theme()
                            {
                                BackgroundColor = "#FFFFFF",
                                PrimaryColor = "#007acc",
                                TextColor = "#007acc",
                                TooltipColor = "#9da6a4"
                            }; break;
                        default:
                            newView.Theme = new Theme()
                            {
                                BackgroundColor = "#263238",
                                PrimaryColor = "#00C853",
                                TextColor = "#00C853",
                                TooltipColor = "#78909C"
                            }; break;
                    }
                }
                else
                {
                    newView.Theme = new Theme()
                    {
                        BackgroundColor = "#FFFFFF",
                        PrimaryColor = "#007acc",
                        TextColor = "#007acc",
                        TooltipColor = "#9da6a4"
                    };
                }
            }

            sceneManager.ChangeScene(newView);
        }
    }
}
