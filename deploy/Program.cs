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
   	            { "p|path=",      v => folder = v },
   	            { "f|from=",  v => @from=v },
                { "n|sitename=",  v => siteName=v },
            };
            p.Parse(args);
            var d = new Deploy(folder, @from, siteName);
            d.Do();
        }
    }
}
