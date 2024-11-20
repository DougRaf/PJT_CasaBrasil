
using MySql.Data.MySqlClient;
using System;
using System.Data;
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

            // Verificar se o produto já existe no banco de dados
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM produto WHERE codigo_barras = @codigo_barras";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@codigo_barras", codigoBarras);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar()); // Retorna o número de registros encontrados

                        if (count > 0)
                        {
                            MessageBox.Show("Produto já cadastrado na base de dados!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // Produto duplicado, sai do método
                        }
                    }

                    // Produto não encontrado, inserir no banco de dados
                    string query = "INSERT INTO produto (nome_produto, codigo_barras, categoria, quantidade, data_entrada, data_vencimento, preco_custo, preco_venda, imposto, observacoes) " +
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

                        // Executar o comando para inserir o novo produto
                        cmd.ExecuteNonQuery();
                    }

                    // Mostrar o formulário de sucesso
                    FormSucesso formSucesso = new FormSucesso();
                    formSucesso.ShowDialog();

                    // Limpar os campos do formulário
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Método para buscar o produto no banco de dados
        private void BuscarProduto(string codigoBarras)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM produto WHERE codigo_barras = @codigo_barras";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codigo_barras", codigoBarras);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Preencher os campos do formulário com os dados do banco
                                txtNomeProduto.Text = reader["nome_produto"].ToString();
                                txtCodigoBarra.Text = reader["codigo_barras"].ToString();
                                txtCategoria.Text = reader["categoria"].ToString();
                                txtqtd.Text = reader["quantidade"].ToString();
                                txtDataEntrada.Text = reader["data_entrada"].ToString();
                                txtdatavencimento.Text = reader["data_vencimento"].ToString();
                                txtPrecoCusto.Text = reader["preco_custo"].ToString();
                                txtPrecoVenda.Text = reader["preco_venda"].ToString();
                                txtImposto.Text = reader["imposto"].ToString();
                                txtObservacoes.Text = reader["observacoes"].ToString();
                            }
                            else
                            {
                               // MessageBox.Show("Produto não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao buscar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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


        private void Form4_Load(object sender, EventArgs e)
        {
            txtDataEntrada.Mask = "00/00/0000 00:00";


            // Atualiza o conteúdo do MaskedTextBox com a data e hora atuais
            txtDataEntrada.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            txtDataEntrada.ReadOnly = true;




        }

        // Método para atualizar o produto no banco de dados
        private void AtualizarProduto()
        {
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
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "UPDATE produto SET nome_produto = @nome_produto, categoria = @categoria, quantidade = @quantidade, " +
                                   "data_entrada = @data_entrada, data_vencimento = @data_vencimento, preco_custo = @preco_custo, " +
                                   "preco_venda = @preco_venda, imposto = @imposto, observacoes = @observacoes " +
                                   "WHERE codigo_barras = @codigo_barras";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Atualizar os dados no banco de dados
            AtualizarProduto();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            // Buscar o produto com o código de barras fornecido
            string codigoBarras = txtCodigoBarra.Text;
            if (string.IsNullOrEmpty(codigoBarras))
            {
                MessageBox.Show("Por favor, insira um código de barras para buscar o produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


        }

        private void txtCodigoBarra_TextChanged(object sender, EventArgs e)
        {


            // Buscar o produto com o código de barras fornecido
            string codigoBarras = txtCodigoBarra.Text;


            BuscarProduto(codigoBarras);

            if (string.IsNullOrEmpty(codigoBarras)) { 

            txtNomeProduto.Clear();
            txtCodigoBarra.Clear();
            // txtDataEntrada.Clear();
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
}
