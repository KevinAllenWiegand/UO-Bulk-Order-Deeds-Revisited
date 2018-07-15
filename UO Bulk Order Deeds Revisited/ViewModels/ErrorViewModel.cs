using System;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ErrorViewModel : ViewModelBase
    {
        private readonly Exception _Exception;

        public string Message => _Exception.Message;

        public ErrorViewModel(Exception exception)
        {
            _Exception = exception;
        }
    }
}
