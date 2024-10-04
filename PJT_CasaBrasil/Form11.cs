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
    
        }

        private void CarregarDados(string filtro = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nome_produto, codigo_barras, categoria, data_entrada, data_vencimento, preco_custo, preco_venda FROM produto";

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

        private void Form11_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string termoPesquisa = textBox1.Text.Trim();   

            // Verifica se a CheckBox "chkProdutos" está marcada
            if (checkBox2.Checked)
            {
                CarregarDados(termoPesquisa); // Chama a função de carregar dados com o filtro
            }
            else
            {
                MessageBox.Show("Marque a opção 'Produtos' para realizar a pesquisa.");
            }


        }
    }
}
