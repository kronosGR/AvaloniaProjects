using System;

namespace AvaloniaChronoCalculator.Models;

public class TimeEntry
{
    public TimeSpan Time { get; }
    
    public Guid Id { get; } = Guid.NewGuid();

    public string DisplayValue =>
        $"+{Time.Days * 24 + Time.Hours}h {Time.Minutes}m {Time.Seconds}";

    public TimeEntry(int hours, int minutes, int seconds)
    {
        Time = new TimeSpan(hours, minutes, seconds);
    }

}