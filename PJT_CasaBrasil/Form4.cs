using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PJT_CasaBrasil
{
    public partial class Form4 : Form
    {
     
        private string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;";

        public Form4()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            // Obtendo os valores dos controles do formulário
            string nomeProduto = txtNomeProduto.Text;
            string codigoBarras = txtCodigoBarra.Text;
            string dataEntrada = txtDataEntrada.Text;
            string dataVencimento = txtDataVencimento.Text;
            decimal precoCusto = decimal.Parse(txtPrecoCusto.Text);
            decimal precoVenda = decimal.Parse(txtPrecoVenda.Text);
            decimal imposto = decimal.Parse(txtImposto.Text);
            string observacoes = txtObservacoes.Text;

            // Inserir dados no banco de dados
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO produto (nome_produto, codigo_barras, data_entrada, data_vencimento, preco_custo, preco_venda, imposto, observacoes) " +
                                   "VALUES (@nome_produto, @codigo_barras, @data_entrada, @data_vencimento, @preco_custo, @preco_venda, @imposto, @observacoes)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Adicionando parâmetros para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@nome_produto", nomeProduto);
                        cmd.Parameters.AddWithValue("@codigo_barras", codigoBarras);
                        cmd.Parameters.AddWithValue("@data_entrada", dataEntrada);
                        cmd.Parameters.AddWithValue("@data_vencimento", dataVencimento);
                        cmd.Parameters.AddWithValue("@preco_custo", precoCusto);
                        cmd.Parameters.AddWithValue("@preco_venda", precoVenda);
                        cmd.Parameters.AddWithValue("@imposto", imposto);
                        cmd.Parameters.AddWithValue("@observacoes", observacoes);

                        // Executar o comando
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Produto inserido com sucesso!");

                        txtNomeProduto.Clear();
                        txtCodigoBarra.Clear();
                        txtDataEntrada.Clear();
                        txtDataVencimento.Clear();
                        txtPrecoCusto.Clear();
                        txtPrecoVenda.Clear();
                        txtImposto.Clear();
                        txtObservacoes.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir produto: {ex.Message}");
            }

        }

        private void btnApagar_Click(object sender, EventArgs e)
        {

        }

        private void lblDataVencimento_Click(object sender, EventArgs e)
        {

        }
    }
}
