#:package FluentAssertions@8.10.0
#:package xunit@2.9.3
#:package xunit.runner.visualstudio@4.0.0

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;

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
    // Soma o subtotal de todos os itens usando LINQ (.Sum)
    decimal somaBruta = _itens.Sum(i => i.CalcularSubtotal());

    // Regra de negócio: Desconto de 10% se passar de 100 reais
    if (somaBruta > 100.0m)
    {
      ValorTotal = somaBruta * 0.9m; // Aplica 10% de desconto
    }
    else
    {
      ValorTotal = somaBruta;
    }
  }

  public void Pagar()
  {
    // Se já foi cancelado, não pode ser pago
    if (Status == StatusPedido.Cancelado)
    {
      throw new InvalidOperationException("Não é possível pagar um pedido que já foi cancelado.");
    }

    // Se já estiver pago, podemos ignorar ou lançar erro, dependendo da regra
    if (Status == StatusPedido.Pago)
    {
      throw new InvalidOperationException("Este pedido já está pago.");
    }

    Status = StatusPedido.Pago;
  }

  public void Cancelar()
  {
    // A regra que você escreveu perfeitamente:
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

public class PedidoTests
{
  [Fact]
  public void Deve_criar_pedido_com_sucesso_e_aplicar_desconto_se_total_maior_que_cem()
  {
    // Arrange
    var itens = new List<ItemPedido>
    {
        new ItemPedido("Café Especial", 2, 30.0m), // 2 * 30 = 60
        new ItemPedido("Bolo de Chocolate", 1, 50.0m) // 1 * 50 = 50
        // Total bruto = 110.0m (Deve aplicar 10% de desconto -> Deve ficar 99.0m)
    };

    // Act
    var pedido = new Pedido(itens);

    // Assert
    pedido.ValorTotal.Should().Be(99.0m);
    pedido.Status.Should().Be(StatusPedido.Pendente);
  }

  [Fact]
  public void Nao_deve_permitir_cancelar_um_pedido_que_ja_foi_pago()
  {
    // Arrange
    var itens = new List<ItemPedido> { new ItemPedido("Café", 1, 10.0m) };
    var pedido = new Pedido(itens);

    pedido.Pagar(); // Mudamos o status para Pago

    // Act
    Action acao = () => pedido.Cancelar();

    // Assert
    acao.Should().Throw<InvalidOperationException>()
        .WithMessage("*já foi pago*");
  }


  [Fact]
  public void NaoDevePagarPedidoCancelado()
  {
    var itens = new List<ItemPedido>
    {
      new ItemPedido("Produto A", 1, 50.0m)
    };

    var pedido = new Pedido(itens);
    pedido.Cancelar();

    Action acao = () => pedido.Pagar();
    acao.Should().Throw<InvalidOperationException>().WithMessage("Não é possível pagar um pedido que já foi cancelado.");
  }

  [Theory]
  [InlineData(50.0, 50.0)]   // Abaixo de 100: Não deve ter desconto (total = 50)
  [InlineData(100.0, 100.0)] // Exatamente 100: Não deve ter desconto (total = 100)
  [InlineData(200.0, 180.0)] // Acima de 100: Deve aplicar 10% de desconto (200 - 20 = 180)
  public void Deve_calcular_o_total_corretamente_com_base_nos_itens(decimal precoUnitario, decimal valorTotalEsperado)
  {
    // Arrange
    var itens = new List<ItemPedido>
    {
        new ItemPedido("Produto Teste", 1, precoUnitario)
    };

    // Act
    var pedido = new Pedido(itens);

    // Assert
    pedido.ValorTotal.Should().Be(valorTotalEsperado);
  }
}