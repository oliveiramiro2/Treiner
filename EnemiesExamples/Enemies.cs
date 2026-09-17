public interface IBehavior
{
  void Execute();
}

public class ChaseBehavior : IBehavior
{
  private readonly string target;
  private readonly float speed;
  private readonly float distance;

  public ChaseBehavior(string target, float speed, float distance)
  {
    this.target = target;
    this.speed = speed;
    this.distance = distance;
  }

  public void Execute()
  {
    Console.WriteLine($"Chasing {target} at speed {speed} and distance {distance}");
  }
}

public class FleeBehavior : IBehavior
{
  private readonly string target;
  private readonly float speed;
  private readonly float distance;

  public FleeBehavior(string target, float speed, float distance)
  {
    this.target = target;
    this.speed = speed;
    this.distance = distance;
  }

  public void Execute()
  {
    Console.WriteLine($"Fleeing from {target} at speed {speed} and distance {distance}");
  }
}

public class PatrolBehavior : IBehavior
{
  private readonly int points;
  private readonly string direction;
  private readonly float speed;

  public PatrolBehavior(int points, string direction, float speed)
  {
    this.points = points;
    this.direction = direction;
    this.speed = speed;
  }

  public void Execute()
  {
    Console.WriteLine($"Patrolling {direction} at speed {speed} through {points} points");
  }
}

public interface IDecisionMaker
{
  IBehavior DecideBehavior();
}

public class DecisionMaker : IDecisionMaker
{
  private int life;
  private float distance;

  public DecisionMaker(float distance, int life)
  {
    Update(distance, life);
  }

  public void Update(float distance, int life)
  {
    this.distance = distance;
    this.life = life;
  }

  public IBehavior DecideBehavior()
  {
    if (distance < 5 && life < 20)
    {
      return new FleeBehavior("Player", 10f, distance);
    }
    else if (distance < 15)
    {
      return new ChaseBehavior("Player", 15f, distance);
    }
    else
    {
      return new PatrolBehavior(5, "East", 5f);
    }
  }
}

public class BehaviorExecutor
{
  private IBehavior currentBehavior;

  public BehaviorExecutor(IBehavior behavior)
  {
    currentBehavior = behavior;
  }

  public void SetBehavior(IBehavior newBehavior)
  {
    currentBehavior = newBehavior;
  }

  public void ExecuteBehavior()
  {
    currentBehavior.Execute();
  }
}

public class EnemyGoblin
{
  private BehaviorExecutor behaviorExecutor;
  private DecisionMaker decisionMaker;

  public EnemyGoblin(float distance, int life)
  {
    decisionMaker = new DecisionMaker(distance, life);
    behaviorExecutor = new BehaviorExecutor(decisionMaker.DecideBehavior());
  }

  public void Update(float distance, int life)
  {
    decisionMaker.Update(distance, life);
    behaviorExecutor.SetBehavior(decisionMaker.DecideBehavior());
  }

  public void PerformAction()
  {
    behaviorExecutor.ExecuteBehavior();
  }
}

public class Game
{
  public static void Main()
  {
    EnemyGoblin goblin = new EnemyGoblin(10f, 30);
    goblin.PerformAction(); // Initial behavior based on distance and life

    // Simulate changes in distance and life
    goblin.Update(3f, 15); // Close distance and low life
    goblin.PerformAction(); // Should trigger flee behavior

    goblin.Update(12f, 25); // Medium distance and moderate life
    goblin.PerformAction(); // Should trigger chase behavior

    goblin.Update(20f, 50); // Far distance and high life
    goblin.PerformAction(); // Should trigger patrol behavior
  }
}