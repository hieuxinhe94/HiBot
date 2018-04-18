using System;
using HiBot.App_Start;
using Microsoft.Bot.Builder.FormFlow;

namespace HiBot.ViewModel
{
    [Serializable]
    public class StudentServey
    {
        [Prompt("Cho tôi biết tên đầy đủ của bạn")]
        public string StudentName;

        [Prompt(" Bạn bao nhiêu tuổi ?, {StudentName}")]
        public int Birthday;

        [Prompt("Nam / Nữ? ")]
        public SEX Sex;

        [Prompt("Cho tôi biết số điện thoại của bạn để có thể thông báo khi có thông tin mới,  {StudentName} ?")]
        //[Pattern(RegexConstants.Phone)]
        public string PhoneNumber;

      
        public static IForm<StudentServey> BuildForm()
        {
            return new FormBuilder<StudentServey>()
                .Message("Welcome to the HiBot ")
                .Build();
        }


    }



    public enum SEX
    {
        [Terms("Nữ", "Girl","1","G")]
        Girl = 1,
        [Terms("Nam", "Trai","2","T")]
        Boy = 2
    }
}