using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HiBot.Constants;
using HiBot.Dialogs.CollegeStudent;
using HiBot.Dialogs.Common;
using HiBot.Dialogs.Students;
using HiBot.Dialogs.Teacher;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs
{
    [Serializable]
    public class IntroduceDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.ShowOptionsAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // event ivoke callback when user entered somethings
            var message = await result;
            switch (message.Text)
            {
                case HiBotOptions.TeachertOptions.ToiLaGiangVien:
                    {
                        await context.Forward(new TeacherMasterDialog(), this.ResumeAfterOptionDialog, message, CancellationToken.None);
                        break;
                    }
                case HiBotOptions.CollegeStudentOptions.ToiLaSinhVien:
                    {

                        await context.Forward(new CollegeMasterDialog(), this.ResumeAfterOptionDialog, message, CancellationToken.None);

                        break;
                    }
                case HiBotOptions.StudentOptions.ToiLaHocSinh:
                    {

                        await context.Forward(new StudentMasterDialog(), this.ResumeAfterOptionDialog, message, CancellationToken.None);

                        break;
                    }
                default:
                    {
                        await context.PostAsync("Choose valid options by click a button.");
                        context.Wait(ShowOptionsAsync);
                        return;
                    }
            }

        }

        private async Task ShowOptionsAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // first time
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            List<Attachment> lstOptionsAttachments = new List<Attachment>()
            {

                GetThumbnailCard(
                    HiBotOptions.StudentOptions.ToiLaHocSinh,
                    HiBotOptions.StudentOptions.ToiLaHocSinhSubtitle,
                    "",
                    new CardImage(
                        url:
                        $"data:image/png; base64,{ Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Resources/Images/highschoolStudent.png")))}"),
                    new CardAction(ActionTypes.ImBack, "NEXT",
                        value: HiBotOptions.StudentOptions.ToiLaHocSinh)),
                GetThumbnailCard(
                    HiBotOptions.CollegeStudentOptions.ToiLaSinhVien,
                    HiBotOptions.CollegeStudentOptions.ToiLaSinhVienSubtitle,
                    "",
                    new CardImage(
                        url:
                        $"data:image/png; base64,{ Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Resources/Images/student.png")))}"),
                    new CardAction(ActionTypes.ImBack, "NEXT",
                        value: HiBotOptions.CollegeStudentOptions.ToiLaSinhVien)),
                GetThumbnailCard(
                    HiBotOptions.TeachertOptions.ToiLaGiangVien,
                    HiBotOptions.TeachertOptions.ToiLaGiangVienSubtitle,
                    "",
                    new CardImage(
                        url:
                        $"data:image/png; base64,{ Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Resources/Images/teacher.jpg")))}"),
                    new CardAction(ActionTypes.ImBack, "NEXT",
                        value: HiBotOptions.TeachertOptions.ToiLaGiangVien)),
            };
            reply.Attachments = lstOptionsAttachments;
            await context.PostAsync(reply);
            // swich to MessageReceivedAsync
            context.Wait(this.MessageReceivedAsync);
        }

        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }


        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<object> result)
        {
            var ticketNumber = await result;

            await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}