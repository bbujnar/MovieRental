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

namespace MovieRental
{
    /// <summary>
    /// Logika interakcji dla klasy MovieRentalClients.xaml
    /// </summary>
    
    public partial class MovieRentalClients : Window
    {
        public string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MovieRental;Integrated Security=True";
        private List<Client> clients;

        public MovieRentalClients()
        {
            InitializeComponent();

            // Pobierz dane klientów z bazy danych
            clients = GetClientsData();

            // Ustaw dane klientów jako źródło dla ListBox
            clientsListView.ItemsSource = clients;
        }

        private List<Client> GetClientsData()
        {
            List<Client> clients = new List<Client>();

            // Połączenie z bazą danych
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    // Otwarcie połączenia
                    connection.Open();
                    string query;
                    // Zapytanie SQL do pobrania danych klientów
                    query = "SELECT Klienci.Imie, Klienci.Nazwisko, COUNT(*) AS LiczbaWypozyczen FROM Wypożyczenia JOIN Klienci ON Wypożyczenia.IDklienta = Klienci.IDklienta JOIN Filmy ON Wypożyczenia.IDFilmu = Filmy.IDFilmu GROUP BY Klienci.IDklienta, Imie, Nazwisko";
                    

                    // Wykonanie zapytania SQL
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Odczyt danych klientów z wyniku zapytania
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader["Imie"].ToString();
                                string surname = reader["Nazwisko"].ToString();
                                string rentedMovies = reader["LiczbaWypozyczen"].ToString();

                                // Tworzenie obiektu klienta na podstawie danych z bazy danych
                                Client client = new Client { Name = name, Surname = surname , RentedMovies = rentedMovies };

                                // Dodanie klienta do listy
                                clients.Add(client);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Błąd połączenia z bazą danych: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return clients;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchKeyword = searchTextBox.Text.ToLower();

            // Filtruj klientów na podstawie wprowadzonego tekstu
            List<Client> filteredClients = clients.Where(c => c.Name.ToLower().Contains(searchKeyword) || c.Surname.ToLower().Contains(searchKeyword)).ToList();

            // Aktualizuj źródło danych dla ListView
            clientsListView.ItemsSource = filteredClients;
        }
    }



    public class Client
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RentedMovies { get; set; }
    }
}
