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
            await context.PostAsync($" Thông tin bạn cần có tại địa chỉ : http://eng.vinhuni.edu.vn/");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent(HiBotIntents.HighSchoolStudent.quession_about_university_major)]
        public async Task MajorOfUniversity(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
              await context.PostAsync($" Mời bạn cung cấp một số thông tin về khối thi và mức điểm ước lượng có thể đạt.");
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

        private   Task ResumeAfterSurveyComeBackAsync(IDialogContext context, IAwaitable<object> result)
        {
            StudentsViewModel currentStudent = null;
            context.UserData.TryGetValue<StudentsViewModel>("current_student", out currentStudent);

            context.PostAsync($"HiBot có một số đề nghị cho bạn như sau: ");
            context.PostAsync($"Với mức điểm : {currentStudent.DiemExpect}  của khối {currentStudent.Khoi} thì ....");

            context.Wait(this.MessageReceived);

            return Task.CompletedTask;
        }

    }
}

//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Connector;

//namespace HiBot.Dialogs.Students
//{
//    [Serializable]
//    public class StudentMasterDialog : IDialog<object>
//    {
//        private Entities.Students CurrentStudent;

//        public Task StartAsync(IDialogContext context)
//        {
//            CurrentStudent = new Entities.Students();
//            context.Wait(this.HandlerIntroduce);
//            return Task.CompletedTask;
//        }

//        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
//        {
//            var message = await result;
//            context.Wait(MessageReceivedAsync);
//        }
//        private async Task HandlerIntroduce(IDialogContext context, IAwaitable<IMessageActivity> result)
//        {

//            var activity = await result as Activity;

//            await context.Forward(new StudentServeyDialog(), this.ResumeAfterIntroduceDialogComeBack, activity, CancellationToken.None);

//        }

//        private Task ResumeAfterIntroduceDialogComeBack(IDialogContext context, IAwaitable<object> result)
//        {
//            return Task.CompletedTask;
//        }
//    }
//}