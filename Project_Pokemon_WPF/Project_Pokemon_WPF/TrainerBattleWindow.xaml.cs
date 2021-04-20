using Project_Pokemon_DAL;
using Project_Pokemon_Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Project_Pokemon_WPF
{
    /// <summary>
    /// Interaction logic for TrainerBattleWindow.xaml
    /// </summary>
    public partial class TrainerBattleWindow : Window
    {
        public TrainerBattleWindow()
        {
            InitializeComponent();
        }


        //Poperties
        private Pokemon pokemon;
        private List<TrainerPokemon> enemyPokemonList;

        int enemyPokemonListIndex = 0;

        PlayerPokemon playerPokemon;
        Pokemon enemyPokemon;

        List<Button> attackButtonList;

        //aanval lijsten
        private List<OwnedPokemonAttack> playerPokemonAttacks = new List<OwnedPokemonAttack>();
        private List<OwnedPokemonAttack> knownAttackList = new List<OwnedPokemonAttack>();



        // TRAINER/GYMLEADER BATTLE

        Npc Trainer;


        public TrainerBattleWindow(Npc battleTrainer)
        {
            InitializeComponent();

            Trainer = battleTrainer;

            //lijst enemy pokemons
            enemyPokemonList = DatabaseOperations.GetTrainerPokemonInformation(battleTrainer.Id);

            // andere trainer UI
            lblEnemyName.Content = battleTrainer.Name;
            txtInformation.Text = $"{battleTrainer.Name} wants to fight you{Environment.NewLine}";
            btnRun.IsEnabled = false;

            enemyPokemon = enemyPokemonList[enemyPokemonListIndex].Pokemon;
            knownAttackList.AddRange(enemyPokemon.OwnedPokemonAttacks);

            EnemyUi();
        }

        // WILD BATTLE
        public TrainerBattleWindow(Found wildPokemon)
        {
            InitializeComponent();

            Random r = new Random();

            Pokedex pokedex = wildPokemon.Pokedex;

            pokemon = new Pokemon
            {
                Level = r.Next(wildPokemon.MinLevel, wildPokemon.MaxLevel + 1),
                Hp = r.Next(0, 32),
                CurrentHp = 0,
                Defense = r.Next(0, 32),
                Attack = r.Next(0, 32),
                SpecialAttack = r.Next(0, 32),
                SpecialDefense = r.Next(0, 32),
                Speed = r.Next(0, 32),
                PokedexId = pokedex.Id,
                Pokedex = pokedex,
            };

            pokemon.SetMaxHp();
            pokemon.CurrentHp = pokemon.CalculatedMaxHP;

            List<PokemonAttack> possibleAttacksList = DatabaseOperations.GetAvailableAttacks(pokemon.PokedexId, pokemon.Level);
            List<Attack> attackList = new List<Attack>();

            foreach (var item in possibleAttacksList)
            {
                attackList.Add(DatabaseOperations.GetAttack(item.AttackId));
            }

            List<int> knownAttackListIds = new List<int>();

            while (knownAttackList.Count != attackList.Count)
            {
                Random rx = new Random();
                int attackId = rx.Next(0, attackList.Count);
                Attack attackToLearn = attackList[attackId];

                OwnedPokemonAttack newAttack = new OwnedPokemonAttack
                {

                    CurrentPp = attackToLearn.Pp,
                    AttackId = attackToLearn.Id,
                    Attack = attackToLearn
                };

                if (knownAttackList.Count < 4)
                {
                    newAttack.KnownAttack = true;

                    if (!knownAttackListIds.Contains(newAttack.AttackId))
                    {
                        knownAttackListIds.Add(newAttack.AttackId);
                        knownAttackList.Add(newAttack);
                    }
                }
                else
                {
                    newAttack.KnownAttack = false;

                    if (!knownAttackListIds.Contains(newAttack.AttackId))
                    {
                        knownAttackListIds.Add(newAttack.AttackId);
                        knownAttackList.Add(newAttack);
                    }
                }
            }


            //wild pokemon UI
            lblEnemyName.Content = "Wild Pokemon";
            txtInformation.Text = $"You encountered a wild {pokemon.Pokedex.Name}{Environment.NewLine}";
            btnRun.IsEnabled = true;

            enemyPokemon = pokemon;

            EnemyUi();

        }


        Player player;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player = DatabaseOperations.GetPlayer(PlayerInformation.PlayerId);

            PlayerPokemonUi();
            StartBattle();
            lblPlayerName.Content = player.Name;
        }

        private void BtnFight_Click(object sender, RoutedEventArgs e)
        {
            // attack buttons in lijst zetten
            attackButtonList = new List<Button>
            {
                btnAttack1,
                btnAttack2,
                btnAttack3,
                btnAttack4
            };

            // attack buttons opvullen en tonen die aan aanval toegewezen hebben gekregen
            for (int i = 0; i < playerPokemonAttacks.Count; i++)
            {
                attackButtonList[i].Content = playerPokemonAttacks[i].Attack.Name;
                attackButtonList[i].Visibility = Visibility.Visible;
                attackButtonList[i].ToolTip = "PP: " + playerPokemonAttacks[i].CurrentPp + " / " +
                    playerPokemonAttacks[i].Attack.Pp;
                if (playerPokemonAttacks[i].CurrentPp == 0)
                {
                    attackButtonList[i].IsEnabled = false;
                }
                else
                {
                    attackButtonList[i].IsEnabled = true;
                }
            }

            //menu buttons dichtklappen
            btnFight.Visibility = Visibility.Collapsed;
            btnBag.Visibility = Visibility.Collapsed;
            btnPokemon.Visibility = Visibility.Collapsed;
            btnRun.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
        }

        private void BtnPokemon_Click(object sender, RoutedEventArgs e)
        {
            //update pokemon
            DatabaseOperations.UpdatePlayerPokemon(playerPokemon);

            if (playerPokemon.Pokemon.CurrentHp == 0)
            {
                               
                PokemonPartyWindow pokemonPartyWindow = new PokemonPartyWindow();
                pokemonPartyWindow.btnChangePokemon.Visibility = Visibility.Visible;
                pokemonPartyWindow.btnAttackSwap.Visibility = Visibility.Collapsed;
                pokemonPartyWindow.btnFirstPokemon.Visibility = Visibility.Collapsed;
                pokemonPartyWindow.btnCancel.Visibility = Visibility.Collapsed;

                pokemonPartyWindow.ShowDialog();
                LevelGained = false;
                PlayerPokemonUi();
                StartBattle();
            }
            else
            {

                PokemonPartyWindow pokemonPartyWindow = new PokemonPartyWindow();
                pokemonPartyWindow.btnChangePokemon.Visibility = Visibility.Visible;
                pokemonPartyWindow.btnAttackSwap.Visibility = Visibility.Collapsed;
                pokemonPartyWindow.btnFirstPokemon.Visibility = Visibility.Collapsed;
                pokemonPartyWindow.btnCancel.Visibility = Visibility.Visible;

                pokemonPartyWindow.ShowDialog();

                //check of nieuwe pokemon is gekozen of cancel button is gebruikt
                if (PlayerPokemonStatics.PlayerPokemon.PokemonId != playerPokemon.PokemonId)
                {
                    //update pokemon 
                    DatabaseOperations.UpdatePlayerPokemon(playerPokemon);

                    //nieuwe pokemon, vul UI op en eindig turn
                    PlayerPokemonUi();
                    EndPlayerTurn(false);
                    LevelGained = false;
                }
                else
                {
                    //if cancel wordt static terug op de huidige pokemon gezet
                    PlayerPokemonStatics.PlayerPokemon.PokemonId = playerPokemon.PokemonId;
                }

            }

        }

        private bool caught;
        private List<PlayerPokedex> playerPokedexList = null;

        private void BtnBag_Click(object sender, RoutedEventArgs e)
        {
            DatabaseOperations.UpdatePlayerPokemon(playerPokemon);

            InventoryWindow inventory = new InventoryWindow(playerPokemon);

            //enemyPokemon, knownAttackList, 

            inventory.ShowDialog();

            PlayerPokemonStatics.PlayerPokemon = DatabaseOperations.GetPlayerPokemon(playerPokemon.Id);//LoadPlayerPokemon(playerPokemon.Id);


            PlayerPokedex entry;

            if (InventoryWindow.catchRateMultiplier > 0)
            {
                caught = Catch((InventoryWindow.catchRateMultiplier), enemyPokemon, knownAttackList);

                playerPokedexList = DatabaseOperations.GetPlayerPokedexEntries(PlayerInformation.PlayerId);

            }
            else if(inventory.cancel == false)
            {
                txtInformation.Text = $"You healed {playerPokemon.Name}";
            }


            if (caught == true)
            {
                entry = new PlayerPokedex
                {
                    PokedexId = pokemon.PokedexId,
                    PlayerId = PlayerInformation.PlayerId,
                    Caught = true,
                    Encountered = true
                };

                if (!playerPokedexList.Contains(entry))
                {
                    

                    DatabaseOperations.AddCaughtPlayerPokedex(entry);
                }
                else
                {
                    foreach (var item in playerPokedexList)
                    {
                        if (item.PokedexId == entry.PokedexId && item.PlayerId == entry.PlayerId)
                        {
                            entry.Id = item.Id;
                            break;
                        }
                    }             

                    DatabaseOperations.UpdatePlayerPokedex(entry);

                }

                EndBattle();

            }
            else
            {
                if (inventory.cancel == false)
                {
                    EndPlayerTurn(false);
                }
            }

            PlayerPokemonUi();


            
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            DatabaseOperations.UpdatePlayerPokemon(playerPokemon);

            this.Close();
            
        }

        //Attackbuttons
        private void BtnAttack1_Click(object sender, RoutedEventArgs e)
        {
            PlayerPokemonAttack(0);

        }

        private void BtnAttack2_Click(object sender, RoutedEventArgs e)
        {
            PlayerPokemonAttack(1);

        }

        private void BtnAttack3_Click(object sender, RoutedEventArgs e)
        {
            PlayerPokemonAttack(2);

        }

        private void Btnattack4_Click(object sender, RoutedEventArgs e)
        {
            PlayerPokemonAttack(3);

        }


        //UI opvullen speler pokemon
        private void PlayerPokemonUi()
        {
            playerPokemon = PlayerPokemonStatics.PlayerPokemon;

            playerPokemonAttacks = DatabaseOperations.GetKnownAttacks(playerPokemon.PokemonId);
            lblPlayerPokemon.Content = playerPokemon.Name + " lvl: " + playerPokemon.Pokemon.Level;

            playerPokemon.Pokemon.SetStats();

            // Hp eigen pokemon weergeven

            pbHpPlayerPokemon.Value = (playerPokemon.Pokemon.CurrentHp / Convert.ToDouble(playerPokemon.Pokemon.CalculatedMaxHP)) * 100;
            lblHpPlayer.Content = "HP: " + playerPokemon.Pokemon.CurrentHp + "/" + playerPokemon.Pokemon.CalculatedMaxHP;

            //xp

            double xpNeeded = Math.Pow(playerPokemon.Pokemon.Level, 3);
            double currentXp = playerPokemon.Xp;

            pbXpPlayerPokemon.Value =   ((currentXp - Math.Pow(playerPokemon.Pokemon.Level - 1, 3)) /
                                        (xpNeeded - Math.Pow(playerPokemon.Pokemon.Level - 1, 3)))
                                        * 100;

            //image
            imgPlayerPokemon.Source = new BitmapImage(new Uri(@"images\sprites\" + playerPokemon.Pokemon.Pokedex.ImgReference, UriKind.Relative));
        }


        public void EnemyUi()
        {

            enemyPokemon.SetStats();
            enemyPokemon.CurrentHp = enemyPokemon.CalculatedMaxHP;
            //zet hp int's om naar doubles       

            lblEnemyPokemon.Content = $"{enemyPokemon.Pokedex.Name} lvl {enemyPokemon.Level}";
            pbHpEnemyPokemon.Value = (enemyPokemon.CurrentHp / Convert.ToDouble(enemyPokemon.CalculatedMaxHP)) * 100;
            lblHpEnemy.Content = $"HP: {enemyPokemon.CurrentHp}/{enemyPokemon.CalculatedMaxHP}";

            imgEnemyPokemon.Source = new BitmapImage(new Uri(@"images\sprites\" + enemyPokemon.Pokedex.ImgReference, UriKind.Relative));

            if (lblEnemyName.Content.ToString() != "Wild Pokemon")
            {
                knownAttackList.Clear();
                
                knownAttackList.AddRange(enemyPokemon.OwnedPokemonAttacks);
            }

        }

        public void StartBattle()
        {
            List<PlayerPokedex> playerPokedexList = null;
            PlayerPokedex entry; 

            playerPokedexList = DatabaseOperations.GetPlayerPokedexEntries(PlayerInformation.PlayerId);

            entry = new PlayerPokedex
            {
                PokedexId = enemyPokemon.PokedexId,
                PlayerId = PlayerInformation.PlayerId,
                Caught = false,
                Encountered = true
            };

            if (!playerPokedexList.Contains(entry))
            {
                DatabaseOperations.AddCaughtPlayerPokedex(entry);
            }


            btnEnemyTurn.Visibility = Visibility.Visible;
            txtInformation.Text = "A new battle has started!" + Environment.NewLine;


            if (enemyPokemon.CalculatedSpeed > playerPokemon.Pokemon.CalculatedSpeed)
            {
                txtInformation.Text += $"the enemy {enemyPokemon.Pokedex.Name} is faster and attacks first{Environment.NewLine}";
                btnEnemyTurn.IsEnabled = true;
                btnBag.IsEnabled = false;
                btnRun.IsEnabled = false;
                btnFight.IsEnabled = false;
                btnPokemon.IsEnabled = false;

                txtInformation.Text += Environment.NewLine + "It is the enemy pokemon's turn!" + Environment.NewLine;
            }
            else
            {
                btnEnemyTurn.IsEnabled = false;
                btnBag.IsEnabled = true;
                btnRun.IsEnabled = true;
                btnFight.IsEnabled = true;
                btnPokemon.IsEnabled = true;

                txtInformation.Text += $"Your {playerPokemon.Name} is faster and attacks first{Environment.NewLine}";
            }

        }

        public bool Attack(Pokemon attackingPokemon, Pokemon defendingPokemon, OwnedPokemonAttack attack)
        {
            Random accuracy = new Random();

            //check accuracy
            if (accuracy.Next(0, 101) <= attack.Attack.Accuracy)
            {

                Random randomAttackDamage = new Random();

                //random attack modifier
                double modifier = Convert.ToDouble(randomAttackDamage.Next(85, 101)) / 100;

                //type modifier
                if (attack.Attack.Type.TypeEffectiveId == defendingPokemon.Pokedex.TypeId)
                {
                    modifier *= 2;
                    txtInformation.Text += ", it was very effective";

                }
                else if (attack.Attack.Type.TypeNotEffectiveId == defendingPokemon.Pokedex.TypeId)
                {
                    modifier *= 0.5;
                    txtInformation.Text += ", it was not very effective";
                }

                //attack type is hetzelfde als pokemon type
                if (attack.Attack.TypeId == attackingPokemon.Pokedex.TypeId)
                {
                    modifier *= 1.5;
                }


                double damage;

                if (attack.Attack.Type.Name.ToLower() == "normal")
                {
                    //normal attack
                    damage = (((((2 * attackingPokemon.Level) / 5) + 2) * attack.Attack.BaseDamage * (attackingPokemon.CalculatedAttack / defendingPokemon.CalculatedDefense) / 50) + 2) * modifier;
                }
                else
                {
                    //special attack
                    damage = (((((2 * attackingPokemon.Level) / 5) + 2) * attack.Attack.BaseDamage * (attackingPokemon.CalculatedSpecialAttack / defendingPokemon.CalculatedSpecialDefense) / 50) + 2) * modifier;
                }

                txtInformation.Text += Environment.NewLine + "it did " + Convert.ToInt32(Math.Round(damage)) + " damage" + Environment.NewLine;

                defendingPokemon.CurrentHp -= Convert.ToInt32(Math.Round(damage));

            }
            else if (attack.Attack.Name.ToLower() == "splash")
            {
                txtInformation.Text += Environment.NewLine + "But nothing happened...";
            }
            else
            {
                txtInformation.Text += Environment.NewLine + "It missed..."; 
            }


            return defendingPokemon.CheckHp();

        }

        private void PlayerPokemonAttack(int attackListIndex)
        {

            txtInformation.Text = $"{playerPokemon.Name} used {playerPokemonAttacks[attackListIndex].Attack.Name}";

            if (playerPokemonAttacks[attackListIndex].CurrentPp > 0)
            {
                playerPokemonAttacks[attackListIndex].CurrentPp--;
            }
            
            DatabaseOperations.UpdateKnownAttacks(playerPokemonAttacks[attackListIndex]);

            bool dead = Attack(playerPokemon.Pokemon, enemyPokemon, playerPokemonAttacks[attackListIndex]);


            attackButtonList[attackListIndex].ToolTip = "PP: " + playerPokemonAttacks[attackListIndex].CurrentPp + " / " +
                    playerPokemonAttacks[attackListIndex].Attack.Pp; ;

            if (playerPokemonAttacks[attackListIndex].CurrentPp == 0)
            {
                attackButtonList[attackListIndex].IsEnabled = false;
            }

            btnBack.Visibility = Visibility.Hidden;

            EndPlayerTurn(dead);

        }

        private void EndEnemyTurn(bool dead)
        {
            btnAttack1.Visibility = Visibility.Collapsed;
            btnAttack2.Visibility = Visibility.Collapsed;
            btnAttack3.Visibility = Visibility.Collapsed;
            btnAttack4.Visibility = Visibility.Collapsed;

            btnFight.Visibility = Visibility.Visible;
            btnBag.Visibility = Visibility.Visible;
            btnPokemon.Visibility = Visibility.Visible;
            btnRun.Visibility = Visibility.Visible;

            //update health
            pbHpPlayerPokemon.Value = (playerPokemon.Pokemon.CurrentHp / Convert.ToDouble(playerPokemon.Pokemon.CalculatedMaxHP)) * 100;
            lblHpEnemy.Content = $"HP: {enemyPokemon.CurrentHp}/{enemyPokemon.CalculatedMaxHP}";

            if (dead)
            {
                txtInformation.Text += Environment.NewLine + "Your " + playerPokemon.Name + " has Fainted." + Environment.NewLine;
                imgPlayerPokemon.Opacity = 0;

                DatabaseOperations.UpdatePlayerPokemon(playerPokemon);

                PokemonPartyWindow switchPokemon = new PokemonPartyWindow();

                bool allDead = true;

                foreach (PlayerPokemon playerPokemon in switchPokemon.datagridParty.Items)
                {
                    if (playerPokemon.Pokemon.CurrentHp > 0)
                    {
                        allDead = false;
                    }
                }

                if (allDead)
                {
                    txtInformation.Text += "All you pokemons have fainted, you have no more to fight with" +
                                            Environment.NewLine + lblEnemyName.Content.ToString() + " has defeated you";

                    EndBattle();
                }
                else
                {
                    imgPlayerPokemon.Opacity = 100;
                    btnPokemon.IsEnabled = true;
                    btnBag.IsEnabled = false;
                    btnRun.IsEnabled = false;
                    btnFight.IsEnabled = false;
                    btnEnemyTurn.IsEnabled = false;
                    txtInformation.Text += "Pick another pokemon";

                }

            }
            else
            {
                btnEnemyTurn.IsEnabled = false;
                btnBag.IsEnabled = true;
                btnRun.IsEnabled = true;
                btnFight.IsEnabled = true;
                btnPokemon.IsEnabled = true;
            }
        }

        private void EndPlayerTurn(bool dead)
        {
            //update health
            pbHpEnemyPokemon.Value = (enemyPokemon.CurrentHp / Convert.ToDouble(enemyPokemon.CalculatedMaxHP)) * 100;
            lblHpEnemy.Content = $"HP: {enemyPokemon.CurrentHp}/{enemyPokemon.CalculatedMaxHP}";
            int gainedXp =0;

            if (dead)
            {
                txtInformation.Text += "the enemy " + enemyPokemon.Pokedex.Name + " has Fainted." + Environment.NewLine;
                txtInformation.Text += playerPokemon.Name + " has gained ";

                if (lblEnemyName.Content.ToString() == "Wild Pokemon")
                {

                    gainedXp += Convert.ToInt32((enemyPokemon.Pokedex.BaseExperience * enemyPokemon.Level * 1) / 7);
                    playerPokemon.Xp += gainedXp;
                    txtInformation.Text += gainedXp + " Xp" + Environment.NewLine;

                    LevelUp();

                    EndBattle();
                }
                else
                {
                    gainedXp += Convert.ToInt32((enemyPokemon.Pokedex.BaseExperience * enemyPokemon.Level * 1.5) / 7);
                    playerPokemon.Xp += gainedXp;
                    txtInformation.Text += gainedXp + " Xp" + Environment.NewLine;

                    LevelUp();
                    
                    btnRun.IsEnabled = false;
                    btnPokemon.IsEnabled = false;
                    btnBag.IsEnabled = false;
                    btnFight.IsEnabled = false;

                    // volgende trainer pokemon
                    if (enemyPokemonListIndex < enemyPokemonList.Count - 1)
                    {
                        txtInformation.Text += $"{Environment.NewLine}{Trainer.Name} has {(enemyPokemonList.Count - 1) - enemyPokemonListIndex} pokemon left";

                        btnEnemyNextPokemon.Visibility = Visibility.Visible;
                        btnEnemyTurn.Visibility = Visibility.Collapsed;

                        imgEnemyPokemon.Opacity = 0;

                    }
                    else
                    {
                        txtInformation.Text += $"{Environment.NewLine}You have defeated {lblEnemyName.Content.ToString()}!";                       
                        PlayerInformation.IncreasePlayerPokedollar(player , Convert.ToInt32(Trainer.Pokedollar));    
                        txtInformation.Text += $"{Environment.NewLine}You gained {Convert.ToInt32(Trainer.Pokedollar)} pokedollars{Environment.NewLine}";

                        if (Trainer.Type.ToLower() == "gym leader")
                        {
                            //add badge to playeritems
                           
                            Item badgeItem = DatabaseOperations.GetGymLeaderBadge(Trainer.Id).Item;
                            PlayerItem playerBadge = new PlayerItem
                            {
                                PlayerId = PlayerInformation.PlayerId,
                                Amount = 1,
                                ItemId = badgeItem.Id,
                            };

                            List<PlayerItem> playerItems = DatabaseOperations.GetPlayerBadges(PlayerInformation.PlayerId);

                            if (!playerItems.Contains(playerBadge))
                            {
                                DatabaseOperations.AddPlayerItem(playerBadge);
                                txtInformation.Text += $"You have earned the {badgeItem.Name} badge, for defeating {Trainer.Name}!";

                            }
                            else
                            {
                                txtInformation.Text += Environment.NewLine + "You didn't get a badge because you already have this badge.";
                            }



                    }

                        EndBattle();
                    }

                }


                
            }
            else
            {          

                btnEnemyTurn.IsEnabled = true;
                btnBag.IsEnabled = false;
                btnRun.IsEnabled = false;
                btnFight.IsEnabled = false;
                btnPokemon.IsEnabled = false;

                txtInformation.Text += Environment.NewLine + "It is the enemy pokemon's turn!" + Environment.NewLine;
            }

            btnAttack1.Visibility = Visibility.Collapsed;
            btnAttack2.Visibility = Visibility.Collapsed;
            btnAttack3.Visibility = Visibility.Collapsed;
            btnAttack4.Visibility = Visibility.Collapsed;

            btnFight.Visibility = Visibility.Visible;
            btnBag.Visibility = Visibility.Visible;
            btnPokemon.Visibility = Visibility.Visible;
            btnRun.Visibility = Visibility.Visible;

        }


        private void BtnEnemyTurn_Click(object sender, RoutedEventArgs e)
        {
            OwnedPokemonAttack enemyAttack;

            Random randomEnemyAttack = new Random();
            do
            {
                enemyAttack = knownAttackList[randomEnemyAttack.Next(0, knownAttackList.Count)];
            } while (enemyAttack.KnownAttack == false);
            // Test attack wild pokemon fix 

            txtInformation.Text = $"{enemyPokemon.Pokedex.Name} used {enemyAttack.Attack.Name}";

            bool dead = Attack(enemyPokemon, playerPokemon.Pokemon, enemyAttack);

            pbHpPlayerPokemon.Value = (playerPokemon.Pokemon.CurrentHp / Convert.ToDouble(playerPokemon.Pokemon.CalculatedMaxHP)) * 100;
            lblHpPlayer.Content = $"HP: {playerPokemon.Pokemon.CurrentHp}/{Convert.ToDouble(playerPokemon.Pokemon.CalculatedMaxHP)}";

            EndEnemyTurn(dead);

        }


        private void EndBattle()
        {

            
            bool evolved = false;
            if (LevelGained)
            {
                evolved = PokemonEvolve();
            }

            //update pokemon
            DatabaseOperations.UpdatePlayerPokemon(playerPokemon);

            if (evolved)
            {
                PlayerPokemonUi();
            }

            btnEnemyNextPokemon.Visibility = Visibility.Collapsed;
            btnEnemyTurn.Visibility = Visibility.Collapsed;

            //update player
            DatabaseOperations.UpdatePlayer(player);

            btnRun.Content = "End battle";
            btnRun.IsEnabled = true;
            btnPokemon.IsEnabled = false;
            btnBag.IsEnabled = false;
            btnFight.IsEnabled = false;
            btnEnemyTurn.IsEnabled = false;
        }


        private bool Catch(double catchMultiplier, Pokemon pokemon, List<OwnedPokemonAttack> attacks)
        {
            if (lblEnemyName.Content.ToString() == "Wild Pokemon")
            {
                int catchRate = pokemon.Pokedex.CatchRate;
                double catchMissingHp = ((100 - pbHpEnemyPokemon.Value) / 100) + 1;
                double catchNumber = ((catchRate * catchMissingHp) * catchMultiplier);

                Random r = new Random();

                if (r.Next(0, 101) <= catchNumber)
                {
                    // pokemon object aanmaken om te inserten
                    Pokemon p = new Pokemon
                    {
                        Level = pokemon.Level,
                        Attack = pokemon.Attack,
                        Defense = pokemon.Defense,
                        Hp = pokemon.Hp,
                        CurrentHp = pokemon.CalculatedMaxHP,
                        SpecialAttack = pokemon.SpecialAttack,
                        SpecialDefense = pokemon.SpecialDefense,
                        Speed = pokemon.Speed,
                        PokedexId = pokemon.PokedexId
                    };

                    int id = DatabaseOperations.AddPokemon(p);

                    // speler pokemon aanmaken om te inserten
                    PlayerPokemon newPlayerPokemon = new PlayerPokemon
                    {
                        Name = pokemon.Pokedex.Name,
                        Height = pokemon.Pokedex.AvgHeight,
                        Weight = pokemon.Pokedex.AvgWeight,
                        Xp = (pokemon.Level - 1) * (pokemon.Level - 1) * (pokemon.Level - 1),
                        InParty = false,
                        PlayerId = PlayerInformation.PlayerId,
                        PokemonId = id,
                    };

                    DatabaseOperations.AddPlayerPokemon(newPlayerPokemon);

                    // pokemon aanvallen aanmaken om te inserten
                    foreach (var item in attacks)
                    {
                        OwnedPokemonAttack attack = new OwnedPokemonAttack
                        {
                            CurrentPp = item.CurrentPp,
                            PokemonId = id,
                            AttackId = item.AttackId,

                        };
                        if (item.KnownAttack == true)
                        {
                           attack.KnownAttack = true;

                        }
                        else
                        {
                            attack.KnownAttack = false;
                        }

                        

                        DatabaseOperations.AddCaughtPokemonAttacks(attack);
                    }

                    txtInformation.Text = $"Congratulations , You caught a {pokemon.Pokedex.Name}!";
                    imgEnemyPokemon.Source = new BitmapImage(new Uri(@"images\sprites\pokeball.png", UriKind.Relative));
                    imgEnemyPokemon.Width = 50;
                    imgEnemyPokemon.Height = 50;
                    imgEnemyPokemon.VerticalAlignment = VerticalAlignment.Center;

                    return true;
                }
                else
                {
                    txtInformation.Text = $"{pokemon.Pokedex.Name} broke free, Try again!";

                    return false;
                }
            }
            else
            {
                txtInformation.Text = $"You can't catch another trainer's pokemon";

                return false;
            }

        }

        private void BtnEnemyNextPokemon_Click(object sender, RoutedEventArgs e)
        {
            enemyPokemonListIndex++;
            enemyPokemon = enemyPokemonList[enemyPokemonListIndex].Pokemon;

            txtInformation.Text = string.Empty;
            btnEnemyNextPokemon.Visibility = Visibility.Collapsed;

            EnemyUi();
            PlayerPokemonUi();

            imgEnemyPokemon.Opacity = 100;

            StartBattle();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            btnAttack1.Visibility = Visibility.Collapsed;
            btnAttack2.Visibility = Visibility.Collapsed;
            btnAttack3.Visibility = Visibility.Collapsed;
            btnAttack4.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Collapsed;

            btnFight.Visibility = Visibility.Visible;
            btnBag.Visibility = Visibility.Visible;
            btnPokemon.Visibility = Visibility.Visible;
            btnRun.Visibility = Visibility.Visible;
        }

        bool LevelGained = false;

        private void LevelUp()
        {
            
            if (playerPokemon.Xp >= Convert.ToInt32(Math.Pow(playerPokemon.Pokemon.Level, 3)))
            {
                LevelGained = true;
                playerPokemon.Pokemon.Level++;
                txtInformation.Text += $"{Environment.NewLine}Your {playerPokemon.Name} leveled up to level {playerPokemon.Pokemon.Level}! {Environment.NewLine}";


                List<PokemonAttack> attackList = DatabaseOperations.GetAvailableAttacks(playerPokemon.Pokemon.PokedexId, playerPokemon.Pokemon.Level);
                List<OwnedPokemonAttack> knownAttacks = DatabaseOperations.GetKnownAttacks(PlayerPokemonStatics.PlayerPokemon.PokemonId);
                List<OwnedPokemonAttack> knownAttacksFalse = DatabaseOperations.GetKnownAttacksEqualsFalse(PlayerPokemonStatics.PlayerPokemon.PokemonId);

                foreach (var item in attackList)
                {
                    if (playerPokemon.Pokemon.Level >= item.RequiredLevel)
                    {
                        OwnedPokemonAttack newAttack = new OwnedPokemonAttack()
                        {
                            AttackId = item.AttackId,
                            CurrentPp = item.Attack.Pp,
                            PokemonId = playerPokemon.PokemonId,
                            KnownAttack = false
                        };


                        if (!knownAttacks.Contains(newAttack) && !knownAttacksFalse.Contains(newAttack))
                        {
                            DatabaseOperations.AddCaughtPokemonAttacks(newAttack);
                            txtInformation.Text += $"Your {playerPokemon.Name} can learn a new move" + Environment.NewLine;
                        }
                        


                        
                    }
                }

                PlayerPokemonUi();
            }
            

        }

        private bool PokemonEvolve()
        {

            if (playerPokemon.Pokemon.Pokedex.EvolveLevel != null)
            {
                if (playerPokemon.Pokemon.Level >= playerPokemon.Pokemon.Pokedex.EvolveLevel)
                {
                    txtInformation.Text += $"{Environment.NewLine}It seems like your {playerPokemon.Pokemon.Pokedex.Name} has evolved into a ";

                    PlayerPokemonStatics.PlayerPokemon = playerPokemon;

                    Pokemon name = playerPokemon.Pokemon;

                    Pokemon evolvedPokemon = new Pokemon()
                    {
                        Id = PlayerPokemonStatics.PlayerPokemon.Pokemon.Id,
                        Level = playerPokemon.Pokemon.Level,
                        Hp = playerPokemon.Pokemon.Hp,
                        CurrentHp = playerPokemon.Pokemon.CurrentHp,
                        Attack = playerPokemon.Pokemon.Attack,
                        Defense = playerPokemon.Pokemon.Defense,
                        SpecialAttack = playerPokemon.Pokemon.SpecialAttack,
                        SpecialDefense = playerPokemon.Pokemon.SpecialDefense,
                        Speed = playerPokemon.Pokemon.Speed,
                        PokedexId = playerPokemon.Pokemon.PokedexId + 1,
                    };

                    playerPokemon.Pokemon = evolvedPokemon;

                    DatabaseOperations.UpdatePokemonCurrentHp(playerPokemon.Pokemon);


                    evolvedPokemon.Pokedex = DatabaseOperations.GetPokedexEntry(evolvedPokemon.PokedexId);

                    if (playerPokemon.Name == name.Pokedex.Name)
                    {
                        playerPokemon.Name = evolvedPokemon.Pokedex.Name;
                    }


                    playerPokedexList = DatabaseOperations.GetPlayerPokedexEntries(PlayerInformation.PlayerId);

                    PlayerPokedex playerPokedexEntry = new PlayerPokedex
                    {
                        PlayerId = PlayerInformation.PlayerId,
                        PokedexId = evolvedPokemon.PokedexId,
                        Caught = true,
                        Encountered = true
                    };


                    if (!playerPokedexList.Contains(playerPokedexEntry))
                    {
                        DatabaseOperations.AddCaughtPlayerPokedex(playerPokedexEntry);
                    }
                    else if (playerPokedexList.Contains(playerPokedexEntry))
                    {
                        foreach (var item in playerPokedexList)
                        {
                            if (item.PokedexId == playerPokedexEntry.PokedexId && item.PlayerId == playerPokedexEntry.PlayerId)
                            {
                                playerPokedexEntry.Id = item.Id;
                            }
                        }
                        DatabaseOperations.UpdatePlayerPokedex(playerPokedexEntry);
                    }

                    txtInformation.Text += playerPokemon.Pokemon.Pokedex.Name + Environment.NewLine;

                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
