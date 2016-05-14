using System.IO;

namespace DeployIISFolder
{
    public partial class HostFile
    {
        public static void AddEntry(string ip, string hostname)
        {
            using (var file = new FileStream(Location, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var w = new StreamWriter(file))
            {
                w.WriteLine("{0}    {1}", ip, hostname);
            }
        }
    }
}
