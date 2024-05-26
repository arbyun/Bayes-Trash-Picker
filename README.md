# Lusófona's Trash Picker

## Autores
#### Daniela Dantas, a22202104
- Implementou o agente IA;
- Implementou o algoritmo *Naïve Bayes Classifier* no agente IA;
- Implementou a classe DataCollector, que envia TrainingData do jogador para o *Naïve Bayer Classifier*;
- Documentou o código.

#### David Mendes, a22203255
- Implementou as funcionalidades do personagem (Ser instanciado, as suas ações (mover-se e apanhar lixo), e ganhar ou perder pontuação);
- Implementou a mecânica de só se verem as células adjacentes ao personagem (Fog of War);
- Fez os controlos para o jogo ser jogado por um utilizador;
- Fez o menu inicial do jogo e a Leaderboard.

#### João Correia, a22202506
- Criou a base do projeto Unity (elementos de UI, geração da grelha de células, e parâmetros costumizáveis);
- Efetuou a pesquisa do artigo relacionado com o projeto;
- Escreveu o relatório e criou os diagramas apresentados nele.

## Introdução
Este projeto teve como objetivo a implementação do jogo *Lusófona’s Trash Picker* em Unity, bem como a utilização de um *Naïve Bayes classifier* para que um agente de IA possa jogar sozinho após aprender com as ações de jogadores humanos, replicando os comportamentos que observa. Este jogo 2D consiste em controlar um personagem, Luso, que começa numa posição aleatória numa grelha de células com tamanho 5x5 por omissão. As células têm 40% de chance por omissão para terem lixo nelas, e o objetivo do jogo é mover o Luso para essas células e apanhar o lixo, ganhando pontuação. O jogador apenas consegue ver a célula em que se encontra e as 4 em seu redor, e também tem um limite de ações que pode realizar antes de o jogo acabar (20 por omissão), podendo mover-se para uma das 4 células em seu redor (quer seja numa direção aleatória ou escolhida pelo utilizador), ficar no mesmo sítio, ou tentar apanhar lixo na célula em que se encontra. O jogador recebe ou perde pontuação com cada ação, ganhando 10 pontos ao apanhar lixo de uma célula com sucesso e perdendo 1 ponto ao tentar apanhar lixo numa célula vazia ou 5 pontos ao tentar andar contra os limites da grelha, portanto o jogador e a IA devem tentar obter a maior pontuação final possível, podendo esta ser negativa caso percam demasiados pontos.

A IA deve aprender com os jogos realizados por jogadores humanos, observando as ações realizadas pelos mesmos com base no estado atual do jogo. Como só podem ser vistas 5 células de cada vez, é possível observar um total de 162 estados (considerando apenas os três estados possíveis para cada célula- vazia, com lixo, ou parede/limite do mapa): o número total de estados que podem ser observados num determinado momento consiste em e1 * e2^n, em que e1 é o número de estados possíveis na célula atual, e2 é o número de estados possíveis das células adjacentes, e n é o número dessas células. Neste caso, temos e1 = 2 porque o jogador só pode estar numa célula vazia ou com lixo, e2 = 3 porque as células em redor podem ser vazias, com lixo, ou paredes, e n = 4 porque o jogador consegue apenas ver 4 células em seu redor. Tem-se assim 2 * 3^4 = 2 * 81 = 162 estados possíveis de jogo. A IA considera cada um destes estados e regista as ações realizadas por jogadores humanos em resposta a cada um, de modo a aprender as ações mais prováveis e os comportamentos mais parecidos com os dados observados dos jogadores humanos. Por exemplo, se o Luso estiver num canto e vir lixo na célula abaixo, um jogador humano irá quase sempre mover-se para baixo, portanto a IA utiliza um *Naïve Bayes classifier* para aumentar a probabilidade dessa ação acontecer, tendo assim maior chance de aplicá-la quando estiver a jogar e observar esse mesmo estado (acabando assim por mover-se para baixo também).

