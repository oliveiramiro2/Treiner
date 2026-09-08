using System;

public interface EntityHealth
{
  int CurrentHealth { get; }
  int MaxHealth { get; }
  event Action<int, int> OnHealthChanged;
  public void TakeDamage(int damage);
}

public class Entity : EntityHealth
{
  private int currentHealth;
  private int maxHealth;

  public int CurrentHealth => currentHealth;
  public int MaxHealth => maxHealth;
  private bool IsDead => currentHealth <= 0;

  public event Action<int, int> OnHealthChanged;

  public Entity(int maxHealth)
  {
    this.maxHealth = maxHealth;
    currentHealth = maxHealth;
  }

  public void TakeDamage(int damage)
  {
    if (IsDead)
    {
      return;
    }

    currentHealth -= damage;
    if (currentHealth < 0)
    {
      currentHealth = 0;
    }

    OnHealthChanged?.Invoke(currentHealth, maxHealth);
  }
}

public class Boss1(int maxHealth) : Entity(maxHealth)
{
}

public class Boss2(int maxHealth) : Entity(maxHealth)
{
}

public class HUD
{
  EntityHealth entity;

  public HUD(EntityHealth entity)
  {
    this.entity = entity;
    Subscribe();
  }

  public void Subscribe()
  {
    entity.OnHealthChanged += UpdateHealthBar;
  }

  public void Unsubscribe()
  {
    entity.OnHealthChanged -= UpdateHealthBar;
  }

  private void UpdateHealthBar(int currentHealth, int maxHealth)
  {
    if (currentHealth <= 0)
    {
      Console.WriteLine($"Entity is Dead");
      Unsubscribe();
      return;
    }
    Console.WriteLine($"Health: {currentHealth}/{maxHealth}");
  }
}

public class AudioSystem
{
  EntityHealth entity;

  public AudioSystem(EntityHealth entity)
  {
    this.entity = entity;
    Subscribe();
  }

  public void Subscribe()
  {
    entity.OnHealthChanged += PlayDamageSound;
  }

  public void Unsubscribe()
  {
    entity.OnHealthChanged -= PlayDamageSound;
  }

  private void PlayDamageSound(int currentHealth, int maxHealth)
  {
    if (currentHealth <= 0)
    {
      Unsubscribe();
      return;
    }
    Console.WriteLine($"Playing damage sound");
  }
}

public class Game
{
  public static void Main()
  {
    Entity boss1 = new Boss1(500);
    Entity boss2 = new Boss2(300);

    HUD hud = new HUD(boss1);
    AudioSystem audioSystem = new AudioSystem(boss2);

    boss1.TakeDamage(100);
    boss1.TakeDamage(200);
    boss1.TakeDamage(250);
    boss1.TakeDamage(50);


    boss2.TakeDamage(50);
    boss2.TakeDamage(200);
    boss2.TakeDamage(60);
  }
}