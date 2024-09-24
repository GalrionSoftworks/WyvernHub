using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace WyvernHub.Models;

public class WyvernHubSettings : ReactiveObject
{
  [Reactive]
  public string ModsDirectory { get; set; }
  [Reactive]
  public string PayDay2Directory { get; set; }
  [Reactive]
  public string PayDay3Directory { get; set; }
  [Reactive]
  public bool AutoUpdate { get; set; }
}