using NDesk.Options;
using System;

namespace create
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = null, siteName = null, hostname = null;
            var p = new OptionSet
            {
                { "p|path=",      v => folder = v },
                { "n|sitename=",  v => siteName=v },
                { "h|hostname=",  v => hostname = v}
            };

            p.Parse(args);
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(hostname) || string.IsNullOrEmpty(siteName))
            {
                p.WriteOptionDescriptions(Console.Out);
                return;
            }
            try
            {
                var c = new Create(folder, hostname, siteName);
                c.Do();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}
