﻿using CrystalDecisions.CrystalReports.Engine;
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

            // Configurações para o formulário
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Desabilita redimensionamento
            this.StartPosition = FormStartPosition.CenterScreen; // Abre centralizado na tela
            this.WindowState = FormWindowState.Maximized;    // Configurações para o formulário
        
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

        private void button1_Click(object sender, EventArgs e)
        {
            string termoPesquisa = textBox1.Text.Trim();

            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                MessageBox.Show("Por favor, escolha pelo menos uma opção.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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
                        PdfSharp.Pdf.PdfDocument pdf = new PdfSharp.Pdf.PdfDocument();
                        pdf.Info.Title = "Exportação de Dados";

                        PdfSharp.Pdf.PdfPage page = pdf.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        XFont fontTitle = new XFont("Arial", 20);
                        XFont fontHeader = new XFont("Arial", 8);
                        XFont fontContent = new XFont("Arial", 5);
                        XFont fontTotal = new XFont("Arial", 9);
                        XFont font = new XFont("Arial", 7);

                        double posX = 20;
                        double posY = 30;

                        string logoPath = "C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\casabrasil.png";
                        if (System.IO.File.Exists(logoPath))
                        {
                            XImage logoImage = XImage.FromFile(logoPath);
                            gfx.DrawImage(logoImage, posX, posY, 55, 55);
                            posX += 70;
                        }

                        gfx.DrawString("Exportação de Dados", fontTitle, XBrushes.Black, new XPoint(posX, posY + 20));
                        posX = 20;
                        posY += 80;

                        gfx.DrawString("Nome do Produto", fontHeader, XBrushes.Black, new XPoint(posX, posY));
                        gfx.DrawString("Quantidade", fontHeader, XBrushes.Black, new XPoint(posX + 150, posY));
                        gfx.DrawString("Preço Custo", fontHeader, XBrushes.Black, new XPoint(posX + 250, posY));
                        gfx.DrawString("Preço Venda", fontHeader, XBrushes.Black, new XPoint(posX + 350, posY));
                        gfx.DrawString("V.Estoque", fontHeader, XBrushes.Black, new XPoint(posX + 450, posY));

                        posY += 20;

                        int totalQuantidade = 0;
                        int totalPrecoCusto = 0;
                        int totalPrecoVenda = 0;
                        int totalValorEstoque = 0;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string nomeProduto = row.Cells["nome_produto"].Value?.ToString() ?? "N/A";

                                int quantidade = int.TryParse(row.Cells["quantidade"].Value?.ToString(), out var q) ? q : 0;
                                int precoCusto = int.TryParse(row.Cells["preco_custo"].Value?.ToString(), out var pc) ? pc : 0;
                                int precoVenda = int.TryParse(row.Cells["preco_venda"].Value?.ToString(), out var pv) ? pv : 0;

                                int custoTotal = quantidade * precoCusto;
                                int vendaTotal = quantidade * precoVenda;
                                int vEstoque = vendaTotal;

                                totalQuantidade += quantidade;
                                totalPrecoCusto += custoTotal;
                                totalPrecoVenda += vendaTotal;
                                totalValorEstoque += vEstoque;

                                gfx.DrawString(nomeProduto, fontContent, XBrushes.Black, new XPoint(posX, posY));
                                gfx.DrawString(quantidade.ToString(), fontContent, XBrushes.Black, new XPoint(posX + 150, posY));
                                gfx.DrawString("¥" + custoTotal.ToString("#,0").Replace(",", "."), fontContent, XBrushes.Black, new XPoint(posX + 250, posY));
                                gfx.DrawString("¥" + vendaTotal.ToString("#,0").Replace(",", "."), fontContent, XBrushes.Black, new XPoint(posX + 350, posY));
                                gfx.DrawString("¥" + vEstoque.ToString("#,0").Replace(",", "."), fontContent, XBrushes.Black, new XPoint(posX + 450, posY));

                                posY += 20;

                                if (posY > page.Height - 50)
                                {
                                    page = pdf.AddPage();
                                    gfx = XGraphics.FromPdfPage(page);
                                    posY = 30;

                                    gfx.DrawString("Nome do Produto", fontHeader, XBrushes.Black, new XPoint(posX, posY));
                                    gfx.DrawString("Quantidade", fontHeader, XBrushes.Black, new XPoint(posX + 150, posY));
                                    gfx.DrawString("Preço Custo", fontHeader, XBrushes.Black, new XPoint(posX + 250, posY));
                                    gfx.DrawString("Preço Venda", fontHeader, XBrushes.Black, new XPoint(posX + 350, posY));
                                    gfx.DrawString("V.Estoque", fontHeader, XBrushes.Black, new XPoint(posX + 450, posY));

                                    posY += 20;
                                }
                            }
                        }

                        gfx.DrawLine(XPens.Black, posX, posY, posX + 500, posY);
                        posY += 10;

                        gfx.DrawString("Relatório Total:", fontTotal, XBrushes.Black, new XPoint(posX, posY));
                        posY += 10;

                        gfx.DrawString("Total Quantidade: " + totalQuantidade.ToString(), fontTotal, XBrushes.Black, new XPoint(posX, posY));
                        posY += 10;

                        gfx.DrawString("Total Preço Custo: ¥" + totalPrecoCusto.ToString("#,0").Replace(",", "."), fontTotal, XBrushes.Black, new XPoint(posX, posY));
                        posY += 10;

                        gfx.DrawString("Total Preço Venda: ¥" + totalPrecoVenda.ToString("#,0").Replace(",", "."), fontTotal, XBrushes.Black, new XPoint(posX, posY));
                        posY += 10;

                        gfx.DrawString("Total V.Estoque: ¥" + totalValorEstoque.ToString("#,0").Replace(",", "."), fontTotal, XBrushes.Black, new XPoint(posX, posY));

                        double currentY = page.Height - 50;
                        string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        gfx.DrawLine(XPens.Black, 30, currentY - 10, page.Width - 30, currentY - 10);
                        gfx.DrawString("Data e Hora: " + dataHora, font, XBrushes.Black, new XPoint(30, currentY));
                        currentY += 15;
                        gfx.DrawString("Komatsu, Ishikawa-ken, Japão", font, XBrushes.Black, new XPoint(30, currentY));

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


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

}



