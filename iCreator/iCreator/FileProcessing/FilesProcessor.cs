using Autofac;
using iCreator.Models;
using iCreator.Providers;
using iCreator.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;

namespace iCreator.FileProcessing
{
    internal interface IFilesProcessor
    {
        void ProcessFiles();
    }

    internal sealed class FilesProcessor : IFilesProcessor
    {
        private readonly ILogger logger;
        private readonly IDirectoryHelper directoryHelper;
        private StartupConfig startupConfig;

        public FilesProcessor(ILogger logger, IDirectoryHelper directoryHelper)
        {
            this.logger = logger;
            this.directoryHelper = directoryHelper;
        }

        public void ProcessFiles()
        {
            copyFilesFromProjectToBuildDir();

            processStartupConfigFile();

            processStartupViewFile();

            checkWhetherControllersExist();
        }

        private void processStartupConfigFile()
        {
            if (!File.Exists($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Configuration\\StartupConfig.json"))
            {
                createStartupConfigFile();
            }
            else
            {
                startupConfig = JsonConvert.DeserializeObject<StartupConfig>(
                    File.ReadAllText($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Configuration\\StartupConfig.json"));
            }
        }

        private void createStartupConfigFile()
        {
            startupConfig = new StartupConfig()
            {
                StartupViewName = "iCreator"
            };

            File.Create($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Configuration\\StartupConfig.json").Dispose();

            using (StreamWriter writer = new StreamWriter(
                $"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Configuration\\StartupConfig.json"))
            {
                writer.Write(JsonConvert.SerializeObject(startupConfig, Formatting.Indented));
            }

            File.Copy($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Configuration\\StartupConfig.json",
                $"{ Directory.GetCurrentDirectory() }\\iCreator\\Configuration\\StartupConfig.json", true);
        }

        private void processStartupViewFile()
        {
            View startupView;

            if (!File.Exists($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json"))
            {
                startupView = createStartupWindowFile();
            }
            else
            {
                startupView = JsonConvert.DeserializeObject<View>(
                    File.ReadAllText($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json"));

                if (startupView == null)
                    startupView = createStartupWindowFile();
            }

            startupView.Filename = startupConfig.StartupViewName;

            copyViewProperties(startupView, ContainerProvider.Scope.Resolve<View>());
        }

        private void copyViewProperties(View sourceView, View destinyView)
        {
            destinyView.Name = sourceView.Name;
            destinyView.Width = sourceView.Width;
            destinyView.Height = sourceView.Height;
            destinyView.Elements = sourceView.Elements;
            destinyView.Theme = sourceView.Theme;
            destinyView.ThemeName = sourceView.ThemeName;
            destinyView.Filename = sourceView.Filename;

            if (destinyView.Elements == null)
                destinyView.Elements = new System.Collections.Generic.List<Element>();

            if (destinyView.Theme == null)
            {
                if (!string.IsNullOrEmpty(destinyView.ThemeName))
                {
                    switch (destinyView.ThemeName)
                    {
                        case "Light":
                            destinyView.Theme = new Theme()
                            {
                                BackgroundColor = "#FFFFFF",
                                PrimaryColor = "#007acc",
                                TextColor = "#007acc",
                                TooltipColor = "#9da6a4"
                            }; break;
                        default:
                            destinyView.Theme = new Theme()
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
                    destinyView.Theme = new Theme()
                    {
                        BackgroundColor = "#FFFFFF",
                        PrimaryColor = "#007acc",
                        TextColor = "#007acc",
                        TooltipColor = "#9da6a4"
                    };
                }
            }
        }

        private View createStartupWindowFile()
        {
            File.Create($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json").Dispose();

            View startupWindow = new View()
            {
                Name = "iCreator",
                Width = 1280,
                Height = 720,
                ThemeName = "Light",
                Elements = new System.Collections.Generic.List<Element>()
            };

            using (StreamWriter writer = new StreamWriter($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json"))
            {
                writer.Write(JsonConvert.SerializeObject(startupWindow, Formatting.Indented));
            }

            File.Copy($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json",
                $"{ Directory.GetCurrentDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json", true);

            return startupWindow;
        }

        private void checkWhetherControllersExist()
        {
            string[] files = Directory.GetFiles($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views");

            string nameSpace = Assembly.GetEntryAssembly().GetTypes().FirstOrDefault().Namespace;

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                if (!File.Exists($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Controllers\\{ fileName }Controller.cs"))
                {
                    File.Create($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Controllers\\{ fileName }Controller.cs").Dispose();

                    using (StreamWriter writer = new StreamWriter($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Controllers\\{ fileName }Controller.cs"))
                    {
                        writer.WriteLine("namespace " + nameSpace + ".iCreator.Controllers");
                        writer.WriteLine("{");
                        writer.WriteLine($"\tpublic class { fileName }Controller : IController");
                        writer.WriteLine("\t{");
                        writer.WriteLine("\t");
                        writer.WriteLine("\t}");
                        writer.WriteLine("}");
                    }

                    File.Copy($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Controllers\\{ fileName }Controller.cs",
                        $"{ Directory.GetCurrentDirectory() }\\iCreator\\Controllers\\{ fileName }Controller.cs", true);
                }
            }
        }

        private void copyFilesFromProjectToBuildDir()
        {
            string[] dirs = new string[] {
                "\\iCreator\\Views\\",
                "\\iCreator\\Controllers\\",
                "\\iCreator\\Resources\\",
                "\\iCreator\\Configuration\\",
                "\\iCreator\\Models\\"
            };

            foreach (string dir in dirs)
            {
                string[] files = Directory.GetFiles(directoryHelper.GetRootProjectDirectory() + dir);

                foreach (string file in files)
                {
                    File.Copy(file, Directory.GetCurrentDirectory() + dir + Path.GetFileName(file), true);
                }
            }
        }
    }
}
