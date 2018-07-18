using Npe.UO.BulkOrderDeeds.Plugins;
using System.Windows;

namespace Npe.UO.BulkOrderDeeds.BuiltInImportPlugins
{
    public class ImportFromPreviousVersionImportPlugin : ImportPlugin
    {
        internal override bool Trusted => true;

        public override string DisplayName => "Import from Previous Version";
        
        public override void Import()
        {
            var import = new Import
            {
                DataContext = new ImportViewModel()
            };

            import.Owner = Application.Current.MainWindow;
            import.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            import.ShowDialog();
        }
    }
}
