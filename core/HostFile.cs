using System.IO;

namespace DeployIISFolder
{
    public partial class HostFile
    {
        private const string etcFolder = @"C:\Windows\System32\drivers\etc";
        public static string Location { get { return Path.Combine(etcFolder, "hosts"); } }

        public readonly Entry[] Entries;
        public HostFile(Entry[] entries)
        {
            Entries = entries;
        }
    }
}
