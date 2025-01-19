namespace task_management.Shared.Models;

public enum TaskItemStatus
{
    Todo,
    InProgress,
    Done
}

public class AssistantChatRequestMessage
{
    public bool IsAssistant { get; set; }
    public required string Text { get; set; }
}

public record AssistantChatRequest(IReadOnlyList<AssistantChatRequestMessage> Messages);

public record AssistantChatReplyItem(AssistantChatReplyItemType Type, string Text, int? SearchResultId = null, int?
    SearchResultProductId = null, int? SearchResultPageNumber = null);

public enum AssistantChatReplyItemType { AnswerChunk, Search, SearchResult, IsAddressedToCustomer };