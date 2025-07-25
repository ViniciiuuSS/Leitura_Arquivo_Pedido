using Leitura_Arquivos_Pedido.Data;
using Leitura_Arquivos_Pedido.Model;
using Leitura_Arquivos_Pedido.Util;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Text;
using System.Text.Json;

namespace Leitura_Arquivos_Pedido
{
    public partial class LeituraPedidos : Form
    {
        private Button btnSelectFile, btnExecute, btnConsultaPedidos, btnBaixarExemplo;
        private Label lblPath, lblTitle;
        private WebView2 navegador;
        private string selectedFilePath = "";

        public LeituraPedidos()
        {
            InitializeComponent();
            InitializeCustomComponents();

            // Add WebView2 to a Panel or set bounds
            var panel = new Panel { Dock = DockStyle.Fill };
            navegador = new WebView2
            {
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(navegador);
            this.Controls.Add(panel);

            InicializarWebView();
            this.Load += (s, e) =>
            {
                DatabaseService.Inicializar();
                CenterControls(s, e);
            };
        }
        private async void InicializarWebView()
        {
            await navegador.EnsureCoreWebView2Async();

            string outputPath = AppDomain.CurrentDomain.BaseDirectory;
            string wwwrootPath = Path.Combine(outputPath, "wwwroot");
            navegador.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "app.local",
                wwwrootPath,
                CoreWebView2HostResourceAccessKind.Allow);

        }

        private void InitializeCustomComponents()
        {
            int buttonWidth = 200;
            int buttonHeight = 40;
            // Botão: Selecionar Arquivo
            btnSelectFile = new Button
            {
                Text = "Selecionar Arquivo",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            btnSelectFile.Click += BtnSelectFile_Click!;
            Controls.Add(btnSelectFile);

            // Botão: Executar Ação
            btnExecute = new Button
            {
                Text = "Executar",
                Width = buttonWidth,
                Height = buttonHeight,
                Enabled = false
            };
            btnExecute.Click += BtnExecute_Click!;
            Controls.Add(btnExecute);

            // Botão: Executar Ação
            btnConsultaPedidos = new Button
            {
                Text = "Consultar Pedidos",
                Width = buttonWidth,
                Height = buttonHeight,
                Enabled = true
            };
            btnConsultaPedidos.Click += BtnConsulta_Click!;
            Controls.Add(btnConsultaPedidos);

            btnBaixarExemplo = new Button
            {
                Text = "Baixar Modelo .TXT",
                Width = 140,
                Height = 30,
                Top = 10,
                Left = 10
            };
            btnBaixarExemplo.Click += BtnBaixarExemplo_Click!;
            this.Controls.Add(btnBaixarExemplo);

            // Label para mostrar caminho
            lblPath = new Label
            {
                Name = "lblFilePath",
                AutoSize = true,
            };
            Controls.Add(lblPath);

            lblTitle = new Label
            {
                Text = "Adicione um arquivo com nome no formato de PED_.TXT \n\nDentro dele divida em linhas como: \n01;CNPJ \n02;EAN;QUANTIDADE",
                AutoSize = true,
                MaximumSize = new Size(300, 0),
            };
            Controls.Add(lblTitle);

            this.Load += CenterControls!;
            this.Resize += CenterControls!;
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = ofd.FileName;

                if (!selectedFilePath.Contains("PED_") && !selectedFilePath.ToLower().Contains(".TXT"))
                {
                    MessageBox.Show($"Tipo de arquivo inválido.", "Processamento Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var lblPath = Controls["lblFilePath"] as Label;
                lblPath!.Text = $"Selecionado: {Path.GetFileName(selectedFilePath)}";

                foreach (Control ctrl in Controls)
                {
                    if (ctrl is Button btn && btn.Text == "Executar")
                    {
                        btn.Enabled = true;
                        break;
                    }
                }
            }
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            Pedido pedido = Funcitons.ProcessarArquivo(selectedFilePath);
            DatabaseService.InserirArquivo(Path.GetFileName(selectedFilePath), selectedFilePath, DateTime.Now);

            bool pedidoInserido = DatabaseService.InserirPedido(pedido);

            if (pedidoInserido)
            {
                MessageBox.Show("Pedido inserido com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var pedidos = DatabaseService.ObterTodosPedidos();
                string jsonContent = JsonSerializer.Serialize(pedidos, new JsonSerializerOptions { WriteIndented = true });
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "pedidos.json");
                File.WriteAllText(jsonPath, jsonContent);

                var formVisualizar = new VisualizarPedidos();
                formVisualizar.Show();
            }
            else
            {
                MessageBox.Show("Algo deu errado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnConsulta_Click(object sender, EventArgs e)
        {
            var pedidos = DatabaseService.ObterTodosPedidos();
            string jsonContent = JsonSerializer.Serialize(pedidos, new JsonSerializerOptions { WriteIndented = true });
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "pedidos.json");
            File.WriteAllText(jsonPath, jsonContent);

            var formVisualizar = new VisualizarPedidos();
            formVisualizar.Show();
        }
        private void BtnBaixarExemplo_Click(object sender, EventArgs e)
        {
            string exemplo = "01;12345678000195\n02;7891234567897;5";

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Arquivo Texto (*.txt)|*.txt",
                Title = "Salvar Modelo de Arquivo",
                FileName = "PED_Arquivo_Modelo.txt"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveDialog.FileName, exemplo, Encoding.UTF8);
                MessageBox.Show("Modelo de arquivo salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CenterControls(object sender, EventArgs e)
        {
            int centerX = (ClientSize.Width - btnSelectFile.Width) / 2;
            int centerY = (ClientSize.Height - btnSelectFile.Height) / 2;

            btnSelectFile.Location = new System.Drawing.Point(centerX, centerY - 20);
            btnExecute.Location = new System.Drawing.Point(centerX, centerY + 40);
            btnConsultaPedidos.Location = new System.Drawing.Point(centerX, centerY + 160);
            lblPath.Location = new System.Drawing.Point(centerX, centerY + 90);
            lblTitle.Location = new System.Drawing.Point(centerX, centerY - 120);
        }
    }
}
