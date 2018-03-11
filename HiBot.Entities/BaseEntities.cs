
using System;

namespace HiBot.Entities
{
    [Serializable]
    public class BaseEntities
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Birthday { get; set; }

        public bool Sex { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastOnline { get; set; }
    }
}
