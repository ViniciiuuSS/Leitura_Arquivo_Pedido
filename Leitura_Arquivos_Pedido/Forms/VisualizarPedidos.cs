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
        await navegador.EnsureCoreWebView2Async();

        string outputPath = AppDomain.CurrentDomain.BaseDirectory;
        string htmlPath = Path.Combine(outputPath, "wwwroot", "ListaPedidos.html");

        navegador.CoreWebView2.SetVirtualHostNameToFolderMapping(
            "app.local",
            Path.Combine(outputPath, "wwwroot"),
            CoreWebView2HostResourceAccessKind.Allow);

        navegador.Source = new Uri(htmlPath);
    }
}
