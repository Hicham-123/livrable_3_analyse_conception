using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Data;


namespace prdv
{
    /// <summary>
    /// Interaction logic for Disponibilite.xaml
    /// </summary>
    public partial class Disponibilite : Window
    {

        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-0TUTE7N;Initial Catalog=prdv;Integrated Security=True");


        public Disponibilite()
        {
            InitializeComponent();
            FillGrid();
        }


        public void FillGrid()
        {
            //CLEAR GRID
            gridEmployee.Columns.Clear();
            //OPEN CONNECTION
            connection.Open();
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


            connection.Close();
        }


        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // Validate the input fields
            if (string.IsNullOrEmpty(txtMatricule.Text))
            {
                MessageBox.Show("Please enter Matricule.");
                return;
            }

            if (string.IsNullOrEmpty(txtNom.Text))
            {
                MessageBox.Show("Please enter Nom.");
                return;
            }

            if (string.IsNullOrEmpty(txtPrenom.Text))
            {
                MessageBox.Show("Please enter Prenom.");
                return;
            }

            if (checkLundi.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txtLundiDebut.Text))
                {
                    MessageBox.Show("Please enter Lundi Debut.");
                    return;
                }

                if (string.IsNullOrEmpty(txtLundiFin.Text))
                {
                    MessageBox.Show("Please enter Lundi Fin.");
                    return;
                }

                if (!TimeSpan.TryParse(txtLundiDebut.Text, out TimeSpan lundiDebut) || !TimeSpan.TryParse(txtLundiFin.Text, out TimeSpan lundiFin))
                {
                    MessageBox.Show("Veuillez entrer une heure valide pour le lundi.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (TimeSpan.Parse(txtLundiFin.Text) - TimeSpan.Parse(txtLundiDebut.Text) <= TimeSpan.Zero)
                {
                    MessageBox.Show("The time difference is not valid. Please enter valid times.");
                    return;
                }
            }

            if (checkMardi.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txtMardiDebut.Text))
                {
                    MessageBox.Show("Please enter Mardi Debut.");
                    return;
                }

                if (string.IsNullOrEmpty(txtMardiFin.Text))
                {
                    MessageBox.Show("Please enter Mardi Fin.");
                    return;
                }


                if (!TimeSpan.TryParse(txtMardiDebut.Text, out TimeSpan mardiDebut) || !TimeSpan.TryParse(txtMardiFin.Text, out TimeSpan mardiFin))
                {
                    MessageBox.Show("Veuillez entrer une heure valide pour le Mardi.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (TimeSpan.Parse(txtMardiFin.Text) - TimeSpan.Parse(txtMardiDebut.Text) <= TimeSpan.Zero)
                {
                    MessageBox.Show("The time difference is not valid. Please enter valid times.");
                    return;
                }
            }

            if (checkMercredi.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txtMercrediDebut.Text))
                {
                    MessageBox.Show("Please enter Mercredi Debut.");
                    return;
                }

                if (string.IsNullOrEmpty(txtMercrediFin.Text))
                {
                    MessageBox.Show("Please enter Mercredi Fin.");
                    return;
                }

                if (!TimeSpan.TryParse(txtMercrediDebut.Text, out TimeSpan mercrediDebut) || !TimeSpan.TryParse(txtMercrediFin.Text, out TimeSpan mercrediFin))
                {
                    MessageBox.Show("Veuillez entrer une heure valide pour le Mercredi.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (TimeSpan.Parse(txtMercrediFin.Text) - TimeSpan.Parse(txtMercrediDebut.Text) <= TimeSpan.Zero)
                {
                    MessageBox.Show("The time difference is not valid. Please enter valid times.");
                    return;
                }
            }

            if (checkJeudi.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txtJeudiDebut.Text))
                {
                    MessageBox.Show("Please enter Jeudi Debut.");
                    return;
                }

