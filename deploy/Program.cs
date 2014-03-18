using System;
using System.IO;
using NDesk.Options;

namespace deploy
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = null, @from = null, siteName = null;
            var p = new OptionSet
            {
                { "f|from=",  v => @from=v },
                { "p|path=",      v => folder = v },
                { "n|sitename=",  v => siteName=v },
            };

            p.Parse(args);
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(@from) || string.IsNullOrEmpty(siteName))
            {
                p.WriteOptionDescriptions(Console.Out);
                return;
            }
            try
            {
                var d = new Deploy(folder, @from, siteName);
                d.Do();
            }
            catch (DirectoryNotFoundException direx)
            {
                Console.Error.WriteLine(direx.Message);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}
