using Conde.Zeckendorf;
using Xunit;

namespace Conde.Zeckendorf.Testes;

public class JogoTestes
{
    [Fact]
    public void ComUmaFichaQuemComecaGanha()
    {
        Assert.True(Jogo.PrimeiroGanha(1));
    }

    [Fact]
    public void ComDuasFichasQuemComecaPerde()
    {
        // Só pode tirar uma, e o outro leva a última.
        Assert.False(Jogo.PrimeiroGanha(2));
    }

    [Fact]
    public void QuemRecebePilhaDeFibonacciPerde()
    {
        for (var pilha = 2; pilha <= 60; pilha++)
            Assert.Equal(!Fibonacci.Eh(pilha), Jogo.PrimeiroGanha(pilha));
    }

    [Fact]
    public void NasPilhasDeFibonacciNaoHaAberturaVencedora()
    {
        foreach (var pilha in new[] { 2, 3, 5, 8, 13, 21, 34 })
            Assert.Empty(Jogo.AberturasQueGanham(pilha));
    }

    [Fact]
    public void NasOutrasPilhasHaPeloMenosUma()
    {
        for (var pilha = 4; pilha <= 40; pilha++)
            if (!Fibonacci.Eh(pilha))
                Assert.NotEmpty(Jogo.AberturasQueGanham(pilha));
    }

    [Fact]
    public void AMenorAberturaVencedoraEhAMenorParcelaDeZeckendorf()
    {
        for (var pilha = 4; pilha <= 60; pilha++)
        {
            if (Fibonacci.Eh(pilha)) continue;
            var abertura = Jogo.AberturasQueGanham(pilha)[0];
            Assert.Equal(Representacao.Gulosa(pilha)[0], abertura);
        }
    }

    [Fact]
    public void PilhaVaziaEhDerrotaDeQuemEstaNaVez()
    {
        Assert.False(Jogo.Ganha(0, 10));
    }

    [Fact]
    public void ComLimiteGrandeOSuficienteSeLevaTudo()
    {
        Assert.True(Jogo.Ganha(7, 7));
        Assert.True(Jogo.Ganha(7, 100));
    }

    [Fact]
    public void AMemoriaCompartilhadaNaoMudaAResposta()
    {
        var memoria = new Dictionary<(int, int), bool>();
        for (var pilha = 2; pilha <= 40; pilha++)
            Assert.Equal(Jogo.PrimeiroGanha(pilha), Jogo.PrimeiroGanha(pilha, memoria));
    }

    [Fact]
    public void PilhaNegativaNaoEhPergunta()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Jogo.Ganha(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Jogo.PrimeiroGanha(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Jogo.AberturasQueGanham(1));
    }
}
