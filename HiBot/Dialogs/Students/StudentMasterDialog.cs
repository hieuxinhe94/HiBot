using System;
using System.Threading;
using System.Threading.Tasks;
using HiBot.Constants;
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
            await context.PostAsync($"Student Master Message Receive Async Start Handler");
            context.Wait(MessageReceivedAsync);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {

            context.Wait(this.MessageReceived);
        }

        [LuisIntent(HiBotIntents.HighSchoolStudent.quession_about_university_exam)]
        public async Task UniversityExam(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            // post link to student
            await context.PostAsync($"quession_about_university_exam");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent(HiBotIntents.HighSchoolStudent.quession_about_university_major)]
        public async Task MajorOfUniversity(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            await context.PostAsync($"quession_about_university_major");

          
            await context.Forward(new StudentInterviewDialog(), this.ResumeAfterIntroduceDialogComeBack, activity, CancellationToken.None);

            context.Wait(this.MessageReceived);
        }

        private async Task HandlerIntroduce(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var activity = await result as Activity;

            await context.Forward(new StudentServeyDialog(), this.ResumeAfterIntroduceDialogComeBack, activity, CancellationToken.None);

        }

        private Task ResumeAfterIntroduceDialogComeBack(IDialogContext context, IAwaitable<object> result)
        {
            // get object
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
    }
}