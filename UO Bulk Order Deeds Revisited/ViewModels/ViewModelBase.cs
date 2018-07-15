using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand BackCommand { get; }
        public ICommand HelpCommand { get; }
        public ICommand CollectionCommand { get; }
        public ICommand HomeCommand { get; }

        private Visibility _BackCommandVisibility = Visibility.Collapsed;
        public Visibility BackCommandVisibility
        {
            get { return _BackCommandVisibility; }
            set
            {
                if (_BackCommandVisibility == value)
                {
                    return;
                }

                _BackCommandVisibility = value;
                NotifyPropertyChanged(nameof(BackCommandVisibility));
            }
        }

        private Visibility _CollectionCommandVisibility = Visibility.Visible;
        public Visibility CollectionCommandVisibility
        {
            get { return _CollectionCommandVisibility; }
            set
            {
                if (_CollectionCommandVisibility == value)
                {
                    return;
                }

                _CollectionCommandVisibility = value;
                NotifyPropertyChanged(nameof(CollectionCommandVisibility));
            }
        }

        private Visibility _HomeCommandVisibility = Visibility.Visible;
        public Visibility HomeCommandVisibility
        {
            get { return _HomeCommandVisibility; }
            set
            {
                if (_HomeCommandVisibility == value)
                {
                    return;
                }

                _HomeCommandVisibility = value;
                NotifyPropertyChanged(nameof(HomeCommandVisibility));
            }
        }

        public string Title { get; }

        protected ViewModelBase()
        {
            BackCommand = new RelayCommand(OnBackCommand);
            HelpCommand = new RelayCommand(OnHelpCommand);
            CollectionCommand = new RelayCommand(OnCollectionCommand);
            HomeCommand = new RelayCommand(OnHomeCommand);

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var name = assembly.GetCustomAttribute(typeof(AssemblyProductAttribute)) is AssemblyProductAttribute assemblyProduct
                ? assemblyProduct.Product
                : assemblyName.Name;

            Title = $"{name} v{assemblyName.Version.ToString(2)} Build {assemblyName.Version.Build}";
        }

        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnBackCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.Back);
        }

        private void OnHelpCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.Help);
        }

        private void OnCollectionCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.BulkOrderDeedCollection);
        }

        private void OnHomeCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.Professions);
        }
    }
}
