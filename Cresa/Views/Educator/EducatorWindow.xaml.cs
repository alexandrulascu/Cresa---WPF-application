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
using Microsoft.Win32;

namespace Cresa.Educator
{
    /// <summary>
    /// </summary>
    public partial class EducatorWindow : Window
    {
        private string numeEducator;
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        private int idGrupa;
        public EducatorWindow(string nume)
        {
            InitializeComponent();
            numeEducator = nume;
            Title = $"Panou Educator - Bine ai venit, {numeEducator}";
            IncarcaIdGrupa(nume);
            IncarcaProgram();
            IncarcaMateriale();
            IncarcaAnunturi();
            IncarcaCopii();
        }
        
        private void IncarcaIdGrupa(string numeEducator)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT G.IdGrupa
                    FROM Grupe G
                    INNER JOIN Educatori E ON G.IdEducator = E.IdEducator
                    WHERE E.NumeComplet = @numeEducator";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@numeEducator", numeEducator);

                var result = cmd.ExecuteScalar();
                if(result!= null)
                {
                    idGrupa = Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show("Nu s-a gasit grupa pentru acest educator.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
        }

        private void IncarcaProgram()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT DataProgram AS [Data], NumeProgram AS [Activitati] FROM Program WHERE IdGrupa=@idGrupa ORDER BY DataProgram", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@idGrupa", idGrupa);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridProgram.ItemsSource = dt.DefaultView;
            }
        }
        private void AdaugaProgram_Click(object sender, RoutedEventArgs e)
        {
            if (dateProgram.SelectedDate == null || string.IsNullOrWhiteSpace(txtActivitate.Text))
            {
                MessageBox.Show("Completeaza toate campurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime data = dateProgram.SelectedDate.Value;
            string activitate = txtActivitate.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AdaugaProgram", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdGrupa", idGrupa);
                cmd.Parameters.AddWithValue("@DataProgram", data);
                cmd.Parameters.AddWithValue("@NumeProgram", activitate);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Activitatea a fost adaugata cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            IncarcaProgram();
            txtActivitate.Clear();
            dateProgram.SelectedDate = null;
        }

        private void IncarcaMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumeMaterial.Text) || string.IsNullOrWhiteSpace(txtCaleFisier.Text))
            {
                MessageBox.Show("Completeaza toate campurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AdaugaMateriale", conn);
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdGrupa", idGrupa);
                cmd.Parameters.AddWithValue("@NumeMaterial", txtNumeMaterial.Text.Trim());
                cmd.Parameters.AddWithValue("@CaleFisier", txtCaleFisier.Text.Trim());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Materialul a fost incarcat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                txtNumeMaterial.Clear();
                txtCaleFisier.Clear();
                IncarcaMateriale();
            }
        }

        private void IncarcaMateriale()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT NumeMaterial AS [Nume], CaleFisier AS [Fisier] FROM Materiale WHERE IdGrupa = @idGrupa", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@IdGrupa", idGrupa);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridMateriale.ItemsSource = dataTable.DefaultView;
            }

        }

        private void SelecteazaFisier_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Selecteaza un fisier";
            openFileDialog.Filter = "Toate fisierele|*.*";

            if(openFileDialog.ShowDialog() == true)
            {
                txtCaleFisier.Text = openFileDialog.FileName;
            }
        }

        private void AdaugaAnunt_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtTitluAnunt.Text) || string.IsNullOrWhiteSpace(txtDescriereAnunt.Text))
            {
                MessageBox.Show("Completeaza toate campurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AdaugaAnunturi", conn);
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdGrupa", idGrupa);
                cmd.Parameters.AddWithValue("@Titlu", txtTitluAnunt.Text.Trim());
                cmd.Parameters.AddWithValue("@Descriere", txtDescriereAnunt.Text.Trim());

                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Anuntul a fost adaugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            txtTitluAnunt.Clear();
            txtDescriereAnunt.Clear();
            IncarcaAnunturi();
        }

        private void IncarcaAnunturi()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT Titlu AS [Titlu], Descriere AS [Descriere] FROM Anunturi WHERE IdGrupa = @IdGrupa", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@IdGrupa", idGrupa);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridAnunturi.ItemsSource = dt.DefaultView;
            }
        }

        private void AdaugaAbsenta_Click(object sender, RoutedEventArgs e)
        {
            if (comboCopii.SelectedValue == null || dateAbsenta.SelectedDate == null)
            {
                MessageBox.Show("Selecteaza un copil si o data!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int idCopil = (int)comboCopii.SelectedValue;
            DateTime data = dateAbsenta.SelectedDate.Value;

            using (SqlConnection conn =new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AdaugaAbsente", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCopil", idCopil);
                cmd.Parameters.AddWithValue("@DataAbsentei", data);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Absenta a fost adaugata cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            dateAbsenta.SelectedDate = null;
            IncarcaAbsente();
        }

        private void IncarcaAbsente()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT C.Nume AS [Copil], A.DataAbsentei AS [Data Absentei] FROM Absente A INNER JOIN Copii C ON A.IdCopil=C.IdCopil WHERE C.IdGrupa = @idGrupa ORDER BY A.DataAbsentei DESC", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@idGrupa", idGrupa);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridAbsente.ItemsSource = dt.DefaultView;
            }
        }

        private void IncarcaCopii()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT IdCopil, Nume AS NumeCopil FROM Copii WHERE IdGrupa = @IdGrupa", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@IdGrupa", idGrupa);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboCopii.ItemsSource = dt.DefaultView;
                comboCopii.DisplayMemberPath = "NumeCopil";
                comboCopii.SelectedValuePath = "IdCopil";
            }
        }

        private void CalculeazaNumarCopii_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("NumarTotalCopii", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdGrupa", idGrupa);

                int numarCopii = (int)cmd.ExecuteScalar();
                txtNumarCopii.Text = $"Grupa ta are {numarCopii} copii.";
            }
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            var login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void DeschideTrimiteNotificareWindow_Click(object sender, RoutedEventArgs e)
        {
            var fereastraNotificare = new TrimiteNotificareWindow();
            fereastraNotificare.ShowDialog();
        }
    }
}
