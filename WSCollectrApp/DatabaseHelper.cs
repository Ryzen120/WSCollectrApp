using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace TradingCardManager
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string dbFilePath)
        {
            connectionString = $"Data Source={dbFilePath};Version=3;";
        }

        public bool CheckConnection()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Card> SearchCards(string keyword, string field = "Name")
        {
            List<Card> results = new List<Card>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Cards WHERE {field} LIKE @Keyword";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Card card = ReadCardFromReader(reader);
                                results.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching cards: {ex.Message}");
            }

            return results;
        }

        public Card GetCardById(string cardId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Cards WHERE CardId = @CardId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CardId", cardId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return ReadCardFromReader(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting card by ID: {ex.Message}");
            }

            return null;
        }

        // Method to load card image on demand
        public byte[] GetCardImageById(string cardId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ImageData FROM Cards WHERE CardId = @CardId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CardId", cardId);

                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return (byte[])result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting card image by ID: {ex.Message}");
            }

            return null;
        }

        // Helper method to read a card from a data reader
        private Card ReadCardFromReader(SQLiteDataReader reader)
        {
            return new Card
            {
                CardId = reader["CardId"].ToString(),
                Name = reader["Name"].ToString(),
                ImageData = reader["ImageData"] as byte[],
                Expansion = reader["Expansion"].ToString(),
                Rarity = reader["Rarity"].ToString(),
                Color = reader["Color"].ToString(),
                CardType = reader["CardType"].ToString(),
                Level = reader["Level"].ToString(),
                Power = reader["Power"].ToString(),
                Soul = reader["Soul"].ToString(),
                Cost = reader["Cost"].ToString(),
                Trigger = reader["Trigger"].ToString(),
                Traits = reader["Traits"].ToString(),
                Effect = reader["Effect"].ToString(),
                Flavor = reader["Flavor"].ToString(),
                Illustrator = reader["Illustrator"].ToString(),
                Side = reader["Side"].ToString()
            };
        }

        public List<string> GetAllExpansions()
        {
            return GetDistinctValues("Expansion");
        }

        public List<string> GetAllColors()
        {
            return GetDistinctValues("Color");
        }

        public List<string> GetAllRarities()
        {
            return GetDistinctValues("Rarity");
        }

        public List<string> GetAllCardTypes()
        {
            return GetDistinctValues("CardType");
        }

        public List<string> GetAllLevels()
        {
            return GetDistinctValues("Level");
        }

        public List<string> GetAllCosts()
        {
            return GetDistinctValues("Cost");
        }

        public List<string> GetAllTriggers()
        {
            return GetDistinctValues("Trigger");
        }

        public List<string> GetAllSides()
        {
            return GetDistinctValues("Side");
        }

        private List<string> GetDistinctValues(string columnName)
        {
            List<string> values = new List<string>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT DISTINCT {columnName} FROM Cards WHERE {columnName} IS NOT NULL AND {columnName} <> '' ORDER BY {columnName}";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string value = reader[columnName].ToString().Trim();
                                if (!string.IsNullOrEmpty(value) && !values.Contains(value))
                                {
                                    values.Add(value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting {columnName} values: {ex.Message}");
            }

            return values;
        }

        // Optimized method for large result sets
        public int CountCardsByFilter(
            string expansion = null,
            string color = null,
            string rarity = null,
            string cardType = null,
            string level = null,
            string cost = null,
            string trigger = null,
            string side = null)
        {
            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(expansion))
                conditions.Add("Expansion = @Expansion");
            if (!string.IsNullOrEmpty(color))
                conditions.Add("Color = @Color");
            if (!string.IsNullOrEmpty(rarity))
                conditions.Add("Rarity = @Rarity");
            if (!string.IsNullOrEmpty(cardType))
                conditions.Add("CardType = @CardType");
            if (!string.IsNullOrEmpty(level))
                conditions.Add("Level = @Level");
            if (!string.IsNullOrEmpty(cost))
                conditions.Add("Cost = @Cost");
            if (!string.IsNullOrEmpty(trigger))
                conditions.Add("Trigger = @Trigger");
            if (!string.IsNullOrEmpty(side))
                conditions.Add("Side = @Side");

            string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM Cards {whereClause}";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(expansion))
                            command.Parameters.AddWithValue("@Expansion", expansion);
                        if (!string.IsNullOrEmpty(color))
                            command.Parameters.AddWithValue("@Color", color);
                        if (!string.IsNullOrEmpty(rarity))
                            command.Parameters.AddWithValue("@Rarity", rarity);
                        if (!string.IsNullOrEmpty(cardType))
                            command.Parameters.AddWithValue("@CardType", cardType);
                        if (!string.IsNullOrEmpty(level))
                            command.Parameters.AddWithValue("@Level", level);
                        if (!string.IsNullOrEmpty(cost))
                            command.Parameters.AddWithValue("@Cost", cost);
                        if (!string.IsNullOrEmpty(trigger))
                            command.Parameters.AddWithValue("@Trigger", trigger);
                        if (!string.IsNullOrEmpty(side))
                            command.Parameters.AddWithValue("@Side", side);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting cards: {ex.Message}");
                return 0;
            }
        }

        // Method to get cards with pagination
        public List<Card> GetCardsByFilterPaged(
            int pageNumber,
            int pageSize,
            string expansion = null,
            string color = null,
            string rarity = null,
            string cardType = null,
            string level = null,
            string cost = null,
            string trigger = null,
            string side = null,
            bool loadImages = true)
        {
            List<Card> results = new List<Card>();
            List<string> conditions = new List<string>();

            // Calculate LIMIT and OFFSET
            int offset = pageNumber * pageSize;

            if (!string.IsNullOrEmpty(expansion))
                conditions.Add("Expansion = @Expansion");
            if (!string.IsNullOrEmpty(color))
                conditions.Add("Color = @Color");
            if (!string.IsNullOrEmpty(rarity))
                conditions.Add("Rarity = @Rarity");
            if (!string.IsNullOrEmpty(cardType))
                conditions.Add("CardType = @CardType");
            if (!string.IsNullOrEmpty(level))
                conditions.Add("Level = @Level");
            if (!string.IsNullOrEmpty(cost))
                conditions.Add("Cost = @Cost");
            if (!string.IsNullOrEmpty(trigger))
                conditions.Add("Trigger = @Trigger");
            if (!string.IsNullOrEmpty(side))
                conditions.Add("Side = @Side");

            string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Select appropriate columns - exclude image data for large queries if not needed
                    string selectClause = loadImages
                        ? "SELECT * FROM Cards"
                        : "SELECT CardId, Name, Expansion, Rarity, Color, CardType, Level, Power, Soul, Cost, Trigger, Traits, Effect, Flavor, Illustrator, Side FROM Cards";

                    string query = $"{selectClause} {whereClause} ORDER BY Name LIMIT {pageSize} OFFSET {offset}";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.CommandTimeout = 120; // 2 minute timeout

                        if (!string.IsNullOrEmpty(expansion))
                            command.Parameters.AddWithValue("@Expansion", expansion);
                        if (!string.IsNullOrEmpty(color))
                            command.Parameters.AddWithValue("@Color", color);
                        if (!string.IsNullOrEmpty(rarity))
                            command.Parameters.AddWithValue("@Rarity", rarity);
                        if (!string.IsNullOrEmpty(cardType))
                            command.Parameters.AddWithValue("@CardType", cardType);
                        if (!string.IsNullOrEmpty(level))
                            command.Parameters.AddWithValue("@Level", level);
                        if (!string.IsNullOrEmpty(cost))
                            command.Parameters.AddWithValue("@Cost", cost);
                        if (!string.IsNullOrEmpty(trigger))
                            command.Parameters.AddWithValue("@Trigger", trigger);
                        if (!string.IsNullOrEmpty(side))
                            command.Parameters.AddWithValue("@Side", side);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!loadImages)
                                {
                                    // Create card without image data
                                    Card card = new Card
                                    {
                                        CardId = reader["CardId"].ToString(),
                                        Name = reader["Name"].ToString(),
                                        ImageData = null, // Skip image data
                                        Expansion = reader["Expansion"].ToString(),
                                        Rarity = reader["Rarity"].ToString(),
                                        Color = reader["Color"].ToString(),
                                        CardType = reader["CardType"].ToString(),
                                        Level = reader["Level"].ToString(),
                                        Power = reader["Power"].ToString(),
                                        Soul = reader["Soul"].ToString(),
                                        Cost = reader["Cost"].ToString(),
                                        Trigger = reader["Trigger"].ToString(),
                                        Traits = reader["Traits"].ToString(),
                                        Effect = reader["Effect"].ToString(),
                                        Flavor = reader["Flavor"].ToString(),
                                        Illustrator = reader["Illustrator"].ToString(),
                                        Side = reader["Side"].ToString()
                                    };
                                    results.Add(card);
                                }
                                else
                                {
                                    // Load full card with image data
                                    Card card = ReadCardFromReader(reader);
                                    results.Add(card);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering cards: {ex.Message}");
                throw; // Rethrow for handling in UI
            }

            return results;
        }
    }
}