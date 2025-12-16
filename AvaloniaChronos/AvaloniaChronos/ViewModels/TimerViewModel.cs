using System;
using System.Timers;
using Avalonia.Threading;
using AvaloniaChronos.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaChronos.ViewModels;

public partial class TimerViewModel: ViewModelBase
{
    private readonly DispatcherTimer _uiTimer;
    private TimeSpan _timeSet;
    private TimeSpan _timeRemaining;
    
    [ObservableProperty]
    private bool _isRunning;

    public override string Header => "Timer";

    [ObservableProperty]
    private string _timeRemainingDisplay="00:00:00";

    [ObservableProperty] private int _hoursInput = 0;
    
    [ObservableProperty] private int _minutesInput = 1;
    
    [ObservableProperty] private int _secondsInput = 0; 
    
    [ObservableProperty] private string _startPauseButtonText = "Start";

    [RelayCommand]
    private void StartPauseTimer()
    {
        // Logic when the timer is currently STOPPED (Start or Resume)
        if (!_isRunning)
        {
            if (_timeRemaining.TotalSeconds <= 0)
            {
                // If time is zero (first run or timer finished), set the time from inputs
                SetInitialTime();
                if (_timeSet.TotalSeconds <= 0) return; // Prevent starting if input time is zero
            }
            
            _uiTimer.Start();
            IsRunning = true; // Use the property setter to trigger INotifyPropertyChanged
            StartPauseButtonText = "Pause";
        }
        // Logic when the timer is currently RUNNING (Pause)
        else
        {
            _uiTimer.Stop();
            IsRunning = false; // Use the property setter
            StartPauseButtonText = "Resume";
        }

        ResetTimerCommand.NotifyCanExecuteChanged();
    }
    
    [RelayCommand(CanExecute = nameof(CanResetTimer))]
    private void ResetTimer()
    {
        _uiTimer.Stop();
        IsRunning = false; // Use the property setter
        
        // Reset remaining time to the time that was originally set
        _timeRemaining = _timeSet;
        TimeRemainingDisplay = FormatTime(_timeRemaining);
        StartPauseButtonText = "Start";

        // Handle case where no time was ever set (e.g., all inputs are 0)
        if (_timeSet.TotalSeconds == 0)
        {
            TimeRemainingDisplay="00:00:00";
        }
        
        ResetTimerCommand.NotifyCanExecuteChanged();
    }
    
    partial void OnHoursInputChanged(int value)
    {
        UpdateInputTime();
    }

    partial void OnMinutesInputChanged(int value)
    {
        UpdateInputTime();
    }

    partial void OnSecondsInputChanged(int value)
    {
        UpdateInputTime();
    }
    
    // Allows reset only if a time has been set or is currently running
    private bool CanResetTimer() => _timeRemaining.TotalSeconds > 0 || _timeSet.TotalSeconds > 0;

    // 🌟 CORRECTED CONSTRUCTOR 🌟
    public TimerViewModel()
    {
        // 1. Initialize DispatcherTimer setup
        _uiTimer = new DispatcherTimer();
        _uiTimer.Interval = TimeSpan.FromSeconds(1);
        _uiTimer.Tick += Timer_Tick;
        
        // 2. Set the initial _timeSet and _timeRemaining based on default inputs (0h, 1m, 0s)
        SetInitialTime(); // Call the setup method to initialize both _timeSet and _timeRemaining
        // TimeRemainingDisplay is updated inside SetInitialTime()
        
        // 3. Remove the invalid/incomplete line that was causing auto-start problems
        // ToggleT <--- DELETED
        
        // 4. Ensure the CanExecute state for the Reset button is updated on load
        ResetTimerCommand.NotifyCanExecuteChanged();
    }

    private void SetInitialTime()
    {
        _timeSet = new TimeSpan(_hoursInput, _minutesInput, _secondsInput);
        _timeRemaining = _timeSet;
        TimeRemainingDisplay = FormatTime (_timeRemaining);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
        TimeRemainingDisplay = FormatTime(_timeRemaining);
        
        if (_timeRemaining.TotalSeconds<=0) 
        {
            _uiTimer.Stop();
            IsRunning = false; // Use the property setter
            TimeRemainingDisplay = "00:00:00";
            _startPauseButtonText = "Start";
            
            // Notify CanExecute changed in case the inputs were 0 and the reset button was previously disabled
            ResetTimerCommand.NotifyCanExecuteChanged(); 
        }
    }

    private void UpdateInputTime()
    {
        if (!IsRunning)
        {
            _timeSet = new TimeSpan(_hoursInput, _minutesInput, _secondsInput);
            _timeRemaining = _timeSet;
            
            TimeRemainingDisplay = FormatTime(_timeRemaining);
            
            ResetTimerCommand.NotifyCanExecuteChanged();
        }
    }

    private string FormatTime(TimeSpan time)
    {
        return time.ToString(@"hh\:mm\:ss");
    }
}