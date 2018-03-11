using System;
using System.Threading.Tasks;
using HiBot.Business.Interfaces;
using HiBot.Entities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        [NonSerialized]
        public readonly IStudentBusiness _studentBusiness;

        public RootDialog(IStudentBusiness studentBusiness)
        {
            // Dependency injection
            _studentBusiness = studentBusiness;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // why _studentBusiness is null when message call back
            _studentBusiness.Add(new Students(){ Id = 1,Name = "Student 01 ",Birthday = DateTime.Now + "", HighSchool = "",LastOnline = DateTime.Now, PhoneNumber = "123", Sex = true});
            
            // TODO : Implement some business rule to reply or forward to new dialog.

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {activity.Text.Length} characters");

            
        }
    }
}