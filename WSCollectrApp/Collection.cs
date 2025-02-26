using System;
using System.Collections.Generic;

namespace TradingCardManager
{
    public class Collection
    {
        public string Name { get; set; }
        public DateTime LastModified { get; set; }
        public List<CollectionCard> Cards { get; set; }

        public Collection()
        {
            Name = "My Collection";
            LastModified = DateTime.Now;
            Cards = new List<CollectionCard>();
        }
    }

    public class CollectionCard
    {
        public string CardId { get; set; }
        public int Quantity { get; set; }
    }
}