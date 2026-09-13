using System;
using System.Threading;

public class LevelLoader
{
  public void LoadLevel(string levelName, Action onLevelLoading, Action<string> onLevelLoaded)
  {
    onLevelLoading?.Invoke();
    Thread.Sleep(1000);
    onLevelLoaded?.Invoke(levelName);
  }
}

public class HUD
{
  public void ShowHUD(string levelName)
  {
    Console.WriteLine("HUD is now visible.");
    Console.WriteLine($"level: {levelName}");
  }

  public void ShowLoadingScreen()
  {
    Console.WriteLine("Loading screen is now visible.");
  }
}

public class Game
{
  public static void Main()
  {
    LevelLoader levelLoader = new LevelLoader();
    HUD hud = new HUD();

    levelLoader.LoadLevel("Level1", hud.ShowLoadingScreen, hud.ShowHUD);
  }
}