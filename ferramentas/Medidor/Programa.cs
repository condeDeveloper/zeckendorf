using Conde.Zeckendorf;

var comando = args.Length > 0 ? args[0] : "tudo";

if (comando is "numeros" or "tudo") Numeros();
if (comando is "unicidade" or "tudo") Unicidade();
if (comando is "crescimento" or "tudo") Crescimento();
if (comando is "jogo" or "tudo") JogoTabela();
if (comando is "ponte" or "tudo") Ponte();

static void Numeros()
{
    Console.WriteLine();
    Console.WriteLine("OS PRIMEIROS NUMEROS, com TODAS as somas de Fibonaccis distintos de cada um.");
    Console.WriteLine();
    Console.WriteLine($"  {"n",4} {"somas",6} {"sem vizinhos",13} {"a de Zeckendorf",-18} {"a preguicosa",-18} {"o guloso acertou",17}");

    foreach (var l in Medidas.PrimeirosNumeros())
        Console.WriteLine($"  {l.Numero,4} {l.Representacoes,6} {l.SemVizinhos,13} {l.Zeckendorf,-18} {l.Preguicosa,-18} {(l.GulosoAcertou ? "sim" : "NAO"),17}");

    Console.WriteLine();
}

static void Unicidade()
{
    Console.WriteLine();
    Console.WriteLine("A UNICIDADE CONTADA, e nao citada.");
    Console.WriteLine();
    Console.WriteLine($"  {"ate",6} {"numeros",9} {"com uma so",12} {"mais somas",12} {"em que n",9} {"o guloso sempre acertou",24}");

    foreach (var l in Medidas.Unicidade())
        Console.WriteLine($"  {l.Ate,6} {l.Numeros,9} {l.ComUmaSo,12} {l.Maximo,12} {l.OndeDaOMaximo,9} {(l.GulosoSempreAcertou ? "sim" : "NAO"),24}");

    Console.WriteLine();
}

static void Crescimento()
{
    Console.WriteLine();
    Console.WriteLine("QUANTAS SOMAS EXISTEM ao todo, sem a regra dos vizinhos.");
    Console.WriteLine();
    Console.WriteLine($"  {"ate",6} {"somas somadas",15} {"media por numero",17} {"a maior",9}");

    foreach (var l in Medidas.Crescimento())
        Console.WriteLine($"  {l.Ate,6} {l.Somadas,15:n0} {l.Media,17:0.00} {l.Maior,9}");

    Console.WriteLine();
}

static void JogoTabela()
{
    Console.WriteLine();
    Console.WriteLine("O NIM DE FIBONACCI por minimax. A regra nao e usada: ela e conferida.");
    Console.WriteLine();
    Console.WriteLine($"  {"pilha",6} {"quem ganha",11} {"aberturas que ganham",21} {"a menor",8} {"e Fibonacci",12} {"a regra bate",13}");

    foreach (var l in Medidas.Jogos())
        Console.WriteLine($"  {l.Pilha,6} {(l.PrimeiroGanha ? "o primeiro" : "o segundo"),11} {l.AberturasQueGanham,21} {l.MenorAbertura,8} {(l.EhFibonacci ? "sim" : "nao"),12} {(l.ARegraBate ? "sim" : "NAO"),13}");

    Console.WriteLine();
}

static void Ponte()
{
    Console.WriteLine();
    var (pilhas, concordam, perdedoras) = Medidas.ARegraDoJogo();
    var (semFib, bateram) = Medidas.AberturaEhAMenorParcela();

    Console.WriteLine("A PONTE entre as duas metades do repositorio:");
    Console.WriteLine();
    Console.WriteLine($"  pilhas conferidas                                 {pilhas,6}");
    Console.WriteLine($"  em que o minimax concorda com a regra             {concordam,6}");
    Console.WriteLine($"  pilhas em que quem comeca PERDE                   {perdedoras,6}");
    Console.WriteLine();
    Console.WriteLine($"  pilhas que nao sao Fibonacci                      {semFib,6}");
    Console.WriteLine($"  em que a menor abertura e a menor parcela         {bateram,6}");
    Console.WriteLine();
}
