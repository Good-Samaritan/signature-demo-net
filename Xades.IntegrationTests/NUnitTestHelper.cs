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
                throw new FileNotFoundException(string.Format("Файл {0} ({1}) не найден в ресурсах сборки {2}", filename, fileResourcePath, thisAssembly.FullName));
            }
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}