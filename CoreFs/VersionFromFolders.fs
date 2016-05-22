module VersionFromFolders
open System.Text.RegularExpressions
open System

let releaseFolders = new Regex("r_(\\d*)")
[<CompiledName("GetReleaseDirectories")>]
let getReleaseDirectories pathNames=
    pathNames |> Seq.filter(fun p-> releaseFolders.IsMatch(p))

[<CompiledName("GetVersionFromPath")>]
let getVersionFromPath p=
    let m = releaseFolders.Match(p)
    if m.Success then
        Some(Int32.Parse(m.Groups.[1].Value))
    else
        None

[<CompiledName("GetVersionFromDirectories")>]
let getVersionFromDirectories pathNames=
    getReleaseDirectories(pathNames) 
    |> Seq.choose getVersionFromPath
    |> Seq.sortDescending
    |> Seq.tryHead

[<CompiledName("OutOfDateDirectories")>]
let outOfDateDirectories (pathNames, keep)=
    getReleaseDirectories(pathNames) 
    |> Seq.map (fun p-> (getVersionFromPath(p),p) )
    |> Seq.sortByDescending fst
    |> Seq.skip keep
    |> Seq.map snd

open System.IO

[<CompiledName("GetNextFolder")>]
let getNextFolder targetFolder=
    let dirs = Directory.GetDirectories(targetFolder)
    let version = match getVersionFromDirectories(dirs) with 
                    | Some v-> v 
                    | None -> 0
    Path.Combine(targetFolder, sprintf "r_%i" (version + 1))

[<CompiledName("CleanupOldDirectories")>]
let cleanupOldDirectories (targetFolder, keep)=
    let dirs = Directory.GetDirectories(targetFolder)
    let outOfDate = outOfDateDirectories(dirs, keep)
    for directory in outOfDate do
        printf "removing %s"  directory
        Directory.Delete(Path.Combine(targetFolder, directory), true)
