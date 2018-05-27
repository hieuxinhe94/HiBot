using System;
using System.Threading.Tasks;
using HiBot.ViewModel;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
 

namespace HiBot.Dialogs.CollegeStudent
{
    [Serializable]
    public class CollegeStudentInterviewDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            
            var form = new FormDialog<CollegeStudentViewModel>(new CollegeStudentViewModel(), BuildSurveyForm, FormOptions.PromptFieldsWithValues);

             context.Call(form, OnSurveyCompleted);
        }
        private static IForm<CollegeStudentViewModel> BuildSurveyForm()
        {
            return new FormBuilder<CollegeStudentViewModel>()
                .Message("Cung cấp một số thông tin xác thực để xác nhận bạn là ai nhé.")

                .AddRemainingFields()

                .Build();
        }
        private async Task OnSurveyCompleted(IDialogContext context, IAwaitable<CollegeStudentViewModel> result)
        {
            var interview = await result;
            try
            {
               
                CollegeStudentViewModel current_college_student = new CollegeStudentViewModel();
              
                current_college_student.StudentName = interview.StudentName;
                current_college_student.StudentId = interview.StudentId;

                context.UserData.SetValue<CollegeStudentViewModel>("current_college_student", current_college_student);

               
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
