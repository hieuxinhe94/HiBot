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

        [Prompt(" How old are you ?, {StudentName}")]
        public int Birthday;

        [Prompt("Which sex you are ? Boy or girl? ")]
        public SEX Sex;

        [Prompt("Can I have your phone number,  {StudentName} ?")]
        [Pattern(RegexConstants.Phone)]
        public string PhoneNumber;


      
    }



    public enum SEX
    {
        [Terms("Wommen", "Girl")]
        Girl = 1,
        [Terms("Man", "Boy")]
        Boy = 2
    }
}