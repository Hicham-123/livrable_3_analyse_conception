using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prdv
{


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenDisponibiliteWindow(object sender, RoutedEventArgs e)
        {
            Disponibilite objectWindow = new Disponibilite();
            objectWindow.Show();
        }

        private void OpenRendezvousWindow(object sender, RoutedEventArgs e)
        {
            Rendezvous objectWindow = new Rendezvous();
            objectWindow.Show();
        }

    }
}
