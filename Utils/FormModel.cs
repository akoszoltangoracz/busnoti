namespace BusNoti.Utils;

public class ChannelFormModel {
    public string Name { get; set; } = string.Empty;
    public string TypesString { get; set; } = string.Empty;
    public List<string> Types {
        get {
            var arr = TypesString.Split(" ");
            return [.. arr];
        }
        set {
            TypesString = string.Join(' ', value);
        }
    }
}
