using System;
using System.Threading;

public class LevelLoader
{
  public void LoadLevel(string levelName, Action onLevelLoading, Action onLevelLoaded)
  {
    onLevelLoading?.Invoke();
    Thread.Sleep(1000);
    Console.WriteLine($"Loaded level: {levelName}");
    onLevelLoaded?.Invoke();
  }
}

public class HUD
{
  public void ShowHUD()
  {
    Console.WriteLine("HUD is now visible.");
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