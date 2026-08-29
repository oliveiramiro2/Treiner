using System;

public interface IDamageable
{
  void TakeDamage(int damage);
}

public class GoblinEnemy : IDamageable
{
  private int health = 100;

  public void TakeDamage(int damage)
  {
    if (health <= 0)
    {
      Console.WriteLine("Goblin is Dead");
      return;
    }

    health -= damage;
    if (health <= 0)
    {
      health = 0;
      Console.WriteLine("Goblin is Dead");
      return;
    }
    Console.WriteLine($"Goblin health: {health}");
  }
}

public class BossEnemy : IDamageable
{
  private int health = 500;
  private int shield = 100;

  public void TakeDamage(int damage)
  {

    if (health == 0)
    {
      Console.WriteLine("Boss is Dead");
      return;
    }

    if (shield > 0)
    {
      shield -= damage;
      if (shield < 0)
      {
        health += shield;
        shield = 0;
      }
      else
      {
        Console.WriteLine($"Boss shield: {shield}");
        return;
      }

      if (health <= 0)
      {
        health = 0;
        Console.WriteLine("Boss is Dead");
      }
      return;
    }

    health -= damage;
    if (health <= 0)
    {
      health = 0;
      Console.WriteLine("Boss is Dead");
      return;
    }
    Console.WriteLine($"Boss health: {health}");
  }
}

class Program
{
  static void ApplyDamage(IDamageable damageable, int damage)
  {
    damageable.TakeDamage(damage);
  }

  static void Main()
  {
    GoblinEnemy goblin = new GoblinEnemy();
    BossEnemy boss = new BossEnemy();

    ApplyDamage(goblin, 10);
    ApplyDamage(boss, 50);
  }
}