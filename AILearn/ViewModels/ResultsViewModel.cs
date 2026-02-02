using System.Collections.Generic;
using System.Diagnostics;
using AILearn.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AILearn.ViewModels;

public partial class ResultsViewModel: ViewModelBase
{
    [ObservableProperty] private ExamData? _examData;
    [ObservableProperty] private int _score = 0;
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(ScorePercentText))]
    private int _scorePercent = 0;
    
    [ObservableProperty] private string _timeTaken = "";
    [ObservableProperty] private string? _scoreRemark;
    [ObservableProperty] private string? _insightText;
    [ObservableProperty] private int _correctAnswerCount = 0;
    [ObservableProperty] private int _wrongAnswerCount = 0;
    [ObservableProperty] private int _attemptedQuestionsCount = 0;

    public string ScorePercentText => $"{Score}%";

    public void CalculateScore(List<ExamQuestion> script)
    {
        Debug.WriteLine($">>> [DEBUG] Started Calculating Score");
        TimeTaken = ExamData.TimeTaken.ToString();
        for (int i = 0; i < script.Count; i++)
        {
            var selectedIndex = script[i].SelectedOptionIndex;

            if (selectedIndex < 0)
            {
                continue;
            }

            AttemptedQuestionsCount++;
            if (script[i].Options[selectedIndex] == script[i].Answer)
            {
                Score++;
                CorrectAnswerCount++;
            }
            else
            {
                WrongAnswerCount++;
            }
        }

        if (script.Count > 0)
        {
            ScorePercent = (int)(((double)Score / script.Count) * 100);
        }
        else
        {
            ScorePercent = 0;
        }

        if (ScorePercent >= 50)
        {
            ScoreRemark = "Passed";
            InsightText = "Great Job";
        }
        else
        {
            ScoreRemark = "Failed";
            InsightText = "Nice attempt";
        }
        Debug.WriteLine($">>> [SUCCESS] Finished calculating score");
    }
}