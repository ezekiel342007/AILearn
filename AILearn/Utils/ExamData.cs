using System.Collections.Generic;
using AILearn.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AILearn.Utils;
public partial class ExamQuestion: ViewModelBase
{
    public int QuestionNumber { get; set; }
    public string QuestionText { get; set; }
    public bool Answered { get; set; }
    public List<string> Options { get; set; }
    public string Answer { get; set; }
    [ObservableProperty]
    private int _selectedOptionIndex = -1;

    partial void OnSelectedOptionIndexChanged(int value)
    {
        if (value != -1 && !Answered)
        {
            Answered = true;
            WeakReferenceMessenger.Default.Send(new QuestionAnsweredMessage());
        }
    }
}

public partial class ExamData: ViewModelBase
{
    public string SelectedMode { get; set; }
    public int NumberOfQuestions { get; set; }
    public int Duration { get; set; }
    public List<ExamQuestion> Questions { get; set; }
}