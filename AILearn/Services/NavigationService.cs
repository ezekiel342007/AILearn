using AILearn.ViewModels;

namespace AILearn.Services;

public class NavigationService
{
   public static NavigationService Instance { get; } = new();
   
   public MainViewModel? Main { get; set; }

   public void NavigateTo(ViewModelBase viewModel)
   {
      if (Main != null)
      {
         Main.CurrentPage = viewModel;
      }
      else
      {
         System.Diagnostics.Debug.WriteLine(">>> [ERROR] NavigationService: Main is null");
      }
   }
}