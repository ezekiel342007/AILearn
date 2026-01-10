using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AILearn.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;

    private readonly HomeViewModel _homePage = new();
    private readonly ResultsViewModel _resultsPage = new();

    [RelayCommand]
    public void Navigate(string pageName)
    {
        CurrentPage = pageName switch
        {
            "Home" => _homePage,
            "Results" => _resultsPage,
            _ => _homePage
        };
    }
}

