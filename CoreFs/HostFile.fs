namespace CoreFs
open System.IO
open System.Text.RegularExpressions

module HostFile = 
    let etcFolder = @"C:\Windows\System32\drivers\etc"
    let Location = Path.Combine(etcFolder, "hosts")

    type Entry={Ip:string;Host:string}

    let addEntry (ip:string) (hostname:string)=
        use file = new FileStream(Location, FileMode.Append, FileAccess.Write, FileShare.Read)
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

    let parse content=
        let comment = new Regex(@"(#.*)")
        let removeComment l=comment.Replace(l,"")

        newLines.Split(content)
            |> Array.map removeComment
            |> Array.map (fun l-> ipAndHost.Match(l))
            |> Array.filter (fun m-> m.Success)
            |> Array.map (fun m->{ Ip=m.Groups.["ip"].Value; Host=m.Groups.["host"].Value })

    let parseStream (stream:Stream)=
        use r = new StreamReader(stream)
        parse(r.ReadToEnd())

    let parseEtcFile ()=
        use file = new FileStream(Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        parseStream(file)

    let ensureHostNameBoundToLocalhost hostname=
        let hosts = parseEtcFile()
        let containsHostname = hosts |> Array.exists(fun h->h.Host = hostname)
        if not containsHostname then
            addEntry "127.0.0.1" hostname
        else
            ()

