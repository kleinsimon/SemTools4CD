using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SEMTools4CD
{
    /// <summary>
    /// Interaktionslogik für about.xaml
    /// </summary>
    public partial class about : Window
    {
        public about()
        {
            InitializeComponent();
            LabelVersion.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            Hyperlink s = (Hyperlink) sender;
            System.Diagnostics.Process.Start(s.NavigateUri.ToString());
        }
    }
}
