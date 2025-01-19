using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace task_management.Web.Components.Pages
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChatService> _logger;

        public ChatService(HttpClient httpClient, ILogger<ChatService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ChatMessage> SendMessageAsync(string message)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/chat/send", new { Message = message });
                response.EnsureSuccessStatusCode();

                var chatMessage = await response.Content.ReadFromJsonAsync<ChatMessage>();
                return chatMessage ?? throw new InvalidOperationException("Received null response from chat API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to chat API.");
                throw;
            }
        }

        public async Task<ChatMessage> ReceiveMessageAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/chat/receive");
                response.EnsureSuccessStatusCode();

                var chatMessage = await response.Content.ReadFromJsonAsync<ChatMessage>();
                return chatMessage ?? throw new InvalidOperationException("Received null response from chat API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error receiving message from chat API.");
                throw;
            }
        }

        public async Task RetrySendMessageAsync(string message, int retryCount = 3)
        {
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    await SendMessageAsync(message);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Retry {RetryCount} failed for sending message.", i + 1);
                    if (i == retryCount - 1)
                    {
                        _logger.LogError(ex, "All retries failed for sending message.");
                        throw;
                    }
                }
            }
        }
    }
}
