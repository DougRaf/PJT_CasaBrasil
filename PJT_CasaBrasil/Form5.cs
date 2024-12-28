using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Drawing; // Necessário para trabalhar com MySQL

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
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                FileName = "Relatorio_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf" // Nome do arquivo com data/hora
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfDocument pdfDoc = new PdfDocument();
                    PdfPage page = pdfDoc.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    XFont titleFont = new XFont("Arial", 12);
                    XFont font = new XFont("Arial", 7); // Fonte menor para encaixe

                    string logoPath = "C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\casabrasil.png";
                    if (System.IO.File.Exists(logoPath))
                    {
                        XImage logoImage = XImage.FromFile(logoPath);
                        gfx.DrawImage(logoImage, 50, 40, 55, 55); // Logo ajustado
                    }

                    gfx.DrawString("Relatório de Vendas", titleFont, XBrushes.Black, new XPoint(120, 65)); // Título alinhado

                    // Ajuste do deslocamento inicial
                    double startX = 30; // Margem ajustada para deslocar conteúdo à direita
                    double startY = 120;
                    double currentY = startY;

                    // Larguras ajustadas
                    double[] columnWidths = { 80, 70, 80, 60, 70, 60, 120 };
                    string[] headers = { "Código Barras", "Pagamento", "Total Com Imposto", "Imposto Valor", "Total Sem Imposto", "Imposto %", "Data/Hora Operação" };

                    // Cabeçalhos
                    for (int i = 0; i < headers.Length; i++)
                    {
                        gfx.DrawString(headers[i], font, XBrushes.Black, new XPoint(startX, currentY));
                        startX += columnWidths[i];
                    }

                    currentY += 20;

                    int totalComImposto = 0, totalSemImposto = 0, totalImposto = 0;
                    int total8Percent = 0, total10Percent = 0;
                    int totalCredito = 0, totalDinheiro = 0, totalPrazo = 0;
                    int totalPercentColumn = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        startX = 30; // Reiniciar a margem ajustada
                        foreach (var columnIndex in new[] {
                "codigoBarras",
                "pagamento",
                "totalComImposto", // Corrigido para estar na ordem certa
                "impostoValor",
                "totalSemImposto",
                "impostoPorcentagem", // Posicionado corretamente
                "dataHoraOperacao"
                })
                        {
                            string cellValue = row.Cells[columnIndex]?.Value?.ToString() ?? string.Empty;
                            gfx.DrawString(cellValue, font, XBrushes.Black, new XPoint(startX, currentY));
                            startX += columnWidths[Array.IndexOf(new[] {
                    "codigoBarras",
                    "pagamento",
                    "totalComImposto",
                    "impostoValor",
                    "totalSemImposto",
                    "impostoPorcentagem",
                    "dataHoraOperacao"
                    }, columnIndex)];
                        }

                        totalComImposto += (int)Math.Floor(Convert.ToDecimal(row.Cells["totalComImposto"].Value ?? 0));
                        totalSemImposto += (int)Math.Floor(Convert.ToDecimal(row.Cells["totalSemImposto"].Value ?? 0));

                        int impostoValor = (int)Math.Floor(Convert.ToDecimal(row.Cells["impostoValor"].Value ?? 0));
                        totalImposto += impostoValor;

                        int impostoPorcentagem = (int)Math.Floor(Convert.ToDecimal(row.Cells["impostoPorcentagem"].Value ?? 0));
                        totalPercentColumn += impostoPorcentagem;

                        if (impostoValor == 8)
                        {
                            total8Percent += impostoValor;
                        }
                        else if (impostoValor == 10)
                        {
                            total10Percent += impostoValor;
                        }

                        string pagamento = row.Cells["pagamento"].Value?.ToString() ?? string.Empty;
                        int totalPagamento = (int)Math.Floor(Convert.ToDecimal(row.Cells["totalComImposto"].Value ?? 0));
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
                    gfx.DrawString("Total Imposto (8%): " + total8Percent + "%", font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto (10%): " + total10Percent + "%", font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total Imposto Cobrado: ¥" + totalImposto, font, XBrushes.Black, new XPoint(30, currentY));
                    currentY += 15;
                    gfx.DrawString("Total  Porcentagem Imposto %: ¥" + totalPercentColumn, font, XBrushes.Black, new XPoint(30, currentY));
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
                    MessageBox.Show("Dados salvos como PDF com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar os dados em PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
    



























