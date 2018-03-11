using System;
using System.Threading.Tasks;
using HiBot.Business.Infrastructures;
using HiBot.Business.Interfaces;
using HiBot.Entities;
using HiBot.Midware;
using HiBot.Repository;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        [NonSerialized]
        private IStudentBusiness _studentBusiness = new StudentBusiness(null);

        public RootDialog(IStudentBusiness studentBusiness)
        {
            // TODO : ISSUE : Cannot pass the dependency injection to here!!
            _studentBusiness = studentBusiness ?? new StudentBusiness(null);
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // add students to this session 
            _studentBusiness.Add(new Students(){ Id = 1,Name = "Student 01 ",Birthday = DateTime.Now + "", HighSchool = "",LastOnline = DateTime.Now, PhoneNumber = "123", Sex = true});
            
            // TODO : Implement some business rule to reply or forward to new dialog.

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {activity.Text.Length} characters");

            context.Wait(MessageReceivedAsync);
        }
    }
}