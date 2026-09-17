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

  public EnemyGoblin(BehaviorExecutor behaviorExecutor, DecisionMaker decisionMaker)
  {
    this.behaviorExecutor = behaviorExecutor;
    this.decisionMaker = decisionMaker;
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

public class EnemyFactory
{
  public EnemyGoblin CreateEnemyGoblin(float distance, int life)
  {
    DecisionMaker decisionMaker = new DecisionMaker(distance, life);
    BehaviorExecutor behaviorExecutor = new BehaviorExecutor(decisionMaker.DecideBehavior());
    return new EnemyGoblin(behaviorExecutor, decisionMaker);
  }
}

public class Game
{
  public static void Main()
  {
    EnemyFactory factory = new EnemyFactory();
    EnemyGoblin goblin1 = factory.CreateEnemyGoblin(10f, 30);
    EnemyGoblin goblin2 = factory.CreateEnemyGoblin(15f, 25);

    goblin1.PerformAction(); // Initial action based on distance and life
    goblin2.PerformAction(); // Initial action based on distance and life

    // Simulate game updates
    goblin1.Update(3f, 10); // Update with new distance and life
    goblin1.PerformAction(); // Perform action after update

    goblin2.Update(12f, 15); // Update with new distance and life
    goblin2.PerformAction(); // Perform action after update

    goblin1.Update(20f, 50); // Update with new distance and life
    goblin1.PerformAction(); // Perform action after update

    goblin2.Update(5f, 5); // Update with new distance and life
    goblin2.PerformAction(); // Perform action after update
  }
}