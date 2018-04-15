
using System;

namespace HiBot.Entities
{
    [Serializable]
    public  class Students : BaseEntities
    {
        public string HighSchool { get; set; }
        public EKhoi Khoi { get; set; }
        public EInfomationQA DiemExpect { get; set; }

    }
}
