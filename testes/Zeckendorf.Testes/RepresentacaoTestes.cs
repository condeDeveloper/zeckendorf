using Conde.Zeckendorf;
using Xunit;

namespace Conde.Zeckendorf.Testes;

public class RepresentacaoTestes
{
    [Fact]
    public void TodoNumeroAteQuatrocentosTemExatamenteUmaRepresentacaoSemVizinhos()
    {
        for (var n = 1; n <= 400; n++)
            Assert.Equal(1, Representacao.QuantasSemVizinhos(n));
    }

    [Fact]
    public void TodaSomaVarridaSomaMesmoONumero()
    {
        for (var n = 1; n <= 60; n++)
            foreach (var soma in Representacao.Todas(n))
            {
                Assert.Equal(n, soma.Sum());
                Assert.Equal(soma.Length, soma.Distinct().Count());
                foreach (var parcela in soma) Assert.True(Fibonacci.Eh(parcela));
            }
    }

    [Fact]
    public void OGulosoSempreDaARepresentacaoDeZeckendorf()
    {
        for (var n = 1; n <= 400; n++)
            Assert.True(Representacao.EhDeZeckendorf(n, Representacao.Gulosa(n)), $"n = {n}");
    }

    [Fact]
    public void APreguicosaTambemSomaCertoEQuaseNuncaEhAMesma()
    {
        var iguais = 0;
        for (var n = 1; n <= 100; n++)
        {
            var preguicosa = Representacao.Preguicosa(n);
            Assert.Equal(n, preguicosa.Sum());
            if (preguicosa.SequenceEqual(Representacao.Gulosa(n))) iguais++;
        }

        Assert.Equal(9, iguais);
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(4, "3 + 1")]
    [InlineData(12, "8 + 3 + 1")]
    [InlineData(20, "13 + 5 + 2")]
    [InlineData(100, "89 + 8 + 3")]
    public void ARepresentacaoDeZeckendorfDeAlgunsNumeros(int n, string texto)
    {
        Assert.Equal(texto, Representacao.Texto(Representacao.Gulosa(n)));
    }

    [Fact]
    public void FibonaccisVizinhosReprovam()
    {
        // 2 + 3 = 5, mas 2 e 3 são vizinhos na lista.
        Assert.False(Representacao.EhDeZeckendorf(5, new[] { 2, 3 }));
        Assert.True(Representacao.EhDeZeckendorf(5, new[] { 5 }));
    }

    [Fact]
    public void SomaErradaReprova()
    {
        Assert.False(Representacao.EhDeZeckendorf(10, new[] { 8, 1 }));
    }

    [Fact]
    public void ParcelaRepetidaReprova()
    {
        Assert.False(Representacao.EhDeZeckendorf(4, new[] { 2, 2 }));
    }

    [Fact]
    public void ParcelaQueNaoEhFibonacciReprova()
    {
        Assert.False(Representacao.EhDeZeckendorf(10, new[] { 6, 4 }));
    }

    [Fact]
    public void ListaVaziaNaoRepresentaNada()
    {
        Assert.False(Representacao.EhDeZeckendorf(0, Array.Empty<int>()));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    [InlineData(8, 3)]
    [InlineData(16, 4)]
    [InlineData(37, 6)]
    public void QuantasSomasCadaNumeroTem(int n, int quantas)
    {
        Assert.Equal(quantas, Representacao.Todas(n).Count);
    }

    [Fact]
    public void ZeroNaoTemRepresentacao()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Representacao.Todas(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Representacao.Gulosa(0));
    }
}
