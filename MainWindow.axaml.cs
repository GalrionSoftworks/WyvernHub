using SukiUI.Controls;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace WyvernHub;

public partial class MainWindow : SukiWindow
{
  public MainWindow()
  {
    InitializeComponent();
    DataContext = new MainWindowViewModel(new SukiToastManager(), new SukiDialogManager());
  }
}