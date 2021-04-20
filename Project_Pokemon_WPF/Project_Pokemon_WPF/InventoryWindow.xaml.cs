using Project_Pokemon_DAL;
using Project_Pokemon_Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Project_Pokemon_WPF
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        public InventoryWindow()
        {
            InitializeComponent();

            btnUse.Visibility = Visibility.Collapsed;
        }

        public InventoryWindow(PlayerPokemon playerPokemon)
        {
            usedPokemon = playerPokemon;
            catchRateMultiplier = -1;
            InitializeComponent();
        }

        public bool cancel;
        private PlayerPokemon usedPokemon;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblPokecoin.Content = "$ " + DatabaseOperations.GetPlayer(PlayerInformation.PlayerId).Pokedollar;
            rbItems.IsChecked = true;
        }

        private void RbItems_Checked(object sender, RoutedEventArgs e)
        {
            datagridInventory.ItemsSource = DatabaseOperations.GetPlayerItems(PlayerInformation.PlayerId, false);
        }

        private void RbBadges_Checked(object sender, RoutedEventArgs e)
        {
            datagridInventory.ItemsSource = DatabaseOperations.GetPlayerBadges(PlayerInformation.PlayerId);
        }

        private void DatagridInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridInventory.SelectedItem is PlayerItem playerItem)
            {
                lblItemDescription.Content = playerItem.Item.Description;

                if (playerItem.Item.Type == "Medicine" || playerItem.Item.Type == "Ball")
                {
                    btnUse.IsEnabled = true;
                }
            }
        }

        private void BtnInventoryBack_Click(object sender, RoutedEventArgs e)
        {

            cancel = true;
            this.Close();
        }

        public static double catchRateMultiplier;

        private void BtnUse_Click(object sender, RoutedEventArgs e)
        {
            if (datagridInventory.SelectedItem is PlayerItem playerItem)
            {
                if (playerItem.Item.Type == "Medicine")
                {
                    this.Close();


                    if (playerItem.Item.HealAmount != null && usedPokemon.Pokemon.CurrentHp < usedPokemon.Pokemon.CalculatedMaxHP)
                    {
                        if (usedPokemon.Pokemon.CurrentHp + (int)playerItem.Item.HealAmount <= usedPokemon.Pokemon.CalculatedMaxHP)
                        {
                            usedPokemon.Pokemon.CurrentHp += (int)playerItem.Item.HealAmount;
                        }
                        else
                        {
                            usedPokemon.Pokemon.CurrentHp = usedPokemon.Pokemon.CalculatedMaxHP;
                        }
                        DatabaseOperations.UpdatePokemonCurrentHp(usedPokemon.Pokemon);

                        playerItem.Amount -= 1;

                        DatabaseOperations.UpdatePlayerItem(playerItem);

                        if (playerItem.Amount <= 0)
                        {
                            DatabaseOperations.DeletePlayerItem(playerItem);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You can't use this item right now", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (playerItem.Item.Type == "Ball")
                {
                    playerItem.Amount--;
                    DatabaseOperations.UpdatePlayerItem(playerItem);

                    if (playerItem.Item.CatchRateMultiplier is decimal c)
                    {
                        catchRateMultiplier = decimal.ToDouble(c);
                    }

                    if (playerItem.Amount <= 0)
                    {
                        DatabaseOperations.DeletePlayerItem(playerItem);
                    }

                    this.Close();
                }
            }
        }
    }
}
