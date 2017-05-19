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

namespace NewLauncher
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public void ShowReportWindow(Exception exception)
        {
            this.ExMsg = exception.Message;
            base.ShowDialog();
        }

        public string ExMsg { get; set; }

        public ReportWindow()
        {
            InitializeComponent();
        }
    }
}
