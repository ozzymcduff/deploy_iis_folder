using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeployIISFolder
{
    public class VersionFolders
    {
        private static readonly Regex releaseFolders = new Regex("r_(\\d*)");
        public static IEnumerable<string> GetReleaseDirectories(IEnumerable<string> pathnames)
        {
            return pathnames.Where(p=> releaseFolders.IsMatch(p));
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
            return Int32.Parse(releaseFolders.Match(p).Groups[1].Value);
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