using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Threading;
using AvaloniaChronos.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaChronos.ViewModels;

public partial class StopWatchViewModel: ViewModelBase
{
    private readonly DispatcherTimer _uiTimer;
    private readonly Stopwatch _chronometer = new Stopwatch();
    private bool _isRunning = false;
    
    [ObservableProperty]
    private string _currentTimeDisplay = "00:00:00.00";

    [ObservableProperty] private string _startStopButtonText = "Start";
    
    public override string Header => "StopWatch";

    public ObservableCollection<LapTime> LapTimes { get; } =
        new ObservableCollection<LapTime>();


    [RelayCommand]
    private void ToggleStopWatch()
    {
        if (_isRunning)
        {
            _chronometer.Stop();
            _uiTimer.Stop();
            _isRunning = false;
            StartStopButtonText = "Start";
        }
        else
        {
            _chronometer.Start();
            _uiTimer.Start();
            _isRunning = true;
            StartStopButtonText = "Stop";
        }

        RecordLapCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanRecordLap))]
    private void RecordLap(){
        LapTimes.Insert(0, new LapTime
        {
            LapNumber = LapTimes.Count+1,
            DisplayTime = _chronometer.Elapsed.ToString("h\\:mm\\:ss\\.ff")
        });
    }
    
    private bool CanRecordLap() => _isRunning;

    [RelayCommand]
    private void ResetStopWatch()
    {
        _chronometer.Reset();
        _uiTimer.Stop();
        _isRunning = false;
        CurrentTimeDisplay = "00:00:00.00";
        StartStopButtonText = "Start";
        LapTimes.Clear();
        RecordLapCommand.NotifyCanExecuteChanged();
    }
    
    public StopWatchViewModel()
    {
        _uiTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(20), DispatcherPriority.Normal, Timer_tick );
    }

    private void Timer_tick(object? sender, EventArgs e)
    {
        CurrentTimeDisplay = _chronometer.Elapsed.ToString("hh\\:mm\\:ss\\.ff");
    }
        
        
   
}