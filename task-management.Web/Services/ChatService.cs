using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components;

namespace task_management.Web.Services;

public class ChatService : IDisposable
{
    private readonly ConcurrentDictionary<string, List<string>> _messages = new();
    private readonly NavigationManager _navigationManager;
    private bool _isPanelOpen = true;

    public event Action? OnChange;
    public bool IsPanelOpen
    {
        get => _isPanelOpen;
        set
        {
            if (_isPanelOpen != value)
            {
                _isPanelOpen = value;
                NotifyStateChanged();
            }
        }
    }

    public ChatService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += HandleLocationChanged;
    }

    public async Task AddMessage(string userId, string message)
    {
        var messages = _messages.GetOrAdd(userId, _ => new List<string>());
        messages.Add(message);
        NotifyStateChanged();
    }

    public IReadOnlyList<string> GetMessages(string userId)
    {
        return _messages.TryGetValue(userId, out var messages)
            ? messages.AsReadOnly()
            : new List<string>().AsReadOnly();
    }

    private void HandleLocationChanged(object? sender, EventArgs e)
    {
        // Preserve chat state across navigation
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void Dispose()
    {
        _navigationManager.LocationChanged -= HandleLocationChanged;
    }
}
