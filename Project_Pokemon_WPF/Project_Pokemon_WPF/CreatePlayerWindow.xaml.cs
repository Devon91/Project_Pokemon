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
    /// Interaction logic for CreatePlayerWindow.xaml
    /// </summary>
    public partial class CreatePlayerWindow : Window
    {
        public CreatePlayerWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private Pokemon pokemon;

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Player player = new Player();
            PlayerPokemon playerPokemon = new PlayerPokemon();

            player.Name = txtPlayerName.Text;
            playerPokemon.Name = txtRenamePokemon.Text;





            if (player.IsGeldig() && playerPokemon.IsGeldig() && PokemonSelected() != false)
            {


                TimeSpan startTime = new TimeSpan(0, 0, 0);


                player.Pokedollar = 10000;
                player.PlayedTime = startTime;

                int ok = DatabaseOperations.AddPlayer(player);

                Random r = new Random();

                PlayerPokedex playerPokedex = new PlayerPokedex();

                int id = 0;

                playerPokedex.PlayerId = player.Id;

                if (tbBulbasaur.IsChecked == true)
                {
                    playerPokemon.Height = 0.7m;
                    playerPokemon.Weight = 6.9m;
                    id = 1;
                    playerPokedex.Caught = true;
                    playerPokedex.Encountered = true;
                    playerPokedex.PokedexId = id;

                }
                else if (tbCharmander.IsChecked == true)
                {
                    playerPokemon.Height = 0.6m;
                    playerPokemon.Weight = 8.5m;
                    id = 4;
                    playerPokedex.Caught = true;
                    playerPokedex.Encountered = true;
                    playerPokedex.PokedexId = id;

                }
                else if (tbSquirtle.IsChecked == true)
                {
                    playerPokemon.Height = 0.5m;
                    playerPokemon.Weight = 9;
                    id = 7;
                    playerPokedex.Caught = true;
                    playerPokedex.Encountered = true;
                    playerPokedex.PokedexId = id;

                }



                DatabaseOperations.AddNewPokemonPlayerPokedex(playerPokedex);


                pokemon = new Pokemon
                {
                    Level = 7,
                    Hp = r.Next(0, 32),
                    CurrentHp = 0,
                    Defense = r.Next(0, 32),
                    Attack = r.Next(0, 32),
                    SpecialAttack = r.Next(0, 32),
                    SpecialDefense = r.Next(0, 32),
                    Speed = r.Next(0, 32),
                    PokedexId = id,

                };

                playerPokemon.PokemonId = DatabaseOperations.AddPokemon(pokemon);

                pokemon.Pokedex = DatabaseOperations.GetPokedexEntry(id);
                pokemon.SetMaxHp();
                pokemon.CurrentHp = pokemon.CalculatedMaxHP;

                DatabaseOperations.UpdatePokemonCurrentHp(pokemon);

                

                
                playerPokemon.InParty = true;
                playerPokemon.PlayerId = player.Id;
                playerPokemon.Xp = Convert.ToInt32(Math.Pow(pokemon.Level - 1, 3));

                DatabaseOperations.AddPlayerPokemon(playerPokemon);

                List<PokemonAttack> pokemonAttacks = DatabaseOperations.GetAvailableAttacks(pokemon.PokedexId, pokemon.Level);

                foreach (var item in pokemonAttacks)
                {
                    if (pokemon.Level >= item.RequiredLevel)
                    {
                        OwnedPokemonAttack ownedPokemonAttack = new OwnedPokemonAttack
                        {
                            AttackId = item.AttackId,
                            CurrentPp = item.Attack.Pp,
                            PokemonId = playerPokemon.PokemonId,
                            KnownAttack = true
                        };
                        DatabaseOperations.AddCaughtPokemonAttacks(ownedPokemonAttack);

                    }

                }

                PlayerInformation.PlayerId = ok;

                MainMenuWindow mainMenuWindow = new MainMenuWindow();
                mainMenuWindow.Show();
                this.Close();
            }
            else if (!player.IsGeldig())
            {
                MessageBox.Show(player.Error);
            }
            else if (!playerPokemon.IsGeldig())
            {
                MessageBox.Show(playerPokemon.Error);
            }
            else
            {
                MessageBox.Show("You need to select a pokemon first.");
            }



        }

       

        private bool PokemonSelected()
        {
            if (tbBulbasaur.IsChecked == false && tbCharmander.IsChecked == false && tbSquirtle.IsChecked == false)
            {
                return false;

            }
            else
            {
                return true;
            }
            
            
        }

        private void TbBulbasaur_Click(object sender, RoutedEventArgs e)
        {
            txtRenamePokemon.Text = "Bulbasaur";
            tbCharmander.IsChecked = false;
            tbSquirtle.IsChecked = false;
        }

        private void TbCharmander_Click(object sender, RoutedEventArgs e)
        {
            txtRenamePokemon.Text = "Charmander";
            tbBulbasaur.IsChecked = false;
            tbSquirtle.IsChecked = false;
        }

        private void TbSquirtle_Click(object sender, RoutedEventArgs e)
        {
            txtRenamePokemon.Text = "Squirtle";
            tbBulbasaur.IsChecked = false;
            tbCharmander.IsChecked = false;
        }
    }
}
