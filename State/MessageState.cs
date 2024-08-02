namespace BusNoti.State;

using BusNoti.Models;

public class BusMessageEvent: EventArgs {
    public BusMessage? BusMessage { get; init; }
}

public class MessageState
{
    public event Action<BusMessageEvent>? OnMessage;

    public void NotifyNewMessage(BusMessage busMessage)
    {
        OnMessage?.Invoke(new BusMessageEvent{
            BusMessage = busMessage,
        });
    }
}
