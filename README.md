# SSSA
South System Sales Analytics

É um sistema de análise de dados de venda que extrai dados de uma fonte, transforma-os e produz relatórios baseados nesses dados.

## Descrição técnica

- O sistema foi criado em [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1);
- É possível separar a aplicação em 3 partes: Apps, Services e Building blocks. Em Apps se econtram as aplicações/projetos que vão gerar o executável de fato.
Em Services encontramos as regras de negócio e como executá-las. Building blocks servem apenas de auxílio às outras partes;

<img src="https://res.cloudinary.com/appinova/image/upload/v1598189940/Pessoal/SSSA.png" alt="SSSA architecture" border="0" />

- O programa observa arquivos criados em um diretório (o caminho do diretório pode ser definido no arquivo de configurações appsettings.json), que por padrão é
"SSSA\src\Apps\SSSA.App.Worker\data\in"; 
- Caso já existam arquivos na pasta, é perguntado ao usuário se ele deseja fazer a geração retroativa de relatórios baseados neles;
- Um gatilho é disparado quando um arquivo for criado na pasta definida. Esse gatilho envia um comando para o **IMediatorHandler** ([mediator pattern](https://pt.wikipedia.org/wiki/Mediator)), que definirá 
quem receberá esse comando, no caso será **EtlCommandHandler**;
- EtlCommandHandler orquestra as classes de domínio: **Extractor**, **Transformer**, **Loader** para produzir um relatório;
- Cada classe dessas pode ter seu comportamento configurado por meio de [estratégias](https://pt.wikipedia.org/wiki/Strategy). Para o problema proposto, por exemplo,
a classe Extractor foi configurada da seguinte maneira:

    - A fonte dos dados é um arquivo de texto;
    - Os dados devem ser separados por um texto, "ç" foi o escolhido;
    - A identificação das entidades é baseada em uma das propriedades separadas anteriormente, no caso a primeira, index 0;
    - A separação dos itens de uma venda também tem uma estratégia própia, separação por texto foi a escolhida, onde, itens separados por "," e suas propriedades por "-".

- A classe Loader é responsável por salvar os arquivos gerados, atualmente, o destino é um arquivo de texto. O caminho do diretório destino pode ser alterado no arquivo de
configurações appsettings.json), que por padrão é "SSSA\src\Apps\SSSA.App.Worker\data\out"
- Existem projetos de teste para as principais partes dos outros projetos.
