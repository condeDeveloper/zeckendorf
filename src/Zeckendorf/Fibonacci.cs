namespace Conde.Zeckendorf;

/// <summary>
/// Os números de Fibonacci usados como base. A lista começa em 1, 2, 3, 5, 8:
/// o 1 aparece uma vez só, porque na base de Fibonacci ter dois uns quebraria a
/// unicidade antes de qualquer teorema.
/// </summary>
public static class Fibonacci
{
    /// <summary>Os Fibonaccis de 1 até o maior que não passa do teto.</summary>
    public static int[] Ate(int teto)
    {
        if (teto < 1) throw new ArgumentOutOfRangeException(nameof(teto));

        var lista = new List<int>();
        int a = 1, b = 2;
        while (a <= teto)
        {
            lista.Add(a);
            (a, b) = (b, a + b);
        }

        return lista.ToArray();
    }

    /// <summary>Os primeiros <paramref name="quantos"/> da lista.</summary>
    public static int[] Primeiros(int quantos)
    {
        if (quantos < 0) throw new ArgumentOutOfRangeException(nameof(quantos));

        var saida = new int[quantos];
        int a = 1, b = 2;
        for (var i = 0; i < quantos; i++)
        {
            saida[i] = a;
            (a, b) = (b, a + b);
        }

        return saida;
    }

    public static bool Eh(int n)
    {
        if (n < 1) return false;
        int a = 1, b = 2;
        while (a < n) (a, b) = (b, a + b);
        return a == n;
    }

    /// <summary>
    /// O índice do número na lista 1, 2, 3, 5, 8..., ou -1 se ele não estiver
    /// nela.
    /// </summary>
    public static int Indice(int n) => Array.IndexOf(Ate(Math.Max(n, 1)), n);
}
