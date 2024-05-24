# Lusófona's Trash Picker

## Autores
#### Daniela Dantas, a22202104
- Implementou o agente IA
- Implementou DataCollector, que reune TrainingData durante o 'turno' do jogador e envia para o algoritmo (*Naive Bayer Classifier*) após o fim deste
- Implementou o algoritmo *Naive Bayer Classifier* para uso e treino do agente IA
- Documentação do código

#### David Mendes, a22203255
- Implementou as funcionalidades do personagem (Ser instanciado, as suas ações (mover-se e apanhar lixo), e ganhar ou perder pontuação)
- Implementou a mecânica de só se verem as células adjacentes ao personagem
- Fez os controlos para o jogo ser jogado por um utilizador
- Fez o menu inicial do jogo e a Leaderboard

#### João Correia, a22202506
- Criou a base do projeto Unity (elementos de UI, geração da grelha de células, e parâmetros costumizáveis)
- Efetuou a pesquisa do artigo relacionado com o projeto
- Escreveu o relatório 

## Introdução
Este projeto teve como objetivo a implementação do jogo *Lusófona’s Trash Picker* em Unity, bem como a utilização de um *naïve Bayes classifier* para que um agente de IA possa jogar sozinho após aprender com as ações de jogadores humanos. Este jogo 2D consiste em controlar um personagem, Luso, que começa numa posição aleatória numa grelha de células com tamanho 5x5 por omissão. As células têm 40% de chance por omissão para terem lixo nelas, e o objetivo do jogo é mover o Luso para essas células e apanhar o lixo para ganhar pontuação. O jogador apenas consegue ver a célula em que se encontra e as 4 em seu redor, e também tem um limite de ações que pode realizar antes de o jogo acabar (20 por omissão), podendo mover-se para uma das 4 células em seu redor (quer seja numa direção aleatória ou escolhida pelo utilizador), ficar no mesmo sítio, ou tentar apanhar lixo na célula em que se encontra. O jogador recebe ou perde pontuação com cada ação, ganhando 10 pontos ao apanhar lixo de uma célula com sucesso e perdendo 1 ponto ao tentar apanhar lixo numa célula vazia ou 5 pontos ao tentar andar contra os limites da grelha, portanto o jogador e a IA devem tentar obter a maior pontuação final possível, podendo esta ser negativa caso percam demasiados pontos.

A IA deve aprender com os jogos realizados por jogadores humanos, observando as ações realizadas pelos mesmos com base no estado atual do jogo. Como só podem ser vistas 5 células de cada vez, é possível observar um total de 162 estados (considerando apenas os três estados possíveis para cada célula- vazia, com lixo, ou parede/limite do mapa): o número total de estados que podem ser observados num determinado momento consiste em e1 * e2^n, em que e1 é o número de estados possíveis na célula atual, e2 é o número de estados possíveis das células adjacentes, e n é o número dessas células. Neste caso, temos e1 = 2 porque o jogador só pode estar numa célula vazia ou com lixo, e2 = 3 porque as células em redor podem ser vazias, com lixo, ou paredes, e n = 4 porque o jogador consegue apenas ver 4 células em seu redor. Tem-se assim 2 * 3^4 = 2 * 81 = 162 estados possíveis de jogo. A IA considera cada um destes estados e guarda as ações realizadas por jogadores humanos em respota a cada um, de modo a aprender a forma mais eficiente de obter resultados melhores e mais parecidos com os dados observados dos jogadores humanos. Por exemplo, se o Luso estiver num canto e vir lixo na célula abaixo, um jogador humano irá quase sempre mover-se para baixo, portanto a IA utiliza um *naïve Bayes classifier* para guardar essa ação e aplicá-la quando estiver a jogar e observar esse mesmo estado (acabando assim por mover-se para baixo também).

### Artigo de pesquisa - ["*Developing a Video Game Capable of Modeling the User*" por Georgios Paktitis](https://hellanicus.lib.aegean.gr/handle/11610/18887)
Como referência para este projeto, foi pesquisada e estudada a tese de graduação "*Developing a Video Game Capable of Modeling the User*" escrita por Georgios Paktitis, para utilizar como referência e comparação durante o decorrer deste projeto. Este artigo explora a ideia de um jogo 2D que utiliza vários algorítmos diferentes para recolher dados de comportamentos de jogadores humanos enquanto jogam, para poder depois utilizar essa informação e modelar um agente de IA que replica e otimiza as ações que observou dos jogadores, podendo depois comparar o modelo criado por cada um dos algorítmos e descobrir qual deles é o mais preciso. As três técnicas de modelação de agentes de IA usadas nesta tese foram *Decision Trees*, *SVMs (Support Vector Machines)*, e um *naïve Bayes classifier*. Após serem treinadas em parâmetros semelhantes e posteriormente testadas e avaliadas, demonstrou-se que o algoritmo de *Decision Trees* teve a maior precisão (98.11% preciso), seguido pelo algoritmo de SVMs (86.78% preciso), e por fim o de *naïve Bayes classifier* (76.37% preciso). Foram também feitas referências a outras três técnicas e algoritmos, sendo eles *K-NN (k-Nearest Neighbour)*, *Neural Network*, e *Linear Regression*, porém estes não foram usados nem testados para este estudo.

