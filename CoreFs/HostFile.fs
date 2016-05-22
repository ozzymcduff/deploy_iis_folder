module HostFile 
open System.IO
open System.Text.RegularExpressions

let etcFolder = @"C:\Windows\System32\drivers\etc"
let location = Path.Combine(etcFolder, "hosts")

type Entry={Ip:string;Host:string}
[<CompiledName("AddEntry")>]
let addEntry (ip:string) (hostname:string)=
    use file = new FileStream(location, FileMode.Append, FileAccess.Write, FileShare.Read)
    use w = new StreamWriter(file)
    w.WriteLine("{0}    {1}", ip, hostname);

    /// <summary>
    /// sample:
    /// 102.54.94.97     rhino.acme.com
    /// 38.25.63.10     x.acme.com
    /// 127.0.0.1       localhost
    /// ::1             localhost
    /// </summary>
let ipAndHost = new Regex(@"(?<ip>[^ \t]+)\s+(?<host>[^ \t]+)", RegexOptions.ExplicitCapture ||| RegexOptions.IgnorePatternWhitespace ||| RegexOptions.IgnoreCase)
let newLines = new Regex(@"[\r\n]+")

[<CompiledName("Parse")>]
let parse content=
    let comment = new Regex(@"(#.*)")
    let removeComment l=comment.Replace(l,"")

    newLines.Split(content)
        |> Array.map removeComment
        |> Array.map (fun l-> ipAndHost.Match(l))
        |> Array.filter (fun m-> m.Success)
        |> Array.map (fun m->{ Ip=m.Groups.["ip"].Value; Host=m.Groups.["host"].Value })

[<CompiledName("ParseStream")>]
let parseStream (stream:Stream)=
    use r = new StreamReader(stream)
    parse(r.ReadToEnd())

[<CompiledName("ParseEtcFile")>]
let parseEtcFile ()=
    use file = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
    parseStream(file)

[<CompiledName("EnsureHostNameBoundToLocalhost")>]
let ensureHostNameBoundToLocalhost hostname=
    let hosts = parseEtcFile()
    let containsHostname = hosts |> Array.exists(fun h->h.Host = hostname)
    if not containsHostname then
        addEntry "127.0.0.1" hostname
    else
        ()

