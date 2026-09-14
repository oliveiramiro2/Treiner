using System;

class AttackStrategy
{
  public void ExecuteAttack(string attackType)
  {
    Console.WriteLine($"Executing {attackType} attack!");
  }
}

class Enemy
{
  public void Attack(Action attackBehavior)
  {
    attackBehavior?.Invoke();
  }
}

class Program
{
  public static void Main()
  {
    AttackStrategy attackStrategy = new AttackStrategy();
    Enemy enemy = new Enemy();

    enemy.Attack(() => Console.WriteLine("Executing a custom attack!"));


    enemy.Attack(() => attackStrategy.ExecuteAttack("Melee"));
    enemy.Attack(() => attackStrategy.ExecuteAttack("Ranged"));
    enemy.Attack(() => attackStrategy.ExecuteAttack("Defensive"));
  }
}