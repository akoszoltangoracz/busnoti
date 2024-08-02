namespace BusNoti.Handlers;

using NATS.Client.Core;

public class BlacklistHandler
{
    private readonly INatsConnection natsConnection;
    public BlacklistHandler(INatsConnection _natsConnection)
    {
        natsConnection = _natsConnection;
    }
    public async Task NotifyBlacklistAsync(string reason)
    {
        await natsConnection.PublishAsync(subject: "busnoti.blacklist.update", data: reason);
    }
}
