module ServerManager
open System
open Microsoft.Web.Administration

type SiteWithHostName={Name:string;Host:string;Folder:string}

let siteWithName (name:string) (site: Site)=
    name.Equals(site.Name, StringComparison.InvariantCultureIgnoreCase)

[<CompiledName("CreateSiteWithHostName")>]
let createSiteWithHostName (site:SiteWithHostName)=
    let iisManager = new ServerManager()
    let siteExists = iisManager.Sites 
                    |> Seq.exists (siteWithName (site.Name))

    if not siteExists then
        // binds to hostname with port 80
        let site = iisManager.Sites.Add (name= site.Name, 
                        bindingProtocol= "http", 
                        bindingInformation= ":80:" + site.Host, 
                        physicalPath= site.Folder)
        site.ServerAutoStart <- true
        let appPool = iisManager.ApplicationPools.Add(site.Name)
        appPool.ManagedRuntimeVersion <- "v4.0"
        site.Applications.[0].ApplicationPoolName <- appPool.Name
        iisManager.CommitChanges()
    else
        ()

[<CompiledName("MoveSiteWithName")>]
let moveSiteWithName(name,path:string)=
    let iisManager = new ServerManager()

    let moveToPath (site:Site)=
        site.Applications.[0].VirtualDirectories.[0].PhysicalPath <- path;

    match iisManager.Sites |> Seq.tryFind( siteWithName name)  with
    | Some site-> 
        moveToPath site
        iisManager.CommitChanges()
    | None -> failwithf "You need to add the site with %s to IIS" name
