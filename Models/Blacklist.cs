using System.ComponentModel.DataAnnotations;

namespace BusNoti.Models;

public record class Blacklist
{
    [Key]
    public int Id { get; set; }
    public string? Topic { get; set; }
}
