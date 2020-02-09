using CQRSAndMediator.Scaffolding.Infrastructure;
using System.IO;

namespace CQRSAndMediator.Scaffolding.Utilities
{
    public static class FileSystemUtility
    {
        public static string CreateDirectory(string[] pathList)
        {
            var path = Path.Combine(pathList);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static string CreateFile(string[] pathList, string data)
        {
            var path = Path.Combine(pathList);

            if (!File.Exists(path))
            {
                using var streamWriter = new StreamWriter($"{path}.cs");
                streamWriter.Write(data);
            }
            else
            {
                LogUtility.Error($"File already exists! {path}");
            }

            return path;
        }
    }
}
