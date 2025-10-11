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
    /// Interaction logic for AsociazaParinteEducator.xaml
    /// </summary>
    public partial class AsociazaParinteEducator : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public AsociazaParinteEducator()
        {
            InitializeComponent();
            IncarcaEducatori();
            IncarcaParinti();
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

        private void IncarcaParinti()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdParinte, NumeComplet FROM Parinti", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboParinti.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la incarcarea parintilor: " + ex.Message);
                }
            }
        }

        private void AsociazaParinteEducator_Click(object sender, RoutedEventArgs e)
        {
            if(comboParinti.SelectedValue == null)
            {
                MessageBox.Show("Selecteaza un parinte", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int idParinte = (int)comboParinti.SelectedValue;

            if(comboEducatori.SelectedValue == null)
            {
                MessageBox.Show("Selecteaza un educator", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int idEducator = (int)comboEducatori.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AsociazaParinteEducator", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdEducator", idEducator);
                    cmd.Parameters.AddWithValue("@IdParinte", idParinte);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Asocierea a fost efectuata cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                    comboEducatori.SelectedIndex = -1;
                    comboParinti.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la asociere: " + ex.Message);
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
