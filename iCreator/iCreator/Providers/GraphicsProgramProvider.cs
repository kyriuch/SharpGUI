
using iCreator.Graphics.Shaders;

namespace iCreator.Providers
{
    internal sealed class GraphicsProgramProvider
    {
        public GraphicsProgram MainProgram { get; private set; }
        public GraphicsProgram TextureProgram { get; private set; }
        public GraphicsProgram ColoredTextureProgram { get; private set; }

        public void SetupMainProgram(GraphicsProgram program) => MainProgram = program;
        public void SetupTextureProgram(GraphicsProgram program) => TextureProgram = program;
        public void SetupColoredTextureProgram(GraphicsProgram program) => ColoredTextureProgram = program;
    }
}
