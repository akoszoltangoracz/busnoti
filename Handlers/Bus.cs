namespace BusNoti.Handlers;

using BusNoti.Models;
using BusNoti.State;
using Microsoft.EntityFrameworkCore;
using NATS.Client.Core;

public class BusHandler : BackgroundService
{
    private readonly INatsConnection natsConnection;
    private readonly IDbContextFactory<Contexts.ChannelContext> channelContext;
    private readonly MessageState messageState;
    public BusHandler(
        INatsConnection _natsConnection,
        IDbContextFactory<Contexts.ChannelContext> _channelContext,
        MessageState _messageState
    )
    {
        natsConnection = _natsConnection;
        channelContext = _channelContext;
        messageState = _messageState;
    }

    private List<string> blacklistedTopics = new();

    private async Task ListenMessagesAsync(CancellationToken stoppingToken)
    {
        await foreach(var msg in natsConnection.SubscribeAsync<string>(">").WithCancellation(stoppingToken))
        {
            Console.WriteLine($"message received {msg.Subject}");

            if (blacklistedTopics.Contains(msg.Subject))
            {
                Console.WriteLine($"Skipping {msg.Subject}");
                continue;
            }

            using var context = await channelContext.CreateDbContextAsync(stoppingToken);
            var busMessage = new BusMessage(){
                Topic = msg.Subject,
                CreatedAt = DateTime.UtcNow,
                Content = msg.Data ?? "",
            };

            context.Add(busMessage);
            await context.SaveChangesAsync();

            messageState.NotifyNewMessage(busMessage);
        }
    }

    private async Task ListenBlacklistUpdatesAsync(CancellationToken stoppingToken)
    {
        await foreach(var _ in natsConnection.SubscribeAsync<string>("busnoti.blacklist.update").WithCancellation(stoppingToken))
        {
            Console.WriteLine("blacklist updated");
            await UpdateBlacklistAsync(stoppingToken);
        }
    }

    private async Task UpdateBlacklistAsync(CancellationToken stoppingToken)
    {
        using var context = await channelContext.CreateDbContextAsync(stoppingToken);
        blacklistedTopics = await (
            from blacklist in context.Blacklists
            select blacklist.Topic
        ).ToListAsync();

        Console.WriteLine($"Blacklist: {string.Join(' ',  blacklistedTopics)}");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting message handler");

        await UpdateBlacklistAsync(stoppingToken);

        await Task.WhenAll([
            ListenMessagesAsync(stoppingToken),
            ListenBlacklistUpdatesAsync(stoppingToken),
        ]);
    }
}
