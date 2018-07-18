using Npe.UO.BulkOrderDeeds.Plugins;

namespace Npe.UO.BulkOrderDeeds.BuiltInImportPlugins
{
    public class ImportFromPreviousVersion : ImportPlugin
    {
        internal override bool Trusted => true;

        public override string DisplayName => "Import from Previous Version";
        
        public override void Import()
        {

        }
    }
}
