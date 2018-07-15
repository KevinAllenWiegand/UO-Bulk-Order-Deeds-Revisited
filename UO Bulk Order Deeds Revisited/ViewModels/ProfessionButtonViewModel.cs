using Npe.UO.BulkOrderDeeds;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ProfessionButtonViewModel : ViewModelBase
    {
        private readonly Profession _Profession;

        public string Name => _Profession.Name;
        public BitmapImage ProfessionIcon { get; }
        public ICommand NavigateToProfessionCommand { get; }

        public ProfessionButtonViewModel(Profession profession)
        {
            NavigateToProfessionCommand = new RelayCommand(OnNavigateToProfessionCommand);
            _Profession = profession;
            ProfessionIcon = new BitmapImage(new Uri($"/Icons/{_Profession.Name}.gif", UriKind.Relative));
        }

        private void OnNavigateToProfessionCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.Profession, parameter);
        }
    }
}
