using System;
using System.Threading;
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

            await Task.Run(() => context.Call(form, OnSurveyCompleted));

        }
            private static IForm<StudentServey> BuildSurveyForm()
        {
            
            return new FormBuilder<StudentServey>()
                .Message("Chào mừng bạn đến với cuộc khảo sát ngắn, Một số thông tin cụ thể sẻ giúp tôi dễ dàng tư vấn chính xác cho bạn !")
                .AddRemainingFields()
                .Build();
        }

        private async Task OnSurveyCompleted(IDialogContext context, IAwaitable<StudentServey> result)
        {
            var survey = await result;
            try
            {
             
                //var student = new StudentsViewModel
                //{
                //    Name = survey.StudentName,
                  
                //    PhoneNumber = survey.PhoneNumber,
                //    Sex = (int)survey.Sex == 1 ? true : false
                //};
                //var db = new HiBotDbContext();
                //context.UserData.SetValue<StudentsViewModel>("current_student", student);
                //db.Students.Add(student);
             //   await db.SaveChangesAsync();

                await context.PostAsync($"Cảm ơn, Vui lòng xác nhận {survey.StudentName} :  {survey.Birthday} tuổi, điện thoại {survey.PhoneNumber}. Tôi có thể giúp gì cho bạn ?");
            }
            catch (Exception e)
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
           
            context.Done<object>(null);
        }
       
    }
}
