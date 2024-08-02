namespace BusNoti.State;
using BusNoti.Models;

public class AppState
{
    public List<Channel> channels = new();
    public List<Channel> Channels
    {
        get => channels;
        set {
            channels = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;
    private void NotifyStateChanged()  => OnChange?.Invoke();
}
