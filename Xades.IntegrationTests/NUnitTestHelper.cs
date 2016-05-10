using System.IO;
using System.Reflection;

namespace Xades.IntegrationTests
{
    public static class NUnitTestHelper
    {
        private const string ResourcePath = "Xades.IntegrationTests.TestXml";

        public static string GetInputFile(string filename)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            var fileResourcePath = ResourcePath + "." + filename.Replace("\\", ".");
            var stream = thisAssembly.GetManifestResourceStream(fileResourcePath);
            if (stream == null)
            {
                throw new FileNotFoundException($"Файл {filename} ({fileResourcePath}) не найден в ресурсах сборки {thisAssembly.FullName}");
            }
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}