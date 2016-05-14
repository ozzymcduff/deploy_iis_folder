using DeployIISFolder;
using NUnit.Framework;

namespace tests
{
    [TestFixture]
    public class CanParseHostsFile
    {
        [Test,
            TestCase(@"102.54.94.97     rhino.acme.com", "102.54.94.97", "rhino.acme.com"),
            TestCase(@"38.25.63.10     x.acme.com", "38.25.63.10", "x.acme.com"),
            TestCase(@"127.0.0.1       localhost", "127.0.0.1", "localhost"),
            TestCase(@"::1             localhost", "::1", "localhost"),
            ]
        public void Can_parse_single_line(string line, string ip, string host)
        {
            var hosts = HostFile.Parse(line);
            hosts.Entries.Should(Be.EquivalentTo(new[] { new HostFile.Entry(ip: ip, host: host) }));
        }

        [Test]
        public void Can_parse_multiple_lines()
        {
            var lines = @"
102.54.94.97     rhino.acme.com
38.25.63.10     x.acme.com
127.0.0.1       localhost
::1             localhost
";
            var hosts = HostFile.Parse(lines);
            hosts.Entries.Should(Be.EquivalentTo(new[] {
                new HostFile.Entry("102.54.94.97", "rhino.acme.com"),
                new HostFile.Entry("38.25.63.10", "x.acme.com"),
                new HostFile.Entry("127.0.0.1", "localhost"),
                new HostFile.Entry("::1", "localhost")
            }));
        }


        [Test]
        public void Wont_include_comments()
        {
            var lines = @"# Some comment
102.54.94.97     rhino.acme.com # some other comment
38.25.63.10     x.acme.com 
# localhost:
127.0.0.1       localhost
# some third comment
# ::1             localhost
";
            var hosts = HostFile.Parse(lines);
            hosts.Entries.Should(Be.EquivalentTo(new[] {
                new HostFile.Entry("102.54.94.97", "rhino.acme.com"),
                new HostFile.Entry("38.25.63.10", "x.acme.com"),
                new HostFile.Entry("127.0.0.1", "localhost")
            }));
        }
    }
}
