// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.
#load "HostFile.fs"
#r "Microsoft.Web.Administration"
#load "ServerManager.fs"
open CoreFs

// Define your library scripting code here

HostFile.ensureHostNameBoundToLocalhost "hostname.com"
ServerManager.createSiteWithHostName "sitename" "hostname.com" "/dir"
