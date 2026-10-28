namespace Conde.Zeckendorf;

/// <summary>
/// Somar Fibonaccis distintos para chegar a um número. Há três coisas separadas
/// aqui, e elas não se falam: a varredura de TODAS as somas possíveis, o guloso
/// que pega sempre o maior que cabe, e o juiz que confere uma soma pronta.
/// </summary>
public static class Representacao
{
    /// <summary>
    /// Todas as maneiras de escrever n como soma de Fibonaccis DISTINTOS, cada
    /// uma como a lista de parcelas em ordem crescente. Varredura burra sobre os
    /// subconjuntos, sem nenhuma esperteza.
    /// </summary>
    public static List<int[]> Todas(int n)
    {
        if (n < 1) throw new ArgumentOutOfRangeException(nameof(n));

        var fibs = Fibonacci.Ate(n);
        var achadas = new List<int[]>();
        var escolhidos = new List<int>();

        void Passo(int indice, int falta)
        {
            if (falta == 0)
            {
                var copia = escolhidos.ToArray();
                Array.Sort(copia);
                achadas.Add(copia);
                return;
            }

            if (indice >= fibs.Length) return;

            // Sem este Fibonacci.
            Passo(indice + 1, falta);

            // Com ele, se couber.
            if (fibs[indice] > falta) return;
            escolhidos.Add(fibs[indice]);
            Passo(indice + 1, falta - fibs[indice]);
            escolhidos.RemoveAt(escolhidos.Count - 1);
        }

        Passo(0, n);
        return achadas;
    }

    /// <summary>
    /// A representação gulosa: pega o maior Fibonacci que cabe, desconta, repete.
    /// Esta é a parte que o teorema de Zeckendorf promete que dá certo, e ela não
    /// sabe nada do teorema.
    /// </summary>
    public static int[] Gulosa(int n)
    {
        if (n < 1) throw new ArgumentOutOfRangeException(nameof(n));

        var saida = new List<int>();
        var falta = n;
        while (falta > 0)
        {
            var fibs = Fibonacci.Ate(falta);
            var maior = fibs[^1];
            saida.Add(maior);
            falta -= maior;
        }

        saida.Sort();
        return saida.ToArray();
    }

    /// <summary>
    /// A representação preguiçosa: em vez do maior, pega o MENOR Fibonacci que
    /// ainda permite terminar. Ela também é única, e quase nunca é a mesma.
    /// </summary>
    public static int[] Preguicosa(int n)
    {
        if (n < 1) throw new ArgumentOutOfRangeException(nameof(n));

        var fibs = Fibonacci.Ate(n);
        var escolhidos = new List<int>();

        bool Passo(int indice, int falta)
        {
            if (falta == 0) return true;
            if (indice >= fibs.Length) return false;

            // Tenta primeiro PEGAR o menor que ainda está na mesa.
            if (fibs[indice] <= falta)
            {
                escolhidos.Add(fibs[indice]);
                if (Passo(indice + 1, falta - fibs[indice])) return true;
                escolhidos.RemoveAt(escolhidos.Count - 1);
            }

            return Passo(indice + 1, falta);
        }

        Passo(0, n);
        var copia = escolhidos.ToArray();
        Array.Sort(copia);
        return copia;
    }

    /// <summary>
    /// É uma representação de Zeckendorf? Soma certa, parcelas distintas, todas
    /// de Fibonacci, e nenhum par de Fibonaccis consecutivos.
    /// </summary>
    public static bool EhDeZeckendorf(int n, IReadOnlyList<int> parcelas)
    {
        ArgumentNullException.ThrowIfNull(parcelas);
        if (parcelas.Count == 0) return false;
        if (parcelas.Sum() != n) return false;
        if (parcelas.Distinct().Count() != parcelas.Count) return false;

        foreach (var parcela in parcelas)
            if (!Fibonacci.Eh(parcela))
                return false;

        var indices = parcelas.Select(Fibonacci.Indice).OrderBy(i => i).ToArray();
        for (var i = 1; i < indices.Length; i++)
            if (indices[i] - indices[i - 1] == 1)
                return false;

        return true;
    }

    /// <summary>Quantas das representações de n não têm Fibonaccis vizinhos.</summary>
    public static int QuantasSemVizinhos(int n) => Todas(n).Count(r => EhDeZeckendorf(n, r));

    /// <summary>O texto "8 + 3 + 1" de uma representação.</summary>
    public static string Texto(IReadOnlyList<int> parcelas) =>
        string.Join(" + ", parcelas.OrderByDescending(p => p));
}
