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
    /// Interaction logic for PokecenterWindow.xaml
    /// </summary>
    public partial class PokecenterWindow : Window
    {
        public PokecenterWindow()
        {
            InitializeComponent();
        }

        List<PlayerPokemon> playerPokemons = DatabaseOperations.GetPlayerPokemons(PlayerInformation.PlayerId);

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSwap_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PokemonPartySwap pokemonPartySwap = new PokemonPartySwap();
            pokemonPartySwap.ShowDialog();

        }

        private void BtnHeal_Click(object sender, RoutedEventArgs e)
        {

            foreach (PlayerPokemon playerPokemon in playerPokemons)
            {
                playerPokemon.Pokemon.SetMaxHp();

                playerPokemon.Pokemon.CurrentHp = playerPokemon.Pokemon.CalculatedMaxHP;

                List<OwnedPokemonAttack> pokemonAttacks = DatabaseOperations.GetKnownAttacks(playerPokemon.PokemonId);

                foreach (var item in pokemonAttacks)
                {
                    item.CurrentPp = item.Attack.Pp;
                    DatabaseOperations.UpdateKnownAttacks(item);
                }

                DatabaseOperations.UpdatePokemonCurrentHp(playerPokemon.Pokemon);
            }
            

            datagridParty.ItemsSource = playerPokemons;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<PlayerPokemon> playerPokemons = DatabaseOperations.GetPlayerPokemons(PlayerInformation.PlayerId);

            foreach (PlayerPokemon playerPokemon in playerPokemons)
            {
                playerPokemon.Pokemon.SetMaxHp();

            }

            datagridParty.ItemsSource = playerPokemons;

        }
    }
}
