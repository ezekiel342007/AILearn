using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AILearn.Utils;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Google.GenAI;

namespace AILearn.ViewModels;

public partial class LoadingExamViewModel: ViewModelBase
{
    public ExamQuery examQueryData;
    
    public async Task GenerateExam()
    {
        Debug.WriteLine(">>> [DEBUG] Starting GenerateExam process....... ");
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

            var client = new Client(apiKey:"AIzaSyBkjUFWmk8d5u54tr0iZBctQvSaDD6oXXk");
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
            
            if (!string.IsNullOrEmpty(jsonResult))
            {
                Debug.WriteLine(">>> [DEBUG] Content successfully retrieved.");
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
}