using System;
using System.IO;

namespace XadesDemo.Helpers
{
    public static class PathHelper
    {
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ToAppAbsolutePath(string fileName)
        {
            return Path.Combine(AppPath, fileName);
        }
    }
}