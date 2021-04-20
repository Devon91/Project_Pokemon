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
    /// Interaction logic for Pokemon_Party.xaml
    /// </summary>
    public partial class PokemonPartyWindow : Window
    {
        public PokemonPartyWindow()
        {
            InitializeComponent();

            List<PlayerPokemon> playerPokemons = DatabaseOperations.GetPlayerPokemons(PlayerInformation.PlayerId);

            foreach (var item in playerPokemons)
            {
                item.Pokemon.SetMaxHp();
                item.Pokemon.CurrentHp = item.Pokemon.CurrentHp;
                item.Pokemon.CheckHp();
            }

            datagridParty.ItemsSource = playerPokemons;
        }

        public PokemonPartyWindow(int itemId)
        {
            InitializeComponent();
            item = itemId;
            
        }

        Npc battleTrainer;

        public PokemonPartyWindow(Npc trainer)
        {
            InitializeComponent();
            btnCancel.Visibility = Visibility.Visible;
            btnAttackSwap.Visibility = Visibility.Collapsed;
            btnChangePokemon.Visibility = Visibility.Collapsed;
            btnFirstPokemon.Visibility = Visibility.Visible;
            battleTrainer = trainer;
        }

        Found wildBattlePokemon;

        public PokemonPartyWindow(Found wildPokemon)
        {
            InitializeComponent();
            wildBattlePokemon = wildPokemon;
        }


        public PokemonPartyWindow(bool fight)
        {
            InitializeComponent();

            if (fight == true)
            {
                btnAttackSwap.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Collapsed;
                btnChangePokemon.Visibility = Visibility.Collapsed;
            }
        }

        //public int selectedPokemon;
        public int selectedPokemonLevel;
        public int item;

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAttackSwap_Click(object sender, RoutedEventArgs e)
        {

            if (datagridParty.SelectedItem is PlayerPokemon playerPokemon)
            {
                PlayerPokemonStatics.PlayerPokemon = playerPokemon;

                PokemonAttackSwapWindow attackSwapWindow = new PokemonAttackSwapWindow();
                attackSwapWindow.ShowDialog();

            }
            else
            {
                MessageBox.Show("Select a pokemon to change it's attacks", "Pokemon Selection", MessageBoxButton.OK, MessageBoxImage.None);
            }

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            List<PlayerPokemon> playerPokemons = DatabaseOperations.GetPlayerPokemons(PlayerInformation.PlayerId);


            foreach (var item in playerPokemons)
            {
                item.Pokemon.SetMaxHp();
                item.Pokemon.CurrentHp = item.Pokemon.CurrentHp;
                item.Pokemon.CheckHp();
            }

            datagridParty.ItemsSource = playerPokemons;

        }

       

        private void BtnChangePokemon_Click(object sender, RoutedEventArgs e)
        {
            if (datagridParty.SelectedItem is PlayerPokemon playerPokemon)
            {
                if (playerPokemon.Pokemon.Fainted == "Fainted")
                {

                    MessageBox.Show($"You can't fight with a pokemon that has been fainted{Environment.NewLine}Pick another one", "Pokemon Selection", MessageBoxButton.OK, MessageBoxImage.None);
                }
                else if (playerPokemon.Id == PlayerPokemonStatics.PlayerPokemon.Id)
                {
                    MessageBox.Show($"You're already using this pokemon!{Environment.NewLine}Pick another one", "Pokemon Selection", MessageBoxButton.OK, MessageBoxImage.None);
                }
                else
                {
                    PlayerPokemonStatics.PlayerPokemon = playerPokemon;
                    this.Close();
                }

            }
            else
            {
                MessageBox.Show("Select a pokemon to sbattle with", "Pokemon Selection", MessageBoxButton.OK, MessageBoxImage.None);
            }

            
        }

        private void BtnFirstPokemon_Click(object sender, RoutedEventArgs e)
        {
            TrainerBattleWindow trainerBattle = null;

            if (datagridParty.SelectedItem is PlayerPokemon playerPokemon)
            {
                if (playerPokemon.Pokemon.CurrentHp <= 0)
                {
                    MessageBox.Show("You can't start with a fainted pokemon", "Pokemon Selection", MessageBoxButton.OK, MessageBoxImage.None);
                }
                else
                {
                    PlayerPokemonStatics.PlayerPokemon = playerPokemon;

                    if (battleTrainer != null)
                    {
                        trainerBattle = new TrainerBattleWindow(battleTrainer);
                    }
                    else if (wildBattlePokemon != null)
                    {
                        trainerBattle = new TrainerBattleWindow(wildBattlePokemon);
                    }

                    this.Close();

                    trainerBattle.ShowDialog();
                }
                
            }
            else
            {
                MessageBox.Show("Select a pokemon to start the battle", "Pokemon Selection",MessageBoxButton.OK, MessageBoxImage.None);
            }
            

        }

    }
}
