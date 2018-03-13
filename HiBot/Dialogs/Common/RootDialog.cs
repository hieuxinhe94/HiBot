using System;
using System.Threading;
using System.Threading.Tasks;
using HiBot.Business.Infrastructures;
using HiBot.Business.Interfaces;
using HiBot.Dialogs.Common;
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
        // Dependency Injecttion fail by : 
        // In the second call back, 2th message -> Bot Call this class first then excute Controller class 
        // This Business class alway null, because this thread not call to constructor

        [NonSerialized]
        private readonly IStudentBusiness _studentBusiness;

        public RootDialog(IStudentBusiness studentBusiness)
        {
            // Dependency injection
            _studentBusiness = studentBusiness;
        }

        public Task StartAsync(IDialogContext context)
        {
            // Root dialog initiates and waits for the next message from the user. 
            // When a message arrives, call MessageReceivedAsync.
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // We've got a message!
            var activity = await result as Activity;
            if (activity.Text.ToLower().Contains("hi")|| activity.Text.ToLower().Contains("hello")|| activity.Text.ToLower().Contains("2"))
            {
                await context.Forward(new IntroduceDialog(), this.ResumeAfterIntroduceDialogComeBack, activity, CancellationToken.None);
            }
            else if (activity.Text.ToLower().Contains("learn english"))
            {
                await context.Forward(new LearningEnglishDialog(), this.ResumeAfterIntroduceDialogComeBack, activity, CancellationToken.None);
            }
            else
            {
                await context.PostAsync($"Say Hi, Please !! ");
               
            }
            //use context.Wait() to specify the callback to invoke the next time the user sends a message
            // not call any nonserializable class because, it will be null
           
        }

        private async Task ResumeAfterIntroduceDialogComeBack(IDialogContext context, IAwaitable<object> result)
        {
            // Store the value that Introduce Dialog returned. 
            // (At this point, Introduce Dialog  has finished and returned some value to use within the root dialog.)
            var resultFromIntroduceDialog = await result;

            await context.PostAsync($"Introduce Dialog said: {resultFromIntroduceDialog}");

            // Again, wait for the next message from the user.
            context.Wait(this.MessageReceivedAsync);
        }
    }

    
}