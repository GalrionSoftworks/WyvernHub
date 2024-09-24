using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using Serilog;

namespace WyvernHub.Models;

public class DirectoryScanner
{
  public static DirectoryInfo? ScanDirectory(string path)
  {
    if (!Directory.Exists(path))
    {
      Log.Information("The directory does not exist.");
      return null;
    }

    Log.Information($"Scanning directory: {path}");
    // Get all directories and files recursively
    var directoryInfo = new DirectoryInfo(path);
    try
    {
      // Report all folders
      foreach (var dir in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
      {
        Log.Information($"[Folder] {dir}");
        var subDirectoryInfo = ScanDirectory(dir);
        if (subDirectoryInfo != null)
        {
          directoryInfo.AddDirectory(subDirectoryInfo);
        }
      }

      // Report all files
      foreach (var file in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
      {
        var fileInfo = new FileInfo(file);
        directoryInfo.AddFile(fileInfo);
        Log.Information($"[File] {file} (Size: {fileInfo.Length} bytes)");
      }
      
      return directoryInfo;
    }
    catch (Exception ex)
    {
      Log.Information($"Error while scanning the directory: {ex.Message}");
    }
    return null;
  }
  
  public static string? FindSpecificFolder(DirectoryInfo directoryInfo, string targetFolderPath)
  {
    // Normalize the target folder path for comparison
    var targetFolderName = Path.GetFileName(targetFolderPath);

    // Check if the current directory matches the target folder name
    if (directoryInfo.Path.EndsWith(targetFolderPath))
    {
      Log.Information($"Found the target folder: {directoryInfo.Path}");
      return directoryInfo.Path;
    }

    // Recursively search in subdirectories
    foreach (var subDirectory in directoryInfo.Directories)
    {
      var foundPath = FindSpecificFolder(subDirectory, targetFolderPath);
      if (foundPath != null)
      {
        return foundPath;
      }
    }

    // Return null if not found
    return null;
  }
  
  public static string? FindPaksFolder(DirectoryInfo directoryInfo, string targetFolderPath)
  {
    if (directoryInfo.Path.EndsWith(targetFolderPath))
    {
      Log.Information($"Found the Paks folder: {directoryInfo.Path}");
      return directoryInfo.Path;
    }

    // Recursively search subdirectories
    foreach (var subDirectory in directoryInfo.Directories)
    {
      var foundPath = FindPaksFolder(subDirectory, targetFolderPath);
      if (foundPath != null)
      {
        return foundPath;
      }
    }

    return null; // If not found, return null
  }

  // Checks if the "~mods" folder exists in the "Paks" folder, creates it if it doesn't
  public static void EnsureModsFolderExists(string paksFolderPath)
  {
    var modsFolderPath = Path.Combine(paksFolderPath, "~mods");

    if (!Directory.Exists(modsFolderPath))
    {
      Directory.CreateDirectory(modsFolderPath);
      Log.Information($"Created the '~mods' folder at: {modsFolderPath}");
    }
    else
    {
      Log.Information($"The '~mods' folder already exists at: {modsFolderPath}");
    }
  }

  // Retrieves the contents of the "~mods" folder
  public static DirectoryInfo? GetModsFolderContents(string paksFolderPath)
  {
    var modsFolderPath = Path.Combine(paksFolderPath, "~mods");

    if (Directory.Exists(modsFolderPath))
    {
      return ScanDirectory(modsFolderPath);
    }
    else
    {
      Log.Information("The '~mods' folder does not exist.");
      return null;
    }
  }
}

public class DirectoryInfo
{
  public string Path { get; set; }
  
  public string Name => System.IO.Path.GetFileName(Path);
  public ObservableCollection<DirectoryInfo> Directories { get; set; }
  public ObservableCollection<FileInfo> Files { get; set; }
  
  public ObservableCollection<object> Children 
  {
    get
    {
      var children = new ObservableCollection<object>();
      foreach (var directory in Directories)
      {
        children.Add(directory);
      }
      foreach (var file in Files)
      {
        children.Add(file);
      }
      return children;
    }
  }
  
  public DirectoryInfo(string path)
  {
    Path = path;
    Directories = new ObservableCollection<DirectoryInfo>();
    Files = new ObservableCollection<FileInfo>();
  }
        
  public void AddDirectory(DirectoryInfo directoryInfo)
  {
    Directories.Add(directoryInfo);
  }
        
  public void AddFile(FileInfo fileInfo)
  {
    Files.Add(fileInfo);
  }
}