using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemPedido
{
  public class ItemPedido
  {
    public string Nome { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    public ItemPedido(string nome, int quantidade, decimal precoUnitario)
    {
      if (quantidade <= 0) throw new ArgumentException("A quantidade deve ser maior que zero.");
      if (precoUnitario <= 0) throw new ArgumentException("O preço deve ser maior que zero.");

      Nome = nome;
      Quantidade = quantidade;
      PrecoUnitario = precoUnitario;
    }

    public decimal CalcularSubtotal() => Quantidade * PrecoUnitario;
  }
}