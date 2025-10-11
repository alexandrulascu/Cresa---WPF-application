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

namespace Cresa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Administrator_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow("Administrator");
            loginWindow.Show();
            this.Close();
        }

        private void Educator_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow("Educator");
            loginWindow.Show();
            this.Close();
        }

        private void Parinte_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow("Parinte");
            loginWindow.Show();
            this.Close();
        }
    }
}
