using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeployIISFolder
{
    public partial class HostFile
    {
        /// <summary>
        /// sample:
        /// 102.54.94.97     rhino.acme.com
        /// 38.25.63.10     x.acme.com
        /// 127.0.0.1       localhost
        /// ::1             localhost
        /// </summary>
        private static readonly Regex ipAndHost = new Regex(@"(?<ip>[^ \t]+)\s+(?<host>[^ \t]+)",
            RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        private static readonly Regex newLines = new Regex(@"[\r\n]+");
        private static readonly Regex comment = new Regex(@"(#.*)");
        public static HostFile Parse(string content)
        {
            return new HostFile(newLines.Split(content)
                .Select(l => comment.Replace(l, string.Empty))
                .Select(l => ipAndHost.Match(l))
                .Where(m => m.Success)
                .Select(m => new HostFile.Entry(m.Groups["ip"].Value, m.Groups["host"].Value))
                .ToArray());
        }
        public static HostFile Parse(Stream content)
        {
            var r = new StreamReader(content);
            return Parse(r.ReadToEnd());
        }
        public static HostFile Parse()
        {
            using (var file = new FileStream(Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                return Parse(file);
        }
    }
}
