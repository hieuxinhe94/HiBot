using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.CollegeStudent
{
    [Serializable]
    public class CollegeMasterDialog :  IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
         await   context.PostAsync("Hey VinhUniversity Student.");
            context.Wait(MessageReceivedAsync);
        }

    }
}