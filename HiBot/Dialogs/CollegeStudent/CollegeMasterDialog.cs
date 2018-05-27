using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using HiBot.ViewModel;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.CollegeStudent
{
    [Serializable]
    public class CollegeMasterDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {

            context.Wait(this.HandlerIntroduce);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var activity = await result as Activity;
            await context.PostAsync($"Oh, Nhận 10 điểm và gửi lời chúc của tôi tới các bạn của bạn nhé!");
            context.Done<string>(null);
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
        private async Task HandlerIntroduce(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            
            await context.Forward(new CollegeStudentInterviewDialog(), this.HandlerInterview, activity, CancellationToken.None);
 
        }



        public async Task HandlerInterview(IDialogContext context, IAwaitable<object> argument)
        {
            var message = await argument;
            var studentName = String.Empty;
            var studentId = String.Empty;
            var current_college_student = new CollegeStudentViewModel();
            context.UserData.TryGetValue<CollegeStudentViewModel>("current_college_student", out current_college_student);

            var date = DateTime.Now.ToShortDateString();

            if (current_college_student.StudentName.Equals("135D4802010245"))
            {
                if (DateTime.Now.ToShortDateString().Equals("05/28/2018"))
                {
                    await context.PostAsync($"Oh, Hi {current_college_student.StudentName}, Hôm nay bạn có buổi bảo vệ đồ án tốt nghiệp. ");
                    await context.PostAsync($"Mọi chuyện sao rồi ?");
                }
            }
            else
            {
                await context.PostAsync($"xin chào {current_college_student.StudentName}, Tôi đang kiểm tra xem có thông tin gì mới cho bạn... ");
            }

            context.Wait(MessageReceivedAsync);
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
            context.Wait(MessageReceivedAsync);
        }

    }
}