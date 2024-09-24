using System;
using System.IO;
using System.Text.Json;

namespace WyvernHub.Models;

public static class SettingsManager
{
  private static string _settingsFilePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
    "WyvernHub", "settings.json"
  );

  public static void SaveSettings(WyvernHubSettings settings)
  {
    var jsonString = JsonSerializer.Serialize(settings);
        
    // Ensure directory exists
    Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath) ?? throw new InvalidOperationException());
        
    File.WriteAllText(_settingsFilePath, jsonString);
  }

  public static WyvernHubSettings LoadSettings()
  {
    if (!File.Exists(_settingsFilePath)) return new WyvernHubSettings();
    var jsonString = File.ReadAllText(_settingsFilePath);
    return JsonSerializer.Deserialize<WyvernHubSettings>(jsonString) ?? throw new InvalidOperationException();
  }
}
