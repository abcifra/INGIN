using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Common;

namespace INGIN.Helper
{
    public partial class HelperView : Window
    {
        //public string version { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string version { get; set; } = VersionInfo.Version;
        public HelperView()
        {
            InitializeComponent();
            DataContext = this;
        }
        

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            this.Close();
        }

        private void btn_Bla_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"%AppDataFolder%\Ingin\InginUpdater.exe");
        }
    }
}
