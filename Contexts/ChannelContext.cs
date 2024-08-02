namespace BusNoti.Contexts;

using Microsoft.EntityFrameworkCore;
using BusNoti.Models;

public class ChannelContext: DbContext
{
    public DbSet<Channel> Channels { get; set; }
    public DbSet<BusMessage> BusMessages { get; set; }
    public DbSet<Blacklist> Blacklists { get; set; }

    public ChannelContext(DbContextOptions<ChannelContext> options): base(options) {}
}
