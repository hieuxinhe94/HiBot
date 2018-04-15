using System;

namespace HiBot.Entities
{
    public class HistoryMissUnderstand  
    {
        public int Id { get; set; }
        public string Sentences { get; set; }
        public string Intent { get; set; }

        public string Reply { get; set; }
        public DateTime  TimeZone { get; set; }
    }
}
