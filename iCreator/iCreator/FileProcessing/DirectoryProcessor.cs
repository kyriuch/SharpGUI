using iCreator.Utils;
using System.Collections.Generic;
using System.IO;

namespace iCreator.FileProcessing
{
    internal interface IDirectoriesProcessor
    {
        void ProcessDirectories();
    }

    class DirectoriesProcessor : IDirectoriesProcessor
    {
        private readonly ILogger logger;
        private readonly IDirectoryHelper directoryHelper;
        private readonly Queue<string> directoriesToCreate;

        public DirectoriesProcessor(ILogger logger, IDirectoryHelper directoryHelper)
        {
            this.logger = logger;
            this.directoryHelper = directoryHelper;
            directoriesToCreate = new Queue<string>();
        }

        public void ProcessDirectories()
        {
            enqueueDirectories();

            createDirectoriesWithinDirectory(directoryHelper.GetRootProjectDirectory());

            createDirectoriesWithinDirectory(Directory.GetCurrentDirectory());
        }

        private void enqueueDirectories()
        {
            directoriesToCreate.Enqueue("iCreator");
            directoriesToCreate.Enqueue("iCreator\\Configuration");
            directoriesToCreate.Enqueue("iCreator\\Views");
            directoriesToCreate.Enqueue("iCreator\\Models");
            directoriesToCreate.Enqueue("iCreator\\Controllers");
            directoriesToCreate.Enqueue("iCreator\\Resources");
        }

        private void createDirectoriesWithinDirectory(string directory)
        {
            foreach (string dir in directoriesToCreate)
            {
                if (!Directory.Exists($"{ directory }\\{ dir }"))
                {
                    Directory.CreateDirectory($"{ directory }\\{ dir }");
                }
            }
        }
    }
}
