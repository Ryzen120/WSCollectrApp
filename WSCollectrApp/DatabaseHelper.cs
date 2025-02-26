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
                                Card card = new Card
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

        public List<string> GetAllExpansions()
        {
            List<string> expansions = new List<string>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT Expansion FROM Cards ORDER BY Expansion";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                expansions.Add(reader["Expansion"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting expansions: {ex.Message}");
            }

            return expansions;
        }

        public List<string> GetAllColors()
        {
            List<string> colors = new List<string>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT Color FROM Cards ORDER BY Color";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                colors.Add(reader["Color"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting colors: {ex.Message}");
            }

            return colors;
        }

        public List<Card> GetCardsByFilter(string expansion = null, string color = null, string rarity = null, string cardType = null)
        {
            List<Card> results = new List<Card>();
            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(expansion))
                conditions.Add("Expansion = @Expansion");
            if (!string.IsNullOrEmpty(color))
                conditions.Add("Color = @Color");
            if (!string.IsNullOrEmpty(rarity))
                conditions.Add("Rarity = @Rarity");
            if (!string.IsNullOrEmpty(cardType))
                conditions.Add("CardType = @CardType");

            string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Cards {whereClause}";

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

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Card card = new Card
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
                                results.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering cards: {ex.Message}");
            }

            return results;
        }
    }
}