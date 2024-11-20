using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using PJT_CasaBrasil;

public class Atualizador
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string caminhoInstalacao;  // Caminho de instalação do aplicativo
    private readonly Form1 form1;  // Usando o Form1 como preloader

    // URL do lançamento do GitHub para o repositório
    private static readonly string releaseUrl = "https://api.github.com/repos/DougRaf/PJT_CasaBrasil/releases/latest";

    // Construtor que recebe os parâmetros necessários
    public Atualizador(string caminhoInstalacao, Form1 form1)
    {
        this.caminhoInstalacao = caminhoInstalacao;
        this.form1 = form1;
    }

    // Método para atualizar o sistema (arquivos e binários)
    public async Task AtualizarSistemaAsync()
    {
        try
        {
            // Exibe o preloader enquanto a atualização é realizada
            ShowPreloader();

            // Realiza a atualização dos arquivos e binários
            await AtualizarArquivosEBinariosAsync(caminhoInstalacao);
        }
        catch (Exception ex)
        {
            // Exibe erro na UI thread
            ExibirErro($"Erro durante a atualização: {ex.Message}");
        }
        finally
        {
            // Fecha o preloader e reinicia o aplicativo
            FecharPreloader();
            ReiniciarAplicativo();
        }
    }

    // Exibe o formulário de preloader
    private void ShowPreloader()
    {
        if (form1.InvokeRequired)
        {
            form1.Invoke(new Action(() => form1.Show()));
        }
        else
        {
            form1.Show();
        }
    }

    // Fecha o preloader
    private void FecharPreloader()
    {
        if (form1.InvokeRequired)
        {
            form1.Invoke(new Action(() => form1.Close()));
        }
        else
        {
            form1.Close();
        }
    }

    // Exibe mensagem de erro na interface
    private void ExibirErro(string mensagem)
    {
        if (form1.InvokeRequired)
        {
            form1.Invoke(new Action(() => MessageBox.Show(mensagem)));
        }
        else
        {
            MessageBox.Show(mensagem);
        }
    }

    // Atualiza todos os arquivos do aplicativo
    private async Task AtualizarArquivosEBinariosAsync(string caminhoInstalacao)
    {
        try
        {
            // Faz uma requisição para obter a última versão do lançamento
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            HttpResponseMessage response = await client.GetAsync(releaseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse JSON para extrair a URL do arquivo zip contendo os assets
            JObject releaseData = JObject.Parse(responseBody);
            string zipDownloadUrl = releaseData["assets"][0]["browser_download_url"].ToString();

            // Baixar e descompactar os arquivos no diretório de instalação
            await BaixarEDescompactarArquivoAsync(zipDownloadUrl, caminhoInstalacao);
        }
        catch (Exception ex)
        {
            // Exibe erro se houver falha no download ou extração
            ExibirErro($"Erro ao atualizar arquivos: {ex.Message}");
        }
    }

    // Baixa e descompacta o arquivo zip no diretório de instalação
    private static async Task BaixarEDescompactarArquivoAsync(string url, string caminhoDestino)
    {
        try
        {
            // Caminho temporário para salvar o arquivo zip
            string caminhoArquivoZip = Path.Combine(Path.GetTempPath(), "atualizacao.zip");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Baixa o arquivo para o caminho temporário
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (FileStream fs = new FileStream(caminhoArquivoZip, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fs);
            }

            // Descompacta o arquivo no diretório de instalação
            if (File.Exists(caminhoArquivoZip))
            {
                // Descompacta o arquivo zip
                using (ZipArchive archive = ZipFile.OpenRead(caminhoArquivoZip))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string caminhoArquivoDestino = Path.Combine(caminhoDestino, entry.FullName);

                        // Cria diretórios, se necessário
                        if (entry.FullName.EndsWith("/")) // Diretórios não são arquivos e precisam ser criados
                        {
                            Directory.CreateDirectory(caminhoArquivoDestino);
                        }
                        else
                        {
                            // Substitui o arquivo no destino
                            using (Stream entryStream = entry.Open())
                            using (FileStream outputStream = new FileStream(caminhoArquivoDestino, FileMode.Create, FileAccess.Write))
                            {
                                await entryStream.CopyToAsync(outputStream);
                            }
                        }
                    }
                }

                // Deleta o arquivo zip após a descompactação
                File.Delete(caminhoArquivoZip);
            }
        }
        catch (Exception ex)
        {
            // Exibe erro caso haja falha no download ou extração
            MessageBox.Show($"Erro ao baixar ou descompactar o arquivo: {ex.Message}");
        }
    }

    // Método para reiniciar o aplicativo após a atualização
    private static void ReiniciarAplicativo()
    {
        try
        {
            string appPath = Application.ExecutablePath;
            System.Diagnostics.Process.Start(appPath); // Reinicia o aplicativo
            Application.Exit(); // Fecha a aplicação atual
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao tentar reiniciar o aplicativo: {ex.Message}");
        }
    }
}
