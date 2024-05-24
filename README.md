# Lusófona's Trash Picker

## Autores
#### Daniela Dantas, a22202104
- 

#### David Mendes, a22203255
- Implementou as funcionalidades do personagem (Ser instanciado, as suas ações (mover-se e apanhar lixo), e ganhar ou perder pontuação)
- Implementou a mecânica de só se verem as células adjacentes ao personagem
- Fez os controlos para o jogo ser jogado por um utilizador

#### João Correia, a22202506
- Criou a base do projeto Unity (elementos de UI, geração da grelha de células, e parâmetros costumizáveis)
- Efetuou a pesquisa do artigo relacionado com o projeto
- Escreveu o relatório 

## Introdução
Este projeto teve como objetivo a implementação do jogo *Lusófona’s Trash Picker* em Unity, bem como a utilização de um *naïve Bayes classifier* para que um agente de IA possa jogar sozinho ou aprender com as ações de jogadores humanos. Este jogo 2D consiste em controlar um personagem, Luso, que começa numa posição aleatória numa grelha de células com tamanho 5x5 por omissão. As células têm 40% de chance por omissão para terem lixo nelas, e o objetivo do jogo é mover o Luso para essas células e apanhar o lixo para ganhar pontuação. O jogador apenas consegue ver a célula em que se encontra e as 4 em seu redor, e também tem um limite de ações que pode realizar antes de o jogo acabar (20 por omissão), podendo mover-se para uma das 4 células em seu redor (quer seja numa direção aleatória ou escolhida pelo utilizador), ficar no mesmo sítio, ou tentar apanhar lixo na célula em que se encontra. O jogador recebe ou perde pontuação com cada ação, ganhando 10 pontos ao apanhar lixo de uma célula com sucesso e perdendo 1 ponto ao tentar apanhar lixo numa célula vazia ou 5 pontos ao tentar andar contra os limites da grelha, portanto o jogador e a IA devem tentar obter a maior pontuação final possível, podendo esta ser negativa caso percam demasiados pontos.

A IA deve aprender com os jogos realizados por jogadores humanos, observando as ações realizadas pelos mesmos com base no estado atual do jogo. Como só podem ser vistas 5 células de cada vez, é possível observar um total de 162 estados (considerando apenas os três estados possíveis para cada célula- vazia, com lixo, ou parede/limite do mapa): o número total de estados que podem ser observados num determinado momento consiste em e1 * e2^n, em que e1 é o número de estados possíveis na célula atual, e2 é o número de estados possíveis das células adjacentes, e n é o número dessas células. Neste caso, temos e1 = 2 porque o jogador só pode estar numa célula vazia ou com lixo, e2 = 3 porque as células em redor podem ser vazias, com lixo, ou paredes, e n = 4 porque o jogador consegue apenas ver 4 células em seu redor. Tem-se assim 2 * 3^4 = 2 * 81 = 162 estados possíveis de jogo. A IA considera cada um destes estados e guarda as ações realizadas por jogadores humanos em respota a cada um, de modo a aprender a forma mais eficiente de obter resultados melhores e mais parecidos com os dados observados dos jogadores humanos. Por exemplo, se o Luso estiver num canto e vir lixo na célula abaixo, um jogador humano irá quase sempre mover-se para baixo, portanto a IA utiliza um *naïve Bayes classifier* para guardar essa ação e aplicá-la quando estiver a jogar e observar esse mesmo estado (acabando assim por mover-se para baixo também).

### Artigo de pesquisa - ["*Developing a Video Game Capable of Modeling the User*" por Georgios Paktitis](https://hellanicus.lib.aegean.gr/handle/11610/18887)
Como referência para este projeto, foi pesquisada e estudada a tese de graduação "*Developing a Video Game Capable of Modeling the User*" escrita por Georgios Paktitis, para utilizar como referência e comparação durante o decorrer deste projeto. Este artigo explora a ideia de um jogo 2D que utiliza vários algorítmos diferentes para recolher dados de comportamentos de jogadores humanos enquanto jogam, para poder depois utilizar essa informação e modelar um agente de IA que replica e otimiza as ações que observou dos jogadores, podendo depois comparar o modelo criado por cada um dos algorítmos e descobrir qual deles é o mais preciso. As três técnicas de modelação de agentes de IA usadas nesta tese foram *Decision Trees*, *SVMs (Support Vector Machines)*, e um *naïve Bayes classifier*. Após serem treinadas em parâmetros semelhantes e posteriormente testadas e avaliadas, demonstrou-se que o algoritmo de *Decision Trees* teve a maior precisão (98.11% preciso), seguido pelo algoritmo de SVMs (86.78% preciso), e por fim o de *naïve Bayes classifier* (76.37% preciso). Foram também feitas referências a outras três técnicas e algoritmos, sendo eles *K-NN (k-Nearest Neighbour)*, *Neural Network*, e *Linear Regression*, porém estes não foram usados nem testados para este estudo.

