using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AILearn.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    public enum Tabs
    {
        Home, Results
    }

    [ObservableProperty] public Tabs _currentTab = Tabs.Home;
    [ObservableProperty] private ViewModelBase _currentPage;
    [ObservableProperty] private bool _isNotBusy = true;
    
    private readonly HomeViewModel _homePage;
    private readonly ResultsViewModel _resultsPage = new();
    private readonly ExamViewModel _examPage = new();

    public MainViewModel()
    {
        AILearn.Services.NavigationService.Instance.Main = this;
        CurrentPage = new HomeViewModel();
    }
    
    [RelayCommand]
    public void Navigate(string pageName)
    {
        switch (pageName)
        {
            case "Home":
                CurrentPage = _homePage;
                CurrentTab = Tabs.Home;
                break;
            case "Results":
                CurrentPage = _resultsPage;
                CurrentTab = Tabs.Results;
                break;
            default:
                CurrentPage = _homePage;
                CurrentTab = Tabs.Home;
                break;
        }
    }
}

