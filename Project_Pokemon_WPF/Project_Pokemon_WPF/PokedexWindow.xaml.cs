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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project_Pokemon_WPF
{
    /// <summary>
    /// Interaction logic for PokedexWindow.xaml
    /// </summary>
    public partial class PokedexWindow : Window
    {
        public PokedexWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<PokedexEntry> pokedexEntries = new List<PokedexEntry>();
            List<PlayerPokedex> playerPokedexList = DatabaseOperations.GetPlayerPokedexEntries(PlayerInformation.PlayerId);
            List<Pokedex> pokedexList = DatabaseOperations.GetPokedexEntries();

            // nieuwe pokedexEntry objecten aanmaken voor het aantal pokemon in de pokedex, startwaardes geven en toevoegen aan lijst
            foreach (var item in pokedexList)
            {
                PokedexEntry p = new PokedexEntry
                {
                    PokedexNumber = item.PokedexEntry,
                    Name = "???",
                    Caught = false,
                    Encountered = false
                };
                pokedexEntries.Add(p);
            }

            // Voor elk item in de pokedexEntries lijst controleren of de speler deze pokemon heeft in zijn pokedex.
            // name, caught en encountered gelijkstellen aan de juist waardens als speler deze pokemon in zijn pokedex heeft.
            for (int i = 0; i < pokedexEntries.Count; i++)
            {
                int index = pokedexEntries[i].PokedexNumber;

                foreach (var record in playerPokedexList)
                {
                    if (record.PokedexId == index)
                    {
                        pokedexEntries[i].Name = pokedexList[i].Name;
                        pokedexEntries[i].Caught = record.Caught;
                        pokedexEntries[i].Encountered = record.Encountered;
                    }
                }
            }
            datagridPokedex.ItemsSource = pokedexEntries;
        }

        private void DatagridPokedex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridPokedex.SelectedItem is PokedexEntry pokedexEntry)
            {
                Pokedex pokedexPokemon = DatabaseOperations.GetPokedexEntry(pokedexEntry.PokedexNumber);
                List<Found> pokemonFoundAreas = DatabaseOperations.GetPlayerPokedexEntriesAreas(pokedexPokemon.Id);

                string areaIds = string.Empty;

                // Alle unieke area id's van de geselecteerd pokemon ophalen.
                foreach (var item in pokemonFoundAreas)
                {
                    if (!areaIds.Contains(item.AreaId.ToString()))
                    {
                        areaIds += item.Area.Name + ", ";
                    }
                }

                // Laatste komma verwijderen
                if (areaIds != string.Empty)
                {
                    areaIds = areaIds.Substring(0, areaIds.Length - 2);
                }
                else
                {
                    areaIds = "unknown";
                }

                // Summary opvullen als speler de pokemon al gezien heeft.
                if (pokedexEntry.Encountered == true)
                {
                    lblSummary.Content = "Summary:" + Environment.NewLine + Environment.NewLine + "Area: " + areaIds + Environment.NewLine + pokedexPokemon.Description + Environment.NewLine + "Height: " + pokedexPokemon.AvgHeight + "m" + Environment.NewLine + "Weigth: " + pokedexPokemon.AvgWeight + "kg";
                    imgPokemon.Source = new BitmapImage(new Uri(@"images\sprites\" + pokedexPokemon.ImgReference, UriKind.Relative));
                }
                else
                {
                    lblSummary.Content = string.Empty;
                    imgPokemon.Source = null;
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
