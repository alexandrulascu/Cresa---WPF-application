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
using Cresa.Admin;


namespace Cresa.Admin
{
    /// <summary>
    /// Interaction logic for ActualizeazaRestWindow.xaml
    /// </summary>
    public partial class ActualizeazaRestWindow : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";

        public ActualizeazaRestWindow()
        {
            InitializeComponent();
            IncarcaCopii();
        }

        private void IncarcaCopii()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdCopil, Nume FROM Copii", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboCopii.ItemsSource=dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la incarcarea copiilor: " + ex.Message);
                }
            }
        }

        private void Actualizeaza_Click(object sender, RoutedEventArgs e)
        {
            if (comboCopii.SelectedValue == null || !decimal.TryParse(txtRestNou.Text.Trim(), out decimal restNou))
            {
                MessageBox.Show("Selecteaza un copil si introdu o valoare valida!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int idCopil = (int)comboCopii.SelectedValue;
            int idPlata = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand getPlatacmd = new SqlCommand("SELECT  TOP 1 IdPlata FROM Plati WHERE IdCopil = @idCopil ORDER BY DataPlata DESC", conn);
                    getPlatacmd.Parameters.AddWithValue("@idCopil", idCopil);

                    var result = getPlatacmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Acest copil nu are plati inregistrate", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    idPlata = Convert.ToInt32(result);

                    SqlCommand cmd = new SqlCommand("ActualizeazaRestPlata", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPlata", idPlata);
                    cmd.Parameters.AddWithValue("@RestNou", restNou);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Restul de plata a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Actualizarea a esuat", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la actualizare" + ex.Message);
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
