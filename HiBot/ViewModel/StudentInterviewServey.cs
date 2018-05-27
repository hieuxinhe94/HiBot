using System;
 
using Microsoft.Bot.Builder.FormFlow;

namespace HiBot.ViewModel
{
    [Serializable]
    public class StudentInterviewServey
    {
        [Prompt("Bạn dự định thi khối nào: A/B/C/D ?")]
        public EKhoi Khoi { get; set; }

        [Prompt("Mức điểm có thể đạt được của bạn là bao nhiêu ? Vd: 10-15, 15-20,20-25, trên 25 ")]
        public EInfomationQA Diem { get; set; }
    }

    public enum EKhoi
    {
        [Terms("A", "a", "1")]
        A = 1,
        [Terms("B", "b", "2")]
        B = 2,
        [Terms("C", "c", "3")]
        C = 3,
        [Terms("D", "d", "4")]
        D = 4,

    }
    public enum EInfomationQA
    {
        [Terms("10-15", "10 đến 15", "từ 10 đến 15", "from 10 đến 15")]
        ThangDiemCoTheDat10_15 = 3,
        [Terms("15-20", "15 đến 20", "từ 15 đến 20", "from 15 đến 20")]
        ThangDiemCoTheDat15_20 = 4,
        [Terms("20-25", "20 đến 25", "từ 20 đến 25", "from 20 đến 25")]
        ThangDiemCoTheDat20_25 = 5,
        [Terms("trên 25", ">25", "từ 25 đến 30", "from 25", "29", "30")]
        ThangDiemCoTheDat20_30 = 6,
    }
}