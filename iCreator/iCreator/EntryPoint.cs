using iCreator.FileProcessing;
using iCreator.Providers;
using iCreator.Utils;
using Autofac;
using iCreator.Window;

namespace iCreator
{
    internal sealed class EntryPoint
    {
        private readonly ILogger logger;
        private readonly IFrameworkFilesProcessor frameworkFilesProcessor;

        public EntryPoint(ILogger logger, IFrameworkFilesProcessor frameworkFilesProcessor)
        {
            this.logger = logger;
            this.frameworkFilesProcessor = frameworkFilesProcessor;
        }

        public void Init()
        {
            frameworkFilesProcessor.ProcessFrameworkFiles();

            ApplicationWindow window = ContainerProvider.Scope.Resolve<ApplicationWindow>();

            window.Run();
        }
    }
}
