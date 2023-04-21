using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace prdv
{
    /// <summary>
    /// Interaction logic for Rendezvous.xaml
    /// </summary>
    /// 

    public partial class Rendezvous : Window
    {

        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-0TUTE7N;Initial Catalog=prdv;Integrated Security=True");
        private DataTable dataTable = new DataTable();

        public Rendezvous()
        {
            InitializeComponent();
            // Define an array of week names in French
            string[] weekNames = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi"};

            // Add the week names to the ComboBox
            foreach (string weekName in weekNames)
            {
                txtCombobox.Items.Add(weekName);
            }


            FillGrid();
        }



        public void FillGrid()
        {
            //CLEAR GRID
            gridEmployee.Columns.Clear();

            //SQL COMMAND
            SqlCommand command = new SqlCommand(
                "SELECT Id, Matricule, Nom, Prenom, LundiDebut AS HeureDebut, LundiFin AS HeureFin, 'Lundi' AS Jour FROM PrisDeRendezVous WHERE LundiDebut IS NOT NULL OR LundiFin IS NOT NULL " +
                "UNION " +
                "SELECT Id, Matricule, Nom, Prenom, MardiDebut AS HeureDebut, MardiFin AS HeureFin, 'Mardi' AS Jour FROM PrisDeRendezVous WHERE MardiDebut IS NOT NULL OR MardiFin IS NOT NULL " +
                "UNION " +
                "SELECT Id, Matricule, Nom, Prenom, MercrediDebut AS HeureDebut, MercrediFin AS HeureFin, 'Mercredi' AS Jour FROM PrisDeRendezVous WHERE MercrediDebut IS NOT NULL OR MercrediFin IS NOT NULL " +
                "UNION " +
                "SELECT Id, Matricule, Nom, Prenom, JeudiDebut AS HeureDebut, JeudiFin AS HeureFin, 'Jeudi' AS Jour FROM PrisDeRendezVous WHERE JeudiDebut IS NOT NULL OR JeudiFin IS NOT NULL " +
                "UNION " +
                "SELECT Id, Matricule, Nom, Prenom, VendrediDebut AS HeureDebut, VendrediFin AS HeureFin, 'Vendredi' AS Jour FROM PrisDeRendezVous WHERE VendrediDebut IS NOT NULL OR VendrediFin IS NOT NULL ",
                connection);

            // Create a data adapter object
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            // Create a dataset object to hold the data
            DataSet dataSet = new DataSet();

            // Fill the dataset with data
            adapter.Fill(dataSet);

            DataGridCheckBoxColumn column = new DataGridCheckBoxColumn();
            column.Header = "Delete";
            column.Binding = new Binding("Delete");
            gridEmployee.Columns.Add(column);

            gridEmployee.ItemsSource = dataSet.Tables[0].DefaultView;
            gridEmployee.AutoGenerateColumns = false;
            gridEmployee.Columns.Add(new DataGridTextColumn { Header = "Id", Binding = new Binding("Id"), Visibility = Visibility.Collapsed });
            gridEmployee.Columns.Add(new DataGridTextColumn { Header = "Matricule", Binding = new Binding("Matricule") });
            gridEmployee.Columns.Add(new DataGridTextColumn { Header = "Nom", Binding = new Binding("Nom") });
            gridEmployee.Columns.Add(new DataGridTextColumn { Header = "Prenom", Binding = new Binding("Prenom") });
            gridEmployee.Columns.Add(new DataGridTextColumn { Header = "Jour", Binding = new Binding("Jour") });
            gridEmployee.Columns.Add(new DataGridTextColumn { Header = "HeureDebut", Binding = new Binding("HeureDebut") });
            gridEmployee.Columns.Add(new DataGridTextColumn { Header = "HeureFin", Binding = new Binding("HeureFin") });
        }
        private void btnRechercher_Click(object sender, RoutedEventArgs e)
        {

            if (!TimeSpan.TryParse(txtHeureDebut.Text, out TimeSpan heureDebut) ||
                !TimeSpan.TryParse(txtHeureFin.Text, out TimeSpan heureFin)
            )
            {
                MessageBox.Show("Veuillez entrer une heure valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Set the column names to search based on the selected day of the week
            string debutColumn = "";
            string finColumn = "";


            string jour = txtCombobox.Text;

            switch (jour)
            {
                case "Lundi":
                    debutColumn = "LundiDebut";
                    finColumn = "LundiFin";
                    break;
                case "Mardi":
                    debutColumn = "MardiDebut";
                    finColumn = "MardiFin";
                    break;
                case "Mercredi":
                    debutColumn = "MercrediDebut";
                    finColumn = "MercrediFin";
                    break;
                case "Jeudi":
                    debutColumn = "JeudiDebut";
                    finColumn = "JeudiFin";
                    break;
                case "Vendredi":
                    debutColumn = "VendrediDebut";
                    finColumn = "VendrediFin";
                    break;
                case "Samedi":
                    debutColumn = "SamediDebut";
                    finColumn = "SamediFin";
                    break;
                case "Dimanche":
                    debutColumn = "DimancheDebut";
                    finColumn = "DimancheFin";
                    break;
                default:
                    MessageBox.Show("Invalid day of the week specified.");
                    return;
            }

            // Get the start and end times from txtHeureDebut and txtHeureFin
            TimeSpan debut, fin;
            if (!TimeSpan.TryParse(txtHeureDebut.Text, out debut) || !TimeSpan.TryParse(txtHeureFin.Text, out fin))
            {
                MessageBox.Show("Invalid start or end time specified.");
                return;
            }

            if (TimeSpan.Parse(txtHeureFin.Text) - TimeSpan.Parse(txtHeureDebut.Text) <= TimeSpan.Zero)
            {
                MessageBox.Show("The time difference is not valid. Please enter valid times.");
                return;
            }

            // Build the SQL query based on the selected day of the week and start and end times
            string query = "SELECT Id, Matricule, Nom, Prenom, " + debutColumn + " AS HeureDebut, " + finColumn + " AS HeureFin, '" + jour + "' AS Jour " +
                           "FROM PrisDeRendezVous " +
                           "WHERE " + debutColumn + " >= '" + debut + "' AND " + finColumn + " <= '" + fin + "'";

            // Create a data adapter object and fill the dataset with data
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            // Set the data source for the gridview to the dataset
            gridEmployee.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtHeureDebut.Clear();
            txtHeureFin.Clear();
            FillGrid();
        }
    }
}
