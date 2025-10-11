using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Cresa.Admin
{
    public partial class AsociazaParinteCopilWindow : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";

        public AsociazaParinteCopilWindow()
        {
            InitializeComponent();
            IncarcaParinti();
            IncarcaCopii();
        }

        private void IncarcaParinti()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT IdParinte, NumeComplet FROM Parinti", conn);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboParinti.ItemsSource = dt.DefaultView;
                comboParinti.DisplayMemberPath = "NumeComplet";
                comboParinti.SelectedValuePath = "IdParinte";
            }
        }

        private void IncarcaCopii()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT IdCopil, Nume FROM Copii", conn);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboCopii.ItemsSource = dt.DefaultView;
                comboCopii.DisplayMemberPath = "Nume";
                comboCopii.SelectedValuePath = "IdCopil";
            }
        }

        private void Asociaza_Click(object sender, RoutedEventArgs e)
        {
            if (comboParinti.SelectedValue == null || comboCopii.SelectedValue == null)
            {
                MessageBox.Show("Selecteaza un parinte si un copil!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int idParinte = (int)comboParinti.SelectedValue;
            int idCopil = (int)comboCopii.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ParintiCopii (IdParinte, IdCopil) VALUES (@IdParinte, @IdCopil)", conn);

                cmd.Parameters.AddWithValue("@IdParinte", idParinte);
                cmd.Parameters.AddWithValue("@IdCopil", idCopil);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Parintele a fost asociat cu copilul cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            comboParinti.SelectedIndex = -1;
            comboCopii.SelectedIndex = -1;
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