##### Comparação com o projeto
- Este artigo selecionado assemelha-se bastante a este projeto, pois ambos têm como objetivo criar um modelo de um agente de IA que aprende a jogar um jogo com jogadores humanos e replica/simula as suas ações. Salienta-se também que ambos este trabalho e o artigo pesquisado utilizam um *naïve Bayes classifier* para a modelação do agente de IA, tomando nota das ações tomadas pelo jogador com base no estado atual do jogo que é determinado pelos parâmetros que a IA regista, o que torna a semelhança ainda mais aparente pois é a abordagem tomada em ambos os trabalhos. Nota-se também que os dois utilizam um jogo 2D para a implementação deste sistema, ambos com um sistema de pontuação, que ajuda a parametrizar os comportamentos positivos e negativos que o jogador (e, posteriormente, o agente de IA) pode ter durante o jogo.

- Esta tese revela-se uma referência bastante importante para a realização deste projeto, nomeadamente as secções sobre a implementação de um *naïve Bayes classifier* no jogo e o seu respetivo algorítmo, pois isso corresponde com o objetivo deste trabalho e reinforça o conhecimento acerca do tema.

## Metodologia
[Como o jogo foi implementado, algoritmos usados (com imagens e diagramas), tabela de valores parameterizáveis, e descrição da implementação, de forma a que o leitor consiga replicar o processo]

## Resultados e discussão
[Apresentação dos resultados, salientando os melhores resultados que o ser humano consegue obter, e se o agente IA consegue imitar adequadamente o comportamento humano]

[Discussão - interpretação dos resultados observados, resultados inesperados, e hipóteses explicativas destes]

## Conclusões
[relacionar o que foi apresentado na introdução, nomeadamente o problema que se propuseram a resolver, com os resultados obtidos, e como o vosso projeto e a vossa abordagem se relacionam com o artigo selecionado e descrito na introdução.] (Escrever de forma a que uma pessoa que leia introdução + conclusão saiba os detalhes de objetivos, resultados obtidos, descobertas, e conhecimentos adquiridos)

## Notas
Devido a uma dúvida sobre a utilização de IAs generativas (ChatGPT) no relatório de um projeto prévio, notou-se a importância de referir o facto de que este documento foi escrito sem qualquer ajuda dessas ferramentas, devendo os créditos da escrita serem atribuídos por completo ao autor (exceto em caso de citações, em que os créditos pertencem aos autores dos artigos/documentos citados).

## Referências

#### Artigos
- Inácio, J., Fachada, N., Matos-Carvalho, J. P., & Fernandes, C. M.(2023). *Humans vs AI: An Exploratory Study With Online and Offline Learners* [Conference paper, Lusófona University].https://scholar.google.com/scholar?hl=en&as_sdt=0%2C5&as_ylo=2010&as_yhi=2024&q=%22Humans+vs+AI%3A+An+Exploratory+Study+With+Online+and+Offline+Learners+%22&btnG=
- Paktitis, G.(2018).*Developing a Video Game Capable of Modeling the User* [Undergraduate Thesis, University of the Aegean]. Department of Information and Communication Systems Engineering.https://hellanicus.lib.aegean.gr/handle/11610/18887

#### Assets Utilizados
- Freepik.(a.d.).*Robot icon* [Hand Drawn Colored Image].Flaticon.https://www.flaticon.com/free-icon/robot_1395030
- lemonade.(a.d.).*Soda Can* [Digital Drawn Colored Image].PngTree.https://pngtree.com/freepng/soda-can_8632591.html