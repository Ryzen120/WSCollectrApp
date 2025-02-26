using System;

namespace TradingCardManager
{
    public class Card
    {
        public string CardId { get; set; }
        public string Name { get; set; }
        public byte[] ImageData { get; set; }
        public string Expansion { get; set; }
        public string Rarity { get; set; }
        public string Color { get; set; }
        public string CardType { get; set; }
        public string Level { get; set; }
        public string Power { get; set; }
        public string Soul { get; set; }
        public string Cost { get; set; }
        public string Trigger { get; set; }
        public string Traits { get; set; }
        public string Effect { get; set; }
        public string Flavor { get; set; }
        public string Illustrator { get; set; }
        public string Side { get; set; }
        public int QuantityInCollection { get; set; }
    }
}