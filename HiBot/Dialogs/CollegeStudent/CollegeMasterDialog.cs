using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.CollegeStudent
{
    [Serializable]
    public class CollegeMasterDialog :  IDialog<object>
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
                Text = "Login to University system",
                Buttons = new List<CardAction> { new CardAction(ActionTypes.Signin, "Sign-in",
                    value: "http://student.vinhuni.edu.vn/CMCSoft.IU.Web.Info/Login.aspx?url=http://student.vinhuni.edu.vn/cmcsoft.iu.web.info/home.aspx"), }
            };

            return signinCard.ToAttachment();
        }


        public ResumeAfter<string> HandlerLoginAfterSignInAsync() => throw new Exception("Don't know resumeAfter ");
        

        private async Task HandlerLoginAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
             await context.PostAsync("Hey Student, send you a login card");
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = new List<Attachment>()
            {
                GetStudentSigninCard ()
            };

            await context.PostAsync(reply);
            PromptDialog.Text(context, HandlerLoginAfterSignInAsync(), "We are sorry, this function isn't completed now, Can you give me your Id ", "Input your student ID, please", 3);

            context.Wait(MessageReceivedAsync);
        }

    }
}