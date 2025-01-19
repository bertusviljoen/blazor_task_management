using System.Runtime.CompilerServices;
using System.Text.Json;
using task_management.Web.Components.Pages.TaskBoardAssistant;

namespace task_management.Web.Services;

public class AssistantService(HttpClient http)
{
    public async IAsyncEnumerable<TaskBoardAssistant.AssistantChatReplyItem> AssistantChatAsync(TaskBoardAssistant.AssistantChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/assistant/chat")
        {
            Content = JsonContent.Create(request),
        };
        var response = await http.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<TaskBoardAssistant.AssistantChatReplyItem>(stream, cancellationToken: cancellationToken))
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }
}