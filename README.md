# Lusófona's Trash Picker

## Autores
#### Daniela Dantas, a22202104
- 

#### David Mendes, a22203255
- Implementou as funcionalidades do personagem (Ser instanciado e as suas ações, como mover-se ou apanhar lixo)
- Implementou a mecânica de só se verem as células adjacentes ao personagem
- Fez os controlos para o jogo ser jogado por um utilizador

#### João Correia, a22202506
- Criou a base do projeto Unity (UI, geração de células com chance de ter lixo, parâmetros costumizáveis)
- Efetuou a pesquisa do artigo relacionado com o projeto
- Escreveu o relatório 

## Introdução
Este projeto teve como objetivo a implementação do jogo *Lusófona’s Trash Picker* em Unity, bem como a utilização de um *naïve Bayes classifier* para que um agente de IA possa jogar sozinho ou aprender com as ações de jogadores humanos. O jogo consiste em controlar um personagem, Luso, que começa numa posição aleatória numa grelha de células com tamanho 5x5 por omissão. As células têm 40% de chance por omissão para terem lixo nelas, e o objetivo do jogo é mover o Luso para essas células e apanhar o lixo. O jogador consegue apenas ver a célula em que se encontra, bem como as 4 em seu redor, e tem um limite de ações que pode realizar (20 por omissão), podendo mover-se para uma das 4 células em seu redor (quer seja numa direção aleatória ou escolhida pelo utilizador), ficar no mesmo sítio, ou tentar apanhar lixo na célula em que se encontra. O jogador recebe ou perde pontuação com cada ação, ganhando mais pontos ao apanhar lixo de uma célula com sucesso e perdendo pontos ao tentar apanhar lixo numa célula vazia ou tentando andar contra os limites da grelha, portanto o jogador e a IA devem tentar obter a maior pontuação final possível, podendo esta ser negativa caso se percam demasiados pontos.

A IA deve aprender com os jogos realizados por jogadores humanos, observando as ações realizadas pelos mesmos com base no estado atual do jogo. Como só podem ser vistas 5 células de cada vez, é possível observar um total de 162 estados (considerando a grelha 5x5): o número de estados que podem ser observados num determinado momento consiste em e1 * e2^n, em que e1 é o número de estados possíveis na célula atual, e2 é o número de estados possíveis das células adjacentes, e n é o número dessas células. Neste caso, temos e1 = 2 porque o jogador só pode estar numa célula vazia ou com lixo, e2 = 3 porque as células em redor podem ser vazias, com lixo, ou paredes, e n = 4 porque o jogador consegue apenas ver 4 células em seu redor. Tem-se assim 2 * 3^4 = 162 estados possíveis de jogo. A IA considera cada um destes estados e guarda as ações realizadas por jogadores humanos em cada um, de modo a aprender a melhor forma de obter resultados melhores e mais parecidos com os dados observados dos jogadores humanos. Por exemplo, se o Luso estiver num canto e vir lixo na célula abaixo, um jogador humano irá quase sempre mover-se para baixo, portanto a IA utiliza um *naïve Bayes classifier* para guardar essa ação e aplicá-la quando estiver a jogar e observar esse mesmo estado (acabando assim por mover-se para baixo também).

### Artigo de pesquisa - "*Developing a Video Game Capable of Modeling the User*" por Georgios Paktitis
Como referência para este projeto, foi pesquisada e estudada a tese "*Developing a Video Game Capable of Modeling the User*" escrita por Georgios Paktitis para utilizar como referência e comparação durante o decorrer deste projeto. [Speak more about it and *stuff* and comparisons]

## Metodologia
[Como o jogo foi implementado, algoritmos usados (com imagens e diagramas), tabela de valores parameterizáveis, e descrição da implementação, de forma a que o leitor consiga replicar o processo]

## Resultados e discussão
[Apresentação dos resultados, salientando os melhores resultados que o ser humano consegue obter, e se o agente IA consegue imitar adequadamente o comportamento humano]

[Discussão - interpretação dos resultados observados, resultados inesperados, e hipóteses explicativas destes]

## Conclusões
[relacionar o que foi apresentado na introdução, nomeadamente o problema que se propuseram a resolver, com os resultados obtidos, e como o vosso projeto e a vossa abordagem se relacionam com o artigo selecionado e descrito na introdução.] (Escrever de forma a que uma pessoa que leia introdução + conclusão saiba os detalhes de objetivos, resultados obtidos, descobertas, e conhecimentos adquiridos)

## Referências

#### Artigos
- [formato APA de referência ao artigo]
- [talvez meter também aquele outro documento do stor que tinha lá este mesmo projeto pode ser útil]

#### Assets Utilizados
- Freepik.(a.d.).*Robot icon* [Hand Drawn Colored Image].Flaticon.https://www.flaticon.com/free-icon/robot_1395030
- lemonade.(a.d.).*Soda Can* [Digital Drawn Colored Image].PngTree.https://pngtree.com/freepng/soda-can_8632591.html