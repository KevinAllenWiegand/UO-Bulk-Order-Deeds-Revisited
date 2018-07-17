using Npe.UO.BulkOrderDeeds.Plugins;
using System.Windows;

namespace Npe.UO.BulkOrderDeeds.SampleImportPlugin
{
    public class SampleImportPlugin : ImportPlugin
    {
        public override string DisplayName => "Sample Importer";

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
