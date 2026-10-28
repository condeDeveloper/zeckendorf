namespace Conde.Zeckendorf;

/// <summary>
/// O Nim de Fibonacci, de 1963: uma pilha de n fichas. Quem começa tira o que
/// quiser, menos a pilha inteira; daí em diante cada um tira no máximo o dobro do
/// que o outro acabou de tirar. Quem leva a última ficha ganha.
///
/// O jogo não tem nada a ver com Fibonacci no enunciado, e a resposta é inteira
/// sobre Fibonacci. É isso que este arquivo mede.
/// </summary>
public static class Jogo
{
    /// <summary>
    /// Quem está na vez ganha, com <paramref name="restam"/> fichas na pilha e
    /// podendo tirar até <paramref name="limite"/>? Minimax com memória, sem
    /// nenhuma fórmula.
    /// </summary>
    public static bool Ganha(int restam, int limite, Dictionary<(int, int), bool>? memoria = null)
    {
        if (restam < 0) throw new ArgumentOutOfRangeException(nameof(restam));
        memoria ??= new Dictionary<(int, int), bool>();
        if (restam == 0) return false;

        var chave = (restam, Math.Min(limite, restam));
        if (memoria.TryGetValue(chave, out var guardado)) return guardado;

        var ganha = false;
        for (var tira = 1; tira <= Math.Min(limite, restam); tira++)
        {
            if (Ganha(restam - tira, 2 * tira, memoria)) continue;
            ganha = true;
            break;
        }

        memoria[chave] = ganha;
        return ganha;
    }

    /// <summary>
    /// Quem começa ganha uma pilha de n? A primeira jogada pode tirar de 1 a
    /// n-1 fichas: levar a pilha inteira na largada não vale.
    /// </summary>
    public static bool PrimeiroGanha(int pilha, Dictionary<(int, int), bool>? memoria = null)
    {
        if (pilha < 1) throw new ArgumentOutOfRangeException(nameof(pilha));
        if (pilha == 1) return true;

        memoria ??= new Dictionary<(int, int), bool>();
        for (var tira = 1; tira < pilha; tira++)
            if (!Ganha(pilha - tira, 2 * tira, memoria))
                return true;

        return false;
    }

    /// <summary>
    /// As jogadas de abertura que ganham, numa pilha de n. Quando a pilha é um
    /// Fibonacci não há nenhuma.
    /// </summary>
    public static List<int> AberturasQueGanham(int pilha)
    {
        if (pilha < 2) throw new ArgumentOutOfRangeException(nameof(pilha));
        var memoria = new Dictionary<(int, int), bool>();
        var saida = new List<int>();
        for (var tira = 1; tira < pilha; tira++)
            if (!Ganha(pilha - tira, 2 * tira, memoria))
                saida.Add(tira);
        return saida;
    }
}
