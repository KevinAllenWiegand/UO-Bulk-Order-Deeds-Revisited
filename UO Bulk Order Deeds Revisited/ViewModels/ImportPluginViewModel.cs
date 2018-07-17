using Npe.UO.BulkOrderDeeds.Plugins;
using System;
using System.Windows.Input;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ImportPluginViewModel : ViewModelBase
    {
        internal ImportPlugin ImportPlugin;

        public string DisplayName => ImportPlugin.DisplayName;

        public ICommand ImportCommand { get; }

        public event EventHandler<EventArgs> ImportCompleted;

        public ImportPluginViewModel(ImportPlugin importPlugin)
        {
            ImportCommand = new RelayCommand(OnImportCommand);
            ImportPlugin = importPlugin;
        }

        private void OnImportCommand(object parameter)
        {
            try
            {
                ImportPlugin.Import();
            }
            catch
            {
            }

            OnImportCompleted();
        }

        private void OnImportCompleted()
        {
            var handler = ImportCompleted;

            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
