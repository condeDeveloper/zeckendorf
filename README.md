# Zeckendorf

Todo número inteiro positivo pode ser escrito como soma de Fibonaccis, de **um
jeito só**, desde que não se usem dois Fibonaccis vizinhos. 100 é 89 + 8 + 3, e
não há outra maneira.

Aqui isso não é citado: para cada número até 400, a varredura monta **todas** as
somas de Fibonaccis distintos e conta quantas não têm vizinhos. A resposta é 1,
quatrocentas vezes.

E tem uma segunda metade, que parece não ter nada a ver: um jogo de fichas em que
quem perde é quem recebe uma pilha de Fibonacci. As duas metades se encontram no
fim.

```bash
dotnet test testes/Zeckendorf.Testes/Zeckendorf.Testes.csproj -c Release    # 66 testes
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release
```

## Os primeiros números

```
     n  somas  sem vizinhos a de Zeckendorf    a preguicosa        o guloso acertou
     1      1             1 1                  1                                sim
     2      1             1 2                  2                                sim
     3      2             1 3                  2 + 1                            sim
     4      1             1 3 + 1              3 + 1                            sim
     5      2             1 5                  3 + 2                            sim
     8      3             1 8                  5 + 2 + 1                        sim
    12      1             1 8 + 3 + 1          8 + 3 + 1                        sim
    16      4             1 13 + 3             8 + 5 + 2 + 1                    sim
    20      1             1 13 + 5 + 2         13 + 5 + 2                       sim
```

A coluna `somas` conta **todas** as maneiras de escrever o número somando
Fibonaccis diferentes, com ou sem vizinhos: 1, 1, 2, 1, 2, 2, 1, 3, 2, 2, 3, 1, 3,
3, 2, 4, 2, 3, 3, 1. Ela sobe e desce sem regra aparente. A coluna do lado é
sempre 1.

Os números que têm uma soma só são 1, 4, 7, 12, 20: **cada um é um Fibonacci menos
um**, e isso cai da varredura sem ninguém pedir.

A coluna da preguiçosa é o mesmo exercício de cabeça para baixo: em vez do maior
Fibonacci que cabe, o **menor** que ainda permite terminar. Ela também é única, e
é quase sempre outra: em 100 números, as duas coincidem 9 vezes.

## A unicidade, contada

```
     ate   numeros   com uma so   mais somas  em que n  o guloso sempre acertou
      20        20           20            4        16                      sim
      50        50           50            6        37                      sim
     100       100          100            9        97                      sim
     200       200          200           13       168                      sim
     400       400          400           16       270                      sim
```

Quatrocentos números, quatrocentas representações sem vizinhos, nem uma a mais nem
uma a menos. E o guloso — pegar sempre o maior Fibonacci que cabe, descontar,
repetir — acerta a representação certa nos 400, sem saber que existe um teorema.

A coluna `mais somas` é o contraste: enquanto a unicidade fica parada no 1, o
número **total** de somas não para de crescer.

```
     ate   somas somadas  media por numero   a maior
      20              42              2.10         4
      50             158              3.16         6
     100             414              4.14         9
     200           1,125              5.63        13
     400           2,960              7.40        16
```

Em média 7,4 maneiras de escrever um número até 400, e o campeão tem 16. **A regra
dos vizinhos não corta um pouco do mato: ela corta tudo menos uma folha.**

## O jogo

Uma pilha de n fichas. Quem começa tira quantas quiser, menos a pilha inteira. Daí
em diante, cada um pode tirar no máximo o dobro do que o outro acabou de tirar.
Quem leva a última ficha ganha.

```
  pilhas conferidas                                     59
  em que o minimax concorda com a regra                 59
  pilhas em que quem comeca PERDE                        8
```

O enunciado não fala de Fibonacci em lugar nenhum, e a resposta é inteira sobre
Fibonacci: **quem começa perde exatamente quando a pilha é 2, 3, 5, 8, 13, 21, 34
ou 55**. As oito pilhas perdedoras entre 2 e 60 são os oito Fibonaccis da faixa.

O minimax que calcula isso não conhece a regra. Ele resolve o jogo de trás para a
frente, e a regra só aparece na coluna que confere, concordando nas 59 pilhas.

## Onde as duas metades se encontram

```
  pilhas que nao sao Fibonacci                          32
  em que a menor abertura e a menor parcela             32
```

Numa pilha que não é Fibonacci, quem começa ganha. Mas **tirando quanto?**

A menor jogada vencedora é exatamente a **menor parcela da representação de
Zeckendorf** da pilha. Em 100 fichas, Zeckendorf dá 89 + 8 + 3, e a jogada é tirar
3. Em 32 pilhas conferidas, 32 acertos.

É assim que a estratégia funciona: tirar a menor parcela deixa o adversário com um
Fibonacci (97 = 89 + 8), e o limite do dobro é justamente apertado o suficiente
para ele não conseguir repetir o truque. Duas perguntas que não pareciam ter
relação, e uma é a resposta da outra.

## O que as medidas me corrigiram

**Achei que a preguiçosa coincidisse com a gulosa na maioria.** Em 100 números,
coincidem em 9. Eu tinha escrito "quase sempre a mesma" no comentário do método, e
o teste que eu escrevi para confirmar isso me devolveu o 9. A frase virou "quase
nunca".

**Escrevi a média de cabeça e errei na terceira casa.** 1.125 dividido por 200 é
5,625, e eu tinha prendido 5,63 no teste. O xUnit arredonda 5,625 para 5,62, e o
teste quebrou por uma casa decimal. Não foi perda de tempo: prender média com duas
casas é prender um arredondamento, não um resultado.

**A lista de Fibonacci da base não é a lista de Fibonacci.** A sequência começa 1,
1, 2, 3, 5; a base de Zeckendorf começa 1, 2, 3, 5. Se os dois uns entrarem, todo
número passa a ter mais de uma representação e o teorema morre antes de começar. O
detalhe cabe numa linha de código e é a diferença entre o repositório existir e não
existir.

**Esperava que a unicidade fosse apertando conforme os números crescem.** É o
contrário: o total de representações cresce sem parar, de 2,1 para 7,4 em média, e
a quantidade das que não têm vizinhos fica em 1 o tempo todo. A regra não fica mais
difícil de satisfazer; ela fica mais **seletiva**.

## Os arquivos

| arquivo | o que faz |
|---|---|
| `src/Zeckendorf/Fibonacci.cs` | a base 1, 2, 3, 5, 8, sem o segundo 1 |
| `src/Zeckendorf/Representacao.cs` | todas as somas por varredura, a gulosa, a preguiçosa e o juiz |
| `src/Zeckendorf/Jogo.cs` | o Nim de Fibonacci por minimax, sem nenhuma fórmula |
| `src/Zeckendorf/Medidas.cs` | as cinco tabelas deste README |
| `ferramentas/Medidor` | imprime as tabelas, uma por comando |

## Rodando

```bash
dotnet test testes/Zeckendorf.Testes/Zeckendorf.Testes.csproj -c Release

dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- numeros
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- unicidade
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- crescimento
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- jogo
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- ponte
```

C# 12 e .NET 8. Zero dependências fora do xUnit, e só nos testes.

66 testes. Os números deste README estão presos em `MedidasTestes`.
