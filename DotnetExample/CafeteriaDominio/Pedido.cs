using System;
using System.Collections.Generic;
using System.Linq;

namespace CafeteriaDominio
{
  public enum StatusPedido
  {
    Pendente,
    Pago,
    Cancelado
  }

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

  public class Pedido
  {
    private readonly List<ItemPedido> _itens = new List<ItemPedido>();

    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();
    public decimal ValorTotal { get; private set; }
    public StatusPedido Status { get; private set; }

    public Pedido(List<ItemPedido> itens)
    {
      if (itens == null || !itens.Any())
      {
        throw new ArgumentException("O pedido precisa ter pelo menos um item.");
      }

      _itens = itens;
      Status = StatusPedido.Pendente;

      CalcularValorTotal();
    }

    private void CalcularValorTotal()
    {
      decimal somaBruta = _itens.Sum(i => i.CalcularSubtotal());

      if (somaBruta > 100.0m)
      {
        ValorTotal = somaBruta * 0.9m;
      }
      else
      {
        ValorTotal = somaBruta;
      }
    }

    public void Pagar()
    {
      if (Status == StatusPedido.Cancelado)
      {
        throw new InvalidOperationException("Não é possível pagar um pedido que já foi cancelado.");
      }

      if (Status == StatusPedido.Pago)
      {
        throw new InvalidOperationException("Este pedido já está pago.");
      }

      Status = StatusPedido.Pago;
    }

    public void Cancelar()
    {
      if (Status == StatusPedido.Pago)
      {
        throw new InvalidOperationException("Não é possível cancelar um pedido que já foi pago.");
      }

      if (Status == StatusPedido.Cancelado)
      {
        throw new InvalidOperationException("Este pedido já está cancelado.");
      }

      Status = StatusPedido.Cancelado;
    }
  }
}