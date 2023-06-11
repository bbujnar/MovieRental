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

                newFilm.IDFilmu = default;
                newFilm.Tytuł = title.Text;
                newFilm.Gatunek = genre.Text;
                newFilm.Reżyser = director.Text;
                newFilm.Rok = int.Parse(year.Text);

                newEntity.Filmy.Add(newFilm);
                newEntity.SaveChanges();
            }

                

            Window_Loaded(sender, e);
        }



    }
}
