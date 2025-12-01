using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaChronoCalculator.Models;

public class ChronoCore
{
    
    private readonly List<TimeEntry> _entries = new List<TimeEntry>();
    private TimeSpan _totalTime = TimeSpan.Zero;
    
    public IReadOnlyList<TimeEntry> Entries => _entries;
    
    //public string FormattedTotalTime => _totalTime.ToString();
     public string FormattedTotalTime => $"{_totalTime.Days * 24 + _totalTime.Hours:D2}:{_totalTime.Minutes:D2}:{_totalTime.Seconds:D2}";

    private void RecalculateTotal()
    {
        _totalTime = TimeSpan.Zero;
        foreach (var entry in _entries)
        {
            _totalTime =_totalTime.Add(entry.Time);
        }
    }

    public TimeEntry AddTime(int hour, int minute, int second)
    {
        var newEntry = new TimeEntry(hour, minute, second);
        _entries.Add(newEntry);
        RecalculateTotal();
        return newEntry;
    }

    public void Clear()
    {
        _entries.Clear();
        RecalculateTotal();
    }

    public bool RemoveEntry(Guid id)
    {
        var entryToRemove = _entries.FirstOrDefault(e=> e.Id == id);
        if (entryToRemove != null)
        {
            _entries.Remove(entryToRemove);
            RecalculateTotal();
            return true;
        }
        return false;
    }
}