using System;
using System.IO;
using NDesk.Options;

namespace deploy
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = null, @from = null, siteName = null;
            var p = new OptionSet
            {
                { "f|from=",  v => @from=v },
                { "p|path=",      v => folder = v },
                { "n|sitename=",  v => siteName=v },
            };

            p.Parse(args);
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(@from) || string.IsNullOrEmpty(siteName))
            {
                p.WriteOptionDescriptions(Console.Out);
                return;
            }
            try
            {
                var rdir = VersionFromFolders.GetNextFolder(folder);
                Directories.CopyDirectory(new DirectoryInfo(@from), new DirectoryInfo(rdir));
                ServerManager.MoveSiteWithName(siteName, rdir);
                VersionFromFolders.CleanupOldDirectories(folder, 4);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}
