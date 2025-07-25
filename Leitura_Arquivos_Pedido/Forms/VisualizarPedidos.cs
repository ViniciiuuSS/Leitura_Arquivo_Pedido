using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows.Forms;

public class VisualizarPedidos : Form
{
    private WebView2 navegador;

    public VisualizarPedidos()
    {
        this.Text = "Visualização de Pedidos";
        this.Width = 800;
        this.Height = 600;

        navegador = new WebView2();
        navegador.Dock = DockStyle.Fill;
        this.Controls.Add(navegador);

        Load += async (s, e) => await InicializarWebView();
    }

    private async Task InicializarWebView()
    {
        try
        {
            // Inicializa o WebView2
            await navegador.EnsureCoreWebView2Async(null);

            string outputPath = AppDomain.CurrentDomain.BaseDirectory;
            string wwwrootPath = Path.Combine(outputPath, "wwwroot");

            // Validação da pasta wwwroot
            if (!Directory.Exists(wwwrootPath))
            {
                System.Windows.Forms.MessageBox.Show($"Pasta {wwwrootPath} não encontrada.");
                return;
            }

            // Configura o mapeamento virtual
            navegador.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "app.local",
                wwwrootPath,
                CoreWebView2HostResourceAccessKind.Allow);

            // Validação do arquivo HTML
            string htmlPath = Path.Combine(wwwrootPath, "ListaPedidos.html");
            if (!File.Exists(htmlPath))
            {
                System.Windows.Forms.MessageBox.Show($"Arquivo {htmlPath} não encontrado.");
                return;
            }

            // Define a fonte como URL virtual
            navegador.Source = new Uri($"http://app.local/ListaPedidos.html");

            // Adiciona um listener para capturar erros de navegação (opcional)
            navegador.NavigationCompleted += (sender, e) =>
            {
                if (!e.IsSuccess)
                {
                    System.Windows.Forms.MessageBox.Show($"Falha na navegação: {e.WebErrorStatus}");
                }
            };
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show($"Erro ao inicializar WebView2: {ex.Message}");
        }
    }
}
