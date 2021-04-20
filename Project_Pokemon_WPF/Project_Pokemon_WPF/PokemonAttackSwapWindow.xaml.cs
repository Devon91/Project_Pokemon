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
    /// Interaction logic for PokemonAttackSwapWindow.xaml
    /// </summary>
    public partial class PokemonAttackSwapWindow : Window
    {
        public PokemonAttackSwapWindow()
        {
            InitializeComponent();
        }

        private List<PlayerPokemon> playerPokemonList;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            playerPokemonList = DatabaseOperations.GetPokemons(PlayerPokemonStatics.PlayerPokemon.Id);

            playerPokemonList[0].Pokemon.SetStats();

            datagridPokemonInformation.ItemsSource = playerPokemonList;

            datagridPokemonKnownAttacks.ItemsSource = DatabaseOperations.GetKnownAttacks(PlayerPokemonStatics.PlayerPokemon.PokemonId);
            datagridPokemonAvailableAttacks.ItemsSource = DatabaseOperations.GetKnownAttacksEqualsFalse(PlayerPokemonStatics.PlayerPokemon.PokemonId);
        }

        private void BtnMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (datagridPokemonAvailableAttacks.SelectedItem is OwnedPokemonAttack attack)
            {
                if (datagridPokemonKnownAttacks.Items.Count < 4)
                {
                    attack.KnownAttack = true;

                    DatabaseOperations.UpdateKnownAttacks(attack);

                    datagridPokemonKnownAttacks.ItemsSource = DatabaseOperations.GetKnownAttacks(PlayerPokemonStatics.PlayerPokemon.PokemonId);
                    datagridPokemonAvailableAttacks.ItemsSource = DatabaseOperations.GetKnownAttacksEqualsFalse(PlayerPokemonStatics.PlayerPokemon.PokemonId);
                }
                else
                {
                    MessageBox.Show("A pokemon can't learn more than 4 attacks", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You need to select a attack first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            datagridPokemonAvailableAttacks.SelectedItem = null;
        }

        private void BtnMoveRight_Click(object sender, RoutedEventArgs e)
        {
            if (datagridPokemonKnownAttacks.SelectedItem is OwnedPokemonAttack knownAttack)
            {
                if (datagridPokemonKnownAttacks.Items.Count > 1)
                {
                    knownAttack.KnownAttack = false;
                    DatabaseOperations.UpdateKnownAttacks(knownAttack);
                    datagridPokemonAvailableAttacks.ItemsSource = DatabaseOperations.GetKnownAttacksEqualsFalse(PlayerPokemonStatics.PlayerPokemon.PokemonId);
                    datagridPokemonKnownAttacks.ItemsSource = DatabaseOperations.GetKnownAttacks(PlayerPokemonStatics.PlayerPokemon.PokemonId);
                }
                else
                {
                    MessageBox.Show("Every pokemon needs atleast 1 attack", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You need to select a attack first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
