using System;

public interface IDamageable
{
  void TakeDamage(int damage);
}

abstract class Damageable : IDamageable
{
  protected int health = 0;

  protected bool isDead()
  {
    if (health <= 0)
    {
      Console.WriteLine($"{this.GetType().Name} is Dead");
      return true;
    }
    return false;
  }

  public virtual void TakeDamage(int damage)
  {
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
    if (isDead())
    {
      return;
    }

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

  private int TakeDamageWithShield(int damage)
  {
    if (shield > 0)
    {
      shield -= damage;
      if (shield <= 0)
      {
        int aux = shield;
        shield = 0;
        return Math.Abs(aux);
      }
      else
      {
        Console.WriteLine($"{this.GetType().Name} shield: {shield}");
        return 0;
      }
    }
    return damage;
  }

  public override void TakeDamage(int damage)
  {
    if (isDead())
    {
      return;
    }

    int remainingDamage = TakeDamageWithShield(damage);
    Console.WriteLine($"remaining damage: {remainingDamage}");
    base.TakeDamage(remainingDamage);
  }
}

class Program
{
  static void ApplyDamage(IDamageable damageable, int damage)
  {
    damageable.TakeDamage(damage);
  }

  static void Main(string[] args)
  {
    GoblinEnemy goblin = new GoblinEnemy();
    BossEnemy boss = new BossEnemy();


    ApplyDamage(goblin, 30);
    ApplyDamage(goblin, 80);
    ApplyDamage(goblin, 10);
    ApplyDamage(boss, 50);
    ApplyDamage(boss, 100);
    ApplyDamage(boss, 400);
    ApplyDamage(boss, 150);
  }
}