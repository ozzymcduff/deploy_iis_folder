using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace deploy
{
    public class VersionFolders
    {
        public static IEnumerable<string> GetReleaseDirectories(IEnumerable<string> pathnames)
        {
            return pathnames.Where(p=>Regex.Match(p,"r_\\d*").Success);
        }

        public static int GetVersionFromDirectories(IEnumerable<string> pathnames)
        {
            var directories = GetReleaseDirectories(pathnames);
            if (directories.IsNullOrEmpty())
                return 0;
            return directories
                .Select(GetVersionFromPath)
                .Max();
        }

        private static int GetVersionFromPath(string p)
        {
            return Int32.Parse(Regex.Match(p, "r_(\\d*)").Groups[1].Value);
        }

        public static IEnumerable<string> OutOfDateDirectories(IEnumerable<string> pathnames, int num)
        {
            return GetReleaseDirectories(pathnames)
                .Select(p => new {v = GetVersionFromPath(p),p})
                .OrderByDescending(pv=>pv.v)
                .Skip(num)
                .Select(pv=>pv.p);
        }
    }
}