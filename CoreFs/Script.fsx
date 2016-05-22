// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.
#load "HostFile.fs"
#r "Microsoft.Web.Administration"
#load "ServerManager.fs"
#load "VersionFromFolders.fs"
#load "Directories.fs"

// Define your library scripting code here

HostFile.ensureHostNameBoundToLocalhost "hostname.com"
ServerManager.createSiteWithHostName {Name="sitename";Host= "hostname.com";Folder= "/dir"}
