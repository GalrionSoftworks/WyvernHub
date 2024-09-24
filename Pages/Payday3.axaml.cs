using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using SukiUI.Controls;
using WyvernHub.ViewModels;

namespace WyvernHub.Pages;

public partial class Payday3 : UserControl
{
  public Payday3()
  {
    InitializeComponent();
    
    DataContext = new Payday3ViewModel(this.GetVisualAncestors().OfType<SukiWindow>().FirstOrDefault());
  }
}