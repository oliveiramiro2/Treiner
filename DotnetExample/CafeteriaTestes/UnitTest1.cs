using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using CafeteriaDominio;

// public class PedidoTests
// {
//     [Fact]
//     public void NaoDevePagarPedidoCancelado()
//     {
//         // Usamos CafeteriaDominio.ItemPedido explicitamente para evitar ambiguidade
//         var itens = new List<CafeteriaDominio.ItemPedido>
//         {
//             new CafeteriaDominio.ItemPedido("Produto A", 1, 50.0m)
//         };

//         var pedido = new CafeteriaDominio.Pedido(itens);
//         pedido.Cancelar();

//         Action acao = () => pedido.Pagar();
//         acao.Should().Throw<InvalidOperationException>()
//             .WithMessage("Não é possível pagar um pedido que já foi cancelado.");
//     }
// }

public class PedidoTests
{
    [Fact]
    public void Deve_criar_pedido_com_sucesso_e_aplicar_desconto_se_total_maior_que_cem()
    {
        // Arrange
        var itens = new List<CafeteriaDominio.ItemPedido>
    {
        new CafeteriaDominio.ItemPedido("Café Especial", 2, 30.0m), // 2 * 30 = 60
        new CafeteriaDominio.ItemPedido("Bolo de Chocolate", 1, 50.0m) // 1 * 50 = 50
        // Total bruto = 110.0m (Deve aplicar 10% de desconto -> Deve ficar 99.0m)
    };

        // Act
        var pedido = new CafeteriaDominio.Pedido(itens);

        // Assert
        pedido.ValorTotal.Should().Be(99.0m);
        pedido.Status.Should().Be(CafeteriaDominio.StatusPedido.Pendente);
    }

    [Fact]
    public void Nao_deve_permitir_cancelar_um_pedido_que_ja_foi_pago()
    {
        // Arrange
        var itens = new List<CafeteriaDominio.ItemPedido> { new CafeteriaDominio.ItemPedido("Café", 1, 10.0m) };
        var pedido = new CafeteriaDominio.Pedido(itens);

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
        var itens = new List<CafeteriaDominio.ItemPedido>
    {
      new CafeteriaDominio.ItemPedido("Produto A", 1, 50.0m)
    };

        var pedido = new CafeteriaDominio.Pedido(itens);
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
        var itens = new List<CafeteriaDominio.ItemPedido>
    {
        new CafeteriaDominio.ItemPedido("Produto Teste", 1, precoUnitario)
    };

        // Act
        var pedido = new CafeteriaDominio.Pedido(itens);

        // Assert
        pedido.ValorTotal.Should().Be(valorTotalEsperado);
    }
}

