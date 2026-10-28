namespace Conde.Zeckendorf;

/// <summary>
/// As medidas do README. Os testes conferem o que estes métodos devolvem; o
/// medidor só imprime.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaNumero(
        int Numero, int Representacoes, int SemVizinhos, string Zeckendorf, string Preguicosa, bool GulosoAcertou);

    public readonly record struct LinhaUnicidade(
        int Ate, int Numeros, int ComUmaSo, int Maximo, int OndeDaOMaximo, bool GulosoSempreAcertou);

    public readonly record struct LinhaJogo(
        int Pilha, bool PrimeiroGanha, int AberturasQueGanham, int MenorAbertura, bool EhFibonacci, bool ARegraBate);

    public readonly record struct LinhaCrescimento(int Ate, long Somadas, double Media, int Maior);

    /// <summary>Os primeiros números, com todas as representações de cada um.</summary>
    public static List<LinhaNumero> PrimeirosNumeros(int ate = 20)
    {
        var linhas = new List<LinhaNumero>();
        for (var n = 1; n <= ate; n++)
        {
            var todas = Representacao.Todas(n);
            var zeck = todas.Single(r => Representacao.EhDeZeckendorf(n, r));
            var guloso = Representacao.Gulosa(n);

            linhas.Add(new LinhaNumero(
                n,
                todas.Count,
                todas.Count(r => Representacao.EhDeZeckendorf(n, r)),
                Representacao.Texto(zeck),
                Representacao.Texto(Representacao.Preguicosa(n)),
                guloso.SequenceEqual(zeck)));
        }
        return linhas;
    }

    /// <summary>
    /// A unicidade conferida em faixa: para todo n até o teto, quantas
    /// representações sem vizinhos existem. Zeckendorf diz que é sempre uma, e
    /// aqui isso é contado em vez de citado.
    /// </summary>
    public static List<LinhaUnicidade> Unicidade()
    {
        var linhas = new List<LinhaUnicidade>();
        foreach (var ate in new[] { 20, 50, 100, 200, 400 })
        {
            var comUmaSo = 0;
            var maximo = 0;
            var onde = 0;
            var gulosoSempre = true;

            for (var n = 1; n <= ate; n++)
            {
                var todas = Representacao.Todas(n);
                var semVizinhos = todas.Count(r => Representacao.EhDeZeckendorf(n, r));
                if (semVizinhos == 1) comUmaSo++;
                if (todas.Count > maximo)
                {
                    maximo = todas.Count;
                    onde = n;
                }
                if (!Representacao.Gulosa(n).SequenceEqual(todas.Single(r => Representacao.EhDeZeckendorf(n, r))))
                    gulosoSempre = false;
            }

            linhas.Add(new LinhaUnicidade(ate, ate, comUmaSo, maximo, onde, gulosoSempre));
        }
        return linhas;
    }

    /// <summary>Quantas representações existem ao todo, e como esse número cresce.</summary>
    public static List<LinhaCrescimento> Crescimento()
    {
        var linhas = new List<LinhaCrescimento>();
        foreach (var ate in new[] { 20, 50, 100, 200, 400 })
        {
            long somadas = 0;
            var maior = 0;
            for (var n = 1; n <= ate; n++)
            {
                var quantas = Representacao.Todas(n).Count;
                somadas += quantas;
                maior = Math.Max(maior, quantas);
            }
            linhas.Add(new LinhaCrescimento(ate, somadas, (double)somadas / ate, maior));
        }
        return linhas;
    }

    /// <summary>
    /// O Nim de Fibonacci, pilha por pilha, resolvido por minimax. A última coluna
    /// compara com a regra "perde quem recebe uma pilha de Fibonacci", que o
    /// minimax não conhece.
    /// </summary>
    public static List<LinhaJogo> Jogos(int ate = 40)
    {
        var linhas = new List<LinhaJogo>();
        for (var pilha = 2; pilha <= ate; pilha++)
        {
            var ganha = Jogo.PrimeiroGanha(pilha);
            var aberturas = Jogo.AberturasQueGanham(pilha);
            var ehFib = Fibonacci.Eh(pilha);

            linhas.Add(new LinhaJogo(
                pilha, ganha, aberturas.Count, aberturas.Count > 0 ? aberturas[0] : 0, ehFib, ganha != ehFib));
        }
        return linhas;
    }

    /// <summary>
    /// A regra do jogo contra o minimax, em faixa: em quantas pilhas as duas
    /// coisas concordam, e qual a primeira abertura vencedora de cada pilha.
    /// </summary>
    public static (int Pilhas, int Concordam, int Perdedoras) ARegraDoJogo(int ate = 60)
    {
        var pilhas = 0;
        var concordam = 0;
        var perdedoras = 0;

        for (var pilha = 2; pilha <= ate; pilha++)
        {
            pilhas++;
            var ganha = Jogo.PrimeiroGanha(pilha);
            if (!ganha) perdedoras++;
            if (ganha != Fibonacci.Eh(pilha)) concordam++;
        }

        return (pilhas, concordam, perdedoras);
    }

    /// <summary>
    /// A menor abertura vencedora é a menor parcela da representação de
    /// Zeckendorf da pilha. É a ponte entre as duas metades do repositório.
    /// </summary>
    public static (int Pilhas, int Bateram) AberturaEhAMenorParcela(int ate = 40)
    {
        var pilhas = 0;
        var bateram = 0;

        for (var pilha = 2; pilha <= ate; pilha++)
        {
            if (Fibonacci.Eh(pilha)) continue;
            pilhas++;

            var aberturas = Jogo.AberturasQueGanham(pilha);
            var zeckendorf = Representacao.Gulosa(pilha);
            if (aberturas.Count > 0 && aberturas[0] == zeckendorf[0]) bateram++;
        }

        return (pilhas, bateram);
    }
}
