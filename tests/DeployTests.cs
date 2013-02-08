using NUnit.Framework;
using deploy;

namespace tests
{
    [TestFixture]
    public class DeployTests
    {
        [Test]
        public void Should_parse_folders()
        {
            var drs = VersionFolders.GetReleaseDirectories(new[] { "r_1", "r_2", "logs", "plupp", ".", ".." });
            drs.Should(Be.EquivalentTo(new[] { "r_1", "r_2" }));
        }
        [Test]
        public void Can_get_version_from_directories()
        {
            var v = VersionFolders.GetVersionFromDirectories(new[] { "somefolder", "r_1", "r_2" });
            v.Should(Be.EqualTo(2));
        }
        [Test]
        public void Can_get_version_from_directories_when_empty()
        {
            var v = VersionFolders.GetVersionFromDirectories(new string[0]);
            v.Should(Be.EqualTo(0));
        }
        [Test]
        public void Can_get_version_from_directories_when_only_initial()
        {
            var v = VersionFolders.GetVersionFromDirectories(new[] { "somefolder" });
            v.Should(Be.EqualTo(0));
        }
        [Test]
        public void Can_get_versions_to_delete()
        {
            VersionFolders.OutOfDateDirectories(new[] { "r_1", "r_10", "r_20", "r_21", "r_22" }, 2)
                .Should(Be.EquivalentTo(new[] { "r_1", "r_10", "r_20" }));

            VersionFolders.OutOfDateDirectories(new[] { "r_21", "r_22", "r_1", "r_10", "r_20" }, 2)
                   .Should(Be.EquivalentTo(new[] { "r_1", "r_10", "r_20" }));
        }
    }
}
