using System;
using System.Threading.Tasks;
using HiBot.ViewModel;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace HiBot.Dialogs.Students
{
    [Serializable]
    public class StudentServeyDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
           
            var form = new FormDialog<StudentServey>(new StudentServey(), BuildSurveyForm, FormOptions.PromptInStart);
            context.Call(form, this.OnSurveyCompleted);
        }

        private static IForm<StudentServey> BuildSurveyForm()
        {
            return new FormBuilder<StudentServey>()
                .Message("I need some your infomation. Share it with me, please !")
                
                .AddRemainingFields()
                .Build();
        }

        private async Task OnSurveyCompleted(IDialogContext context, IAwaitable<StudentServey> result)
        {
            try
            {
                var survey = await result;

                await context.PostAsync($"Thanks, Got it... {survey.StudentName} you've been  {survey.Birthday} years and use {survey.PhoneNumber}.");
            }
            catch (FormCanceledException<StudentServey> e)
            {
                string reply;

                if (e.InnerException == null)
                {
                    reply = "You have canceled the survey";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {e.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }

            context.Done(string.Empty);
        }
    }
}
