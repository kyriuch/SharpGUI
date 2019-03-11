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
            string startupConfigDir = $"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Configuration\\StartupConfig.json";

            if (!File.Exists(startupConfigDir))
            {
                createStartupConfigFile(startupConfigDir);
            }
            else
            {
                startupConfig = JsonConvert.DeserializeObject<StartupConfig>(
                    File.ReadAllText(startupConfigDir));
            }
        }

        private void createStartupConfigFile(string startupConfigDir)
        {
            startupConfig = new StartupConfig()
            {
                StartupViewName = "iCreator"
            };

            File.Create(startupConfigDir).Dispose();

            using (StreamWriter writer = new StreamWriter(
                startupConfigDir))
            {
                writer.Write(JsonConvert.SerializeObject(startupConfig, Formatting.Indented));
            }

            string currentDirectoryStartupConfigDir = $"{ Directory.GetCurrentDirectory() }\\iCreator\\Configuration\\StartupConfig.json";

            File.Copy(startupConfigDir, currentDirectoryStartupConfigDir, true);
        }

        private void processStartupViewFile()
        {
            View startupView;

            string startupViewFileDir = $"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json";

            if (!File.Exists(startupViewFileDir))
            {
                startupView = createStartupWindowFile(startupViewFileDir);
            }
            else
            {
                startupView = JsonConvert.DeserializeObject<View>(
                    File.ReadAllText(startupViewFileDir));

                if (startupView == null)
                    startupView = createStartupWindowFile(startupViewFileDir);
            }

            startupView.Filename = startupConfig.StartupViewName;

            copyViewProperties(startupView, ContainerProvider.Scope.Resolve<View>());
        }

        private View createStartupWindowFile(string startWindowFileDir)
        {
            File.Create(startWindowFileDir).Dispose();

            View startupWindow = new View()
            {
                Name = "iCreator",
                Width = 1280,
                Height = 720,
                ThemeName = "Light",
                Elements = new System.Collections.Generic.List<Element>()
            };

            using (StreamWriter writer = new StreamWriter(startWindowFileDir))
            {
                writer.Write(JsonConvert.SerializeObject(startupWindow, Formatting.Indented));
            }

            string currentDirectoryStartupViewFileDir = $"{ Directory.GetCurrentDirectory() }\\iCreator\\Views\\{ startupConfig.StartupViewName }.json";

            File.Copy(startWindowFileDir, currentDirectoryStartupViewFileDir, true);

            return startupWindow;
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

        private void checkWhetherControllersExist()
        {
            string[] files = Directory.GetFiles($"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Views");

            string nameSpace = Assembly.GetEntryAssembly().GetTypes().FirstOrDefault().Namespace;

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileDir = $"{ directoryHelper.GetRootProjectDirectory() }\\iCreator\\Controllers\\{ fileName }Controller.cs";

                if (!File.Exists(fileDir))
                {
                    File.Create(fileDir).Dispose();

                    using (StreamWriter writer = new StreamWriter(fileDir))
                    {
                        writer.WriteLine("namespace " + nameSpace + ".iCreator.Controllers");
                        writer.WriteLine("{");
                        writer.WriteLine($"\tpublic class { fileName }Controller : IController");
                        writer.WriteLine("\t{");
                        writer.WriteLine("\t");
                        writer.WriteLine("\t}");
                        writer.WriteLine("}");
                    }

                    string currentDirectoryFileDir = $"{ Directory.GetCurrentDirectory() }\\iCreator\\Controllers\\{ fileName }Controller.cs";

                    File.Copy(fileDir, currentDirectoryFileDir, true);
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
