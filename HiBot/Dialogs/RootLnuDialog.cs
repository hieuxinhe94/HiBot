using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HiBot.Constants;
using HiBot.Dialogs.Common;
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
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent(HiBotIntents.greetings)]
        public async Task Greetings(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
           
            
            await context.Forward(new IntroduceDialog(), this.ResumeAfterFormDialog, message, CancellationToken.None);
        }

        [LuisIntent(HiBotIntents.learn_english)]
        public async Task LearnEnglishPlugIn(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
          
            await context.Forward(new LearningEnglishDialog(), this.ResumeAfterFormDialog, message, CancellationToken.None);
            
        }

        [LuisIntent(HiBotIntents.help)]
        public async Task Help(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
         
            await context.Forward(new HelpDialog(), this.ResumeAfterFormDialog, message, CancellationToken.None);
            context.Done<object>(null);
        }

        private async Task ResumeAfterFormDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var searchQuery = await result;
                await context.PostAsync("ResumeAfterFormDialog");
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
            }
        }
    }
}