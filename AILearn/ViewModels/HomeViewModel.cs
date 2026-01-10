using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.GenAI;
using Google.GenAI.Types;

namespace AILearn.ViewModels;

public partial class HomeViewModel: ViewModelBase
{
    public List<string> CourseMajors { get; } = new() { "Computer Science", "Civil Engineering" };
    public List<string> Courses { get; } = new() { "CS101: Intro to Programming", "SE217: Software Development Lifecycle", "MTH101: Elementary Mathematics I", "MTH102: Elementary Mathematics II", "PHY101: General Physics I (Mechannics)" };
    public enum  ExamMode
    {
        Study, Practice
    }
    
    [ObservableProperty] private string _major = "Computer Science";
    [ObservableProperty] private string _course = "CS101: Intro to Programming";
    [ObservableProperty] private ExamMode _selectedMode = ExamMode.Practice;
    [ObservableProperty] private int _numberOfQuestions= 40;

    [RelayCommand]
    private void SetQuestionCount(int number)
    {
        NumberOfQuestions = number;
    }
    [ObservableProperty] private int _durationOfExam = 30;

    [RelayCommand]
    private void SetExamDuration(int duration)
    {
        DurationOfExam = duration;
    }

    [ObservableProperty] private string? _examQuestions;

    [ObservableProperty] private bool _isBusy;

    [RelayCommand]
    private async Task GenerateExam()
    {
        try
        {
            IsBusy = true;

            var prompt = $"generate a {NumberOfQuestions} question exam for me in json format." +
                         $"The exam should be about the major {Major} specifically the course {Course}." +
                         $"The json should have these fields; examMode (with the value {SelectedMode})," +
                         $" total number of questions (which should be {NumberOfQuestions}), duration (which should be {DurationOfExam})," +
                         " and the questions field that contains the array of questions, the questions should be as follows; the questionNumber " +
                         " (i.e the question's index), the questionText (i.e the question to be answered), the options field containing an array of options," +
                         " each option should have the optionsText (i.e the option itself), 'correct' field (showing if it is the correct option or not, i.e a boolean)," +
                         " the options don't need to be named A, B, C etc. Respond ONLY with valid JSON. Do not include markdown formatting or backticks like ```json.";

            var client = new Client();
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash", contents: prompt
            );
            ExamQuestions = response.Candidates?[0].Content?.Parts?[0].Text;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error generating exam: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}