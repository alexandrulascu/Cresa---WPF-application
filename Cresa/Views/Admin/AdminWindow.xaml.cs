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
using Cresa.Admin;

namespace Cresa.Admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow(string numeAdmin)
        {
            InitializeComponent();
            Title += $" - Bine ai venit, {numeAdmin}";
        }

        private void AdaugaParinte_Click(object sender, RoutedEventArgs e)
        {
            var adaugaParinte = new AdaugaParinteWindow();
            adaugaParinte.Show();
            this.Close();
        }

        private void AdaugaEducator_Click(object sender, RoutedEventArgs e)
        {
            var adaugaEducator = new AdaugaEducatorWindow();
            adaugaEducator.Show();
            this.Close();
        }

        private void AdaugaCopil_Click(object sender, RoutedEventArgs e)
        {
            var adaugaCopil = new AdaugaCopilWindow();
            adaugaCopil.Show();
            this.Close();
        }

        private void CreeazaGrupa_Click(object sender, RoutedEventArgs e)
        {
            var adaugaGrupa = new AdaugaGrupaWindow();
            adaugaGrupa.Show();
            this.Close();
        }

        private void AsociazaParinteEducator_Click(object sender, RoutedEventArgs e)
        {
            var asociazaParinteEducator = new AsociazaParinteEducator();
            asociazaParinteEducator.Show();
            this.Close();
        }

        private void AsociazaPlata_Click(object sender, RoutedEventArgs e)
        {
            var asociazaPlata = new AsociazaPlata();
            asociazaPlata.Show();
            this.Close();
        }

        private void AsociazaParinteCopil_Click(object sender, RoutedEventArgs e)
        {
            var asociazaParinteCopil = new AsociazaParinteCopilWindow();
            asociazaParinteCopil.Show();
            this.Close();
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
