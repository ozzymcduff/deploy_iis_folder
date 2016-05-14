using System;
using System.IO;
using Microsoft.Web.Administration;
using DeployIISFolder;

namespace deploy
{
    public class Deploy
    {
        public Deploy(string targetFolder, string @from, string siteName)
        {
            _targetFolder = targetFolder;
            _from = @from;
            _siteName = siteName;
        }

        private readonly string _targetFolder;
        private readonly string _from;
        private readonly string _siteName;

        public void CleanupOldDirectories()
        {
            var dirs = Directory.GetDirectories(_targetFolder);

            var outOfDate = VersionFolders.OutOfDateDirectories(dirs, 4);
            outOfDate.Each(d =>
                               {
                                   Console.WriteLine("removing " + d);
                                   Directory.Delete(Path.Combine(_targetFolder, d), true);
                               });
        }
        public string GetNextFolder()
        {
            var dirs = Directory.GetDirectories(_targetFolder);
            var v = VersionFolders.GetVersionFromDirectories(dirs);
            return Path.Combine(_targetFolder, "r_" + (v + 1));
        }

        public void Do()
        {
            var rdir = GetNextFolder();
            DeployFiles(rdir);
            MoveSite(rdir);
            CleanupOldDirectories();
        }

        private void MoveSite(string rdir)
        {
            var iisManager = new ServerManager();
            var site = iisManager.GetSiteWithName(_siteName);
            if (null != site)
            {
                site.Applications.MoveSingleToPath(rdir);
                iisManager.CommitChanges();
            }
            else
            {
                Console.Error.WriteLine("You need to add the site with {0} to IIS", _siteName);
                Environment.Exit(1);
            }
        }

        private void DeployFiles(string rdir)
        {
            Directory.CreateDirectory(rdir);
            CopyRecursive(rdir);
        }

        private void CopyRecursive(string rdir)
        {
            _from.AsDir().CopyDirectory(rdir.AsDir());
        }

    }
}