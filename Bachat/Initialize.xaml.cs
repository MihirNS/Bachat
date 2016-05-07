using FirstFloor.ModernUI.Windows.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;

namespace Bachat
{
    /// <summary>
    /// Interaction logic for Initialize.xaml
    /// </summary>
    public partial class Initialize : Window
    {
        public Initialize()
        {
            InitializeComponent();
            icon.Fill = new SolidColorBrush(Color.FromRgb(0x33, 0x99, 0xff));
            MainWindow window = new MainWindow();
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += (sender, args) =>
            {
                // do your lengthy stuff here -- this will happen in a separate thread
                Thread.Sleep(9000);
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                // do any UI stuff after the long operation here
                this.Close();                
                window.Show();
            };

            bw.RunWorkerAsync(); 
        }
    }
}
