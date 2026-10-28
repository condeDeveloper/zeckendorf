using Conde.Zeckendorf;
using Xunit;

namespace Conde.Zeckendorf.Testes;

/// <summary>Todo número do README está preso aqui.</summary>
public class MedidasTestes
{
    private static readonly List<Medidas.LinhaNumero> Primeiros = Medidas.PrimeirosNumeros();

    [Theory]
    [InlineData(1, 1, "1", "1")]
    [InlineData(3, 2, "3", "2 + 1")]
    [InlineData(8, 3, "8", "5 + 2 + 1")]
    [InlineData(12, 1, "8 + 3 + 1", "8 + 3 + 1")]
    [InlineData(16, 4, "13 + 3", "8 + 5 + 2 + 1")]
    [InlineData(20, 1, "13 + 5 + 2", "13 + 5 + 2")]
    public void CadaLinhaDosPrimeirosNumeros(int n, int somas, string zeckendorf, string preguicosa)
    {
        var linha = Primeiros.Single(l => l.Numero == n);
        Assert.Equal(somas, linha.Representacoes);
        Assert.Equal(1, linha.SemVizinhos);
        Assert.Equal(zeckendorf, linha.Zeckendorf);
        Assert.Equal(preguicosa, linha.Preguicosa);
        Assert.True(linha.GulosoAcertou);
    }

    [Fact]
    public void ASequenciaDeSomasDosVintePrimeiros()
    {
        Assert.Equal(
            new[] { 1, 1, 2, 1, 2, 2, 1, 3, 2, 2, 3, 1, 3, 3, 2, 4, 2, 3, 3, 1 },
            Primeiros.Select(l => l.Representacoes).ToArray());
    }

    [Fact]
    public void OsFibonaccisMenosUmTemUmaSomaSo()
    {
        // 1, 4, 7, 12, 20: cada um é um Fibonacci menos um, e cada um tem uma
        // única maneira de ser escrito.
        foreach (var n in new[] { 1, 4, 7, 12, 20 })
            Assert.Equal(1, Primeiros.Single(l => l.Numero == n).Representacoes);
    }

    [Theory]
    [InlineData(20, 20, 4, 16)]
    [InlineData(50, 50, 6, 37)]
    [InlineData(100, 100, 9, 97)]
    [InlineData(200, 200, 13, 168)]
    [InlineData(400, 400, 16, 270)]
    public void CadaLinhaDaUnicidade(int ate, int comUmaSo, int maximo, int onde)
    {
        var linha = Medidas.Unicidade().Single(l => l.Ate == ate);
        Assert.Equal(ate, linha.Numeros);
        Assert.Equal(comUmaSo, linha.ComUmaSo);
        Assert.Equal(maximo, linha.Maximo);
        Assert.Equal(onde, linha.OndeDaOMaximo);
        Assert.True(linha.GulosoSempreAcertou);
    }

    [Fact]
    public void TodoNumeroDaFaixaTemUmaSoRepresentacaoSemVizinhos()
    {
        foreach (var linha in Medidas.Unicidade())
            Assert.Equal(linha.Numeros, linha.ComUmaSo);
    }

    [Theory]
    [InlineData(20, 42L, 2.10, 4)]
    [InlineData(50, 158L, 3.16, 6)]
    [InlineData(100, 414L, 4.14, 9)]
    [InlineData(200, 1_125L, 5.625, 13)]
    [InlineData(400, 2_960L, 7.40, 16)]
    public void CadaLinhaDoCrescimento(int ate, long somadas, double media, int maior)
    {
        var linha = Medidas.Crescimento().Single(l => l.Ate == ate);
        Assert.Equal(somadas, linha.Somadas);
        Assert.Equal(media, linha.Media, 3);
        Assert.Equal(maior, linha.Maior);
    }

    [Fact]
    public void AMediaDeSomasNaoParaDeCrescerEnquantoAUnicidadeFicaEmUm()
    {
        var linhas = Medidas.Crescimento();
        for (var i = 1; i < linhas.Count; i++)
            Assert.True(linhas[i].Media > linhas[i - 1].Media);
        Assert.Equal(3.52, linhas[^1].Media / linhas[0].Media, 2);
    }

    [Fact]
    public void OMinimaxConcordaComARegraEmTodaPilha()
    {
        var (pilhas, concordam, perdedoras) = Medidas.ARegraDoJogo();
        Assert.Equal(59, pilhas);
        Assert.Equal(59, concordam);
        Assert.Equal(8, perdedoras);
    }

    [Fact]
    public void AMenorAberturaEhSempreAMenorParcela()
    {
        var (pilhas, bateram) = Medidas.AberturaEhAMenorParcela();
        Assert.Equal(32, pilhas);
        Assert.Equal(pilhas, bateram);
    }

    [Fact]
    public void NaTabelaDoJogoAsPerdedorasSaoExatamenteOsFibonaccis()
    {
        foreach (var linha in Medidas.Jogos())
        {
            Assert.True(linha.ARegraBate, $"pilha {linha.Pilha}");
            Assert.Equal(linha.EhFibonacci, !linha.PrimeiroGanha);
            Assert.Equal(linha.EhFibonacci, linha.AberturasQueGanham == 0);
        }
    }
}
