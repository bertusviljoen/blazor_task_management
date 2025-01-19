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
                await Task.Delay(100);
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
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "Hello<br>"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "How can I help you?<br>"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "I can help you with task management<br>"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "Let me tell you a joke<br>"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "Why did the scarecrow win an award? Because he was outstanding in his field!<br>"),
            new AssistantChatReplyItem(AssistantChatReplyItemType.AnswerChunk, "I hope you enjoyed that!<br>"),
        };
        
        for (int i = 0; i < assistantMessages.Length; i++)
        {
            await Task.Delay(50); // Simulate asynchronous work
            yield return assistantMessages[i];
        }
    }
}