                if (string.IsNullOrEmpty(txtJeudiFin.Text))
                {
                    MessageBox.Show("Please enter Jeudi Fin.");
                    return;
                }

                if (!TimeSpan.TryParse(txtJeudiDebut.Text, out TimeSpan jeurdiDebut) || !TimeSpan.TryParse(txtJeudiFin.Text, out TimeSpan jeudiFin))
                {
                    MessageBox.Show("Veuillez entrer une heure valide pour le Jeudi.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (TimeSpan.Parse(txtJeudiFin.Text) - TimeSpan.Parse(txtJeudiDebut.Text) <= TimeSpan.Zero)
                {
                    MessageBox.Show("The time difference is not valid. Please enter valid times.");
                    return;
                }
            }

            if (checkVendredi.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txtVendrediDebut.Text))
                {
                    MessageBox.Show("Please enter Vendredi Debut.");
                    return;
                }

                if (string.IsNullOrEmpty(txtVendrediFin.Text))
                {
                    MessageBox.Show("Please enter Vendredi Fin.");
                    return;
                }

                if (!TimeSpan.TryParse(txtVendrediDebut.Text, out TimeSpan vendrediDebut) || !TimeSpan.TryParse(txtVendrediFin.Text, out TimeSpan vendrediFin))
                {
                    MessageBox.Show("Veuillez entrer une heure valide pour le Vendredi.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (TimeSpan.Parse(txtVendrediFin.Text) - TimeSpan.Parse(txtVendrediDebut.Text) <= TimeSpan.Zero)
                {
                    MessageBox.Show("The time difference is not valid. Please enter valid times.");
                    return;
                }
            }

      
            //OPEN CONNECTION
            connection.Open();
            //QUERY STRING
            string insertSql = "INSERT INTO PrisDeRendezVous (Matricule, Nom, Prenom, LundiDebut, LundiFin, MardiDebut, MardiFin, MercrediDebut, MercrediFin, JeudiDebut, JeudiFin, VendrediDebut, VendrediFin) VALUES (@Matricule, @Nom, @Prenom, @LundiDebut, @LundiFin, @MardiDebut, @MardiFin, @MercrediDebut, @MercrediFin, @JeudiDebut, @JeudiFin, @VendrediDebut, @VendrediFin)";
            //SQL COMMAND
            SqlCommand command = new SqlCommand(insertSql, connection);
            //REPLACING VALUES
            command.Parameters.AddWithValue("@Matricule", txtMatricule.Text);
            command.Parameters.AddWithValue("@Nom", txtNom.Text);
            command.Parameters.AddWithValue("@Prenom", txtPrenom.Text);

            if (checkLundi.IsChecked == true)
            {
                command.Parameters.AddWithValue("@LundiDebut", txtLundiDebut.Text);
                command.Parameters.AddWithValue("@LundiFin", txtLundiFin.Text);
            }
            else
            {
                command.Parameters.AddWithValue("@LundiDebut", DBNull.Value);
                command.Parameters.AddWithValue("@LundiFin", DBNull.Value);
            }

            if (checkMardi.IsChecked == true)
            {
                command.Parameters.AddWithValue("@MardiDebut", txtMardiDebut.Text);
                command.Parameters.AddWithValue("@MardiFin", txtMardiFin.Text);
            }
            else
            {
                command.Parameters.AddWithValue("@MardiDebut", DBNull.Value);
                command.Parameters.AddWithValue("@MardiFin", DBNull.Value);
            }

            if (checkMercredi.IsChecked == true)
            {
                command.Parameters.AddWithValue("@MercrediDebut", txtMercrediDebut.Text);
                command.Parameters.AddWithValue("@MercrediFin", txtMercrediFin.Text);
            }
            else
            {
                command.Parameters.AddWithValue("@MercrediDebut", DBNull.Value);
                command.Parameters.AddWithValue("@MercrediFin", DBNull.Value);
            }

            if (checkJeudi.IsChecked == true)
            {
                command.Parameters.AddWithValue("@JeudiDebut", txtJeudiDebut.Text);
                command.Parameters.AddWithValue("@JeudiFin", txtJeudiFin.Text);
            }
            else
            {
                command.Parameters.AddWithValue("@JeudiDebut", DBNull.Value);
                command.Parameters.AddWithValue("@JeudiFin", DBNull.Value);
            }

            if (checkVendredi.IsChecked == true)
            {
                command.Parameters.AddWithValue("@VendrediDebut", txtVendrediDebut.Text);
                command.Parameters.AddWithValue("@VendrediFin", txtVendrediFin.Text);
            }
            else
            {
                command.Parameters.AddWithValue("@VendrediDebut", DBNull.Value);
                command.Parameters.AddWithValue("@VendrediFin", DBNull.Value);
            }

            // Execute the command and get the number of rows affected
            int rowsAffected = command.ExecuteNonQuery();

                // Close the connection
                connection.Close();

                // Check if the insert was successful
                if (rowsAffected > 0)
                {
                    // Show a success message
                    MessageBox.Show("Data inserted successfully.");
                    FillGrid();
            }
                else
                {
                    // Show an error message
                    MessageBox.Show("Error inserting data.");
                }
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

            if (gridEmployee.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one row to update.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to update the selected rows?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                if (gridEmployee.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select at least one row to delete.");
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete the selected rows?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var selectedRows = gridEmployee.SelectedItems.Cast<DataRowView>().ToList();
                    foreach (var row in selectedRows)
                    {
                        // get the id of the row to delete
                        int id = int.Parse(row["Id"].ToString());

                        // create a SqlCommand to update the selected row with null values for the day and time
                        SqlCommand command = new SqlCommand("UPDATE PrisDeRendezVous SET " + row["Jour"] + "Debut = NULL, " + row["Jour"] + "Fin = NULL WHERE Id = @id", connection);
                        command.Parameters.AddWithValue("@id", id);

                        // execute the command
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        //REFILL
                        FillGrid();

                    }
                }

            }
        }

