public delegate int PriceCalculator(int basePrice);

public class Shop
{
  public void BuyItem(string itemName, int basePrice, PriceCalculator calculator)
  {
    Console.WriteLine($"Item: {itemName} | Preço Base: {basePrice} gold");

    int finalPrice = calculator(basePrice);

    Console.WriteLine($"Preço Final a pagar: {finalPrice} gold\n");
  }
}

public class Game
{
  public void Start()
  {
    Shop shop = new Shop();

    shop.BuyItem("Poção de Vida", 200, basePrice => basePrice * 80 / 100); // 20% off
    shop.BuyItem("Espada", 100, basePrice => basePrice * 50 / 100); // 50% off
    shop.BuyItem("Armadura", 200, basePrice => basePrice);
  }

  public static void Main()
  {
    Game game = new Game();
    game.Start();
  }
}