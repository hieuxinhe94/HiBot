using System;
using System.Threading.Tasks;
using HiBot.Entities;
using HiBot.Repository.EntityFramework;
using HiBot.ViewModel;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.Students
{
    [Serializable]
    public class StudentServeyDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
 
            var form = new FormDialog<StudentServey>(new StudentServey(), BuildSurveyForm, FormOptions.PromptFieldsWithValues);

            await Task.Run(() =>  context.Call(form, OnSurveyCompleted));
        }

       

        private static IForm<StudentServey> BuildSurveyForm()
        {
            return new FormBuilder<StudentServey>()
                .Message("Welcome to the Student Servey. I need some your infomation. Share it with me, please !")
                 
                .AddRemainingFields()
                .Build();
        }

        private async Task OnSurveyCompleted(IDialogContext context, IAwaitable<StudentServey> result)
        {
            try
            {
                var survey = await result;

                var student = new Entities.Students
                {
                    Name = survey.StudentName,
                    HighSchool = survey.HighSchool,

                    PhoneNumber = survey.PhoneNumber,
                    Sex = (int)survey.Sex == 1 ? true : false
                };
                var db = new HiBotDbContext();
                context.UserData.SetValue<Entities.Students>("current_student", student);
                db.Students.Add(student);
                db.SaveChangesAsync();

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
