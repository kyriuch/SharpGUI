using Autofac;
using iCreator.Providers;
using iCreator.Utils;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace iCreator.Graphics.Shaders
{
    internal sealed class Shader
    {
        public int ShaderHandle { get; private set; }

        public Shader(string shaderName, ShaderType shaderType)
        {
            ShaderHandle = GL.CreateShader(shaderType);

            string shaderSource = File.ReadAllText(getShaderPath(shaderName));

            GL.ShaderSource(ShaderHandle, shaderSource);
            GL.CompileShader(ShaderHandle);

            string infoLog = GL.GetShaderInfoLog(ShaderHandle);

            if (!string.IsNullOrEmpty(infoLog))
            {
                ILogger logger = ContainerProvider.Scope.Resolve<ILogger>();

                logger.Error($"Error linking a shader named \"{ shaderName }\": { infoLog }.");
            }
        }

        private string getShaderPath(string shaderName)
        {
            return $"{ Directory.GetCurrentDirectory() }\\iCreator\\Shaders\\{ shaderName }.shader";
        }
    }
}
