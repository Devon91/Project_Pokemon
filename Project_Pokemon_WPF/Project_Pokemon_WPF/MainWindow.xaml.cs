using Project_Pokemon_DAL;
using Project_Pokemon_Models;
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

namespace Project_Pokemon_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<Player> players = new List<Player>();
        Player createPlayer;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //speleres ophalen
            players = DatabaseOperations.GetPlayers();

            //max 3 spelers
            for (int i = players.Count; i <= 4; i++)
            {

                //leeg player object 
                createPlayer = new Player
                {
                    Name = "Create New Player",
                    Id = 0,
                };

                players.Add(createPlayer);

            }

            lbPlayers.ItemsSource = players;

        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayers.SelectedItem is Player player)
            {
                if (player.Id != 0)
                {
                    PlayerInformation.PlayerId = player.Id;

                    MainMenuWindow mainMenuWindow = new MainMenuWindow();
                    mainMenuWindow.Show();
                    this.Close();

                }
                else
                {
                    CreatePlayerWindow createPlayerWindow = new CreatePlayerWindow();
                    createPlayerWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(
                    "Select a player or" + Environment.NewLine +
                    "select 'Create new player' to start a new game",
                    "Select Player", MessageBoxButton.OK, MessageBoxImage.None);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayers.SelectedItem is Player player && player.Id != 0)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {player.Name}?{Environment.NewLine}You can't undo this", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.Yes)
                {
                    DatabaseOperations.DeletePlayer(player);

                    players = DatabaseOperations.GetPlayers();

                    for (int i = players.Count; i <= 4; i++)
                    {
                        createPlayer = new Player
                        {
                            Name = "Create New Player",
                            Id = 0,
                        };

                        players.Add(createPlayer);
                    }
                }
            }
            lbPlayers.ItemsSource = players;
        }
    }
}
