using System.Text.RegularExpressions;

namespace Task01.Common
{
    public class FilePathManager
    {
        public static string GetLocalFolderPathForDataInput()
        {
            return Path.Combine(GetApplicationRoot(), "DataInput"); ;
        }

        public static string GetLocalFolderPathForDataOutput()
        {
            return Path.Combine(GetApplicationRoot(), "DataOutput"); ;
        }


        static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;

            return appRoot;
        }

    }
}