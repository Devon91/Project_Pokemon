using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Project_Pokemon_Models;

namespace Project_Pokemon_DAL
{
    public static class DatabaseOperations
    {
        // speler ophalen aan de hand van de Id
        public static Player GetPlayer(int playerid)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Players
                    .Where(x => x.Id == playerid)
                    .SingleOrDefault();
            }
        }

        // Ophalen van specifiek item.
        public static Item GetItem(int itemId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Items.Where(x => x.Id == itemId)
                    .SingleOrDefault();
            }
        }

        // Ophalen specifiek player item
        public static PlayerItem GetPlayerItem(int itemId, int playerId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PlayerItems
                    .Include(y => y.Item)
                    .Where(x => x.ItemId == itemId)
                    .Where(x => x.PlayerId == playerId)
                    .SingleOrDefault();
            }
        }

        // Opalen van de pokedex informatien van een specifieke pokemon.
        public static Pokedex GetPokedexEntry(int pokemonId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Pokedexes.
                    Include(x => x.PlayerPokedexes).
                    Where(x => x.PokedexEntry == pokemonId)
                    .SingleOrDefault();
            }
        }

        //Ophalen gekozen pokemon voor battle
        public static PlayerPokemon GetPlayerPokemon(int playerPokemonId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PlayerPokemons
                    .Include(z => z.Pokemon.OwnedPokemonAttacks)
                    .Include(x => x.Pokemon.Pokedex.Type)
                    .Where(z => z.Id == playerPokemonId).SingleOrDefault();
            }
        }

        // Ophalen van 1 specifieke aanval
        public static Attack GetAttack(int attackId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Attacks
                    .Include(x => x.Type)
                    .Where(x => x.Id == attackId).SingleOrDefault();
            }
        }

        // Ophalen van de gym leader zijn/haar badge.
        public static NpcItem GetGymLeaderBadge(int gymleaderId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.NpcItems
                    .Include(x => x.Item)
                    .Where(x => x.Item.Type.ToLower() == "badge")
                    .Where(x => x.NpcId == gymleaderId)
                    .SingleOrDefault();
            }
        }

        // Ophalen van de speler zijn items uit de database.
        public static List<PlayerItem> GetPlayerItems(int playerid, bool sell)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                if (sell == false)
                {
                    // alle Items van een speler tonen behalve zijn Badges.
                    return projectPokemonEntities.PlayerItems.Include(x => x.Item)
                        .Where(y => y.PlayerId == playerid)
                        .Where(z => z.Item.Type != "Badge")
                        .ToList();
                }
                else
                {
                    // alle items van een speler tonen die verkocht kunnen worden.
                    return projectPokemonEntities.PlayerItems.Include(x => x.Item)
                        .Where(y => y.PlayerId == playerid)
                        .Where(z => z.Item.SellPrice != null)
                        .ToList();
                }

            }
        }

        // Ophalen van de speler zijn badges uit de database.
        public static List<PlayerItem> GetPlayerBadges(int playerid)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PlayerItems.Include(x => x.Item)
                    .Where(y => y.PlayerId == playerid)
                    .Where(z => z.Item.Type == "Badge")
                    .OrderBy(x => x.Id)
                    .ToList();
            }
        }

        // Ophalen van alle spelers
        public static List<Player> GetPlayers()
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Players.ToList();
            }
        }

        //ophalen areas
        public static List<Area> GetAreas()
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Areas
                    .OrderBy(x => x.Id)
                    .ToList();
            }
        }

        // Ophalen van alle items die verkocht worden.
        public static List<Item> GetItems()
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Items.Where(x => x.BuyPrice != null)
                    .ToList();
            }
        }

        // Ophalen van de speler zijn party pokemons.
        public static List<PlayerPokemon> GetPlayerPokemons(int playerid)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PlayerPokemons.Where(y => y.PlayerId == playerid)
                    .Where(x => x.InParty != false)
                    .Include(x => x.Pokemon.Pokedex)
                    .ToList();
            }
        }

        // Ophalen van alle pokemons die gevangen zijn en niet in de party staan
        public static List<PlayerPokemon> GetCatchedPlayerPokemons(int playerid)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PlayerPokemons.Where(y => y.PlayerId == playerid)
                    .Where(x => x.InParty == false)
                    .Include(x => x.Pokemon)
                    .Include(x => x.Pokemon.Pokedex)
                    .ToList();
            }
        }

        //Ophalen van alle trainers in een bepaalde area
        public static List<Npc> GetTrainers(int areaId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Npcs
                    .Where(x => x.Type.ToLower() == "trainer" || x.Type.ToLower() == "gym leader")
                    .Where(x => x.AreaId == areaId)
                    .ToList();
            }
        }

        // Ophalen van de speler zijn pokemons.
        public static List<PlayerPokemon> GetPokemons(int playerPokemonId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PlayerPokemons
                    .Include(x => x.Pokemon)
                    .Include(z => z.Pokemon.Pokedex.Type)
                    .Where(z => z.Id == playerPokemonId).ToList();
            }
        }

        // Ophalen welke aanvallen de pokemon kent.
        public static List<OwnedPokemonAttack> GetKnownAttacks(int pokemonId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.OwnedPokemonAttacks.Include(z => z.Attack)
                    .Include(x => x.Attack.Type)
                    .Where(y => y.PokemonId == pokemonId)
                    .Where(z => z.KnownAttack == true)
                    .ToList();
            }
        }

        //Ophalen van alle aanvallen die een specifieke pokemon kan gebruiken maar momenteel niet gebruikt wordt.
        public static List<OwnedPokemonAttack> GetKnownAttacksEqualsFalse(int pokemonId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.OwnedPokemonAttacks.Include(z => z.Attack)
                    .Include(x => x.Attack.Type)
                    .Where(y => y.PokemonId == pokemonId)
                    .Where(z => z.KnownAttack == false)
                    .ToList();
            }
        }

        //ophalen pokemon, stats en aanvallen
        public static List<TrainerPokemon> GetTrainerPokemonInformation(int trainerId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.TrainerPokemons
                    .Include(x => x.Pokemon.Pokedex.Type)
                    .Include(x => x.Pokemon.OwnedPokemonAttacks.Select(y => y.Attack.Type))
                    .Where(x => x.NpcId == trainerId)
                    .ToList();
            }
        }


        // Alle aanvalleen die de pokemon kan leren ophalen.
        public static List<PokemonAttack> GetAvailableAttacks(int pokemonId, int currentLevel)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PokemonAttacks.Include(z => z.Attack)
                    .Include(x => x.Attack.Type)
                    .Where(y => y.RequiredLevel <= currentLevel)
                    .Where(z => z.PokedexId == pokemonId)
                    .ToList();
            }
        }

        // Ophalen van alle pokemons die in de algemene pokedex aanwezig zijn
        public static List<Pokedex> GetPokedexEntries()
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Pokedexes.Include(x => x.PlayerPokedexes)
                    .ToList();
            }
        }

        // Ophalen van de speler zijn specifieke pokedex.
        public static List<PlayerPokedex> GetPlayerPokedexEntries(int playerId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.PlayerPokedexes.Include(x => x.Pokedex).Where(x => x.PlayerId == playerId)
                    .ToList();
            }
        }

        // Ophalen van de Area id's waar een bepaalde pokemon kan voorkomen.
        public static List<Found> GetPlayerPokedexEntriesAreas(int pokedexId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Founds
                    .Include(x => x.Pokedex)
                    .Include(x => x.Area)
                    .Where(x => x.PokedexId == pokedexId)
                    .ToList();
            }
        }

        // Ophalen van alle pokemons die in een bepaald gebied gevonden kunnen worden.
        public static List<Found> GetFoundPokemonViaAreaId(int areaId)
        {
            using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
            {
                return projectPokemonEntities.Founds
                    .Include(x => x.Pokedex.Type)
                    .Where(x => x.AreaId == areaId)
                    .ToList();
            }
        }

        // speler attributen updaten.
        public static int UpdatePlayer(Player player)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(player).State = EntityState.Modified;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        // player items updaten
        public static int UpdatePlayerItem(PlayerItem playerItem)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(playerItem).State = EntityState.Modified;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        public static int UpdatePlayerPartyPokemons(PlayerPokemon playerPokemon)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(playerPokemon).State = EntityState.Modified;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        // Updaten van specifieke pokemon
        public static int UpdatePokemonCurrentHp(Pokemon pokemon)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(pokemon).State = EntityState.Modified;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        // Updaten welke aanvallen de pokemon momenteel kent.
        public static int UpdateKnownAttacks(OwnedPokemonAttack knownAttack)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(knownAttack).State = EntityState.Modified;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        // Updaten van specifieke playerpokemon
        public static int UpdatePlayerPokemon(PlayerPokemon pokemon)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(pokemon).State = EntityState.Modified;
                    projectPokemonEntities.Entry(pokemon.Pokemon).State = EntityState.Modified;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        // Updaten van playerpokedex
        public static int UpdatePlayerPokedex(PlayerPokedex playerPokedexEntry)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(playerPokedexEntry).State = EntityState.Modified;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }

        }

        // player items toevoegen
        public static int AddPlayerItem(PlayerItem playerItem)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.PlayerItems.Add(playerItem);
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }

        }

        // Pokemon object toevoegen
        public static int AddPokemon(Pokemon pokemon)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Pokemons.Add(pokemon);
                    projectPokemonEntities.SaveChanges();
                    int id = pokemon.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }

        }

        // Playerpokemon object toevoegen
        public static int AddPlayerPokemon(PlayerPokemon playerPokemon)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.PlayerPokemons.Add(playerPokemon);
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        // OwnedPokemonAttacks toevoegen
        public static int AddCaughtPokemonAttacks(OwnedPokemonAttack ownedPokemonAttack)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.OwnedPokemonAttacks.Add(ownedPokemonAttack);
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        //Playerpokedex entry toevoegen.
        public static int AddCaughtPlayerPokedex(PlayerPokedex entry)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.PlayerPokedexes.Add(entry);
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        //Speler toevoegen
        public static int AddPlayer(Player player)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Players.Add(player);
                    projectPokemonEntities.SaveChanges();

                    return player.Id;
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }


        //Nieuwe pokemon toevoegen aan de playerpokedex
        public static int AddNewPokemonPlayerPokedex(PlayerPokedex entry)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.PlayerPokedexes.Add(entry);
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        //Player items verwijderen 
        public static int DeletePlayerItem(PlayerItem playerItem)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(playerItem).State = EntityState.Deleted;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        public static int DeletePlayer(Player player)
        {
            try
            {
                using (Project_Pokemon_Entities projectPokemonEntities = new Project_Pokemon_Entities())
                {
                    projectPokemonEntities.Entry(player).State = EntityState.Deleted;
                    return projectPokemonEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

    }
}
