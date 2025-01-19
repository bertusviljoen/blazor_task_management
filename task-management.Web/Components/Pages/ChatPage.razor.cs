using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace task_management.Web.Components.Pages
{
    public partial class ChatPage : ComponentBase
    {
        private string UserMessage { get; set; } = string.Empty;
        private List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        private bool IsLoading { get; set; } = false;

        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(UserMessage))
            {
                var message = new ChatMessage
                {
                    Text = UserMessage,
                    Timestamp = DateTime.Now,
                    Sender = "User"
                };

                Messages.Add(message);
                UserMessage = string.Empty;

                // Simulate bot response
                IsLoading = true;
                Task.Delay(2000).ContinueWith(_ =>
                {
                    var botMessage = new ChatMessage
                    {
                        Text = "This is a bot response.",
                        Timestamp = DateTime.Now,
                        Sender = "Bot"
                    };

                    Messages.Add(botMessage);
                    IsLoading = false;
                    InvokeAsync(StateHasChanged);
                });
            }
        }

        private void AttachFile()
        {
            // Logic to handle file attachment
        }

        private void StartCall()
        {
            // Logic to handle call button click
        }
    }

    public class ChatMessage
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
    }
}
