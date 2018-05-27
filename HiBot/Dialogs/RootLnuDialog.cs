using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HiBot.Constants;
using HiBot.Dialogs.Common;
using HiBot.Dialogs.Students;
using HiBot.Repository.EntityFramework;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs
{
    [LuisModel("98d69908-d401-40f9-b207-1289d0c79087", "0cf6cddfce5f4440b4046e4d1b444a32")]
    [Serializable]
    public class RootLnuDialog : LuisDialog<object>
    {

        public override Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceived);
            return Task.CompletedTask;
        }

        protected override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            return base.MessageReceived(context, item);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (userName == string.Empty)
            {
                context.Call(new GreetingDialog(), Callback);
            }
            var msg = await activity;
            // check history xem 
            var db = new HiBotDbContext();
            var ex = db.HistoryMissUnderstand.Where(t => t.Sentences.Equals(msg.Text.ToLower())).FirstOrDefault();
            if (ex != null)
            {
                if (!string.IsNullOrWhiteSpace(ex.Intent))
                {
                    if (string.IsNullOrEmpty(ex.Reply))
                    {
                        string message = $"Sorry {userName}, I know you want to talk about {ex.Intent}, But I can't say anything to reply now!";
                        await context.PostAsync(message);
                    }
                    else
                    {
                        string message = ex.Reply;
                        await context.PostAsync(message);
                    }
                }
            }
            else
            {
                var stateOfWaitForTeaching = false;
                context.UserData.TryGetValue<bool>("is_waiting_for_teaching", out stateOfWaitForTeaching);
                var missUnderstandUtterance = string.Empty;
                context.UserData.TryGetValue<string>("miss_understand_sentences", out missUnderstandUtterance);

                if (stateOfWaitForTeaching && !string.IsNullOrWhiteSpace(missUnderstandUtterance))
                {
                    // insert into inmemory db to learn

                    db.HistoryMissUnderstand.Add(new Entities.HistoryMissUnderstand
                    {
                        Sentences = missUnderstandUtterance.ToLower(),
                        TimeZone = DateTime.Now,
                        Intent = msg.Text
                    });

                    string message = $"Thanks {userName} very much, No one can perfect and I need learning every day.";
                    context.UserData.SetValue<bool>("is_waiting_for_teaching", false);
                    context.UserData.SetValue<string>("miss_understand_sentences", string.Empty);
                    await context.PostAsync(message);
                }
                else
                {
                    string message = $"Sorry {userName}, I don't understand now. However, I hope I can learn more from you ";
                    await context.PostAsync(message);

                    message = $" Can you give me a intent of your sentences ?";
                    await context.PostAsync(message);

                    context.UserData.SetValue<bool>("is_waiting_for_teaching", true);
                    context.UserData.SetValue<string>("miss_understand_sentences", msg.Text);
                }
            }

            context.Wait(this.MessageReceived);
        }

        [LuisIntent(HiBotIntents.greetings)]
        public async Task Greetings(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var stateOfWaitForTeaching = false;
            context.UserData.TryGetValue<bool>("is_waiting_for_teaching", out stateOfWaitForTeaching);
            if (stateOfWaitForTeaching)
            {
                context.UserData.SetValue<bool>("is_waiting_for_teaching", false);
                await None(context, activity, result);
                return;
            }
            var message = await activity;
            context.Call(new GreetingDialog(), Callback);
        }

        [LuisIntent(HiBotIntents.learn_english)]
        public async Task LearnEnglishPlugIn(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var stateOfWaitForTeaching = false;
            context.UserData.TryGetValue<bool>("is_waiting_for_teaching", out stateOfWaitForTeaching);
            if (stateOfWaitForTeaching)
            {
                context.UserData.SetValue<bool>("is_waiting_for_teaching", false);
                await None(context, activity, result);
                return;
            }

            var message = await activity;
            await context.Forward(new LearningEnglishDialog(), this.Callback, message, CancellationToken.None);
        }

        [LuisIntent(HiBotIntents.help)]
        public async Task Help(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var stateOfWaitForTeaching = false;
            context.UserData.TryGetValue<bool>("is_waiting_for_teaching", out stateOfWaitForTeaching);
            if (stateOfWaitForTeaching)
            {
                context.UserData.SetValue<bool>("is_waiting_for_teaching", false);
                await None(context, activity, result);
                return;
            }
            var message = await activity;
            context.Call(new HelpDialog(), Callback);
        }

        [LuisIntent(HiBotIntents.needABusinessHelp)]
        public async Task BusinessHelp(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var stateOfWaitForTeaching = false;
            context.UserData.TryGetValue<bool>("is_waiting_for_teaching", out stateOfWaitForTeaching);
            if (stateOfWaitForTeaching)
            {
                context.UserData.SetValue<bool>("is_waiting_for_teaching", false);
                await None(context, activity, result);
                return;
            }
            var message = await activity;
            await context.Forward(new IntroduceDialog(), Callback, message, CancellationToken.None);
        }

        [LuisIntent(HiBotIntents.about)]
        public async Task About(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);

            string message = $"Hi {userName}, I'm Hibot.";
            await context.PostAsync(message);

            message = $" HiBot là dự án dựa research về ChatBot với ý tưởng là hệ thống trả lời tin nhắn tự động trong trường đại học.";
            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }
        [LuisIntent(HiBotIntents.weather_getForecast)]
        public async Task GetForecast(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
        }

        [LuisIntent(HiBotIntents.joking)]
        public async Task Joking(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            string message = $"Haha, Are you kidding me ? You are stupid! ";
            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }

        [LuisIntent(HiBotIntents.swear)]
        public async Task Swear(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            int count_swear = 1;
            context.UserData.TryGetValue<int>("count_swear", out count_swear);
            count_swear++;
            context.UserData.SetValue<int>("count_swear", count_swear++);

            if (count_swear >= 3)
            {
                string message = $"Shut up and go away!! ";
                await context.PostAsync(message);
                message = $"if you don't understand: 'Im mẹ mồm đi !!'";
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent(HiBotIntents.student)]
        public async Task IAmStudent(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.Forward(new StudentMasterDialog(), this.Callback, message, CancellationToken.None);
        }

        [LuisIntent(HiBotIntents.Places_CheckAreaTraffic)]
        public async Task PlacesCheckAreaTraffic(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
        }

        //===============================

        private async Task ResumeAfterFormDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var searchQuery = await result;
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation.";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
                context.Wait(MessageReceived);
            }
        }
        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

    }
}