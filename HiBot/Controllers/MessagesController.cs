﻿using System;
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
          readonly RootDialog rootDialog;

        private readonly ILifetimeScope _scope;
        
        public MessagesController(RootDialog _rootDialog)
        {
            rootDialog = _rootDialog;
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
                        if (newMember.Id != message.Recipient.Id)
                        {
                            attachment = GetInlineAttachment();

                            var reply = message.CreateReply();
                            reply.Attachments = new List<Attachment>{attachment};
                            reply.Text = $"Welcome {newMember.Name} are chatting with Hibot!";
                            reply.Speak = "Hello";
                            client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }

                //ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                //Activity reply = message.CreateReply("Hello from my simple Bot!");
                //connector.Conversations.ReplyToActivityAsync(reply);

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

        private static Attachment GetInlineAttachment()
        {
            var imagePath = HttpContext.Current.Server.MapPath("~/Resources/Images/logo.png");

            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = "small-image.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}"
            };
        }

        #endregion

    }
}