        private void checkLundi_Checked(object sender, RoutedEventArgs e)
        {
            if (checkLundi.IsChecked == true)
            {
                txtLundiDebut.IsEnabled = true;
                txtLundiFin.IsEnabled = true;
            }
            else
            {
                txtLundiDebut.IsEnabled = false;
                txtLundiFin.IsEnabled = false;
            }
        }

        private void checkMardi_Checked(object sender, RoutedEventArgs e)
        {
            if (checkMardi.IsChecked == true)
            {
                txtMardiDebut.IsEnabled = true;
                txtMardiFin.IsEnabled = true;
            }
            else
            {
                txtMardiDebut.IsEnabled = false;
                txtMardiFin.IsEnabled = false;
            }
        }

        private void checkMercredi_Checked(object sender, RoutedEventArgs e)
        {
            if (checkMercredi.IsChecked == true)
            {
                txtMercrediDebut.IsEnabled = true;
                txtMercrediFin.IsEnabled = true;
            }
            else
            {
                txtMercrediDebut.IsEnabled = false;
                txtMercrediFin.IsEnabled = false;
            }
        }

        private void checkJeudi_Checked(object sender, RoutedEventArgs e)
        {
            if (checkJeudi.IsChecked == true)
            {
                txtJeudiDebut.IsEnabled = true;
                txtJeudiFin.IsEnabled = true;
            }
            else
            {
                txtJeudiDebut.IsEnabled = false;
                txtJeudiFin.IsEnabled = false;
            }
        }

        private void checkVendredi_Checked(object sender, RoutedEventArgs e)
        {
            if (checkVendredi.IsChecked == true)
            {
                txtVendrediDebut.IsEnabled = true;
                txtVendrediFin.IsEnabled = true;
            }
            else
            {
                txtVendrediDebut.IsEnabled = false;
                txtVendrediFin.IsEnabled = false;
            }
        }
    }
}
