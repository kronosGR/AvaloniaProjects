using System.Collections.ObjectModel;
using AvaloniaChronoCalculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaChronoCalculator.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ChronoCore _chronoCore = new();


    [ObservableProperty]
    public ObservableCollection<TimeEntry> addedEntries = new();

    [ObservableProperty] private string errorMessage = "";

    [ObservableProperty] private int hour;
    [ObservableProperty] private int minute;
    [ObservableProperty] private int second;
    [ObservableProperty] private string totalTimeDisplay;

    public MainWindowViewModel()
    {
        TotalTimeDisplay = _chronoCore.FormattedTotalTime;
    }

    [RelayCommand]
    private void AddTime()
    {
        if (CanAddTime())
        {
            var newEntry = _chronoCore.AddTime(Hour, Minute, Second);
            AddedEntries.Add(newEntry);
            TotalTimeDisplay = _chronoCore.FormattedTotalTime;

            Hour = 0;
            Minute = 0;
            Second = 0;
            ShowNotification("");
        }
        else
        {
            ShowNotification(
                "Error: At least one time component (H, M, or S) must be greater than zero.");
        }
    }

    [RelayCommand]
    private void Clear()
    {
        _chronoCore.Clear();
        AddedEntries.Clear();
        TotalTimeDisplay = _chronoCore.FormattedTotalTime;

        Hour = 0;
        Minute = 0;
        Second = 0;
    }

    [RelayCommand]
    private void RemoveEntry(TimeEntry entryToRemove)
    {
        if (entryToRemove != null)
            if (_chronoCore.RemoveEntry(entryToRemove.Id))
            {
                AddedEntries.Remove(entryToRemove);
                TotalTimeDisplay = _chronoCore.FormattedTotalTime;
            }
    }

    private bool CanAddTime()
    {
        return Hour > 0 || Minute > 0 || Second > 0;
    }

    private void ShowNotification(string message)
    {
        ErrorMessage = message;
    }
}