using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Autofac;
using HiBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HiBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        readonly RootLnuDialog rootDialog;

        private readonly ILifetimeScope _scope;

        public MessagesController(RootLnuDialog _rootLnuDialog )
        {
            rootDialog = _rootLnuDialog;
        }
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        /// // TODO: "service locator"

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity, CancellationToken token)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                // first time call to root dialog, and this will be display options to start 
                await Conversation.SendAsync(activity, () => this.rootDialog);
            }
            else
            {
                // Xử lý các sự kiện hệ thống đối với cuộc trò chuyện như thêm người mới vào hộp thoại, người nào đó rời đi ....
                HandleSystemMessage(activity);
            }
            // Alway return OK
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {

                Attachment attachment = null;
                // Note: Add introduction here:
                IConversationUpdateActivity update = message;
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());
                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                   
                    foreach (var newMember in update.MembersAdded)
                    {
                        List<CardImage> cardImages = new List<CardImage>();
                        cardImages.Add(GetInlineAttachment());
                        HeroCard plCard = new HeroCard()
                        {
                            Title = $"I am HiBot",
                            Subtitle = "A chatbot is a computer program which conducts a conversation via auditory or textual methods. I was developed by HieuPham",
                            Images = cardImages

                        };

                        if (newMember.Id != message.Recipient.Id)
                        {
                            var reply = message.CreateReply();
                            reply.Attachments = new List<Attachment> { plCard.ToAttachment() };
                            reply.Text = $"Welcome {newMember.Name} !\n";
                            reply.Speak = "Hello";
                            reply.InputHint = InputHints.ExpectingInput;
                            client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }



            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        #region private method

        private static CardImage GetInlineAttachment()
        {
            var imagePath = HttpContext.Current.Server.MapPath("~/Resources/Images/logo.png");

            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new CardImage
            {

                Alt = "Welcome card",
                Url = $"data:image/png;base64,{imageData}"
            };
        }

        #endregion

    }
}