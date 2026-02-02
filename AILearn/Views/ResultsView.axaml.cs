using System.Diagnostics;
using AILearn.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AILearn.Views;

public partial class ResultsView : UserControl
{
    public ResultsView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Debug.WriteLine($">>> [DEBUG] Entered ResultsView");
        if (DataContext is ResultsViewModel viewModel)
        {
            viewModel.CalculateScore(viewModel.ExamData.Questions);
        }
    }
}