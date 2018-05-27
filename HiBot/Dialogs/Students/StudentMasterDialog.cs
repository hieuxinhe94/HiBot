using System;
using System.Threading;
using System.Threading.Tasks;
using HiBot.Constants;
using HiBot.ViewModel;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.Students
{
    [LuisModel("98d69908-d401-40f9-b207-1289d0c79087", "0cf6cddfce5f4440b4046e4d1b444a32")]
    [Serializable]
    public class StudentMasterDialog : LuisDialog<object>
    {
        protected override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            return base.MessageReceived(context, item);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            context.Wait(this.HandlerIntroduce);
        }

        [LuisIntent(HiBotIntents.HighSchoolStudent.quession_about_university_exam)]
        public async Task UniversityExam(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            // post link to student
            await context.PostAsync($" Thông tin bạn cần có tại địa chỉ : http://eng.vinhuni.edu.vn/abc.html");
            await context.PostAsync($" Tôi luôn sẵn sàng giải đáp các thắc mắc cho bạn 24/7.");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent(HiBotIntents.HighSchoolStudent.quession_about_university_major)]
        public async Task MajorOfUniversity(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
              await context.PostAsync($" Mời bạn cung cấp một số thông tin về khối thi và mức điểm ước lượng có thể đạt. Bạn sẵn sàng chứ ");
              context.Wait(this.HandlerInterview);
        }

        private async Task HandlerIntroduce(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var activity = await result as Activity;
            await context.Forward(new StudentServeyDialog(), this.ResumeAfterIntroduceDialogComeBackAsync, activity, CancellationToken.None);
        }
        private async Task HandlerInterview(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            await context.Forward(new StudentInterviewDialog(), this.ResumeAfterSurveyComeBackAsync, activity, CancellationToken.None);
        }
        private Task ResumeAfterIntroduceDialogComeBackAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(this.MessageReceived);
            return Task.CompletedTask;
        }

        private async Task ResumeAfterSurveyComeBackAsync(IDialogContext context, IAwaitable<object> result)
        {
            StudentsViewModel currentStudent = new StudentsViewModel();
            context.UserData.TryGetValue<StudentsViewModel>("current_student", out currentStudent);

            await context.PostAsync($"HiBot có một số đề nghị cho bạn như sau: ");
           
            await context.PostAsync($"1/ CNTT 15-20 điểm. Công nghệ thông tin là một ngành có triễn vọng trong thời gian tới.... ");
            await context.PostAsync($"2/ CNTT 15-17 điểm. Nếu yêu thích sư phạm, Hãy chọn Sư phạm toán ");
            await context.PostAsync($"Tôi có thể giúp gì cho bạn tiếp theo ? ");

            context.Wait(this.MessageReceived);

              
        }

    }
}