### Artigo de pesquisa - ["*Developing a Video Game Capable of Modeling the User*" por Georgios Paktitis](https://hellanicus.lib.aegean.gr/handle/11610/18887)
Como referência para este projeto, foi pesquisada e estudada a tese de graduação "*Developing a Video Game Capable of Modeling the User*" escrita por Georgios Paktitis, para utilizar como referência e comparação durante o decorrer deste projeto. Este artigo explora a ideia de um jogo 2D que utiliza vários algorítmos diferentes para recolher dados de comportamentos de jogadores humanos enquanto jogam, para poder depois utilizar essa informação e modelar um agente de IA que replica e otimiza as ações que observou dos jogadores, podendo depois comparar o modelo criado por cada um dos algorítmos e descobrir qual deles é o mais preciso. As três técnicas de modelação de agentes de IA usadas nesta tese foram *Decision Trees*, *SVMs (Support Vector Machines)*, e um *Naïve Bayes classifier*. Após serem treinadas em parâmetros semelhantes e posteriormente testadas e avaliadas, demonstrou-se que o algoritmo de *Decision Trees* teve a maior precisão (98.11% preciso), seguido pelo algoritmo de SVMs (86.78% preciso), e por fim o de *Naïve Bayes classifier* (76.37% preciso). Foram também feitas referências a outras três técnicas e algoritmos, sendo eles *K-NN (k-Nearest Neighbour)*, *Neural Network*, e *Linear Regression*, porém estes não foram usados nem testados neste estudo.

##### Comparação com o projeto
- Este artigo selecionado assemelha-se bastante a este projeto, pois ambos têm como objetivo criar um modelo de um agente de IA que aprende a jogar um jogo com jogadores humanos e replica/simula as suas ações. Salienta-se também que ambos este trabalho e o artigo pesquisado utilizam um *Naïve Bayes classifier* para a modelação do agente de IA, tomando nota das ações tomadas pelo jogador com base no estado atual do jogo que é determinado pelos parâmetros que a IA regista, o que torna a semelhança ainda mais aparente pois é a abordagem tomada em ambos os trabalhos. Nota-se também que os dois utilizam um jogo 2D para a implementação deste sistema, ambos com um sistema de pontuação, que ajuda a parametrizar os comportamentos positivos e negativos que o jogador (e, posteriormente, o agente de IA) pode ter durante o jogo.

- Esta tese revela-se uma referência bastante importante para a realização deste projeto, nomeadamente as secções sobre a implementação de um *Naïve Bayes classifier* no jogo e o seu respetivo algorítmo, pois isso corresponde com o objetivo deste trabalho e reinforça o conhecimento acerca do tema.

## Metodologia
De modo a atingir os objetivos deste projeto, necessitou-se primeiro de implementar o jogo *Lusófona’s Trash Picker* em Unity antes de se poder começar a treinar o agente IA. Para isto, criou-se uma grelha em UI que fica no centro do ecrã do jogo, e criou-se uma classe GameManager para gerar um número de células na grelha de acordo com um número de linhas e colunas dados como parâmetro, adicionando duas linhas e colunas a esses valores para servirem como limites do mapa. Os parâmetros que esta classe recebe pelo editor são:

| Parâmetro | Descrição |
| ------ | ------ |
| NumOfRows | O número de linhas de células a ser gerado. |
| NumOfCollumns | O número de colunas de células a ser gerado. |
| NumOfSteps | O número de ações que o jogador pode realizar antes de o jogo acabar. |
| ItemPickSuccessScore | O número de pontos adicionado ao score quando se apanha lixo numa célula com lixo. |
| ItemPickFailureScore | O número de pontos adicionado ao score quando se apanha lixo numa célula sem lixo. |
| WallBumpScore | O número de pontos adicionado ao score quando se tenta mover para uma célula que seja parede. |
| ProbabilityOfTrash | A probabilidade das células serem geradas com lixo. Valor varia entre 0 e 100. |

As células da grelha são também elementos de UI, guardadas como prefab para facilitar a sua instanciação quando o jogo é inicializado, e cada célula contém dois ícones para além da imagem da própria célula: um pedaço de lixo, e um quadrado cinzento (nevoeiro) que esconde os conteúdos da célula. Ambos começam desligados, mas serão depois ligados durante o jogo, mostrando o ícone de lixo em células que tiverem lixo, e mostrando o nevoeiro em células que não estejam adjacentes ao jogador (para reduzir a visibilidade do jogador e colocá-lo no mesmo nível do agente IA, já que este também só vai conseguir avaliar a célula em que se encontra e as quatro em seu redor).
Abaixo encontra-se um exemplo do que o jogador consegue ver a cada momento de jogo: Vê a própria célula e as quatro adjacentes, exceto as que forem paredes.

