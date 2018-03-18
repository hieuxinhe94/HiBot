using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.Teacher
{
    [Serializable]
    public class TeacherMasterDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.HandlerLoginAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
         
            context.Wait(MessageReceivedAsync);
        }
        private static Attachment GetStudentSigninCard()
        {
            var signinCard = new SigninCard
            {
                Text = "BotFramework Sign-in Card",
                Buttons = new List<CardAction> { new CardAction(ActionTypes.Signin, "Sign-in", 
                    value: "https://student.vinhuni.edu.vn/") }
            };

            return signinCard.ToAttachment();
        }
        private async Task HandlerLoginAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = new List<Attachment>()
            {
                GetStudentSigninCard ()
            };

            await context.PostAsync(reply);
        }
        private async Task HandlerLoginAfterSignInAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
             
        }


    }
}