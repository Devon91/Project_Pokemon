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
using Project_Pokemon_DAL;
using Project_Pokemon_Models;

namespace Project_Pokemon_WPF
{
    /// <summary>
    /// Interaction logic for PokemonPartySwap.xaml
    /// </summary>
    public partial class PokemonPartySwap : Window
    {
        public PokemonPartySwap()
        {
            InitializeComponent();
        }

        List<PlayerPokemon> playerPokemons;
        List<PlayerPokemon> catchedPlayerPokemons;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            UpdatePokemonGrids();

        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PokecenterWindow pokecenterWindow = new PokecenterWindow();
            pokecenterWindow.ShowDialog();
        }

        private void BtnMoveRight_Click(object sender, RoutedEventArgs e)
        {
            if (datagridParty.SelectedItem is PlayerPokemon playerPokemon)
            {
                if (datagridParty.Items.Count > 1)
                {
                    playerPokemon.InParty = false;
                    DatabaseOperations.UpdatePlayerPartyPokemons(playerPokemon);

                    datagridParty.ItemsSource = DatabaseOperations.GetPlayerPokemons(PlayerInformation.PlayerId);

                    foreach (var item in playerPokemons)
                    {
                        item.Pokemon.SetMaxHp();
                    }

                    foreach (var item in catchedPlayerPokemons)
                    {
                        item.Pokemon.SetMaxHp();
                    }

                }
                else
                {
                    MessageBox.Show("Every party needs atleast 1 pokemon", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You need to select a pokemon first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdatePokemonGrids();

        }

        private void BtnMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (datagridCatchedPokemons.SelectedItem is PlayerPokemon playerPokemon)
            {
                if (datagridParty.Items.Count < 6)
                {
                    playerPokemon.InParty = true;
                    DatabaseOperations.UpdatePlayerPartyPokemons(playerPokemon);

                    datagridCatchedPokemons.ItemsSource = DatabaseOperations.GetCatchedPlayerPokemons(PlayerInformation.PlayerId);

                    foreach (var item in playerPokemons)
                    {
                        item.Pokemon.SetMaxHp();
                    }

                    foreach (var item in catchedPlayerPokemons)
                    {
                        item.Pokemon.SetMaxHp();
                    }

                }
                else
                {
                    MessageBox.Show("Every party can't have more than 6 pokemons", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                foreach (var item in playerPokemons)
                {
                    item.Pokemon.SetMaxHp();
                }

                foreach (var item in catchedPlayerPokemons)
                {
                    item.Pokemon.SetMaxHp();
                }
            }
            else
            {
                MessageBox.Show("You need to select a pokemon first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            UpdatePokemonGrids();
        }

        public void UpdatePokemonGrids()
        {
            playerPokemons = DatabaseOperations.GetPlayerPokemons(PlayerInformation.PlayerId);

            foreach (var item in playerPokemons)
            {
                item.Pokemon.SetMaxHp();
            }

            datagridParty.ItemsSource = playerPokemons;

            catchedPlayerPokemons = DatabaseOperations.GetCatchedPlayerPokemons(PlayerInformation.PlayerId);

            foreach (var item in catchedPlayerPokemons)
            {
                item.Pokemon.SetMaxHp();
            }

            datagridCatchedPokemons.ItemsSource = catchedPlayerPokemons;

        }
    }
}
