using System;

public interface IDamageable
{
  void TakeDamage(int damage);
}

abstract class Damageable : IDamageable
{
  protected int health = 0;

  public virtual void TakeDamage(int damage)
  {
    if (health <= 0)
    {
      Console.WriteLine($"{this.GetType().Name} is Dead");
      return;
    }

    health -= damage;
    if (health <= 0)
    {
      health = 0;
      Console.WriteLine($"{this.GetType().Name} is Dead");
      return;
    }
    Console.WriteLine($"{this.GetType().Name} health: {health}");
  }
}

class GoblinEnemy : Damageable
{
  public GoblinEnemy()
  {
    health = 100;
  }

  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
  }
}

class BossEnemy : Damageable
{
  private int shield = 100;

  public BossEnemy()
  {
    health = 500;
  }

  public override void TakeDamage(int damage)
  {
    if (health == 0)
    {
      Console.WriteLine($"{this.GetType().Name} is Dead");
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
        Console.WriteLine($"{this.GetType().Name} shield: {shield}");
        return;
      }

      if (health <= 0)
      {
        health = 0;
        Console.WriteLine($"{this.GetType().Name} is Dead");
        return;
      }
    }

    base.TakeDamage(damage);
  }
}

class Program
{
  static void Main(string[] args)
  {
    GoblinEnemy goblin = new GoblinEnemy();
    BossEnemy boss = new BossEnemy();


    goblin.TakeDamage(30);
    goblin.TakeDamage(80);
    boss.TakeDamage(50);
    boss.TakeDamage(100);
    boss.TakeDamage(400);
  }
}