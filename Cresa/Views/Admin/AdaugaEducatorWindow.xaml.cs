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
using System.Data.SqlClient;
using System.Data;


namespace Cresa.Admin
{
    /// <summary>
    /// Interaction logic for AdaugaEducatorWindow.xaml
    /// </summary>
    public partial class AdaugaEducatorWindow : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public AdaugaEducatorWindow()
        {
            InitializeComponent();
        }

        private void AdaugaEducator_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string parola = txtParola.Password.Trim();
            string numeComplet = txtNumeComplet.Text.Trim();
            string adresa = txtAdresa.Text.Trim();
            string telefon = txtTelefon.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(parola) ||
                string.IsNullOrEmpty(numeComplet) || string.IsNullOrEmpty(adresa) ||
                string.IsNullOrEmpty(telefon))
            {
                MessageBox.Show("Completeaza toate campurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("AdaugaEducator", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Parola", parola);
                    cmd.Parameters.AddWithValue("@NumeComplet", numeComplet);
                    cmd.Parameters.AddWithValue("@Adresa", adresa);
                    cmd.Parameters.AddWithValue("@NumarTelefon", telefon);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Educatorul a fost adaugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                    txtAdresa.Text = "";
                    txtEmail.Text = "";
                    txtParola.Password = "";
                    txtNumeComplet.Text = "";
                    txtTelefon.Text = "";
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la adaugare" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow("Administrator");
            adminWindow.Show();
            this.Close();
        }
    }
}
