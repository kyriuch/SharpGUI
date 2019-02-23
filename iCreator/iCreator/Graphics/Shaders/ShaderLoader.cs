using iCreator.Providers;
using iCreator.Utils;
using OpenTK.Graphics.OpenGL4;

namespace iCreator.Graphics.Shaders
{
    internal interface IShaderLoader
    {
        void LoadShaders();
    }

    internal sealed class ShaderLoader : IShaderLoader
    {
        private readonly ILogger logger;
        private readonly GraphicsProgramProvider graphicsProgramProvider;

        public ShaderLoader(ILogger logger, GraphicsProgramProvider graphicsProgramProvider)
        {
            this.logger = logger;
            this.graphicsProgramProvider = graphicsProgramProvider;
        }

        public void LoadShaders()
        {
            Shader mainVertexShader = new Shader("MainVertexShader", ShaderType.VertexShader);
            Shader mainFragmentShader = new Shader("MainFragmentShader", ShaderType.FragmentShader);

            graphicsProgramProvider.SetupMainProgram(new GraphicsProgram(mainVertexShader, mainFragmentShader));

            Shader textureVertexShader = new Shader("TextureVertexShader", ShaderType.VertexShader);
            Shader textureFragmentShader = new Shader("TextureFragmentShader", ShaderType.FragmentShader);

            graphicsProgramProvider.SetupTextureProgram(new GraphicsProgram(textureVertexShader, textureFragmentShader));

            Shader coloredTextureVertexShader = new Shader("ColoredTextureVertexShader", ShaderType.VertexShader);
            Shader coloredTextureFragmentShader = new Shader("ColoredTextureFragmentShader", ShaderType.FragmentShader);

            graphicsProgramProvider.SetupColoredTextureProgram(new GraphicsProgram(coloredTextureVertexShader, coloredTextureFragmentShader));
        }
    }
}
