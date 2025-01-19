using System.Text.Json;
using task_management.Shared.Models;

namespace task_management.ApiService.Endpoints;

public static class AssistantEndpoint
{
    public static void MapAssistantEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/assistant").WithTags(nameof(Assistant));

        group.MapPost("/chat", async (AssistantChatRequest request,
                HttpContext httpContext,
                CancellationToken token) =>
        {
            await httpContext.Response.WriteAsync("[null");
            
            await foreach(var chuck in GetAssistantMessages())
            {
                await httpContext.Response.WriteAsync(",\n");
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(chuck));
                await Task.Delay(200);
            }
                
            // Signal to the UI that we're finished
            await httpContext.Response.WriteAsync("]");
            
            
        })
        .WithName("AssistantChat")
        .WithOpenApi();
    }

    internal record Assistant();
    
    internal static async IAsyncEnumerable<AssistantChatReplyItem> GetAssistantMessages()
    {
        var assistantMessages = new[]
        {
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "Hello"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "How can I help you?"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "I can help you with task management"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "Let me tell you a joke"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "Why did the scarecrow win an award? Because he was outstanding in his field!"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "I hope you enjoyed that!"),
        };
        
        for (int i = 0; i < assistantMessages.Length; i++)
        {
            await Task.Delay(1000); // Simulate asynchronous work
            yield return assistantMessages[i];
        }
    }
}