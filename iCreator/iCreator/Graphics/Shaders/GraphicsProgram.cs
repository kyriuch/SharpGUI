using Autofac;
using iCreator.Providers;
using iCreator.Utils;
using OpenTK.Graphics.OpenGL4;

namespace iCreator.Graphics.Shaders
{
    internal sealed class GraphicsProgram
    {
        public int ProgramHandle { get; private set; }

        public GraphicsProgram(params Shader[] shaders)
        {
            ProgramHandle = GL.CreateProgram();

            for (int i = 0; i < shaders.Length; i++)
            {
                GL.AttachShader(ProgramHandle, shaders[i].ShaderHandle);
            }

            GL.LinkProgram(ProgramHandle);

            string infoLog = GL.GetProgramInfoLog(ProgramHandle);

            if (!string.IsNullOrEmpty(infoLog))
            {
                ILogger logger = ContainerProvider.Scope.Resolve<ILogger>();

                logger.Error($"Error linking a program: { infoLog }.");
            }
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(ProgramHandle, uniformName);
        }

        public void Bind() => GL.UseProgram(ProgramHandle);
    }
}
