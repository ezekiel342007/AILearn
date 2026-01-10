using CommunityToolkit.Mvvm.ComponentModel;

namespace AILearn.ViewModels;

public partial class ExamViewModel: ViewModelBase
{
    [ObservableProperty] private int _questionCount;
    [ObservableProperty] private int _questionNumber;
    [ObservableProperty] private int _precentComplete;
    
    [ObservableProperty] private string _questionText = "Which Gestalt design principle states that elements that are close together are perceived to be more related than elements that are further apart?";
    [ObservableProperty] private string _remainingTime = "";
    
    [ObservableProperty] private string _firstOption = "Law of Closure";
    [ObservableProperty] private string _secondOption = "Law of Proximity";
    [ObservableProperty] private string _thirdOption = "Law of Similarity";
    [ObservableProperty] private string _fourthOption = "Law of Common Region";
    [ObservableProperty] private string _correctOption = "";
    [ObservableProperty] private string _selectedOption = "";
}