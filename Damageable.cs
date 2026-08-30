using System;

public interface IDamageable
{
  void TakeDamage(int damage);
}

class Damageable(int initialHealth) : IDamageable
{
  private int health = initialHealth;

  protected bool IsDead()
  {
    return health <= 0;
  }

  protected bool ValidValue(int damage)
  {
    return damage > 0;
  }

  public virtual void TakeDamage(int damage)
  {
    if (!ValidValue(damage))
    {
      Console.WriteLine("Damage must be greater than 0 to apply damage");
      return;
    }

    if (IsDead())
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
  public GoblinEnemy() : base(100) { }
}

class BossEnemy : Damageable
{
  private int shield = 100;

  public BossEnemy() : base(500) { }

  private int TakeDamageWithShield(int damage)
  {

    if (IsDead())
    {
      Console.WriteLine($"{this.GetType().Name} is Dead");
      return 0;
    }

    if (!ValidValue(damage))
    {
      Console.WriteLine("Damage must be greater than 0 to apply damage");
      return 0;
    }

    if (shield > 0)
    {
      shield -= damage;
      if (shield <= 0)
      {
        int remainingShield = shield;
        shield = 0;
        return Math.Abs(remainingShield);
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
    int remainingDamage = TakeDamageWithShield(damage);

    if (remainingDamage > 0)
    {
      base.TakeDamage(remainingDamage);
    }
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

    // invalid damage values
    ApplyDamage(goblin, -10);
    ApplyDamage(boss, -10);
    ApplyDamage(goblin, 0);
    ApplyDamage(boss, 0);

    // valid damage values
    ApplyDamage(goblin, 30);
    ApplyDamage(goblin, 80);
    ApplyDamage(goblin, 10);
    ApplyDamage(boss, 50);
    ApplyDamage(boss, 100);
    ApplyDamage(boss, 400);
    ApplyDamage(boss, 150);
  }
}