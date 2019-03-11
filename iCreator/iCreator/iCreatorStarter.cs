using Autofac;
using iCreator.Container;
using iCreator.Providers;

namespace iCreator
{
    public class iCreatorStarter
    {
        private static IContainer container;

        private iCreatorStarter()
        {

        }

        public static void Start()
        {
            configureContainer();
            initializeApplication();
        }

        private static void configureContainer()
        {
            container = ContainerConfig.Configure();
        }

        private static void initializeApplication()
        {
            using (var scope = container.BeginLifetimeScope())
            {
                ContainerProvider.SetupScope(scope);

                var entryPoint = scope.Resolve<EntryPoint>();

                entryPoint.Init();
            }
        }
    }
}
