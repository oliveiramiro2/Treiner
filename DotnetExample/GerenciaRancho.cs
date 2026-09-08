using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

public class Reserva
{
  public DateTime DataInicio { get; private set; }
  public DateTime DataFim { get; private set; }

  public Reserva(DateTime dataInicio, DateTime dataFim)
  {
    // Validação da regra: Se a data de início for menor que hoje, barra a criação!
    if (dataInicio < DateTime.Today)
    {
      throw new ArgumentException("A data de início não pode ser no passado.");
    }

    DataInicio = dataInicio;
    DataFim = dataFim;
  }
}

public class Rancho
{
  private readonly List<Reserva> _reservas = new List<Reserva>();

  public IReadOnlyCollection<Reserva> Reservas => _reservas.AsReadOnly();

  public void AdicionarReserva(Reserva reserva)
  {
    _reservas.Add(reserva);
  }

  public void CriarReserva(DateTime inicio, DateTime fim)
  {
    // 1. Verifica se já existe alguma reserva na lista que conflita com o novo período
    bool temConflito = _reservas.Any(r =>
        // Há conflito se o início da nova for antes do fim da existente 
        // E o fim da nova for depois do início da existente
        inicio <= r.DataFim && fim >= r.DataInicio
    );

    if (temConflito)
    {
      throw new InvalidOperationException("Já existe uma reserva confirmada para este período (conflito de datas).");
    }

    // 2. Se passou da validação, cria e adiciona a nova reserva
    var novaReserva = new Reserva(inicio, fim);
    _reservas.Add(novaReserva);
  }
}

public class ReservaTestes
{
  [Fact]
  public void Nao_deve_permitir_criar_uma_reserva_com_data_no_passado()
  {
    // Arrange (preparação dos dados)
    var dataOntem = DateTime.Today.AddDays(-1);
    var dataAmanha = DateTime.Today.AddDays(1);

    // Act & Assert (ação e verificação do comportamento esperado)
    Action acao = () => new Reserva(dataOntem, dataAmanha);

    acao.Should().Throw<ArgumentException>()
        .WithMessage("*passado*");
  }

  [Fact]
  public void Nao_deve_permitir_criar_uma_reserva_com_data_no_passado()
  {
    var dataOntem = DateTime.Today.AddDays(-1);
    var dataAmanha = DateTime.Today.AddDays(1);

    Action acao = () => new Reserva(dataOntem, dataAmanha);

    acao.Should().Throw<ArgumentException>()
        .WithMessage("*passado*");
  }

  [Fact]
  public void Nao_deve_permitir_duas_reservas_no_mesmo_periodo()
  {
    // Arrange
    var rancho = new Rancho();

    var dataInicio = DateTime.Today.AddDays(5);
    var dataFim = DateTime.Today.AddDays(10);

    // Primeira reserva feita com sucesso
    var primeiraReserva = new Reserva(dataInicio, dataFim);
    rancho.AdicionarReserva(primeiraReserva);

    // Tentando criar uma nova reserva dentro do mesmo período (ex: do dia 7 ao 9)
    var tentativaInicio = DateTime.Today.AddDays(7);
    var tentativaFim = DateTime.Today.AddDays(9);

    // Act
    Action acao = () => rancho.CriarReserva(tentativaInicio, tentativaFim);

    // Assert
    acao.Should().Throw<InvalidOperationException>()
        .WithMessage("*conflito*");
  }

  [Fact]
  public void Deve_retornar_apenas_reservas_futuras_ordenadas_por_data()
  {
    // Arrange
    var rancho = new Rancho();

    // Simulando uma reserva no passado (se o sistema permitisse forçar para o teste, ou usando datas relativas)
    // Para o teste, vamos adicionar diretamente na lista ou usar datas válidas:
    var dataProxima = DateTime.Today.AddDays(10);
    var dataMaisDistante = DateTime.Today.AddDays(25);
    var dataIntermediaria = DateTime.Today.AddDays(15);

    rancho.CriarReserva(dataProxima, DateTime.Today.AddDays(12));
    rancho.CriarReserva(dataMaisDistante, DateTime.Today.AddDays(30));
    rancho.CriarReserva(dataIntermediaria, DateTime.Today.AddDays(18));

    // Act
    var reservasFuturas = rancho.ObterReservasFuturas();

    // Assert
    reservasFuturas.Should().HaveCount(3);
    reservasFuturas.First().DataInicio.Should().Be(dataProxima); // A primeira deve ser a de 10 dias
    reservasFuturas.Last().DataInicio.Should().Be(dataMaisDistante); // A última deve ser a de 25 dias
  }
}