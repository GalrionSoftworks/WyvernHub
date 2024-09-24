using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using ReactiveUI.Fody.Helpers;  // For Reactive Properties
using Avalonia.ReactiveUI;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;
using Serilog;
using WyvernHub.Models;

namespace WyvernHub
{
    public partial class MainWindowViewModel : ReactiveObject
    {
        public ISukiToastManager ToastManager { get; }
        public ISukiDialogManager DialogManager { get; }

        public SukiWindow Parent { get; set; }

        public string Title => "WyvernHub";

        // The WyvernHubSettings object is reactive, so we don't need additional properties for PayDay2 and PayDay3
        private WyvernHubSettings _settings;
        
        [Reactive]
        public WyvernHubSettings Settings
        {
            get { return _settings; }
            set { this.RaiseAndSetIfChanged(ref _settings, value); }
        }

        public MainWindowViewModel(ISukiToastManager toastManager, ISukiDialogManager dialogManager, SukiWindow parent)
        {
            ToastManager = toastManager;
            DialogManager = dialogManager;
            Parent = parent;

            Settings = SettingsManager.LoadSettings();
        }

        [RelayCommand]
        public void SaveSettings()
        {
            SettingsManager.SaveSettings(Settings);
        }

        [RelayCommand]
        private async Task BrowsePayDay2Async()
        {
            Log.Information("BrowsePayDay2Command executed!");

            if (Parent == null)
            {
                Log.Error("BrowsePayDay2Command Parent window is null!");
                return;
            }

            Log.Information("BrowsePayDay2Command Parent window is valid, opening folder picker...");

            var result = await Parent.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select PAYDAY 2 Folder",
                AllowMultiple = false
            });

            if (result != null && result.Count > 0)
            {
                // Directly modify the Settings object
                Settings.PayDay2Directory = result[0].Path.LocalPath;
                Log.Information($"BrowsePayDay2Command Selected PAYDAY 2 directory: {Settings.PayDay2Directory}");
            }
            else
            {
                Log.Information("BrowsePayDay2Command No directory selected for PAYDAY 2.");
            }

            // Save the updated settings
            SaveSettings();
        }

        [RelayCommand]
        private async Task BrowsePayDay3Async()
        {
            Log.Information("BrowsePayDay3Command executed!");

            if (Parent == null)
            {
                Log.Error("Parent window is null!");
                return;
            }

            Log.Information("Parent window is valid, opening folder picker...");

            var result = await Parent.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select PAYDAY 3 Folder",
                AllowMultiple = false
            });

            if (result != null && result.Count > 0)
            {
                // Directly modify the Settings object
                Settings.PayDay3Directory = result[0].Path.LocalPath;
                Log.Information($"Selected PAYDAY 3 directory: {Settings.PayDay3Directory}");
            }
            else
            {
                Log.Information("BrowsePayDay3Command No directory selected for PAYDAY 3.");
            }

            // Save the updated settings
            SaveSettings();
        }
    }
}
