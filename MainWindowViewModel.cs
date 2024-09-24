using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace WyvernHub;

public class MainWindowViewModel(ISukiToastManager toastManager, ISukiDialogManager dialogManager)
{
    public ISukiToastManager ToastManager { get; } = toastManager;
    public ISukiDialogManager DialogManager { get; } = dialogManager;
}