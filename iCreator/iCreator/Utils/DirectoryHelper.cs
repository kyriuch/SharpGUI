using System.IO;
using System.Reflection;

namespace iCreator.Utils
{
    internal interface IDirectoryHelper
    {
        string GetRootProjectDirectory();
    }

    internal sealed class DirectoryHelper : IDirectoryHelper
    {
        public string GetRootProjectDirectory() => Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                        Assembly.GetEntryAssembly().Location)
                        .ToString()).ToString()).ToString() + "\\";
    }
}
