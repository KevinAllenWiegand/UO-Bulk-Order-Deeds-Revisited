using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class HelpViewModel : ViewModelBase
    {
        private bool _IsUpdating;
        public bool IsUpdating
        {
            get { return _IsUpdating; }
            set
            {
                if (_IsUpdating == value) return;

                _IsUpdating = value;
                NotifyPropertyChanged(nameof(IsUpdating));
            }
        }

        private string _DebugInfo;
        public string DebugInfo
        {
            get { return _DebugInfo; }
            set
            {
                if (_DebugInfo == value) return;

                _DebugInfo = value;
                NotifyPropertyChanged(nameof(DebugInfo));
            }
        }

        public ICommand UpdateCommand { get; }

        public HelpViewModel()
        {
            BackCommandVisibility = Visibility.Visible;
            UpdateCommand = new RelayCommand(OnUpdateCommand, () => !IsUpdating);
        }

        private void OnUpdateCommand(object parameter)
        {
            IsUpdating = true;
            DebugInfo = String.Empty;

            Task.Run(() => {
                var processStartInfo = new ProcessStartInfo("Updater.exe")
                {
                    Arguments = $"\"{Process.GetCurrentProcess().MainModule.FileName}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = new Process
                {
                    StartInfo = processStartInfo
                };

                process.OutputDataReceived += OnProcessDataReceived;
                process.ErrorDataReceived += OnProcessDataReceived;
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit(Int32.MaxValue);
                process.CancelOutputRead();
                process.ErrorDataReceived -= OnProcessDataReceived;
                process.OutputDataReceived -= OnProcessDataReceived;

                Application.Current.Dispatcher.Invoke(() => {
                    IsUpdating = false;
                });
            });
        }

        private void OnProcessDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }

            var newLine = !String.IsNullOrEmpty(DebugInfo) ? Environment.NewLine : String.Empty;

            DebugInfo += $"{newLine}{e.Data}";
        }
    }
}
