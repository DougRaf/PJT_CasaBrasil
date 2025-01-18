using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web.UI.WebControls;

namespace PJT_CasaBrasil
{
    public partial class Form5 : Form
    {
        // Base da string de conexão
        private string connectionString = "Server=localhost; Database=casabrasil; Uid=root; Pwd=root;";
        private double posX;
        private double posY;

        public Form5()
        {
            InitializeComponent();
            // Configurações para o formulário
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Desabilita redimensionamento
            this.StartPosition = FormStartPosition.CenterScreen; // Abre centralizado na tela
            this.WindowState = FormWindowState.Maximized;    // Configura o formulário para abrir maximizado
            CentralizarPanel(panel1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Consulta SQL para obter registros do último dia
                        string query = "SELECT * FROM `vendas` WHERE DATE(dataHoraOperacao) = CURDATE()";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                // Configura o DataGridView
                                ConfigureDataGridView(dataGridView1, dataTable);
                            }
                        }
                    }
                    catch (MySqlException sqlEx)
                    {
                        MessageBox.Show("Erro de banco de dados: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro inesperado: " + ex.Message);
                    }
                }
            }
            else
            {
                // Limpa os dados do DataGridView
                dataGridView1.DataSource = null;
            }
        }

        private void ConfigureDataGridView(DataGridView dgv, DataTable data)
        {
            dgv.DataSource = data;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajusta automaticamente as colunas
            dgv.ReadOnly = true; // Configura o DataGridView como somente leitura
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Seleção de linha completa

            // Remove a coluna "id" programaticamente, se ela existir
            if (dgv.Columns.Contains("id"))
            {
                dgv.Columns["id"].Visible = false;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Verifica se há dados no DataGridView
            if (dataGridView1.Rows.Count == 0 || dataGridView1.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Não há dados disponíveis para gerar o relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                FileName = "Relatorio_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfDocument pdfDoc = new PdfDocument();
                    PdfPage page = pdfDoc.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    XFont titleFont = new XFont("Arial", 12);
                    XFont font = new XFont("Arial", 7);

                    string logoPath = "C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\casabrasil.png";
                    if (System.IO.File.Exists(logoPath))
                    {
                        XImage logoImage = XImage.FromFile(logoPath);
                        gfx.DrawImage(logoImage, 50, 40, 55, 55);
                    }

                    gfx.DrawString("Relatório de Vendas", titleFont, XBrushes.Black, new XPoint(120, 65));

                    // Ajuste do deslocamento inicial
                    double startX = 30;
                    double startY = 120;
                    double currentY = startY;

                    // Larguras ajustadas
                    double[] columnWidths = { 80, 70, 80, 60, 70, 60, 120 };
                    string[] headers = { "Código Barras", "Pagamento", "Total C/ Imp.", "Imp. Valor", "Total S/ Imp.", "Imp. %", "Data/Hora Operação" };

                    // Cabeçalhos
                    for (int i = 0; i < headers.Length; i++)
                    {
                        gfx.DrawString(headers[i], font, XBrushes.Black, new XPoint(startX, currentY));
                        startX += columnWidths[i];
                    }

                    currentY += 20;

                    int totalComImposto = 0, totalSemImposto = 0, totalImposto = 0;
                    int total8Percent = 0, total10Percent = 0;
                    int totalCredito = 0, totalDinheiro = 0, totalPrazo = 0; int totalPercentColumn = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        startX = 30;
                        foreach (var columnIndex in new[] {
                    "codigoBarras", "pagamento", "totalComImposto", "impostoValor", "totalSemImposto", "impostoPorcentagem", "dataHoraOperacao"
                })
                        {
                            string cellValue = row.Cells[columnIndex]?.Value?.ToString() ?? string.Empty;
                            gfx.DrawString(cellValue, font, XBrushes.Black, new XPoint(startX, currentY));
                            startX += columnWidths[Array.IndexOf(new[] {
                        "codigoBarras", "pagamento", "totalComImposto", "impostoValor", "totalSemImposto", "impostoPorcentagem", "dataHoraOperacao"
                    }, columnIndex)];
                        }

                        // Cálculos dos totais
                        int valorComImposto = Convert.ToInt32(row.Cells["totalComImposto"].Value ?? 0);  // Ex: 500
                        int valorSemImposto = Convert.ToInt32(row.Cells["totalSemImposto"].Value ?? 0);  // Ex: 460
                        int impostoValor = Convert.ToInt32(row.Cells["impostoValor"].Value ?? 0);  // Ex: 40
                        int impostoPorcentagem = Convert.ToInt32(row.Cells["impostoPorcentagem"].Value ?? 0);  // Ex: 8

                        totalComImposto += valorComImposto;
                        totalSemImposto += valorSemImposto;
                        totalImposto += impostoValor;
                        totalPercentColumn += impostoPorcentagem;

                        // Verifica os percentuais de imposto e soma a porcentagem
                        if (impostoValor == 8)
                        {
                            total8Percent += impostoValor;  // Soma a porcentagem de 8%
                        }
                        else if (impostoValor == 10)
                        {
                            total10Percent += impostoValor;  // Soma a porcentagem de 10%
                        }

                        // Soma por tipo de pagamento
                        string pagamento = row.Cells["pagamento"]?.Value?.ToString() ?? string.Empty;
                        if (pagamento.Equals("Crédito", StringComparison.OrdinalIgnoreCase))
                            totalCredito += valorComImposto;
                        if (pagamento.Equals("Dinheiro", StringComparison.OrdinalIgnoreCase))
                            totalDinheiro += valorComImposto;
                        if (pagamento.Equals("Prazo", StringComparison.OrdinalIgnoreCase))
                            totalPrazo += valorComImposto;

                        currentY += 15;
                    }

                    // Linha separadora
                    currentY += 10;
                    gfx.DrawLine(XPens.Black, 30, currentY, page.Width - 30, currentY);
                    currentY += 20;

                    // Título "Total"
                    gfx.DrawString("Total", titleFont, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 20;

                    // Totais em linhas separadas
                    gfx.DrawString("Total Sem Imposto: ¥" + totalSemImposto, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Com Imposto: ¥" + totalComImposto, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto Cobrado: ¥" + totalPercentColumn, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto (8%): " + total8Percent + "%", font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto (10%): " + total10Percent + "%", font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total em Crédito: ¥" + totalCredito, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total em Dinheiro: ¥" + totalDinheiro, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total a Prazo: ¥" + totalPrazo, font, XBrushes.Black, new XPoint(30, currentY));

                    // Footer com data, hora e local
                    currentY = page.Height - 50;
                    string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    gfx.DrawLine(XPens.Black, 30, currentY - 10, page.Width - 30, currentY - 10);
                    gfx.DrawString("Data e Hora: " + dataHora, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Komatsu, Ishikawa-ken, Japão", font, XBrushes.Black, new XPoint(30, currentY));

                    pdfDoc.Save(saveFileDialog.FileName);
                    MessageBox.Show("Relatório salvo como PDF com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao gerar o relatório: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        






        // Função para adicionar o rodapé
        private void AddFooter(XGraphics gfx, PdfPage page)
        {
            double footerY = page.Height - 50;
            XFont font = new XFont("Arial", 7);

            string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            gfx.DrawLine(XPens.Black, 30, footerY - 10, page.Width - 30, footerY - 10);
            gfx.DrawString("Data e Hora: " + dataHora, font, XBrushes.Black, new XPoint(30, footerY));
            gfx.DrawString("Komatsu, Ishikawa-ken, Japão", font, XBrushes.Black, new XPoint(30, footerY + 15));
        }




        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = ".csv",
                FileName = "Relatorio_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv" // Nome do arquivo com data/hora
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Criando o arquivo CSV com codificação UTF-8 para suportar caracteres acentuados
                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        // Adicionando título e data
                        writer.WriteLine("Relatório de Vendas");
                        writer.WriteLine("Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        writer.WriteLine(new string('-', 80)); // Linha separadora

                        // Cabeçalhos
                        writer.WriteLine(
                            $"{AddSpacing("Código Barras")},{AddSpacing("Pagamento")},{AddSpacing("Total C/ Imp.")},{AddSpacing("Imp. Valor")},{AddSpacing("Total S/ Imp.")},{AddSpacing("Imp. %")},{AddSpacing("Data/Hora Operação")}"
                        );

                        int totalComImposto = 0, totalSemImposto = 0, totalImposto = 0;
                        int total8Percent = 0, total10Percent = 0;
                        int totalCredito = 0, totalDinheiro = 0, totalPrazo = 0;
                        int totalPercentColumn = 0;

                        // Dados
                        foreach (DataGridViewRow dataRow in dataGridView1.Rows)
                        {
                            if (dataRow.IsNewRow) continue;

                            try
                            {
                                string codigoBarras = dataRow.Cells["codigoBarras"]?.Value?.ToString() ?? string.Empty;
                                string pagamento = dataRow.Cells["pagamento"]?.Value?.ToString() ?? string.Empty;

                                // Validação e conversão dos valores
                                int totalComImpostoValue = 0;
                                if (!int.TryParse(dataRow.Cells["totalComImposto"]?.Value?.ToString(), out totalComImpostoValue))
                                    totalComImpostoValue = 0;

                                int impostoValor = 0;
                                if (!int.TryParse(dataRow.Cells["impostoValor"]?.Value?.ToString(), out impostoValor))
                                    impostoValor = 0;

                                int totalSemImpostoValue = 0;
                                if (!int.TryParse(dataRow.Cells["totalSemImposto"]?.Value?.ToString(), out totalSemImpostoValue))
                                    totalSemImpostoValue = 0;

                                int impostoPorcentagem = 0;
                                if (!int.TryParse(dataRow.Cells["impostoPorcentagem"]?.Value?.ToString(), out impostoPorcentagem))
                                    impostoPorcentagem = 0;

                                string dataHoraOperacao = dataRow.Cells["dataHoraOperacao"]?.Value?.ToString() ?? string.Empty;

                                // Escrever dados com espaçamento
                                writer.WriteLine(
                                    $"{AddSpacing(codigoBarras)},{AddSpacing(pagamento)},{AddSpacing(totalComImpostoValue.ToString())},{AddSpacing(impostoValor.ToString())},{AddSpacing(totalSemImpostoValue.ToString())},{AddSpacing(impostoPorcentagem.ToString())},{AddSpacing(dataHoraOperacao)}"
                                );

                                // Atualizar totais
                                totalComImposto += totalComImpostoValue;
                                totalSemImposto += totalSemImpostoValue;
                                totalImposto += impostoValor;
                                totalPercentColumn += impostoPorcentagem;

                                if (impostoValor == 8)
                                {
                                    total8Percent += impostoValor;
                                }
                                else if (impostoValor == 10)
                                {
                                    total10Percent += impostoValor;
                                }

                                int totalPagamento = totalComImpostoValue;
                                if (pagamento.Equals("Crédito", StringComparison.OrdinalIgnoreCase))
                                {
                                    totalCredito += totalPagamento;
                                }
                                else if (pagamento.Equals("Dinheiro", StringComparison.OrdinalIgnoreCase))
                                {
                                    totalDinheiro += totalPagamento;
                                }
                                else if (pagamento.Equals("Prazo", StringComparison.OrdinalIgnoreCase))
                                {
                                    totalPrazo += totalPagamento;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Erro ao processar a linha: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        // Linha separadora
                        writer.WriteLine();
                        writer.WriteLine(new string('-', 80));

                        // Título Totais de Vendas
                        writer.WriteLine("Totais de Vendas:");

                        // Totais
                        writer.WriteLine($"Total Sem Imposto,¥{totalSemImposto}");
                        writer.WriteLine($"Total Com Imposto,¥{totalComImposto}");
                        writer.WriteLine($"Total Imposto (8%),{total8Percent}%");
                        writer.WriteLine($"Total Imposto (10%),{total10Percent}%");
                        writer.WriteLine($"Total Imposto Cobrado,¥{totalImposto}");
                        writer.WriteLine($"Total Porcentagem Imposto,{totalPercentColumn}");
                        writer.WriteLine($"Total em Crédito,¥{totalCredito}");
                        writer.WriteLine($"Total em Dinheiro,¥{totalDinheiro}");
                        writer.WriteLine($"Total a Prazo,¥{totalPrazo}");

                        MessageBox.Show("Relatório salvo como CSV com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar os dados em CSV: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Função auxiliar para adicionar espaçamento
        private string AddSpacing(string text, int width = 20)
        {
            return text.PadRight(width).Replace(",", " "); // Evita vírgulas nos dados
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Obtém a data selecionada no DateTimePicker
            DateTime dataSelecionada = dateTimePicker1.Value.Date;

            // Filtra os dados no DataGridView com base na data selecionada
            FiltrarDadosPorData(dataSelecionada);

            // Pergunta ao usuário se deseja salvar o relatório
            DialogResult resultado = MessageBox.Show("Deseja salvar o relatório em PDF?", "Salvar Relatório", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // Chama a função para gerar o relatório em PDF com os dados filtrados
                GerarRelatorioPDF(dataSelecionada);
            }
        }

        private void FiltrarDadosPorData(DateTime dataSelecionada)
        {
            // Conexão com o banco de dados
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Consulta SQL para obter registros com base na data selecionada
                    string query = "SELECT * FROM `vendas` WHERE DATE(dataHoraOperacao) = @DataSelecionada";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Adiciona o parâmetro para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@DataSelecionada", dataSelecionada);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Configura o DataGridView
                            ConfigureDataGridView(dataGridView1, dataTable);

                   
                        }
                    }
                }
                catch (MySqlException sqlEx)
                {
                    MessageBox.Show("Erro de banco de dados: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado: " + ex.Message);
                }
            }
        }

        private void CentralizarPanel(Control panel)
        {
            // Obtém o contêiner pai (geralmente o Form ou outro painel)
            Control parent = panel.Parent;

            if (parent != null)
            {
                // Calcula a posição central
                int x = (parent.ClientSize.Width - panel.Width) / 2;
                int y = (parent.ClientSize.Height - panel.Height) / 2;

                // Define a nova posição do painel
                panel.Location = new Point(x, y);
            }
        }


        private void GerarRelatorioPDF(DateTime dataSelecionada)
        {

            // Verifica se há dados no DataGridView
            if (dataGridView1.Rows.Count == 0 || dataGridView1.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Não há dados disponíveis para gerar o relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                FileName = "Relatorio_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfDocument pdfDoc = new PdfDocument();
                    PdfPage page = pdfDoc.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    XFont titleFont = new XFont("Arial", 12);
                    XFont font = new XFont("Arial", 7);

                    string logoPath = "C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\casabrasil.png";
                    if (System.IO.File.Exists(logoPath))
                    {
                        XImage logoImage = XImage.FromFile(logoPath);
                        gfx.DrawImage(logoImage, 50, 40, 55, 55);
                    }

                    gfx.DrawString("Relatório de Vendas", titleFont, XBrushes.Black, new XPoint(120, 65));

                    // Ajuste do deslocamento inicial
                    double startX = 30;
                    double startY = 120;
                    double currentY = startY;

                    // Larguras ajustadas
                    double[] columnWidths = { 80, 70, 80, 60, 70, 60, 120 };
                    string[] headers = { "Código Barras", "Pagamento", "Total C/ Imp.", "Imp. Valor", "Total S/ Imp.", "Imp. %", "Data/Hora Operação" };

                    // Cabeçalhos
                    for (int i = 0; i < headers.Length; i++)
                    {
                        gfx.DrawString(headers[i], font, XBrushes.Black, new XPoint(startX, currentY));
                        startX += columnWidths[i];
                    }

                    currentY += 20;

                    int totalComImposto = 0, totalSemImposto = 0, totalImposto = 0;
                    int total8Percent = 0, total10Percent = 0;
                    int totalCredito = 0, totalDinheiro = 0, totalPrazo = 0; int totalPercentColumn = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        startX = 30;
                        foreach (var columnIndex in new[] {
                    "codigoBarras", "pagamento", "totalComImposto", "impostoValor", "totalSemImposto", "impostoPorcentagem", "dataHoraOperacao"
                })
                        {
                            string cellValue = row.Cells[columnIndex]?.Value?.ToString() ?? string.Empty;
                            gfx.DrawString(cellValue, font, XBrushes.Black, new XPoint(startX, currentY));
                            startX += columnWidths[Array.IndexOf(new[] {
                        "codigoBarras", "pagamento", "totalComImposto", "impostoValor", "totalSemImposto", "impostoPorcentagem", "dataHoraOperacao"
                    }, columnIndex)];
                        }

                        // Cálculos dos totais
                        int valorComImposto = Convert.ToInt32(row.Cells["totalComImposto"].Value ?? 0);  // Ex: 500
                        int valorSemImposto = Convert.ToInt32(row.Cells["totalSemImposto"].Value ?? 0);  // Ex: 460
                        int impostoValor = Convert.ToInt32(row.Cells["impostoValor"].Value ?? 0);  // Ex: 40
                        int impostoPorcentagem = Convert.ToInt32(row.Cells["impostoPorcentagem"].Value ?? 0);  // Ex: 8

                        totalComImposto += valorComImposto;
                        totalSemImposto += valorSemImposto;
                        totalImposto += impostoValor;
                        totalPercentColumn += impostoPorcentagem;

                        // Verifica os percentuais de imposto e soma a porcentagem
                        if (impostoValor == 8)
                        {
                            total8Percent += impostoValor;  // Soma a porcentagem de 8%
                        }
                        else if (impostoValor == 10)
                        {
                            total10Percent += impostoValor;  // Soma a porcentagem de 10%
                        }

                        // Soma por tipo de pagamento
                        string pagamento = row.Cells["pagamento"]?.Value?.ToString() ?? string.Empty;
                        if (pagamento.Equals("Crédito", StringComparison.OrdinalIgnoreCase))
                            totalCredito += valorComImposto;
                        if (pagamento.Equals("Dinheiro", StringComparison.OrdinalIgnoreCase))
                            totalDinheiro += valorComImposto;
                        if (pagamento.Equals("Prazo", StringComparison.OrdinalIgnoreCase))
                            totalPrazo += valorComImposto;

                        currentY += 15;
                    }

                    // Linha separadora
                    currentY += 10;
                    gfx.DrawLine(XPens.Black, 30, currentY, page.Width - 30, currentY);
                    currentY += 20;

                    // Título "Total"
                    gfx.DrawString("Total", titleFont, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 20;

                    // Totais em linhas separadas
                    gfx.DrawString("Total Sem Imposto: ¥" + totalSemImposto, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Com Imposto: ¥" + totalComImposto, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto Cobrado: ¥" + totalPercentColumn, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto (8%): " + total8Percent + "%", font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto (10%): " + total10Percent + "%", font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total em Crédito: ¥" + totalCredito, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total em Dinheiro: ¥" + totalDinheiro, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total a Prazo: ¥" + totalPrazo, font, XBrushes.Black, new XPoint(30, currentY));

                    // Footer com data, hora e local
                    currentY = page.Height - 50;
                    string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    gfx.DrawLine(XPens.Black, 30, currentY - 10, page.Width - 30, currentY - 10);
                    gfx.DrawString("Data e Hora: " + dataHora, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Komatsu, Ishikawa-ken, Japão", font, XBrushes.Black, new XPoint(30, currentY));

                    pdfDoc.Save(saveFileDialog.FileName);
                    MessageBox.Show("Relatório gerado com sucesso para a data: " + dataSelecionada.ToString("dd/MM/yyyy"), "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao gerar o relatório: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            CentralizarPanel(panel1);
        }
    }
}













































