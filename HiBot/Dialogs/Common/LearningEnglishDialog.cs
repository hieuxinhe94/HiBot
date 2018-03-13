using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HiBot.Midware;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs.Common
{
    [Serializable]
    public class LearningEnglishDialog : IDialog<object>
    {

        public Task StartAsync(IDialogContext context)
        {
            // Root dialog initiates and waits for the next message from the user. 
            // When a message arrives, call MessageReceivedAsync.
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("Say some things and wen can start learning english now.");
            context.Wait(TalkRecieveAsync);
        }
        private async Task TalkRecieveAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            RootObject dataGrammarObj = await LanguageCheckingHelper.CheckGrammarAsync(activity.Text);
            if (dataGrammarObj.matches.Any()) // has any grammar error
            {
                //  get words error 
                List<ShowGrammarViewModel> lstError = new List<ShowGrammarViewModel>();
                lstError.AddRange(dataGrammarObj.matches.Select(t => new ShowGrammarViewModel
                {
                    ErrorMessage = t.message,
                    ErrorRule = t.rule.description,
                    Category = t.shortMessage
                }));
                // Create the activity and attach a set of Hero cards.
                var listOfAttachments = new List<Attachment>();

                foreach (var itemError in lstError)
                {
                    List<CardImage> cardImages = new List<CardImage>();
                    int asomewareWidth = 300;
                    if (itemError.ErrorMessage.Length > 50)
                    {
                        asomewareWidth += 200;
                    }
                    cardImages.Add(new CardImage(url: "data:image/png;base64," +
                                                      Convert.ToBase64String(ImageHelper.ConvertTextToImage(itemError.ErrorRule, "Bookman Old Style", 12, Color.AliceBlue, asomewareWidth, 100))));

                    HeroCard plCard = new HeroCard()
                    {
                        Title = itemError.ErrorMessage,
                        Subtitle = itemError.Category,
                        Images = cardImages

                    };
                    listOfAttachments.Add(plCard.ToAttachment());
                }

                var message = context.MakeMessage();
                message.Attachments = listOfAttachments;
                await context.PostAsync(message);
            }
            else
            {
                await context.PostAsync("All right,Good Job");
            }
            context.Wait(TalkRecieveAsync);

        }
    }

    public class ShowGrammarViewModel
    {
        public string ErrorMessage { get; set; }
        public string ErrorRule { get; set; }
        public string Category { get; set; }
    }
}