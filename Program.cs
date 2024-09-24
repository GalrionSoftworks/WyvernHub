using Avalonia;
using System;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using Serilog;

namespace WyvernHub;

class Program
{
  // Initialization code. Don't use any Avalonia, third-party APIs or any
  // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
  // yet and stuff might break.
  [STAThread]
  public static void Main(string[] args)
  {
    // Step 1: Set up Serilog as the logging provider
    Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Information()  // Log level
      .WriteTo.Console()           // Output logs to the console
      .CreateLogger();

    try
    {
      Log.Information("Starting WyvernHub...");

      // Step 2: Build and run the Avalonia app
      BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }
    catch (Exception ex)
    {
      Log.Fatal(ex, "Application terminated unexpectedly");
      throw;
    }
    finally
    {
      Log.CloseAndFlush();  // Ensure logs are flushed
    }
  }

  // Avalonia configuration, don't remove; also used by visual designer.
  public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
      .UseReactiveUI() // Use ReactiveUI
      .UsePlatformDetect()
      .WithInterFont()
      .LogToTrace();
}