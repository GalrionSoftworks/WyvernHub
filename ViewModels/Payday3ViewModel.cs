using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SukiUI.Controls;
using WyvernHub.Models;  // Import the DirectoryScanner and DirectoryInfo
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;

namespace WyvernHub.ViewModels
{
  public partial class Payday3ViewModel : ReactiveObject
  {
    public SukiWindow Parent { get; set; }
        
    // Bindable list of files in the ~mods folder
    [Reactive]
    public ObservableCollection<object> ModsFolderContents { get; set; }

    [Reactive]
    public string FoundPaksFolder { get; set; }

    public Payday3ViewModel(SukiWindow parent)
    {
      Parent = parent;
      ModsFolderContents = new ObservableCollection<object>();
      ScanPayday3DirectoryAsync();
    }

    // Command to scan for Payday 3 files and handle the mods folder
    [RelayCommand]
    public void ScanPayday3DirectoryAsync()
    {
      var path = SettingsManager.LoadSettings().PayDay3Directory;
      var directoryInfo = DirectoryScanner.ScanDirectory(path);

      if (directoryInfo != null)
      {
        // Search for the specific folder "PAYDAY3\\PAYDAY3\\Content\\Paks"
        FoundPaksFolder = DirectoryScanner.FindPaksFolder(directoryInfo, "PAYDAY3\\PAYDAY3\\Content\\Paks");

        if (FoundPaksFolder != null)
        {
          Log.Information($"Found the Paks folder at: {FoundPaksFolder}");

          // Ensure the ~mods folder exists
          DirectoryScanner.EnsureModsFolderExists(FoundPaksFolder);

          // Get the contents of the ~mods folder and bind them to the ObservableCollection
          var modsFiles = DirectoryScanner.GetModsFolderContents(FoundPaksFolder);
          if (modsFiles != null)
          {
            ModsFolderContents = new ObservableCollection<object>(modsFiles.Children);
          }
        }
        else
        {
          Log.Information("Could not find the Paks folder.");
        }
      }
      else
      {
        // Log or display an error if the directory scan failed
        Log.Information("Failed to scan the Payday 3 directory.");
      }
    }
  }
}
