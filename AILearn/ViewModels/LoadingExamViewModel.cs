using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AILearn.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Google.GenAI;

namespace AILearn.ViewModels;

public partial class LoadingExamViewModel: ViewModelBase
{
    public ExamQuery examQueryData;
    private bool _isGenerationComplete = false;

    [ObservableProperty] private int _generationProgress = 0;
    [ObservableProperty] private string _waitingText = "Generating Questions...";
    public string GenerationProgressPercent => ($"{GenerationProgress}%");
    
    public async Task GenerateExam()
    {
        GenerationProgress = 0;
        var progressSimulation = SimulateProgressAsync();
        Debug.WriteLine(">>> [DEBUG] Starting GenerateExam process....... ");
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")?? throw new Exception("API Key Missing");
        Debug.WriteLine($">>> [DEBUG] Retrieved API Key: {apiKey}");
        AILearn.Services.NavigationService.Instance.ToggleNav(false);
        try
        {
            var prompt = $"generate a {examQueryData.NumberOfQuestions} question exam for me in json format." +
                         $"The exam should be about the major {examQueryData.Major} specifically the course {examQueryData.Course}." +
                         $"The json should have these fields; examMode (with the value {examQueryData.SelectedMode} as string)," +
                         $" numberOfQuestions (which should be {examQueryData.NumberOfQuestions}), duration (which should be {examQueryData.Duration})," +
                         " and the questions field that contains the array of questions, the questions should be as follows; the questionNumber " +
                         " an 'answered' field indicating whether the question has been anwered " +
                         " (i.e the question's index), the questionText (i.e the question to be answered), the options field containing an array of options which are strings,"+
                         " and an answer which is exact copy of the correct option's value (the string content)" +
                         " the options don't need to be named A, B, C etc. Respond ONLY with valid JSON. Do not include markdown formatting or backticks like ```json.";

            var client = new Client(apiKey: apiKey);
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
            Debug.WriteLine($">>> [Debug] Finish reason: {finishReason}");
            string jsonResult = response.Candidates?[0].Content?.Parts?[0].Text;
            _isGenerationComplete = true;
            GenerationProgress = 100;
            
            if (!string.IsNullOrEmpty(jsonResult))
            {
                Debug.WriteLine(">>> [DEBUG] Content successfully retrieved.");
                try
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    ExamData myExam = JsonSerializer.Deserialize<ExamData>(jsonResult, options);
                    await Task.Delay(500);
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
                Debug.WriteLine($">>> [Debug] AI returned an empty string");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($">>> [ERROR] Error generating exam: {ex.Message}");
        }
        finally
        {
            Debug.WriteLine(">>> [DEBUG] GenerateExam process finished.");
        }
    }

    private async Task SimulateProgressAsync()
    {
        _isGenerationComplete = false;
        while (!_isGenerationComplete && GenerationProgress < 90)
        {
            if (GenerationProgress > 70)
            {
                WaitingText = "Preparing Exam...";
            }
            GenerationProgress += Random.Shared.Next(1, 5);
            await Task.Delay(1000);
        }
    }
}