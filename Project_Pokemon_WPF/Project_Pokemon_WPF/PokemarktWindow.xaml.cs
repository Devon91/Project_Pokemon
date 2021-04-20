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
    /// Interaction logic for Pokemarkt.xaml
    /// </summary>
    public partial class PokemarktWindow : Window
    {
        public PokemarktWindow()
        {
            InitializeComponent();
        }

        private void BtnPokemarktCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RbBuy_Checked(object sender, RoutedEventArgs e)
        {
            datagridPokemarktBuy.Visibility = Visibility.Visible;
            datagridPokemarktSell.Visibility = Visibility.Hidden;

            lblItemDescription.Content = string.Empty;

            datagridPokemarktBuy.ItemsSource = DatabaseOperations.GetItems();
        }

        private void RbSell_Checked(object sender, RoutedEventArgs e)
        {
            datagridPokemarktBuy.Visibility = Visibility.Hidden;
            datagridPokemarktSell.Visibility = Visibility.Visible;

            lblItemDescription.Content = string.Empty;

            datagridPokemarktSell.ItemsSource = DatabaseOperations.GetPlayerItems(PlayerInformation.PlayerId, true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rbBuy.IsChecked = true;
            lblPokecoin.Content = "$ " + DatabaseOperations.GetPlayer(PlayerInformation.PlayerId).Pokedollar;
        }

        private void DatagridPokemarktBuy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridPokemarktBuy.SelectedItem is Item item)
            {
                lblItemDescription.Content = item.Description + Environment.NewLine;
            }
        }

        private void DatagridPokemarktSell_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridPokemarktSell.SelectedItem is PlayerItem playerItem)
            {
                lblItemDescription.Content = playerItem.Item.Description + Environment.NewLine;
            }
        }

        private string foutmelding;

        private void BtnPokemarktConfirm_Click(object sender, RoutedEventArgs e)
        {
            foutmelding = Valideer("txtAmount");

            // Kopen
            if (rbBuy.IsChecked == true)
            {
                foutmelding += Valideer("datagridPokemarktBuy");

                if (string.IsNullOrWhiteSpace(foutmelding))
                {
                    if (datagridPokemarktBuy.SelectedItem is Item item)
                    {
                        BuyItem(item);
                    }
                }
                datagridPokemarktBuy.Items.Refresh();
            }
            // Verkopen
            else
            {
                foutmelding += Valideer("datagridPokemarktSell");

                if (string.IsNullOrWhiteSpace(foutmelding))
                {
                    if (datagridPokemarktSell.SelectedItem is PlayerItem playerItem)
                    {
                        SellItem(playerItem);
                    }
                }
                datagridPokemarktSell.ItemsSource = DatabaseOperations.GetPlayerItems(PlayerInformation.PlayerId, true);
                
            }
            if (foutmelding != string.Empty)
            {
                lblItemDescription.Content += foutmelding;
            }
            lblPokecoin.Content = "$ " + DatabaseOperations.GetPlayer(PlayerInformation.PlayerId).Pokedollar;

            txtAmount.Text = "";
        }

        private void SellItem(PlayerItem playerItem)
        {
            Player player = DatabaseOperations.GetPlayer(PlayerInformation.PlayerId);

            // label van description terug instellen op enkel item description zonder foutmelding
            lblItemDescription.Content = playerItem.Item.Description + Environment.NewLine;

            if (playerItem != null && playerItem.Amount >= Convert.ToInt32(txtAmount.Text))
            {
                // speler item hoeveelheid aanpassen
                playerItem.Amount -= short.Parse(txtAmount.Text);
                DatabaseOperations.UpdatePlayerItem(playerItem);

                // speler te ontvangen bedrag berekenen
                Item item = DatabaseOperations.GetItem(playerItem.ItemId);
                int addAmount = (int)item.SellPrice * Convert.ToInt32(txtAmount.Text);

                // speler zijn pokedollars aanpassen
                PlayerInformation.IncreasePlayerPokedollar(player, addAmount);
            }
            else
            {
                foutmelding += "U kan zoveel niet verkopen" + Environment.NewLine;
            }

            if (playerItem.Amount <= 0)
            {
                DatabaseOperations.DeletePlayerItem(playerItem);
                lblItemDescription.Content = string.Empty;
            }
        }

        private void BuyItem(Item item)
        {

            PlayerItem playerItem = DatabaseOperations.GetPlayerItem(item.Id, PlayerInformation.PlayerId);

            Player player = DatabaseOperations.GetPlayer(PlayerInformation.PlayerId);

            // label van description terug instellen op enkel item description zonder foutmelding
            lblItemDescription.Content = item.Description + Environment.NewLine;

            // bereken hoeveel pokedollars er afgetrokken moeten worden
            int substractAmount = (int)item.BuyPrice * Convert.ToInt32(txtAmount.Text);

            if (playerItem != null && player.Pokedollar >= substractAmount)
            {
                // speler item hoeveelheid aanpassen
                playerItem.Amount += short.Parse(txtAmount.Text);
                DatabaseOperations.UpdatePlayerItem(playerItem);

                // speler zijn pokedollars aanpassen
                PlayerInformation.ReducePlayerPokedollar(player, substractAmount);
            }
            else if (playerItem == null && item != null && player.Pokedollar >= substractAmount)
            {
                // nieuwe playerItem aanmaken
                // Nodige validatie is al eerder gebeurd
                PlayerItem playerItemToAdd = new PlayerItem
                {
                    ItemId = item.Id,
                    Amount = short.Parse(txtAmount.Text),
                    PlayerId = PlayerInformation.PlayerId,
                };
                // playerItem inserten in de database
                DatabaseOperations.AddPlayerItem(playerItemToAdd);

                // speler zijn pokedollars aanpassen
                PlayerInformation.ReducePlayerPokedollar(player, substractAmount);
            }
            else
            {
                foutmelding += "Niet voldoende geld" + Environment.NewLine;
            }
        }

        // Methode om alle nodige validaties te doen.
        private string Valideer(string columnName)
        {
            if (columnName == "datagridPokemarktBuy" && datagridPokemarktBuy.SelectedItem == null)
            {
                return "Selecteer een Item om te kopen!" + Environment.NewLine;
            }
            if (columnName == "datagridPokemarktSell" && datagridPokemarktSell.SelectedItem == null)
            {
                return "Selecteer een Item om te verkopen!" + Environment.NewLine;
            }
            if (columnName == "txtAmount" && !int.TryParse(txtAmount.Text, out int test))
            {
                return "Geef een numeriek hoeveelheid in!" + Environment.NewLine;
            }
            if (columnName == "txtAmount" && int.TryParse(txtAmount.Text, out int amount))
            {
                if (amount <= 0)
                {
                    return "Hoeveelheid moet meer zijn dan 0" + Environment.NewLine;
                }
            }
            return "";
        }
    }
}
