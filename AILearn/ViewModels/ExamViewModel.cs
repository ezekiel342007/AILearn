using System;
using CommunityToolkit.Mvvm.ComponentModel;
using AILearn.Utils;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AILearn.ViewModels;

public partial class ExamViewModel: ViewModelBase
{

    public ExamViewModel()
    {
        WeakReferenceMessenger.Default.Register<QuestionAnsweredMessage>(this, (r, m) =>
        {
            AnsweredQuestions++;
        });
    }
    [ObservableProperty] private ExamData? _examData;

    partial void OnExamDataChanged(ExamData? value)
    {
        if (value != null)
        {
            StartTimer(value.Duration);
        }
    }
    
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(PercentComplete))]
    private int _answeredQuestions = 0;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(CurrentQuestion))]
    private int _currentQuestionIndex = 0;

    [ObservableProperty] 
    private string _timerText = "00:00:00";

    [ObservableProperty] private bool _isTimeCritical = false;

    private DispatcherTimer? _timer;
    private TimeSpan _timeRemaining;

    public ExamQuestion? CurrentQuestion =>
        (ExamData != null && CurrentQuestionIndex < ExamData.Questions.Count) ? ExamData.Questions[CurrentQuestionIndex] : null;

    public void StartTimer(int minutes)
    {
        if (ExamData == null) return;
        
        _timeRemaining = TimeSpan.FromMinutes(minutes);

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    public void Timer_Tick(object? sender, EventArgs e)
    {
        _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
        TimerText = _timeRemaining.ToString(@"hh\:mm\:ss");

        IsTimeCritical = _timeRemaining.TotalMinutes <= 5;

        if (_timeRemaining.TotalSeconds <= 0)
        {
            _timer.Stop();
            SubmitExam();
        }
    }
    
    public void StopTimer()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
            _timer = null;
        }
    }


    public void SubmitExam()
    {
        StopTimer();
    }

    public string PercentComplete
    {
        get
        {
            if (ExamData == null || ExamData.NumberOfQuestions == 0)
            {
                return "0%";
            }
            double percentage = (double)AnsweredQuestions / ExamData.NumberOfQuestions*100;

            return $"{percentage:0}%";
        }
    }
    
    [RelayCommand]
    public void NextQuestion()
    {
        if (CurrentQuestionIndex < ExamData?.NumberOfQuestions - 1)
        {
            ++CurrentQuestionIndex;
        }
    }
    
    [RelayCommand]
    public void PreviousQuestion()
    {
        if (CurrentQuestionIndex > 0)
        {
            --CurrentQuestionIndex;
        }
    }
}