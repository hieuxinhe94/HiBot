using System;
using System.Threading.Tasks;
using HiBot.ViewModel;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace HiBot.Dialogs.Students
{
    [Serializable]
    public class StudentInterviewDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {

            var form = new FormDialog<StudentInterviewServey>(new StudentInterviewServey(), BuildSurveyForm, FormOptions.PromptFieldsWithValues);

            await Task.Run(() => context.Call(form, OnSurveyCompleted));
        }
         
        private static IForm<StudentInterviewServey> BuildSurveyForm()
        {
            return new FormBuilder<StudentInterviewServey>()
                .Message("Cảm ơn sự quan tâm của bạn, bạn hãy cung cấp một số thông tin về học lực để được tư vấn chính xác hơn nhé.")

                .AddRemainingFields()
                
                .Build();
        }

        private async Task OnSurveyCompleted(IDialogContext context, IAwaitable<StudentInterviewServey> result)
        {
            try
            {
                var interview = await result;
                StudentsViewModel currentStudent = null;
                context.UserData.TryGetValue<StudentsViewModel>("current_student", out currentStudent);

                currentStudent.DiemExpect = interview.Diem;
                currentStudent.Khoi = interview.Khoi;

                context.UserData.SetValue<StudentsViewModel>("current_student", currentStudent);

                // analysis

               //await context.PostAsync($"Thanks, Tôi đã nhận được thông tin của bạn, tôi có một số gợi ý như sau: ");
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
