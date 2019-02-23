using Autofac;
using iCreator.Container;
using iCreator.Providers;

namespace iCreator
{
    public class iCreatorStarter
    {
        private iCreatorStarter()
        {

        }

        public static void Start()
        {
            var container = ContainerConfig.Configure();

            using(var scope = container.BeginLifetimeScope())
            {
                ContainerProvider.SetupScope(scope);

                var entryPoint = scope.Resolve<EntryPoint>();

                entryPoint.Init();
            }
        }
    }
}
