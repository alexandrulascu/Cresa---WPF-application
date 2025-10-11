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
using System.Data;
using System.Data.SqlClient;

namespace Cresa.Admin
{
    /// <summary>
    /// Interaction logic for AdaugaGrupaWindow.xaml
    /// </summary>
    public partial class AdaugaGrupaWindow : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public AdaugaGrupaWindow()
        {
            InitializeComponent();
            IncarcaEducatori();
        }

        private void IncarcaEducatori()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdEducator, NumeComplet FROM Educatori", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboEducatori.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la incarcarea educatorilor: " + ex.Message);
                }
            } 
                
        }

        private void AdaugaGrupa_Click(object sender, RoutedEventArgs e)
        {
            string nume = txtNume.Text.Trim();
            if (comboEducatori.SelectedValue == null)
            {
                MessageBox.Show("Selecteaza un educator", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int idEducator = (int)comboEducatori.SelectedValue;

            if (string.IsNullOrEmpty(nume))
            {
                MessageBox.Show("Completeaza toate campurile corect!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection (connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AdaugaGrupa", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeGrupa", nume);
                    cmd.Parameters.AddWithValue("@IdEducator", idEducator);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Grupa a fost adaugata cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                    txtNume.Text = "";
                    comboEducatori.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la adaugare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow admin = new AdminWindow("Administrator");
            admin.Show();
            this.Close();
        }
    }
}
