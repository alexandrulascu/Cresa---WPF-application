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
    /// Interaction logic for AsociazaPlata.xaml
    /// </summary>
    public partial class AsociazaPlata : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public AsociazaPlata()
        {
            InitializeComponent();
        }

        private void PosteazaPlati_Click(object sender, RoutedEventArgs e)
        {
            decimal suma = 2000;
            DateTime dataPlata = DateTime.Now;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand selectCmd = new SqlCommand("SELECT IdCopil FROM Copii", conn);
                    SqlDataReader reader = selectCmd.ExecuteReader();


                    List<int> copiiIds = new List<int>();
                    while (reader.Read())
                    {
                        copiiIds.Add((int)reader["IdCopil"]);
                    }
                    reader.Close();

                    foreach(int id in copiiIds)
                    {
                        SqlCommand insertCmd = new SqlCommand("AsociazaPlati", conn);
                        insertCmd.CommandType = CommandType.StoredProcedure;

                        insertCmd.Parameters.AddWithValue("@IdCopil", id);
                        insertCmd.Parameters.AddWithValue("@DataPlata", dataPlata);
                        insertCmd.Parameters.AddWithValue("@Suma", suma);
                        insertCmd.Parameters.AddWithValue("@RestDePlata", suma);

                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Platile au fost postate cu succes pentru toti copiii.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la postarea platii" + ex.Message);
                }
            }
        }

        private void ActualizeazaRest_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActualizeazaRestWindow();
            window.Show();
            this.Close();
        }
    }
}
