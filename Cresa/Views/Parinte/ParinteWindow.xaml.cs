using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Cresa.Parinte
{
    public partial class ParinteWindow : Window
    {
        private int idParinte;
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        private string numeParinte;

        public ParinteWindow(string nume)
        {
            InitializeComponent();
            numeParinte = nume;
            Title = $"Panou Parinte - Bine ai venit, {numeParinte}";
            idParinte = ObtineIdParinte(numeParinte);
            IncarcaCopii();
        }

        private int ObtineIdParinte(string nume)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT IdParinte FROM Parinti WHERE NumeComplet = @nume", conn);
                cmd.Parameters.AddWithValue("@nume", nume);

                object result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
                else
                {
                    MessageBox.Show("Nu s-a putut identifica părintele.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return -1;
                }
            }
        }


        private void IncarcaCopii()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "EXEC VizualizeazaCopiiiParintelui @IdParinte", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@IdParinte", idParinte);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboCopii.ItemsSource = dt.DefaultView;
                comboCopii.DisplayMemberPath = "Nume";
                comboCopii.SelectedValuePath = "IdCopil";
            }
        }
        private void comboCopii_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCopii.SelectedValue is int idCopil)
            {
                IncarcaMateriale(idCopil);
                IncarcaAbsente(idCopil);
                IncarcaPlati(idCopil);
            }
        }

        private void IncarcaMateriale(int idCopil)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("EXEC VizualizeazaMateriale @IdCopil", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@IdCopil", idCopil);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridMateriale.ItemsSource = dt.DefaultView;
            }
        }

        private void IncarcaAbsente(int idCopil)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("EXEC VizualizeazaAbsente @IdCopil", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@IdCopil", idCopil);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                if(ds.Tables.Count>0)
                {
                    dataGridAbsente.ItemsSource = ds.Tables[0].DefaultView;
                }
            }
        }

        private void IncarcaPlati(int idCopil)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("EXEC VizualizeazaIstoricPlati @IdCopil", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@IdCopil", idCopil);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridPlati.ItemsSource = dt.DefaultView;
            }
        }

        private void DescarcaMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is DataRowView row)
            {
                string caleFisier = row["CaleFisier"].ToString();

                if (System.IO.File.Exists(caleFisier))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = caleFisier,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la deschidere: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Fisierul nu exista la calea specificata.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
