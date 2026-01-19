using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AILearn.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.GenAI;

namespace AILearn.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public List<string> CourseMajors { get; } = new() { "Computer Science", "Civil Engineering" };
    public List<string> Courses { get; } = new() { "CS101: Intro to Programming", "SE217: Software Development Lifecycle", "MTH101: Elementary Mathematics I", "MTH102: Elementary Mathematics II", "PHY101: General Physics I (Mechannics)" };
    public enum  ExamMode
    {
        Study, Practice
    }

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

    [ObservableProperty] private bool _isBusy;

    [RelayCommand]
    public async Task GenerateExam()
    {
        Debug.WriteLine(">>> [DEBUG] Starting GenerateExam process....... ");
        IsBusy = true;
        try
        {
            var prompt = $"generate a {NumberOfQuestions} question exam for me in json format." +
                         $"The exam should be about the major {Major} specifically the course {Course}." +
                         $"The json should have these fields; examMode (with the value {SelectedMode} as string)," +
                         $" numberOfQuestions (which should be {NumberOfQuestions}), duration (which should be {DurationOfExam})," +
                         " and the questions field that contains the array of questions, the questions should be as follows; the questionNumber " +
                         " an 'answered' field indicating whether the question has been anwered " +
                         " (i.e the question's index), the questionText (i.e the question to be answered), the options field containing an array of options which are strings,"+
                         " and an answer which is exact copy of the correct option's value (the string content)" +
                         " the options don't need to be named A, B, C etc. Respond ONLY with valid JSON. Do not include markdown formatting or backticks like ```json.";

            var client = new Client(apiKey:"AIzaSyCcDUQ4PA4JcoLW6vir7ekn5k9l8OIwhMw");
            var sw = Stopwatch.StartNew();
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash", contents: prompt
            );
            sw.Stop();
            Debug.WriteLine($">>> [Debug] AI Response received in {sw.ElapsedMilliseconds} milliseconds.");

            if (response.UsageMetadata != null)
            {
                Debug.WriteLine($">>> [Debug] Tokens used: Prompt={response.UsageMetadata.PromptTokenCount}, " +
                                $"Response={response.UsageMetadata.CandidatesTokenCount}");
            }

            var finishReason = response.Candidates?[0].FinishReason;
            Debug.WriteLine($">>>[Debug] Finish reason: {finishReason}");
            string jsonResult = response.Candidates?[0].Content?.Parts?[0].Text;
            
            if (!string.IsNullOrEmpty(jsonResult))
            {
                Debug.WriteLine(">>> [DEBUG] Content successfully retrieved.");
                if (_main == null)
                {
                    Debug.WriteLine(">>> [ERROR] _main is null Navigation will not work ");
                }

                try
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    ExamData myExam = JsonSerializer.Deserialize<ExamData>(jsonResult, options);
                    var examsPage = new ExamViewModel { ExamData = myExam };
                    AILearn.Services.NavigationService.Instance.NavigateTo(examsPage);
                }
                catch (JsonException ex)
                {
                    Debug.WriteLine($">>> [ERROR] Failed to parse JSON: {ex.Message}");
                }

            }
            else
            {
                Debug.WriteLine($">>>[Debug] AI returned an empty string");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error generating exam: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            Debug.WriteLine(">>> [DEBUG] GenerateExam process finished.");
        }
    }
}