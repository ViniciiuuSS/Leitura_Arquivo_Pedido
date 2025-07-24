# Leitura de Arquivos de Pedido 📂

Este é um aplicativo desktop desenvolvido em C# que permite a leitura e processamento de arquivos de pedido no formato `PED_.TXT`. O programa utiliza SQLite para armazenar os dados e WebView2 para exibir uma interface web integrada com informações dos pedidos. 🚀

## Descrição 📝

O aplicativo foi criado para facilitar a importação de arquivos de pedido, validando seu formato (ex.: `PED_.TXT`) e extraindo informações como código do cliente, data e quantidade. Os dados são salvos em um banco de dados SQLite e exibidos em uma tabela dinâmica via WebView2. 📊

## Requisitos ✅

- **Sistema Operacional**: Windows (compatível com o runtime .NET). 🖥️
- **Dependências**:  
  - Runtime do .NET (incluído na versão "Self-Contained", caso contrário, instale o [runtime .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)). ⚙️  
  - Microsoft Edge WebView2 Runtime (instale via [link oficial](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) se não estiver embutido). 🌐  

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
- Uma mensagem confirmará o sucesso ou erro do processo. ✔️

### Visualizar Pedidos 👁️
- Após a inserção, uma nova janela será aberta para visualizar os pedidos processados.  
- A tabela é gerada dinamicamente a partir dos dados salvos. 📈

### Formato do Arquivo 📑
- Certifique-se de que o arquivo segue o padrão `PED_.TXT` e contém as linhas no formato especificado.

## Estrutura do Projeto 🗂️
- `Leitura_Arquivos_Pedido.exe`: Arquivo executável principal. 🖱️  
- `wwwroot/`: Pasta contendo os arquivos HTML, JS e JSON usados pela WebView2 (ex.: `pedidos.json`). 🌐  
- `pedidos.db`: Banco de dados SQLite (será criado automaticamente se não existir). 💾
