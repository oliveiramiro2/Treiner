using System;
using System.Threading;

public enum Level
{
  Forest,
  Cave,
  Bridge
}

public class LevelLoader
{
  public void LoadLevel(Level levelName, Func<Level, bool> canLoadLevel, Action onLevelLoading, Action<Level> onLevelLoaded)
  {
    if (!canLoadLevel(levelName))
    {
      Console.WriteLine("Cannot load this level.");
      return;
    }

    onLevelLoading?.Invoke();
    Thread.Sleep(1000);
    onLevelLoaded?.Invoke(levelName);
  }
}

public class HUD
{
  public void ShowHUD(Level levelName)
  {
    Console.WriteLine("HUD is now visible.");
    Console.WriteLine($"level: {levelName} ");
  }

  public void ShowLoadingScreen()
  {
    Console.WriteLine("Loading screen is now visible.");
  }
}

public class Game
{
  private static bool CanLoadLevel(Level level)
  {
    return level switch
    {
      Level.Forest => true,
      Level.Cave => false,
      Level.Bridge => true,
      _ => false
    };
  }

  public static void Main()
  {
    LevelLoader levelLoader = new LevelLoader();
    HUD hud = new HUD();


    levelLoader.LoadLevel(Level.Cave, Game.CanLoadLevel, hud.ShowLoadingScreen, hud.ShowHUD); // can't load level
    levelLoader.LoadLevel(Level.Forest, Game.CanLoadLevel, hud.ShowLoadingScreen, hud.ShowHUD); // can load level
  }
}