using System;
using System.Threading.Tasks;
using HiBot.Business.Infrastructures;
using HiBot.Entities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private StudentBusiness _studentBusiness;
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // add students to this session 
            _studentBusiness.Add(new Students(){Name = "Student 01 "});

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }
    }
}