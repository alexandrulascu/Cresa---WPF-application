using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Cresa.Educator
{
    /// <summary>
    /// Interaction logic for TrimiteNotificareWindow.xaml
    /// </summary>
    public partial class TrimiteNotificareWindow : Window
    {
        private readonly string connectionString = "Server=localhost;Database=CresaDB;Trusted_Connection=True;";
        public TrimiteNotificareWindow()
        {
            InitializeComponent();
            IncarcaParinti();
        }

        public class ParinteItem
        {
            public int IdParinte { get; set; }
            public string Nume { get; set; }
        }

        private void IncarcaParinti()
        {
            List<ParinteItem> parinti = new List<ParinteItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT IdParinte, NumeComplet FROM Parinti";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    parinti.Add(new ParinteItem
                    {
                        IdParinte = (int)reader["IdParinte"],
                        Nume = reader["NumeComplet"].ToString()
                    });
                }
            }

            ParinteComboBox.ItemsSource = parinti;
        }

        private void Trimite_Notificare_Click(object sender, RoutedEventArgs e)
        {
            if(ParinteComboBox.SelectedValue == null)
            {
                MessageBox.Show("Selecteaza un parinte!");
                return;
            }

            int idParinte = (int)ParinteComboBox.SelectedValue;
            string mesaj = MesajTextBox.Text.Trim();

            if(string.IsNullOrEmpty(mesaj))
            {
                MessageBox.Show("Introdu un mesaj!");
                return;
            }
            string email = AdaugaNotificareInDB(idParinte, mesaj);
            if(!string.IsNullOrEmpty(email))
            {
                try
                {
                    TrimiteEmail(email, "Notificare de la cresa", mesaj);
                    MessageBox.Show("Notificare trimisa cu succes!");
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Eroare la trimiterea emailului: {ex.Message}");
                }
            }
        }

        private string AdaugaNotificareInDB(int idParinte, string mesaj)
        {
            string email = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AdaugaNotificare", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdParinte", idParinte);
                cmd.Parameters.AddWithValue("@Mesaj", mesaj);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    email = reader["Email"].ToString();
                }
            }

            return email;
        }

        private void TrimiteEmail(string destinatar, string subiect, string mesaj)
        {
            var fromAddress = new MailAddress("alexlascu.20@gmail.com", "Cresa");
            var toAddress = new MailAddress(destinatar);
            const string fromPassword = "nurh wobx ogvg yezb";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subiect,
                Body = mesaj
            })
            {
                smtp.Send(mailMessage);
            }
        }
    }
}
