using HiBot.Constants;
using HiBot.Entities;
using Microsoft.Bot.Builder.FormFlow;

namespace HiBot.ViewModel
{
    public class StudentInterviewServey
    {
        [Prompt("Bạn dự định thi khối nào ?")]
        public EKhoi Khoi { get; set; }

        [Prompt("Mức điểm có thể đạt được của bạn là bao nhiêu ?")]
        public EInfomationQA Diem { get; set; }
    }

   
}