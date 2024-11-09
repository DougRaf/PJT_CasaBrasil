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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;


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
            string categoria = txtCategoria.Text;
            string quantidade = txtqtd.Text;
            string dataEntrada = txtDataEntrada.Text;
            string dataVencimento = txtdatavencimento.Text;            
            string precoCusto = txtPrecoCusto.Text;
            string precoVenda = txtPrecoVenda.Text;
            string imposto = txtImposto.Text;
            string observacoes = txtObservacoes.Text;

            // Verificar se todos os campos obrigatórios estão preenchidos
            if (string.IsNullOrWhiteSpace(nomeProduto) ||
                string.IsNullOrWhiteSpace(codigoBarras) ||
                string.IsNullOrWhiteSpace(categoria) ||
                string.IsNullOrWhiteSpace(quantidade) ||
                string.IsNullOrWhiteSpace(dataVencimento) ||
                string.IsNullOrWhiteSpace(precoCusto) ||
                string.IsNullOrWhiteSpace(precoVenda) ||
                string.IsNullOrWhiteSpace(imposto) ||
                string.IsNullOrWhiteSpace(observacoes))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Sai do método se houver campos vazios
            }


            // Inserir dados no banco de dados
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {            

                    conn.Open();

                    string query = "INSERT INTO produto (nome_produto, codigo_barras,categoria, quantidade, data_entrada, data_vencimento, preco_custo, preco_venda, imposto, observacoes) " +
                                   "VALUES (@nome_produto, @codigo_barras, @categoria, @quantidade, @data_entrada, @data_vencimento, @preco_custo, @preco_venda, @imposto, @observacoes)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Adicionando parâmetros para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@nome_produto", nomeProduto);
                        cmd.Parameters.AddWithValue("@codigo_barras", codigoBarras);
                        cmd.Parameters.AddWithValue("@categoria", categoria);
                        cmd.Parameters.AddWithValue("@quantidade", quantidade);
                        cmd.Parameters.AddWithValue("@data_entrada", dataEntrada);
                        cmd.Parameters.AddWithValue("@data_vencimento", dataVencimento);
                        cmd.Parameters.AddWithValue("@preco_custo", precoCusto);
                        cmd.Parameters.AddWithValue("@preco_venda", precoVenda);
                        cmd.Parameters.AddWithValue("@imposto", imposto);
                        cmd.Parameters.AddWithValue("@observacoes", observacoes);

                        // Executar o comando
                        cmd.ExecuteNonQuery();

                        // Mostrar o formulário de sucesso
                        FormSucesso formSucesso = new FormSucesso();
                        formSucesso.ShowDialog();

                        txtNomeProduto.Clear();
                        txtCodigoBarra.Clear();
                        txtDataEntrada.Clear();
                        txtdatavencimento.Clear();
                        txtCategoria.Clear();
                        txtqtd.Clear();
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
            txtNomeProduto.Clear();
            txtCodigoBarra.Clear();           
            txtdatavencimento.Clear();
            txtCategoria.Clear();
            txtqtd.Clear();
            txtPrecoCusto.Clear();
            txtPrecoVenda.Clear();
            txtImposto.Clear();
            txtObservacoes.Clear();

        }

        private void lblDataVencimento_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            txtDataEntrada.Mask = "00/00/0000 00:00";
     

            // Atualiza o conteúdo do MaskedTextBox com a data e hora atuais
            txtDataEntrada.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            txtDataEntrada.ReadOnly = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
