# Leitura de Arquivos de Pedido

Este é um aplicativo desktop desenvolvido em C# que permite a leitura e processamento de arquivos de pedido no formato `PED_.TXT`. O programa utiliza SQLite para armazenar os dados e WebView2 para exibir uma interface web integrada com informações dos pedidos.

## Descrição

O aplicativo foi criado para facilitar a importação de arquivos de pedido, validando seu formato (ex.: `PED_.TXT`) e extraindo informações como código do cliente, data e quantidade. Os dados são salvos em um banco de dados SQLite e exibidos em uma tabela dinâmica via WebView2.

## Requisitos

- **Sistema Operacional**: Windows (compatível com o runtime .NET).
- **Dependências**:
  - Runtime do .NET (incluído na versão "Self-Contained", caso contrário, instale o [runtime .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)).
  - Microsoft Edge WebView2 Runtime (instale via [link oficial](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) se não estiver embutido).

## Instalação

1. Baixe o arquivo compactado (`publish.zip`) na seção de [Releases](https://github.com/seu-usuario/seu-repositorio/releases) deste repositório.
2. Extraia o conteúdo do arquivo `.zip` para uma pasta de sua escolha.
3. Execute o arquivo `Leitura_Arquivos_Pedido.exe` localizado na pasta extraída.
4. (Opcional) Se solicitado, instale o runtime do WebView2 seguindo o link acima.

## Uso

1. **Selecionar Arquivo**:
   - Clique em "Selecionar Arquivo" e escolha um arquivo no formato `PED_.TXT`.
   - O arquivo deve conter linhas no formato, por exemplo:
     - `01;CNPJ`
     - `02;EAN;QUANTIDADE`
   - O nome do arquivo selecionado será exibido na interface.

2. **Executar Processamento**:
   - Após selecionar um arquivo válido, o botão "Executar" será habilitado.
   - Clique em "Executar" para processar o arquivo e salvar os dados no banco de dados.
   - Uma mensagem confirmará o sucesso ou erro do processo.

3. **Visualizar Pedidos**:
   - Após a inserção, uma nova janela será aberta para visualizar os pedidos processados.
   - A tabela é gerada dinamicamente a partir dos dados salvos.

4. **Formato do Arquivo**:
   - Certifique-se de que o arquivo segue o padrão `PED_.TXT` e contém as linhas no formato especificado.

## Estrutura do Projeto

- `Leitura_Arquivos_Pedido.exe`: Arquivo executável principal.
- `wwwroot/`: Pasta contendo os arquivos HTML, JS e JSON usados pela WebView2 (ex.: `pedidos.json`).
- `pedidos.db`: Banco de dados SQLite (será criado automaticamente se não existir).

## Contribuições

Sinta-se à vontade para abrir issues ou pull requests neste repositório para sugestões, correções ou melhorias. Contribuições são bem-vindas!

## Licença

[Especifique a licença, ex.: MIT, GPL, ou "Sem licença explícita" se não aplicável.] Este projeto pode ser usado livremente, mas qualquer uso comercial ou distribuição deve seguir os termos da licença escolhida.

## Contato

Para dúvidas ou suporte, entre em contato pelo [seu e-mail ou link] ou abra uma issue neste repositório.

---

*Última atualização: 23 de julho de 2025, 22:03 (-03)*
