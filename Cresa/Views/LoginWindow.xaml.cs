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
using Cresa.Educator;
using Cresa.Parinte;

namespace Cresa
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string userType;

        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public LoginWindow(string userType)
        {
            InitializeComponent();
            this.userType = userType;
            Title = $"Login - {userType}";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string parola = txtParola.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(parola))
            {
                MessageBox.Show("Completeaza toate campurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string procedura = "";

            if (userType == "Administrator")
                procedura = "AutentificareAdministrator";
            else if (userType == "Educator")
                procedura = "AutentificareEducator";
            else if (userType == "Parinte")
                procedura = "AutentificareParinti";
            else
                throw new Exception("Rol necunoscut!");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(procedura, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Parola", parola);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string nume = reader["NumeComplet"].ToString();
                        MessageBox.Show($"Bine ai venit, {nume}!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                        if(userType=="Administrator")
                        {
                            var adminWindow = new AdminWindow(nume);
                            adminWindow.Show();
                        }
                        if (userType == "Educator")
                        {
                            var educatorWindow = new EducatorWindow(nume);
                            educatorWindow.Show();
                        }
                        if(userType=="Parinte")
                        {
                            var parinteWindow = new ParinteWindow(nume);
                            parinteWindow.Show();
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Email sau parola incorecte.", "Autentificare esuata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            }
        }
    }
}
