﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Npe.UO.BulkOrderDeeds.BuiltInImportPlugins
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModelBase()
        {
        }

        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
