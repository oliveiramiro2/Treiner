using System;

public interface EntityHealth
{
  int CurrentHealth { get; }
  int MaxHealth { get; }
  event Action<int, int> OnHealthChanged;
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
    if (maxHealth <= 0)
      throw new ArgumentOutOfRangeException(nameof(maxHealth));

    this.maxHealth = maxHealth;
    currentHealth = maxHealth;
  }

  public void TakeDamage(int damage)
  {
    if (IsDead || damage <= 0)
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

public class HealthNotificationSystem
{
  private readonly Dictionary<EntityHealth, List<Action<EntityHealth, int, int>>> subscribers = new();
  private readonly Dictionary<EntityHealth, Action<int, int>> handlers = new();

  public void Subscribe(
      EntityHealth entity,
      Action<EntityHealth, int, int> callback)
  {
    if (!subscribers.TryGetValue(entity, out var callbacks))
    {
      callbacks = new List<Action<EntityHealth, int, int>>();
      subscribers.Add(entity, callbacks);

      Action<int, int> handler = (current, max) => NotifySubscribers(entity, current, max);
      handlers.Add(entity, handler);
      entity.OnHealthChanged += handler;
    }

    callbacks.Add(callback);
  }

  public void Unsubscribe(
      EntityHealth entity,
      Action<EntityHealth, int, int> callback)
  {
    if (!subscribers.TryGetValue(entity, out var callbacks))
      return;

    callbacks.Remove(callback);

    if (callbacks.Count == 0)
    {
      if (handlers.TryGetValue(entity, out var handler))
      {
        entity.OnHealthChanged -= handler;
        handlers.Remove(entity);
      }
      subscribers.Remove(entity);
    }
  }

  private void NotifySubscribers(EntityHealth entity, int currentHealth, int maxHealth)
  {
    if (subscribers.TryGetValue(entity, out var callbacks))
    {
      var snapshot = callbacks.ToArray();

      foreach (var callback in snapshot)
      {
        callback(entity, currentHealth, maxHealth);
      }
    }
  }
}

public class HUD
{

  public void UpdateHealthBar(EntityHealth entity, int currentHealth, int maxHealth)
  {
    if (currentHealth <= 0)
    {
      Console.WriteLine($"{entity.GetType().Name} is Dead");
      return;
    }
    Console.WriteLine($"{entity.GetType().Name} - Health: {currentHealth}/{maxHealth}");
  }
}

public class AudioSystem
{

  public void PlayDamageSound(EntityHealth entity, int currentHealth, int maxHealth)
  {
    if (currentHealth <= 0)
    {
      return;
    }
    Console.WriteLine($"{entity.GetType().Name} - Playing damage sound");
  }
}

public class Game
{
  public static void Main()
  {
    Entity boss1 = new Boss1(500);
    Entity boss2 = new Boss2(300);

    HealthNotificationSystem notificationSystem = new HealthNotificationSystem();
    HUD hud = new HUD();
    AudioSystem audioSystem = new AudioSystem();

    notificationSystem.Subscribe(boss1, hud.UpdateHealthBar);
    notificationSystem.Subscribe(boss2, hud.UpdateHealthBar);
    notificationSystem.Subscribe(boss1, audioSystem.PlayDamageSound);

    boss1.TakeDamage(100);
    boss1.TakeDamage(200);
    boss1.TakeDamage(250);
    boss1.TakeDamage(50);

    boss2.TakeDamage(50);
    boss2.TakeDamage(200);
    boss2.TakeDamage(60);
  }
}