##### Comparação com o projeto
- Este artigo selecionado assemelha-se bastante a este projeto, pois ambos têm como objetivo criar um modelo de um agente de IA que aprende a jogar um jogo com jogadores humanos e replica/simula as suas ações. Salienta-se também que ambos este trabalho e o artigo pesquisado utilizam um *naïve Bayes classifier* para a modelação do agente de IA, tomando nota das ações tomadas pelo jogador com base no estado atual do jogo que é determinado pelos parâmetros que a IA regista, o que torna a semelhança ainda mais aparente pois é a abordagem tomada em ambos os trabalhos. Nota-se também que os dois utilizam um jogo 2D para a implementação deste sistema, ambos com um sistema de pontuação, que ajuda a parametrizar os comportamentos positivos e negativos que o jogador (e, posteriormente, o agente de IA) pode ter durante o jogo.

- Esta tese revela-se uma referência bastante importante para a realização deste projeto, nomeadamente as secções sobre a implementação de um *naïve Bayes classifier* no jogo e o seu respetivo algorítmo, pois isso corresponde com o objetivo deste trabalho e reinforça o conhecimento acerca do tema.

## Metodologia
De modo a atingir os objetivos deste projeto, necessitou-se primeiro de implementar o jogo *Lusófona’s Trash Picker* em Unity antes de se poder começar a treinar a IA. Para isto, criou-se uma grelha em UI que fica no centro do ecrã do jogo, e com base nos parâmetros dados, utilizou-se uma classe GameManager para criar um número de células na grelha de acordo com os números de linhas e colunas dados. As células são também elementos de UI, guardadas como prefab para facilitar a sua instanciação quando o jogo é inicializado, e cada célula contém duas *sprites* para além da *sprite* da própria célula: um pedaço de lixo e uma nuvem de nevoeiro. Ambos começam desligados mas serão depois ligados durante o jogo, mostrando o lixo em células que tiverem lixo e mostrando o nevoeiro em células que não estejam adjacentes ao jogador (para reduzir a visibilidade do jogador e lhe colocar a mesma desvantagem do que a IA).

Voltando ao método de inicialização das células na grelha, implementou-se a percentagem de colocar lixo em cada célula e a escolha da célula aleatória onde o jogador começa, bem como a atualização do nevoeiro que impede que o jogador veja células fora do seu alcance. Depois implementou-se o próprio jogador e as suas ações, permitindo que este se mova pelo mapa, apanhe lixo, e possa passar o turno (não fazer nada). As diferentes opções de ações do jogador foram todas separadas em métodos diferentes, e adicionaram-se também elementos visuais de UI durante o jogo para mostrar a pontuação atual e o número de ações restante do personagem. Adicionou-se também um menu inicial onde se pode decidir se o jogo vai ser jogado por um humano ou pelo AI, ou para visualizar uma *Leaderboard* com as 6 maiores pontuações obtidas no jogo até ao momento. Os controlos do jogador foram tornados não-responsivos caso seja decidido deixar a IA jogar, para impedir que o jogador influencie as suas decisões. Criou-se também um menu de fim de jogo para mostrar a pontuação final quando o jogo termina, e adicioná-la à *Leaderboard* caso seja alta o suficiente para ser lá colocada.

Por fim, para atingir o segundo objetivo do projeto, implementou-se o *naïve Bayes classifier* no agente de IA, fazendo com que, a cada ação do jogador, o *classifier* recebe e regista o estado da célula em que o personagem está atualmente e as células em seu redor, bem como a ação realizada (direção a mexer, apanhar lixo, ou não fazer nada). Depois, quando for a IA a jogar, esta verifica a ação mais escolhida pelo jogador no estado em que se encontra e chama o método dessa ação em resposta. Assim, quantas mais vezes a IA for treinada, mais estados irá reconhecer e terá escolhas mais sólidas acerca de quais as ações a realizar em cada momento.

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
- Código dado em aula (especificamente Aula 10, Projeto Bayes Monsters), usado como template para o algoritmo *Naive Bayer Classifier* 
