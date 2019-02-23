using Autofac;

namespace iCreator.Providers
{
    internal sealed class ContainerProvider
    {
        public static ILifetimeScope Scope { get; private set; }

        public static void SetupScope(ILifetimeScope scope) => Scope = scope;
    }
}
