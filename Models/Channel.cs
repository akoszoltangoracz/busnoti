using System.ComponentModel.DataAnnotations;

namespace BusNoti.Models;

public record class Channel {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Types { get; set; } = new();
}
