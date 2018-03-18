using System;
using HiBot.App_Start;
using Microsoft.Bot.Builder.FormFlow;

namespace HiBot.ViewModel
{
    [Serializable]
    public class StudentServey
    {

     
        [Prompt("Hello... What's your name?")]
        public string StudentName;

        [Prompt(" How old are you ?, {Name}")]
        public int Birthday;

        [Prompt("Which sex you are ? Boy or girl? ")]
        public SEX Sex;

        [Prompt("Can I have your phone number,  {Name} ?")]
        [Pattern(RegexConstants.Phone)]
        public string PhoneNumber;


        //public static IForm<StudentServey> BuildOrderForm()
        //{
        //    return new FormBuilder<StudentServey>()
        //        .Field(nameof(Name))
        //        .Build();

        //}
    }



    public enum SEX
    {
        [Terms("Wommen", "Girl")]
        Girl = 1,
        [Terms("Man", "Boy")]
        Boy = 2
    }
}