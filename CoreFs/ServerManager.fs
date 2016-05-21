namespace CoreFs
open System
open System.Linq
open Microsoft.Web.Administration

module ServerManager = 
    let createSiteWithHostName siteName hostName folder=
        let iisManager = new ServerManager()
        let siteExists = iisManager.Sites 
                        |> Seq.exists (fun s'->s'.Name.Equals(siteName, StringComparison.InvariantCultureIgnoreCase))

        if not siteExists then
            // binds to hostname with port 80
            let site = iisManager.Sites.Add (name= siteName, 
                            bindingProtocol= "http", 
                            bindingInformation= ":80:" + hostName, 
                            physicalPath= folder)
            site.ServerAutoStart <- true
            let appPool = iisManager.ApplicationPools.Add(siteName)
            appPool.ManagedRuntimeVersion <- "v4.0"
            site.Applications.[0].ApplicationPoolName <- appPool.Name
            iisManager.CommitChanges()
        else
            ()