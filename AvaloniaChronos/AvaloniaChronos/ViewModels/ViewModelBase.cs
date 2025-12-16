using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaChronos.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    
    public virtual string Header { get; set; } 

    protected ViewModelBase()
    {
        Header = string.Empty;
    }
}