using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaChronos.ViewModels;
using AvaloniaChronos.Views;

namespace AvaloniaChronos;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        // Explicit mapping: The compiler now "sees" these classes are used.
        return param switch
        {
            MainViewModel vm => new MainView(),
            TimerViewModel vm => new TimerView(),
            StopWatchViewModel vm => new StopWatchView(),
            // Add all your tab ViewModels here
            
            _ => new TextBlock { Text = $"Not Found: {param.GetType().FullName}" }
        };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}