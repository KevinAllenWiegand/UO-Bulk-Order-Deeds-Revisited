﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UO_Bulk_Order_Deeds.Views
{
    /// <summary>
    /// Interaction logic for CollectionView.xaml
    /// </summary>
    public partial class CollectionView : UserControl
    {
        public CollectionView()
        {
            InitializeComponent();
        }

        // This is the stupidest thing ever...but for whateve reason the MenuItems are not
        // closing when clicked.  So I'm forced to so this shit.
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Focus();
        }
    }
}
