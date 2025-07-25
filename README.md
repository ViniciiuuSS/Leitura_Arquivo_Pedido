Em desenvolvimento...
# Leitura de Arquivos de Pedido 📂

Este é um aplicativo desktop desenvolvido em C# que permite a leitura e processamento de arquivos de pedido no formato `PED_.TXT`. O programa utiliza SQLite para armazenar os dados e WebView2 para exibir uma interface web integrada com informações dos pedidos. 🚀 O projeto também inclui uma API separada em ASP.NET Core que gerencia os dados dos pedidos, permitindo consultas e atualizações.

## Descrição 📝

O aplicativo foi criado para facilitar a importação de arquivos de pedido, validando seu formato (ex.: `PED_.TXT`) e extraindo informações como código do cliente, data e quantidade. Os dados são salvos em um banco de dados SQLite e exibidos em uma tabela dinâmica via WebView2. 📊 A API complementa o sistema, retornando todos os pedidos armazenados e permitindo a atualização da quantidade pedida diretamente na tela de detalhes do pedido. A interface web utiliza os plugins **flowbite** (para estilização moderna), **pnotify** (para notificações) e **nprogress** (para barra de progresso) para uma experiência de usuário aprimorada.

## Requisitos ✅

- **Sistema Operacional**: Windows (compatível com o runtime .NET). 🖥️
- **Dependências**:  
  - Runtime do .NET (incluído na versão "Self-Contained", caso contrário, instale o [runtime .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)). ⚙️  
  - Microsoft Edge WebView2 Runtime (instale via [link oficial](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) se não estiver embutido). 🌐  
  - Para a API: Runtime .NET 8.0 e um ambiente de execução ASP.NET Core. 🌐  
  - **Plugins Front-end**: Certifique-se de que os arquivos de `flowbite`, `pnotify` e `nprogress` estão disponíveis em `wwwroot/Content/Plugin/` ou via CDN no código HTML/JS. 🌐

## Uso 🛠️

### Selecionar Arquivo 📥
- Clique em "Selecionar Arquivo" e escolha um arquivo no formato `PED_.TXT`.  
- O arquivo deve conter linhas no formato, por exemplo:  
  - **Cabeçalho**: `01;CNPJ`  
  - **Corpo (Itens podem se repetir, cabeçalho não)**: `02;EAN;QUANTIDADE`  
- O nome do arquivo selecionado será exibido na interface. 📋

### Executar Processamento ▶️
- Após selecionar um arquivo válido, o botão "Executar" será habilitado.  
- Clique em "Executar" para processar o arquivo e salvar os dados no banco de dados.  
- Uma mensagem confirmará o sucesso ou erro do processo, exibida via **pnotify**. ✔️

### Visualizar Pedidos 👁️
- Após a inserção, uma nova janela será aberta para visualizar os pedidos processados.  
- A tabela é gerada dinamicamente a partir dos dados salvos, estilizada com **flowbite**. 📈  
- Clique em um pedido para abrir a tela de detalhes, altere a quantidade desejada e clique em "Alterar" para atualizar via API. O progresso da atualização é mostrado com **nprogress**, e notificações são geradas com **pnotify**. 🔄

### Formato do Arquivo 📑
- Certifique-se de que o arquivo segue o padrão `PED_.TXT` e contém as linhas no formato especificado.

## Estrutura do Projeto 🗂️
- `Leitura_Arquivos_Pedido.exe`: Arquivo executável principal. 🖱️  
- `wwwroot/`: Pasta contendo os arquivos HTML, JS e JSON usados pela WebView2 (ex.: `pedidos.json`), incluindo os plugins **flowbite**, **pnotify** e **nprogress** em `wwwroot/Content/Plugin/`. 🌐  
- `pedidos.db`: Banco de dados SQLite (será criado automaticamente se não existir). 💾  
- **API (separada)**: Projeto ASP.NET Core que gerencia os endpoints para consultar e atualizar pedidos. 🌐

## API 🛠️
A API, hospedada separadamente, oferece os seguintes recursos:
- **GET /Pedido**: Retorna todos os pedidos armazenados no banco de dados SQLite.  
- **PUT /Pedido/{id}**: Atualiza a quantidade pedida de um pedido específico. O corpo da requisição deve conter o objeto `Pedido` com os campos `EAN` e `Quantidade`.  
  - Exemplo de requisição:
    ```json
    {
      "EAN": "7891234567345",
      "Quantidade": 10
    }
