using System.IO;
using System.Reflection;

namespace Npe.UO.BulkOrderDeeds.Internal
{
    internal class Internals
    {
        public static string GetRootLocation()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
