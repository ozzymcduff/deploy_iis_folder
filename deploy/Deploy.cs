using System;
using System.IO;
using Microsoft.Web.Administration;

namespace deploy
{
    public class Deploy
    {
        public Deploy(string folder, string @from, string siteName)
        {
            _folder = folder;
            _from = @from;
            _siteName = siteName;
        }

        private readonly string _folder;
        private readonly string _from;
        private readonly string _siteName;

        public void CleanupOldDirectories()
        {
            var dirs = Directory.GetDirectories(_folder);

            var outOfDate = VersionFolders.OutOfDateDirectories(dirs, 4);
            outOfDate.Each(d =>
                               {
                                   Console.WriteLine("removing " + d);
                                   Directory.Delete(Path.Combine(_folder, d), true);
                               });
        }
        public string GetNextFolder()
        {
            var dirs = Directory.GetDirectories(_folder);
            var v = VersionFolders.GetVersionFromDirectories(dirs);
            return Path.Combine(_folder, "r_" + (v + 1));
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