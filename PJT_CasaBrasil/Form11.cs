using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using PdfSharp.Pdf;  // PdfSharp PdfDocument
using PdfSharp.Drawing;  // PdfSharp drawing methods

namespace PJT_CasaBrasil
{
    public partial class Form11 : Form
    {
        private string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;";

        public Form11()
        {
            InitializeComponent();

            // Inicializar os CheckBox
            checkBox1.CheckedChanged += CheckBox_CheckedChanged;
            checkBox2.CheckedChanged += CheckBox_CheckedChanged;
            checkBox3.CheckedChanged += CheckBox_CheckedChanged;
            checkBox4.CheckedChanged += CheckBox_CheckedChanged;
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == checkBox1)
            {
                if (checkBox1.Checked)
                {
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                }
            }
            else if (sender == checkBox2)
            {
                if (checkBox2.Checked)
                {
                    checkBox1.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                }
            }
            else if (sender == checkBox3)
            {
                if (checkBox3.Checked)
                {
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox4.Checked = false;
                }
            }
            else if (sender == checkBox4)
            {
                if (checkBox4.Checked)
                {
                    checkBox1.Checked = false;
                    checkBox3.Checked = false;
                    checkBox2.Checked = false;
                }
            }
        }

        private void CarregarDados(string filtro = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nome_produto, codigo_barras, categoria, quantidade, data_entrada, data_vencimento, preco_custo, preco_venda, imposto FROM produto";

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        query += " WHERE nome_produto LIKE @filtro OR codigo_barras LIKE @filtro OR categoria LIKE @filtro";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        private void CarregarDadosFuncionarios(string filtro = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nome_completo, endereco, data_nascimento, estado_civil, email, telefone, documento FROM funcionarios";

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        query += " WHERE nome_completo LIKE @filtro OR email LIKE @filtro OR documento LIKE @filtro";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        private void Form11_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string termoPesquisa = textBox1.Text.Trim();

            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                MessageBox.Show("Por favor, escolha pelo menos uma opção.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (checkBox2.Checked)
            {
                CarregarDados(termoPesquisa);
            }

            if (checkBox4.Checked)
            {
                CarregarDadosFuncionarios(termoPesquisa);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Salvar como PDF";
                saveFileDialog.FileName = "Exportacao.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Usar PdfSharp corretamente para evitar ambiguidade
                        PdfSharp.Pdf.PdfDocument pdf = new PdfSharp.Pdf.PdfDocument();
                        pdf.Info.Title = "Exportação de Dados";

                        PdfSharp.Pdf.PdfPage page = pdf.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        XFont fontTitle = new XFont("Arial", 20);  // Fonte para o título
                        XFont fontHeader = new XFont("Arial", 8); // Fonte para o cabeçalho da tabela
                        XFont fontContent = new XFont("Arial", 5); // Fonte para os dados

                        // Definir a posição inicial na página
                        double posX = 20;
                        double posY = 30;

                        // Carregar e desenhar o logo (imagem) antes do título
                        string logoPath = "C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\casabrasil.png";  // Caminho para a imagem do logo
                        if (System.IO.File.Exists(logoPath))
                        {
                            XImage logoImage = XImage.FromFile(logoPath);
                            double logoWidth = 55;  // Largura desejada do logo
                            double logoHeight = 55; // Altura desejada do logo
                            gfx.DrawImage(logoImage, posX, posY, logoWidth, logoHeight);  // Reduzir o tamanho da imagem para 50px x 25px
                            posY += 30;  // Ajuste a posição Y após a imagem
                        }
                        else
                        {
                            MessageBox.Show("Logo não encontrado no caminho especificado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Calcular a largura do título para centralizá-lo à direita da imagem
                        string title = "Exportação de Dados";
                        double titleWidth = gfx.MeasureString(title, fontTitle).Width;
                        double pageWidth = page.Width;
                        double imageWidth = 50; // A largura da imagem do logo
                        double positionXTitle = imageWidth +70;  // A distância da imagem para o título, ajustável

                        // Desenhar o título centralizado à direita da imagem
                        gfx.DrawString(title, fontTitle, XBrushes.Black, new XPoint(positionXTitle, posY));

                        posY += 40;

                        // Cabeçalho da tabela - incluir os nomes das colunas dinamicamente
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            gfx.DrawString(column.HeaderText, fontHeader, XBrushes.Black, new XPoint(posX, posY));
                            posX += 100;
                        }

                        posY += 20; // Nova linha
                        posX = 20; // Resetar posição X

                        // Adicionar linhas de dados
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    string cellValue = cell.Value?.ToString() ?? "N/A"; // Garantir que valores nulos sejam tratados
                                    gfx.DrawString(cellValue, fontContent, XBrushes.Black, new XPoint(posX, posY));
                                    posX += 100;
                                }

                                posY += 20; // Nova linha
                                posX = 20; // Resetar posição X

                                // Verificar se a página está cheia
                                if (posY > page.Height - 50)
                                {
                                    // Adicionar nova página
                                    page = pdf.AddPage();
                                    gfx = XGraphics.FromPdfPage(page);
                                    posY = 30; // Resetar posição Y
                                }
                            }
                        }

                        // Salvar o PDF
                        pdf.Save(saveFileDialog.FileName);

                        MessageBox.Show("PDF salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao salvar PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
