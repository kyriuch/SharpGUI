using iCreator.Utils;

namespace iCreator.FileProcessing
{
    internal interface IFrameworkFilesProcessor
    {
        void ProcessFrameworkFiles();
    }

    internal sealed class FrameworkFilesProcessor : IFrameworkFilesProcessor
    {
        private readonly ILogger logger;
        private readonly IDirectoriesProcessor directoryProcessor;
        private readonly IFilesProcessor filesProcessor;

        public FrameworkFilesProcessor(ILogger logger, IDirectoriesProcessor directoryProcessor, IFilesProcessor filesProcessor)
        {
            this.logger = logger;
            this.directoryProcessor = directoryProcessor;
            this.filesProcessor = filesProcessor;
        }

        public void ProcessFrameworkFiles()
        {
            directoryProcessor.ProcessDirectories();
            filesProcessor.ProcessFiles();
        }
    }
}
