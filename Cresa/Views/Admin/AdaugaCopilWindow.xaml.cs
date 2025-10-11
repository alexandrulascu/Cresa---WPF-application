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
    /// Interaction logic for AdaugaCopilWindow.xaml
    /// </summary>
    public partial class AdaugaCopilWindow : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public AdaugaCopilWindow()
        {
            InitializeComponent();
            IncarcaGrupe();
        }


        private void AdaugaCopil_Click(object sender, RoutedEventArgs e)
        {
            string nume = txtNume.Text.Trim();
            DateTime? dataNasterii = dateNastere.SelectedDate;

            if (string.IsNullOrEmpty(nume) || !dataNasterii.HasValue || comboGrupe.SelectedValue == null)
            {
                MessageBox.Show("Completeaza toate campurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int idGrupa = (int)comboGrupe.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("AdaugaCopil", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nume", nume);
                    cmd.Parameters.AddWithValue("@DataNasterii", dataNasterii.Value);
                    cmd.Parameters.AddWithValue("@IdGrupa", idGrupa);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Copilul a fost adaugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtNume.Text = "";
                    dateNastere.SelectedDate = null;
                    comboGrupe.SelectedIndex = -1;
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

        private void IncarcaGrupe()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT IdGrupa, NumeGrupa FROM Grupe", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboGrupe.ItemsSource = dt.DefaultView;
            }
        }

    }
}
