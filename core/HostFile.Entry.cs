using System;

namespace DeployIISFolder
{
    public partial class HostFile
    {
        public class Entry : IEquatable<Entry>
        {
            public readonly string Ip;
            public readonly string Host;
            public Entry(string ip, string host)
            {
                Ip = ip;
                Host = host;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Entry);
            }

            public bool Equals(Entry other)
            {
                if (ReferenceEquals(null, other)) { return false; }
                return Ip.Equals(other.Ip) && Host.Equals(other.Host);
            }

            public override int GetHashCode()
            {
                int hash = 17;

                hash = hash * 23 + Ip.GetHashCode();
                hash = hash * 23 + Host.GetHashCode();
                return hash;
            }

            public override string ToString()
            {
                return string.Format("{0}\t{1}", Ip, Host);
            }
        }
    }
}