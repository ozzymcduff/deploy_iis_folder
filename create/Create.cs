using DeployIISFolder;
using Microsoft.Web.Administration;
using System;
using System.Linq;

namespace create
{
    class Create
    {
        private readonly string _folder;
        private readonly string _hostName;
        private readonly string _siteName;

        public Create(string folder, string hostname, string siteName)
        {
            _folder = folder;
            _hostName = hostname;
            _siteName = siteName;
        }

        internal void Do()
        {
            var hosts = HostFile.Parse();
            if (!hosts.Entries.Any(e => e.Host.Equals(_hostName)))
            {
                HostFile.AddEntry("127.0.0.1", _hostName);
            }

            var iisManager = new ServerManager();
            var site = iisManager.GetSiteWithName(_siteName);
            if (site == null)
            {
                site = iisManager.Sites.Add(name: _siteName, 
                    bindingProtocol: "http", 
                    bindingInformation: ":80:" + _hostName, 
                    physicalPath: _folder);
                site.ServerAutoStart = true;
                var appPool = iisManager.ApplicationPools.Add(_siteName);
                appPool.ManagedRuntimeVersion = "v4.0";
                site.Applications.Single().ApplicationPoolName = appPool.Name;
                iisManager.CommitChanges();
            }

        }
    }
}
