using Conde.Zeckendorf;
using Xunit;

namespace Conde.Zeckendorf.Testes;

public class FibonacciTestes
{
    [Fact]
    public void AListaComecaEmUmEDoisESemRepetirOUm()
    {
        Assert.Equal(new[] { 1, 2, 3, 5, 8, 13, 21, 34 }, Fibonacci.Primeiros(8));
    }

    [Fact]
    public void AteOTetoNaoPassaDoTeto()
    {
        Assert.Equal(new[] { 1, 2, 3, 5, 8, 13 }, Fibonacci.Ate(13));
        Assert.Equal(new[] { 1, 2, 3, 5, 8 }, Fibonacci.Ate(12));
        Assert.Equal(new[] { 1 }, Fibonacci.Ate(1));
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(4, false)]
    [InlineData(13, true)]
    [InlineData(14, false)]
    [InlineData(55, true)]
    [InlineData(100, false)]
    public void QuemEhFibonacci(int n, bool eh)
    {
        Assert.Equal(eh, Fibonacci.Eh(n));
    }

    [Fact]
    public void ZeroENegativoNaoSaoFibonacci()
    {
        Assert.False(Fibonacci.Eh(0));
        Assert.False(Fibonacci.Eh(-5));
    }

    [Fact]
    public void OIndiceEhALugarNaLista()
    {
        Assert.Equal(0, Fibonacci.Indice(1));
        Assert.Equal(3, Fibonacci.Indice(5));
        Assert.Equal(-1, Fibonacci.Indice(4));
    }

    [Fact]
    public void CadaTermoEhASomaDosDoisAnteriores()
    {
        var fibs = Fibonacci.Primeiros(15);
        for (var i = 2; i < fibs.Length; i++)
            Assert.Equal(fibs[i - 1] + fibs[i - 2], fibs[i]);
    }

    [Fact]
    public void TetoMenorQueUmNaoEhPergunta()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Fibonacci.Ate(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Fibonacci.Primeiros(-1));
    }
}
