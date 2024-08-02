
using System.ComponentModel.DataAnnotations;

namespace BusNoti.Models;

public class BusMessage
{
    [Key]
    public int Id { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
