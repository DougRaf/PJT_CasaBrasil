using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
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

                    // Se houver um filtro, ajusta a consulta
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        query += " WHERE nome_produto LIKE @filtro OR codigo_barras LIKE @filtro OR categoria LIKE @filtro";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Se houver um filtro, adiciona o parâmetro
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt; // Preenche o DataGridView com os dados

                    // Opcional: Ajusta a largura das colunas
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

                    // Se houver um filtro, ajusta a consulta
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        query += " WHERE nome_completo LIKE @filtro OR email LIKE @filtro OR documento LIKE @filtro";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Se houver um filtro, adiciona o parâmetro
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt; // Preenche o DataGridView com os dados

                    // Opcional: Ajusta a largura das colunas
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

            // Verifica se nenhum dos checkboxes está selecionado
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                MessageBox.Show("Por favor, escolha pelo menos uma opção.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Verifica se a CheckBox "chkProdutos" está marcada
            if (checkBox2.Checked)
            {
                CarregarDados(termoPesquisa); // Chama a função de carregar dados com o filtro
            }

            if (checkBox4.Checked)
            {
                CarregarDadosFuncionarios(termoPesquisa); // Chama a função de carregar dados com o filtro
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            TryGenerateReport();
        }

        private void TryGenerateReport()
        {
            if (HasDataInGrid())
            {
                GenerateReport();
            }
            else
            {
                ShowNoDataMessage();
            }
        }

        private bool HasDataInGrid()
        {
            return dataGridView1.Rows.Count > 0;
        }

        private void GenerateReport()
        {
            // Limpa as fontes de dados do ReportViewer
         
        }

        private void ShowNoDataMessage()
        {
            MessageBox.Show("Não há dados para imprimir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
