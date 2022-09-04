using System;
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
using System.Windows.Shapes;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace Turniej
{
    /// <summary>
    /// Logika interakcji dla klasy settigns.xaml
    /// </summary>
    public partial class settigns : Window
    {
        public settigns()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            running.Default.CharsList = CharListOption.IsChecked.Value;
            running.Default.DocxFile = DocxOption.IsChecked.Value;
            running.Default.XlsxFile = XlsxOption.IsChecked.Value;
            running.Default.MessagesOnMailbox = MessagesOnMailbox.IsChecked.Value;
            running.Default.LargeGroups = LargeGroups.IsChecked.Value;
            running.Default.Save();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
