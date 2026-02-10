using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AILearn.Models;
using AILearn.Services;
using AILearn.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AILearn.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
   // public List<string> CourseMajors { get; } = new() { "Computer Science", "Civil Engineering" };
    //public List<string> Courses { get; } = new() { "CS101: Intro to Programming", "SE217: Software Development Lifecycle", "MTH101: Elementary Mathematics I", "MTH102: Elementary Mathematics II", "PHY101: General Physics I (Mechannics)" };
    
    private readonly MajorService _majorService;

    // The list of Majors for the first dropdown
    [ObservableProperty] 
    private ObservableCollection<Major> _majors = new();

    // The selected Major
    [ObservableProperty] 
    private Major? _selectedMajor;

    // The list of Courses (updates automatically when Major changes)
    [ObservableProperty] 
    private ObservableCollection<string> _courses = new();

    [ObservableProperty] 
    private string? _selectedCourse;
    
    private async void LoadData()
    {
        try 
        {
            // Ensure the service isn't null (though constructor fixes this)
            if (_majorService == null) return;

            var data = await _majorService.GetMajorsAsync();
            
            // UI Update
            Majors = new ObservableCollection<Major>(data);
            
            // Debug check to confirm data loaded
            Debug.WriteLine($">>> [DEBUG] Loaded {Majors.Count} majors.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($">>> [ERROR] Failed to load majors: {ex.Message}");
        }
    }
    
    // Magic Hook: When user picks a Major, update the Courses list
    partial void OnSelectedMajorChanged(Major? value)
    {
        Courses.Clear();
        SelectedCourse = null; // Reset selection

        if (value != null)
        {
            foreach (var course in value.Courses)
            {
                Courses.Add(course);
            }
        }
    }

    private readonly MainViewModel _main;

    public HomeViewModel()
    {
        _majorService = new MajorService();
        LoadData();
    }

    public HomeViewModel(MainViewModel main): this()
    {
        
        _main = main;
    }

    [ObservableProperty] private ExamMode _selectedMode = ExamMode.Practice;
    [ObservableProperty] private int _numberOfQuestions= 40;

    [RelayCommand]
    public void SetQuestionCount(int number)
    {
        NumberOfQuestions = number;
    }
    [ObservableProperty] private int _durationOfExam = 30;

    [RelayCommand]
    public void SetExamDuration(int duration)
    {
        DurationOfExam = duration;
    }

    [ObservableProperty] private string? _examQuestions;

    [RelayCommand]
    public void StartLoading()
    {
        Debug.WriteLine($">>> [DEBUG] Started loading process");
        AILearn.Services.NavigationService.Instance.ToggleNav(false);
        Debug.WriteLine($">>> [DEBUG] Toggled nav successfully");
        var examQueryData = new ExamQuery
        {
            Course = SelectedCourse, 
            Duration = DurationOfExam, 
            Major = SelectedMajor.Title, 
            NumberOfQuestions = NumberOfQuestions,
            SelectedMode = SelectedMode.ToString()
        };
        Debug.WriteLine($">>> [DEBUG] Created ExamQuery successfully");
        LoadingExamViewModel loadingExamsPage = new LoadingExamViewModel { examQueryData = examQueryData};
        Debug.WriteLine($">>> [DEBUG] Created LoadingPage successfully");
        try
        {

            AILearn.Services.NavigationService.Instance.NavigateTo(loadingExamsPage);
            Debug.WriteLine($">>> [DEBUG] Started transition to loading page");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($">>> [ERROR] Encoutered error while transitioning: {ex.Message}");
        }
    }
}