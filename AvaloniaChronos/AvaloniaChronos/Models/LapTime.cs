using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaChronos.Models;

public class LapTime:ObservableObject
{
    public int LapNumber { get; set; }
    public string DisplayTime { get; set; }
}