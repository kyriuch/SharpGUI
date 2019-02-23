using Autofac;
using iCreator.Elements;
using iCreator.Graphics.Advanced;
using iCreator.Graphics.GLWrappers;
using iCreator.Graphics.Shaders;
using iCreator.Models;
using iCreator.Providers;
using iCreator.Window;
using System.Linq;
using System.Reflection;

namespace iCreator.Container
{
    internal sealed class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SceneManager>()    .As<ISceneManager>()    .SingleInstance();
            builder.RegisterType<FontProvider>()    .As<IFontProvider>()    .SingleInstance();
            builder.RegisterType<CursorProvider>()  .As<ICursorProvider>()  .SingleInstance();

            builder.RegisterType<ShaderLoader>()    .As<IShaderLoader>();
            builder.RegisterType<Scene>()           .As<IScene>();

            builder.RegisterType<ApplicationWindow>()       .AsSelf().SingleInstance();
            builder.RegisterType<View>()                    .AsSelf().SingleInstance();
            builder.RegisterType<GraphicsProgramProvider>() .AsSelf().SingleInstance();

            builder.RegisterType<EntryPoint>()      .AsSelf();
            builder.RegisterType<Texture>()         .AsSelf();
            builder.RegisterType<MainVao>()         .AsSelf();
            builder.RegisterType<TextureVao>()      .AsSelf();
            builder.RegisterType<ColorTextureVao>() .AsSelf();
            builder.RegisterType<Text>()            .AsSelf();

            builder.RegisterType<Button>()      .FindConstructorsWith(new InternalConstructorFinder()).AsSelf();
            builder.RegisterType<Image>()       .FindConstructorsWith(new InternalConstructorFinder()).AsSelf();
            builder.RegisterType<Label>()       .FindConstructorsWith(new InternalConstructorFinder()).AsSelf();
            builder.RegisterType<TextField>()   .FindConstructorsWith(new InternalConstructorFinder()).AsSelf();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(iCreator)))
                .Where(t => t.Namespace.Contains("FileProcessing"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(iCreator)))
                .Where(t => t.Namespace.Contains("Utils"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name))
                .SingleInstance();

            return builder.Build();
        }
    }
}
