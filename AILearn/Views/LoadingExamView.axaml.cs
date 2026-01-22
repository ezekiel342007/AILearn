using AILearn.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AILearn.Views;

public partial class LoadingExamView : UserControl
{
    public LoadingExamView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is LoadingExamViewModel viewModel)
        {
           viewModel.GenerateExam();
        }
    }
}