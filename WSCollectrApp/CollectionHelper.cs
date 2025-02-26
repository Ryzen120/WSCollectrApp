using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TradingCardManager
{
    public class CollectionHelper
    {
        private DatabaseHelper dbHelper;
        private DateTime lastLoadTime = DateTime.MinValue;
        private string lastLoadedFile = string.Empty;

        public CollectionHelper(DatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public bool SaveCollection(Collection collection, string filePath)
        {
            try
            {
                // Update the last modified timestamp
                collection.LastModified = DateTime.Now;

                string jsonString = JsonSerializer.Serialize(collection, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonString);

                // Update our tracking variables
                lastLoadTime = collection.LastModified;
                lastLoadedFile = filePath;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving collection: {ex.Message}");
                return false;
            }
        }

        public Collection LoadCollection(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // Check if we need to reload the file
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.LastWriteTime > lastLoadTime || filePath != lastLoadedFile)
                    {
                        string jsonString = File.ReadAllText(filePath);
                        Collection loadedCollection = JsonSerializer.Deserialize<Collection>(jsonString);

                        // Update tracking variables
                        lastLoadTime = fileInfo.LastWriteTime;
                        lastLoadedFile = filePath;

                        return loadedCollection;
                    }
                    else
                    {
                        // File hasn't changed, can safely return the current collection
                        return new Collection(); // This will actually be replaced by the caller's current collection
                    }
                }
                return new Collection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading collection: {ex.Message}");
                return new Collection();
            }
        }

        public List<Card> GetCardsInCollection(Collection collection)
        {
            List<Card> cards = new List<Card>();

            foreach (var collectionCard in collection.Cards)
            {
                Card card = dbHelper.GetCardById(collectionCard.CardId);
                if (card != null)
                {
                    card.QuantityInCollection = collectionCard.Quantity;
                    cards.Add(card);
                }
            }

            // Sort cards by name for easier browsing
            return cards.OrderBy(c => c.Name).ToList();
        }

        public bool AddCardToCollection(Collection collection, string cardId, int quantity = 1)
        {
            try
            {
                var existingCard = collection.Cards.Find(c => c.CardId.Equals(cardId));

                if (existingCard != null)
                {
                    existingCard.Quantity += quantity;
                }
                else
                {
                    collection.Cards.Add(new CollectionCard
                    {
                        CardId = cardId,
                        Quantity = quantity
                    });
                }

                collection.LastModified = DateTime.Now;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding card to collection: {ex.Message}");
                return false;
            }
        }

        public bool RemoveCardFromCollection(Collection collection, string cardId, int quantity = 1)
        {
            try
            {
                var existingCard = collection.Cards.Find(c => c.CardId.Equals(cardId));

                if (existingCard != null)
                {
                    existingCard.Quantity -= quantity;

                    if (existingCard.Quantity <= 0)
                    {
                        collection.Cards.Remove(existingCard);
                    }

                    collection.LastModified = DateTime.Now;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing card from collection: {ex.Message}");
                return false;
            }
        }
    }
}