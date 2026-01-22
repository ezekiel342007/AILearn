using System.Collections.Generic;
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
    
    [ObservableProperty] private int _timeTaken = 0;
    [ObservableProperty] private string _scoreRemark;

    public string ScorePercentText => $"{Score}%";

    public void CalculateScore(List<ExamQuestion> script)
    {
        for (int i = 0; i < script.Count; i++)
        {
            var selectedIndex = script[i].SelectedOptionIndex;
            if (script[i].Options[selectedIndex] == script[i].Answer)
            {
                Score++;
            }
        }

        ScorePercent = (Score / script.Count) * 100;
        if (ScorePercent >= 50)
            ScoreRemark = "Passed";
    }
}