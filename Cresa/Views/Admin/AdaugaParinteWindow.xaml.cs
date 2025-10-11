using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Data;

namespace Cresa.Admin
{
    /// <summary>
    /// Interaction logic for AdaugaParinteWindow.xaml
    /// </summary>
    public partial class AdaugaParinteWindow : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public AdaugaParinteWindow()
        {
            InitializeComponent();
        }

        private void AdaugaParinte_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string parola = txtParola.Password.Trim();
            string numeComplet = txtNumeComplet.Text.Trim();
            string telefon = txtTelefon.Text.Trim();

            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(parola) || 
                string.IsNullOrWhiteSpace(numeComplet) || string.IsNullOrWhiteSpace(telefon))
            {
                MessageBox.Show("Completeaza toate campurile.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AdaugaParinte", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Parola", parola);
                    cmd.Parameters.AddWithValue("@NumeComplet", numeComplet);
                    cmd.Parameters.AddWithValue("@Telefon", telefon);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Parintele a fost adaugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReseteazaCampurile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la adaugare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ReseteazaCampurile()
        {
            txtEmail.Text = "";
            txtParola.Password = "";
            txtNumeComplet.Text = "";
            txtTelefon.Text = "";
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            var adminWindow = new AdminWindow("Administrator");
            adminWindow.Show();
            this.Close();
        }
    }
}
