using System;
using System.Collections.Generic;
using System.Diagnostics;
using AILearn.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AILearn.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public List<string> CourseMajors { get; } = new() { "Computer Science", "Civil Engineering" };
    public List<string> Courses { get; } = new() { "CS101: Intro to Programming", "SE217: Software Development Lifecycle", "MTH101: Elementary Mathematics I", "MTH102: Elementary Mathematics II", "PHY101: General Physics I (Mechannics)" };


    private readonly MainViewModel _main;
    public HomeViewModel() {}
    public HomeViewModel(MainViewModel main)
    {
        _main = main;
    }

    [ObservableProperty] private string _major = "Computer Science";
    [ObservableProperty] private string _course = "CS101: Intro to Programming";
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
            Course = Course, 
            Duration = DurationOfExam, 
            Major = Major, 
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