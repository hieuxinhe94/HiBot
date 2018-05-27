using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiBot.ViewModel
{
    [Serializable]
    public class CollegeStudentViewModel
    {
        [Prompt("Cho tôi biết tên đầy đủ của bạn")]
        public string StudentName;

        [Prompt("Mã số sinh viên :")]
        public string StudentId;

        [Prompt("Mật khẩu đăng nhập :")]
        public string Password;
        public static IForm<CollegeStudentViewModel> BuildOrderForm()
        {
            return new FormBuilder<CollegeStudentViewModel>()
                .Message("Cung cấp một số thông tin xác thực để xác nhận bạn là ai nhé.")

                .AddRemainingFields()

                .Build();
        }
    }
}