![Imagem da visão do jogo, mostrando a visibilidade das células, e havendo uma que contém lixo](https://i.postimg.cc/g2kMCnPR/gridPNG.png)

Ao inicializar as células, cada uma recebe um nome que corresponde às suas coordenadas para facilitar a sua identificação e permitir que sejam acedidas mais fácilmente, sendo que os valores começam em -1 (por exemplo, a terceira célula da segunda linha terá o nome "(2,0)"), como exemplificado na imagem abaixo. Após todas as células serem inicializadas, a primeira e última de cada linha são convertidas em paredes (ou seja, as células nas duas linhas e colunas que foram adicionadas aos valores iniciais, daí ter-se começado as coordenadas em -1), e as suas imagens são desligadas, permanecendo ativas mas ficando invisíveis. De seguida, as restantes células têm uma chance dada nos parâmetros (40% por omissão) de ficarem com lixo, ligando o ícone do lixo nas mesmas, e por fim é escolhida uma célula aleatória que não seja uma parede para ser a posição inicial do jogador.
As seguintes imagens demonstram, respetivamente, a designação de coordenadas e a visibilidade entre células normais e paredes:

![Esquema da Grelha, explicando as Coordenadas](https://i.postimg.cc/C1NnpL0B/grid-Num-Example-PNG.png)
![Esquema da Grelha, explicando os limites (paredes)](https://i.postimg.cc/2y1k9zSf/grid-Layout-Example-PNG.png)

Depois, implementou-se o próprio jogador e as suas ações através da classe LusoBehaviour, permitindo que o personagem se mova pelo mapa, apanhe lixo, e possa passar o turno (não fazer nada) através de métodos separados para cada respetiva ação. Estes métodos são chamados em outra classe, ControlsManager, a qual trata de receber os comandos do jogador e registá-los para efetuar a ação pretendida, e depois envia essa informação para o agente IA mais tarde. A classe LusoBehaviour observa a célula em que o jogador se encontra, bem como as adjacentes, para depois avaliar o resultado da ação pretendida. Chamando a classe GameManager, o jogador recebe ou perde um valor de pontos igual ao respetivo parâmetro dado no editor quando realiza uma ação com sucesso, como ganhar pontos ao apanhar lixo numa célula que tenha lixo, ou perder pontos ao andar contra uma parede. Em vez de usar um vetor de movimento, o jogador move-se pelo mapa tornando-se num *child* da célula para a qual se pretende deslocar, centrando-se nela, caso seja possível realizar o movimento (o jogador não se pode deslocar para células que sejam paredes, ignorando o movimento caso tente fazê-lo). Quando o jogador realiza o limite de ações por jogo (parametrizável no editor), o jogo termina.
Abaixo encontra-se um esquema que simplifica a forma como as três classes interagem quando o personagem realiza uma ação:

![Flowchart do funcionamento de receção de controlos](https://i.postimg.cc/nLMQftz0/IAControls-Flowchart-drawio-PNG.png)

De seguida, adicionou-se a classe UIManager para mostrar e atualizar elementos visuais de UI durante o jogo, que mostram a pontuação atual e o número de ações restante do jogador. Adicionou-se também um menu inicial onde se pode decidir se o jogo vai ser jogado por um humano ou pelo agente IA, ou para visualizar uma *Leaderboard* com as 6 maiores pontuações obtidas no jogo até ao momento, como mostrado na imagem abaixo. Os controlos do jogador na classe ControlsManager foram tornados não-responsivos quando seja decidido deixar a IA jogar, para impedir que o jogador influencie as suas decisões, e também quando o jogo acaba. Criou-se também um menu de fim de jogo para mostrar a pontuação final quando o jogo termina, e essa pontuação é adicionada à *Leaderboard* caso seja mais alta do que algum valor lá colocado, através de outra classe, ScoreManager, que trata de atualizar esses valores. A *Leaderboard* é mostrada no ecrã quando o menu de fim de jogo é fechado, e volta-se para o menu principal quando a *Leaderboard* é fechada também. As pontuações na *Leaderboard* são ordenadas de forma decrescente usando *LINQ*, mostrando o valor mais alto no topo e o mais pequeno em baixo, tendo-se usado o código de Anthony Pegram no StackOverflow como referência para esta ordenação.

![A Leaderboard do jogo](https://i.postimg.cc/7YqXzfhc/leaderboard-PNG.png)

Por fim, para atingir o segundo objetivo do projeto, implementou-se o *Naïve Bayes classifier* no agente de IA através de uma classe com o mesmo nome (NaiveBayesClassifier), fazendo com que, a cada ação do jogador, o *classifier* recebe o estado da célula em que o personagem está atualmente, bem como as células em seu redor e a ação realizada (direção a mexer, apanhar lixo, ou não fazer nada), através da classe DataCollector, e depois guarda esse conjunto de informação (estado e ação) sob a forma de TrainingData. Depois, se o agente IA for selecionado para jogar, em vez de usar a classe ControlManager, o agente utiliza a classe LusoAIBehaviour para verificar a ação mais escolhida pelo jogador no estado em que se encontra, e chama o método dessa ação em resposta. Assim, quantas mais vezes a IA for treinada, mais estados diferentes irá reconhecer, e terá escolhas mais sólidas acerca de quais as ações a realizar em cada momento.
O diagrama de classes abaixo mostra a relação entre as várias classes que tratam do funcionamento do agente IA:

![Diagrama de classes que representam o agente IA](https://i.postimg.cc/76hvNmKC/IAAgent-Class-Diagram-drawio.png)

A probabilidade de cada ação ser escolhida é um valor que é ordenado usando o método *OrderByDescending* de *LINQ*, pelo que se várias ações tiverem a mesma probabilidade de serem escolhidas pelo agente, ele irá escolher, por defeito, a primeira que registou/aprendeu. É importante notar que o *Naïve Bayes classifier* aprende com as ações do jogador, não se tornando cada vez mais inteligente sozinho. Em vez disso, age de forma cada vez mais parecida com o jogador que observou, o que significa que, dependendo da forma como o jogador humano joga (bem ou mal), o agente IA poderá ou tornar-se mais eficiente, ou acabar por cometer ainda mais erros.

## Resultados e discussão

#### Resultados obtidos
Foram registados nas tabelas abaixo vários resultados entre conjuntos de dez jogos de cada vez, anotando-se periódicamente cinco pontuações finais que o jogador humano obtém, e depois cinco pontuações finais obtidas pelo agente IA, de modo a registar a aprendizagem do agente. A pontuação máxima que pode ser obtida num jogo são 100 pontos, assumindo que todas as células no caminho percorrido têm lixo (incluindo a célula onde o jogador começa), e assumindo que não são recebidas quaisqueres reduções de pontos. Ao repetir o ciclo de apanhar lixo e mover-se para a célula seguinte, o jogador pode apanhar um máximo de 10 peças de lixo, resultando em 10 pontos cada uma, o que dá o total de 100 pontos.

| Número do jogo | Jogador | Pontuação Final | | Número do jogo | Jogador | Pontuação Final | | Número do jogo | Jogador | Pontuação Final |
|-|-|-|-|-|-|-|-|-|-|-|
| 1 | Humano | 90 | | 11 | Humano | 90 | | 21 | Humano | 80 | 
| 2 | Humano | 80 | | 12 | Humano | 80 | | 22 | Humano | 80 |
| 3 | Humano | 80 | | 13 | Humano | 89| | 23 | Humano | 90 |
| 4 | Humano | 70 | | 14 | Humano | 60 | | 24 | Humano | 69 |
| 5 | Humano | 60 | | 15 | Humano | 50 | | 25 | Humano | 70 |
||||||||||||
| 6 | Agente IA | 70 | | 16 | Agente IA | 40 | | 26 | Agente IA | 40 |
| 7 | Agente IA | 30 | | 17 | Agente IA | 70 | | 27 | Agente IA | 30 |
| 8 | Agente IA | 40 | | 18 | Agente IA | 80 | | 28 | Agente IA | 60 |
| 9 | Agente IA | 60 | | 19 | Agente IA | 60 | | 29 | Agente IA | 70 |
| 10 | Agente IA | 80 | | 20 | Agente IA | 60 | | 30 | Agente IA | 70 |

Em todos os testes realizados, a pontuação máxima obtida pelo jogador humano foi 90 pontos, e a pontuação máxima do agente IA foi 80 pontos. Notou-se que o agente IA apresentava um comportamento por vezes repetitivo ao início (por exemplo, alternando entre duas células até acabar o jogo), mas à medida que se realizaram jogos controlados pelo jogador humano, o comportamento do agente IA for-se tornando mais estável e eficiente, tendo apresentado uma melhor execução nos últimos cinco jogos (por exemplo, já não ficou preso entre duas células). Foi também observado que o agente IA foi capaz de imitar o comportamento do jogador, pelo que apanhou sempre o lixo quando se moveu para uma célula com lixo, e não cometeu qualquer erro como mover-se para uma parede ou apanhar lixo numa célula vazia.

#### Discussão
Nos primeiros cinco testes do agente IA, este apresentou um comportamento menos estável devido ao pouco treino recebido, tendo-se baralhado com as intruções aprendidas e ficado preso entre duas células em duas ocasiões. Isto deve-se ao facto de ter escolhido mover-se para uma célula aberta quando se encontrava ao lado de uma parede, e depois escolhia mover-se na direção oposta quando se movia para essa célula, pois eram essas as intruções que aprendeu do jogador nesses estados específicos. Tirando esses problemas, os restantes jogos funcionaram inexperadamente bem, tendo o agente IA conseguido pontuações altas, não tendo cometido qualquer erro e apanhando sempre o lixo que encontrava. Uma possível explicação para estes resultados é a sorte de terem sido geradas células com lixo relativamente próximas umas das outras, permitindo que o agenteIA se movesse seguindo o trilho dessas células e não entrasse num ciclo entre duas vazias.

Já nos segundos cinco testes, o agente IA desempenhou-se bastante bem devido ao treino aprendido, demonstrando já não ficar preso ou qualquer problema semelhante. Contudo, teve algum azar no primeiro teste, não tendo encontrado bastante lixo, resultando na pontuação baixa registada. O seu movimento foi mais fluído, imitando melhor as ações do jogador e conseguindo pontuações altas consistentemente.

Por fim, nos 5 últimos testes, o desempenho do agente tornou-se inexplicávelmente parecido com o comportamento que tinha inicialmente, embora continuasse a prioritizar mover-se para células com lixo e apanhá-lo. O agente voltou a ficar preso em duas ocasiões, e o seu movimento tornou-se mais cíclico e previsível do que fluído, distanciando-se do estilo de jogo do jogador humano. Uma hipótese para este fenómeno é o facto de o agente IA ter dado igual probabilidade a certos comportamentos antigos e novos, pois como referido anteriormente na secção de Metodologia, a ordem de ações com igual prioridade é a ordem de que foram aprendidas, ou seja, o agente voltou a prioritizar os comportamentos antigos por terem probabilidade de escolha igual aos novos comportamentos aprendidos, o que reduziu o seu desempenho. Mesmo assim, conseguiu apresentar pontuações decentemente altas.

## Conclusões
Com a realização deste projeto foi possível criar um agente IA que usa um *Naïve Bayes classifier* para aprender com as ações dos jogadores e replicar os seus comportamentos no jogo que foi implementado- *Lusófona’s Trash Picker*. Após a observação de vários jogos, alternando entre 5 treinos do agente e 5 testes da sua aprendizagem, notou-se o desenvolvimento das capacidades da IA à medida que aprendia com os jogos controlados por um humano, e embora ficasse ocasionalmente baralhada no jogo devido a comportamentos conflituosos, o seu movimento em geral demonstrou várias melhorias entre cada ronda de testes. Como o agente IA aprendeu a replicar os comportamentos que observou, é possível que alguns erros no seu comportamento se devessem aos comandos do jogador humano que observou, e não ao próprio funcionamento do agente. No final, acabou por conseguir obter a segunda pontuação mais alta do jogo (80 pontos), o que já é bastante ótimo e demonstra o sucesso na sua implementação, e é notável referir que o agente IA também conseguiu obter outras pontuações altas quase frequentemente.

Comparativamente ao estudo realizado por Georgios Paktitis, ambos estes trabalhos conseguiram criar um modelo de um agente de IA que aprende a jogar um jogo através dos comportamentos de jogadores humanos, tendo ambos conseguido replicar com sucesso as ações que observaram, formando um modelo do jogador observado com alta (mas imperfeita) precisão. A abordagem deste trabalho seguiu um caminho semelhante ao estudo de Paktitis, pois ambos apresentaram a utilização de um *Naïve Bayes classifier* para registar os comportamentos observados e utilizar esses dados para determinar a probabilidade de certas ações serem escolhidas em vez de outras, modelando assim um sistema que determina qual a ação mais provável de acontecer em cada determinado estado de jogo, com base no(s) jogador(es) estudados. A maior diferença entre este projeto e o artigo selecionado são os seus objetivos, pois Paktitis pretendeu criar vários modelos de jogadores usando diferentes técnicas- *Decision Trees*, *SVMs*, e um *Naïve Bayes classifier*-, enquanto este trabalho se focou apenas numa delas (o *Naïve Bayes classifier*). 

O jogo estudado no artigo de Georgios Paktitis é considerávelmente mais complexo, tendo assim bastantes mais parâmetros e variáveis de teste que permitiram obter resultados mais específicos e precisos sobre os modelos de IA que criou, porém este projeto foi também capaz de capturar os parâmetros necessários para a determinar a precisão da replicação dos comportamentos do jogador humano. Contudo, embora o foco deste trabalho tenha sido apenas a utilização de um *Naïve Bayes classifier*, os resultados do estudo de Paktitis levantam a questão: será que a utilização de um algorítmo de modelação de IA diferente poderia apresentar resultados mais precisos/eficientes, comparativamente aos que foram obtidos nos testes do agente IA atual? Isto é um tópico que poderia ser explorado em futuras iterações deste projeto, talvez visando alcançar um modelo de agente IA que consiga replicar melhor/mais precisamente as ações dos jogadores que observa.

## Notas
Devido a uma dúvida sobre a utilização de IAs generativas (ChatGPT) no relatório de um projeto prévio, notou-se a importância de referir o facto de que este documento foi escrito sem qualquer ajuda dessas ferramentas, devendo os créditos da escrita serem atribuídos por completo ao autor (exceto em caso de citações, em que os créditos pertencem aos autores dos artigos/documentos citados).

## Referências

#### Artigos
- Inácio, J., Fachada, N., Matos-Carvalho, J. P., & Fernandes, C. M.(2023). *Humans vs AI: An Exploratory Study With Online and Offline Learners* [Conference paper, Lusófona University].https://scholar.google.com/scholar?hl=en&as_sdt=0%2C5&as_ylo=2010&as_yhi=2024&q=%22Humans+vs+AI%3A+An+Exploratory+Study+With+Online+and+Offline+Learners+%22&btnG=
- Paktitis, G.(2018).*Developing a Video Game Capable of Modeling the User* [Undergraduate Thesis, University of the Aegean]. Department of Information and Communication Systems Engineering.https://hellanicus.lib.aegean.gr/handle/11610/18887

#### Assets Utilizados
- caryblade.(a.d.).*School infrastructure. View from above. Vector illustration.* [Digital Drawn Colored Image].AdobeStock.https://stock.adobe.com/br/images/school-infrastructure-view-from-above-vector-illustration/169282551
- Freepik.(a.d.).*Robot icon* [Hand Drawn Colored Image].Flaticon.https://www.flaticon.com/free-icon/robot_1395030
- lemonade.(a.d.).*Soda Can* [Digital Drawn Colored Image].PngTree.https://pngtree.com/freepng/soda-can_8632591.html

#### Código Externo
- Anthony Pegram - Ordenação de listas de forma decrescente (https://stackoverflow.com/questions/3062513/how-can-i-sort-generic-list-desc-and-asc)
- Nuno Fachada - Projeto Bayes Monsters e código dado em aula relativo ao algoritmo *Naïe Bayes Classifier*