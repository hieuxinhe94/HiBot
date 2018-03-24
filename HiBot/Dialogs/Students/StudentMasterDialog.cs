using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.Students
{
    [Serializable]
    public class StudentMasterDialog : IDialog<object>
    {
        private Entities.Students CurrentStudent;

        public Task StartAsync(IDialogContext context)
        {
            CurrentStudent = new Entities.Students();
            context.Wait(this.HandlerIntroduce);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Wait(MessageReceivedAsync);
        }
        private async Task HandlerIntroduce(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var activity = await result as Activity;
            
            await context.Forward(new StudentServeyDialog(), this.ResumeAfterIntroduceDialogComeBack, activity, CancellationToken.None);
           
        }

        private Task ResumeAfterIntroduceDialogComeBack(IDialogContext context, IAwaitable<object> result)
        {
            return Task.CompletedTask;
        }
    }
}