using System.Collections.ObjectModel;
using AvaloniaChronos.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaChronos.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    
    public ObservableCollection<ViewModelBase> Tabs { get; }
    
    [ObservableProperty]
    private ViewModelBase _selectedTab;

    public MainViewModel()
    {
        var stopwatch = new StopWatchViewModel();
        var timer = new TimerViewModel();
        
        Tabs = new ObservableCollection<ViewModelBase>{stopwatch, timer};
        SelectedTab = stopwatch;
    }
}