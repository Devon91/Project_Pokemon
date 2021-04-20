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
using System.Windows.Shapes;

namespace Project_Pokemon_WPF
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        List<Npc> trainers;
        List<Area> areas;
        List<PlayerItem> badges;

        //initiele area 'Route 1' index
        int areasListIndex = 0;

        DateTime startPlayTime;

        //generatedPokemon voor wild battle

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //areas ophalen en in een lijst zetten
            areas = DatabaseOperations.GetAreas();

            //start area Name 'Route 1' met index 0
            txtArea.Text = areas[areasListIndex].Name;
            
            //trainers inladen voor route 1
            trainers = DatabaseOperations.GetTrainers(areasListIndex + 1);

            //start playtime
            startPlayTime = DateTime.Now;

            //buiten cities buttons hidden
            btnGym.Visibility = Visibility.Hidden;
            btnPokemMart.Visibility = Visibility.Hidden;
            btnPokeCenter.Visibility = Visibility.Hidden;

            badges = DatabaseOperations.GetPlayerBadges(PlayerInformation.PlayerId);
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if(areasListIndex > 0)
            {
                //naam van en spelers voor previous area
                areasListIndex--;
                txtArea.Text = areas[areasListIndex].Name;
                trainers = DatabaseOperations.GetTrainers(areasListIndex + 1);
            }

            if (txtArea.Text.ToLower().Contains("city"))
            {
                btnGym.Visibility = Visibility.Visible;
                btnPokemMart.Visibility = Visibility.Visible;
                btnPokeCenter.Visibility = Visibility.Visible;
                btnWild.IsEnabled = false;
                btnTrainer.IsEnabled = false;

            }
            else
            {
                btnWild.IsEnabled = true;
                btnTrainer.IsEnabled = true;
                btnGym.Visibility = Visibility.Hidden;
                btnPokemMart.Visibility = Visibility.Hidden;
                btnPokeCenter.Visibility = Visibility.Hidden;

            }

            if (areasListIndex == 0)
            {
                btnPrevious.Visibility = Visibility.Hidden;
            }

            btnNext.Visibility = Visibility.Visible;
            btnNext.IsEnabled = true;

        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (areasListIndex < areas.Count - 1)
            {
                //naam van en spelers voor next area
                areasListIndex++;
                txtArea.Text = areas[areasListIndex].Name;
                trainers = DatabaseOperations.GetTrainers(areasListIndex + 1);
            }

            if (txtArea.Text.ToLower().Contains("city"))
            {
                btnGym.Visibility = Visibility.Visible;
                btnPokemMart.Visibility = Visibility.Visible;
                btnPokeCenter.Visibility = Visibility.Visible;
                btnWild.IsEnabled = false;
                btnTrainer.IsEnabled = false;

            }
            else
            {
                btnWild.IsEnabled = true;
                btnTrainer.IsEnabled = true;
                btnGym.Visibility = Visibility.Hidden;
                btnPokemMart.Visibility = Visibility.Hidden;
                btnPokeCenter.Visibility = Visibility.Hidden;

            }

            btnPrevious.Visibility = Visibility.Visible;
            btnNext.IsEnabled = true;

            CheckBadges();

        }

        private void BtnWild_Click(object sender, RoutedEventArgs e)
        {
            Found wildPokemon = GenerateRandomPokemon(areas[areasListIndex].Id);


            PokemonPartyWindow firstPokemonPartyWindow = new PokemonPartyWindow(wildPokemon);

            firstPokemonPartyWindow.btnCancel.Visibility = Visibility.Visible;
            firstPokemonPartyWindow.btnAttackSwap.Visibility = Visibility.Collapsed;
            firstPokemonPartyWindow.btnChangePokemon.Visibility = Visibility.Collapsed;
            firstPokemonPartyWindow.btnFirstPokemon.Visibility = Visibility.Visible;
            firstPokemonPartyWindow.lblPokemon.Content = "Pick a pokemon to start the battle with";

            

            firstPokemonPartyWindow.ShowDialog();

            

        }

        private void BtnTrainer_Click(object sender, RoutedEventArgs e)
        {
            Random randomTrainer = new Random();

            if(trainers.Count > 0)
            {
                int trainerIndex =  randomTrainer.Next(0,trainers.Count);

                PokemonPartyWindow firstPokemonPartyWindow = new PokemonPartyWindow(trainers[trainerIndex]);

                firstPokemonPartyWindow.lblPokemon.Content = "Pick a pokemon to start the battle with";

                firstPokemonPartyWindow.ShowDialog();

            }
            else
            {
                MessageBox.Show("There are no trainers in this area");
            }
            

        }

        private void BtnParty_Click(object sender, RoutedEventArgs e)
        {

            PokemonPartyWindow pokemonPartyWindow = new PokemonPartyWindow();
            pokemonPartyWindow.btnChangePokemon.Visibility = Visibility.Collapsed;
            pokemonPartyWindow.btnAttackSwap.Visibility = Visibility.Visible;
            pokemonPartyWindow.btnFirstPokemon.Visibility = Visibility.Collapsed;
            pokemonPartyWindow.btnCancel.Visibility = Visibility.Visible;
            pokemonPartyWindow.ShowDialog();
        }

        private void BtnPokedex_Click(object sender, RoutedEventArgs e)
        {
            PokedexWindow pokedexWindow = new PokedexWindow();
            pokedexWindow.ShowDialog();
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow inventoryWindow = new InventoryWindow();
            inventoryWindow.ShowDialog();
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void BtnPokemMart_Click(object sender, RoutedEventArgs e)
        {
            PokemarktWindow pokemarktWindow = new PokemarktWindow();
            pokemarktWindow.ShowDialog();
        }

        private void btnPokeCenter_Click(object sender, RoutedEventArgs e)
        {
            PokecenterWindow pokecenterWindow = new PokecenterWindow();
            pokecenterWindow.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //verschil start playtime en eind playtime, updaten in DB voor PlayedTime
            Player player = DatabaseOperations.GetPlayer(PlayerInformation.PlayerId);

            TimeSpan sessionPlaytime = (DateTime.Now - startPlayTime);
            player.PlayedTime += sessionPlaytime;

            DatabaseOperations.UpdatePlayer(player);

            App.Current.Shutdown();

        }

        private Found GenerateRandomPokemon(int areaId)
        {
            List<Found> foundPokemons = DatabaseOperations.GetFoundPokemonViaAreaId(areaId);
            List<int> pokemonEncounterValues = new List<int>();
            

            Random randomPokemon = new Random();

            //encounter rate eerste pokemon
            int encounterRatePokemon = 0;
            

            for (int i = 0; i < foundPokemons.Count; i++)
            {
                encounterRatePokemon += Convert.ToInt32(foundPokemons[i].EncounterRate);
                pokemonEncounterValues.Add(encounterRatePokemon);
            }

            //random pokemon encounter value

            int rolledEncounterValue = randomPokemon.Next(1, encounterRatePokemon + 1);

            int indexCounter = 0;

            Found generatedPokemon = null;

            do
            {
                if (rolledEncounterValue <= pokemonEncounterValues[indexCounter])
                {
                    generatedPokemon = foundPokemons[indexCounter];

                }

                indexCounter++;

            } while (generatedPokemon == null);

            return generatedPokemon;
 
        }

        private void btnGym_Click(object sender, RoutedEventArgs e)
        {
            if (trainers.Count > 0)
            {
                PokemonPartyWindow pokemonPartyWindow = new PokemonPartyWindow(trainers[0]);

                pokemonPartyWindow.ShowDialog();
                badges = DatabaseOperations.GetPlayerBadges(PlayerInformation.PlayerId);
                CheckBadges();
            }
        }

        private void CheckBadges()
        {
            btnNext.IsEnabled = true;

            switch (areasListIndex)
            {
                case 2:
                    if (badges.Count < 1)
                    {
                        btnNext.IsEnabled = false;
                    }
                    break;
                case 5:
                    if (badges.Count < 2)
                    {
                        btnNext.IsEnabled = false;
                    }
                    break;
                case 8:
                    if (badges.Count < 3)
                    {
                        btnNext.IsEnabled = false;
                    }
                    break;
                case 11:
                    btnNext.Visibility = Visibility.Hidden;
                    
                    break;
            }
        }
    }
}
