using System.IO;

namespace deploy
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo AsDir(this string dir)
        {
            return new DirectoryInfo(dir);
        }

        public static void CopyDirectory(this DirectoryInfo source, DirectoryInfo destination)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            // Copy all files.
            var files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(destination.FullName,
                                         file.Name));
            }

            // Process subdirectories.
            var dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // Get destination directory.
                var destinationDir = Path.Combine(destination.FullName, dir.Name);

                // Call CopyDirectory() recursively.
                dir.CopyDirectory(destinationDir.AsDir());
            }
        }
    }
}