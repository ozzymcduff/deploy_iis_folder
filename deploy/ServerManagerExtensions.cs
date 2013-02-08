using System;
using System.Linq;
using Microsoft.Web.Administration;

namespace deploy
{
    public static class ServerManagerExtensions
    {
        public static Site GetSiteWithName(this ServerManager iisManager, string sitename)
        {
            return iisManager.Sites.SingleOrDefault(s =>
                                                    s.Name.Equals(sitename, StringComparison.InvariantCultureIgnoreCase));
        }
        public static void MoveSingleToPath(this ApplicationCollection applicationCollection, string path)
        {
            applicationCollection.Single().VirtualDirectories.Single().PhysicalPath = path;
        }
    }
}