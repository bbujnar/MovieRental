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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace MovieRental
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MovieRentalEntities context = new MovieRentalEntities();
        CollectionViewSource filmViewSource;

        
        public MainWindow()
        {
            InitializeComponent();
            filmViewSource = (CollectionViewSource)(FindResource("filmyViewSource"));
            DataContext = this;



        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource filmyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("filmyViewSource")));
            

            context.Filmy.Load();
            filmViewSource.Source = context.Filmy.Local;
        }

        private void collectData(object sender, RoutedEventArgs e)
        {


            using (MovieRentalEntities newEntity = new MovieRentalEntities())
            {
                Filmy newFilm = new Filmy();

                bool titleExists = newEntity.Filmy.Any(c => c.Tytuł == title.Text);
                if (!titleExists && (int.Parse(year.Text)>999 && int.Parse(year.Text)<10000))
                {
                    newFilm.Tytuł = title.Text;
                    newFilm.Gatunek = genre.Text;
                    newFilm.Reżyser = director.Text;
                    newFilm.Kraj = nation.Text;
                    newFilm.Rok = int.Parse(year.Text);

                    newEntity.Filmy.Add(newFilm);
                    newEntity.SaveChanges();
                    MessageBox.Show("Pomyślnie dodano film!");
                }
                else if(titleExists)
                {
                    MessageBox.Show("Tytuł już występuje w bazie");
                }
                else
                {
                    MessageBox.Show("Zły rok produkcji");
                }
               

            }

                

            ReloadPage();
        }


        private void RegexCheck( object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void deleteMovie(object sender, RoutedEventArgs e )
        {

            using(MovieRentalEntities newEntity = new MovieRentalEntities())
            {
                int idToDelete = int.Parse(deleteID.Text);

                bool movieExists = newEntity.Filmy.Any(movie => movie.IDFilmu == idToDelete);
                if (movieExists)
                {

                    var recordToDelete = newEntity.Filmy.FirstOrDefault(movie => movie.IDFilmu == idToDelete);


                    if (recordToDelete != null)
                    {
                        try
                        {
                            newEntity.Filmy.Remove(recordToDelete);
                            newEntity.SaveChanges();
                            MessageBox.Show("Pozytywnie usunięto film!");
                        }
                        catch ( DbUpdateException ex )
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Film nie istnieje!");
                }
            }


            ReloadPage();

        }

        MovieRentalClients clientsWindow = new MovieRentalClients();

        private void switchToClients( object sender, RoutedEventArgs e)
        {

            if(clientsWindow == null || !clientsWindow.IsVisible)
            {
                clientsWindow= new MovieRentalClients();
                clientsWindow.Closed += ClientsWindow_Closed;
                clientsWindow.Show();

            }
        }

        private void ClientsWindow_Closed(object sender, EventArgs e)
        {
            clientsWindow = null;
        }

        private void ReloadPage()
        {
            Window newWindow = new MainWindow();
            newWindow.Show();
            Close();

        }
    }
}
