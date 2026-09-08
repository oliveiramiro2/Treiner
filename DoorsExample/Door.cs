using System;

public interface ILever
{
  event Action<bool> OnLeverPulled;
  void PullLever();
}

public class Lever : ILever
{
  public event Action<bool> OnLeverPulled;
  private bool isPulled = false;

  public void PullLever()
  {
    isPulled = !isPulled;
    OnLeverPulled?.Invoke(isPulled);
  }
}

public class Door
{
  private bool isOpen = false;
  private readonly ILever lever;


  public Door(ILever lever)
  {
    this.lever = lever;
    lever.OnLeverPulled += HandleLeverPulled;
  }

  private void HandleLeverPulled(bool isPulled)
  {
    if (isPulled)
    {
      OpenDoor();
    }
    else
    {
      CloseDoor();
    }
  }

  public void Unsubscribe()
  {
    lever.OnLeverPulled -= HandleLeverPulled;
  }

  private void OpenDoor()
  {
    isOpen = true;
    Console.WriteLine("Door is now open.");
  }

  private void CloseDoor()
  {
    isOpen = false;
    Console.WriteLine("Door is now closed.");
  }

  public void CheckDoorStatus()
  {
    if (isOpen)
    {
      Console.WriteLine("The door is open.");
    }
    else
    {
      Console.WriteLine("The door is closed.");
    }
  }
}

public class Program
{
  public static void Main(string[] args)
  {
    ILever lever = new Lever();
    Door door = new Door(lever);

    lever.PullLever();
    door.CheckDoorStatus();

    lever.PullLever();
    door.CheckDoorStatus();


    lever.PullLever();
    door.CheckDoorStatus();

    door.Unsubscribe();

    lever.PullLever();
    door.CheckDoorStatus();
  }
}

