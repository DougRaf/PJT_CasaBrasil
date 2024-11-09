using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{
    public partial class Form7 : Form
    {
        private string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;";
        private int contadorItens = 0; // Contador de itens encontrados
        private DataTable dataTable;
        private int itemCount;

        public Form7()
        {
            InitializeComponent();
            txtCodigoDeBarras.TextChanged += TxtCodigoDeBarras_TextChanged;
            // Para esconder a barra de tarefas (opcional, se for necessário)

            InitializeDataTable();
            dataGrid1.DataSource = dataTable;

         

            itemCount = 0; // Inicializa o contador
            UpdateItemCount();
        }

        private void InitializeDataTable()
        {
            // Cria uma tabela de dados com as colunas necessárias
            dataTable = new DataTable();
            dataTable.Columns.Add("Item", typeof(string));
            dataTable.Columns.Add("Código", typeof(string));
            dataTable.Columns.Add("Produto", typeof(string));
            dataTable.Columns.Add("Descrição", typeof(string));
            dataTable.Columns.Add("Valor", typeof(string));
            dataTable.Columns.Add("Taxa", typeof(string));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Cria uma instância de Form2
            Form12 form2 = new Form12();

            // Passa o texto do TextBox para Form2

           // form2.DataToDisplay = dataTable.Copy();

        }


        private void TxtCodigoDeBarras_TextChanged(object sender, EventArgs e)
        {
            // Verifica se o TextBox não está vazio
            if (!string.IsNullOrWhiteSpace(txtCodigoDeBarras.Text))
            {
                // Chama o método para buscar os dados
                BuscarProduto(txtCodigoDeBarras.Text);
       
                // Adiciona os dados dos TextBoxes ao DataTable
                if (!string.IsNullOrWhiteSpace(txtNumeracao.Text) &&
                    !string.IsNullOrWhiteSpace(txtCodigoDeBarras.Text) &&
                    !string.IsNullOrWhiteSpace(txtNomeProduto.Text) &&
                    !string.IsNullOrWhiteSpace(txtCategoria.Text) &&
                     !string.IsNullOrWhiteSpace(txtPrecoVenda.Text) &&
                     !string.IsNullOrWhiteSpace(txtImposto.Text))
                {
                    dataTable.Rows.Add(
                        txtNumeracao.Text,
                        txtCodigoDeBarras.Text,
                        txtNomeProduto.Text,
                        txtCategoria.Text,
                        txtPrecoVenda.Text,
                        txtImposto.Text
                    );

                    ClearTextBoxes();


                    // Caminho do arquivo TXT onde os dados serão salvos
                    string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";

                    try
                    {
                        // Verifica se o diretório existe e, caso contrário, cria o diretório
                        string directoryPath = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Obtém o conteúdo da DataTable
                        string content = GetDataTableContent(dataTable);

                        // Salva o conteúdo no arquivo
                        SaveToFile(filePath, content);

                        // Mensagem de sucesso
                       // MessageBox.Show("Dados salvos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (IOException ioEx)
                    {
                        // Exceção específica para problemas com arquivos
                        MessageBox.Show($"Erro ao salvar os dados no arquivo: {ioEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (UnauthorizedAccessException uaEx)
                    {
                        // Exceção caso o aplicativo não tenha permissão para acessar o arquivo ou diretório
                        MessageBox.Show($"Erro de permissão: {uaEx.Message}", "Erro de Permissão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        // Exceção genérica para outros tipos de erros
                        MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                else
                {
                   // MessageBox.Show("Por favor, preencha todos os campos corretamente.");
                }
            }
        }

        private void BuscarProduto(string codigoDeBarras)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Primeiro, buscar o produto
                    string query = "SELECT id, nome_produto, categoria, preco_venda, imposto, quantidade FROM produto WHERE codigo_barras = @codigo";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigoDeBarras);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Exibir os dados no TextBox
                                txtNomeProduto.Text = reader["nome_produto"].ToString();
                                txtCategoria.Text = reader["categoria"].ToString();
                                txtPrecoVenda.Text = reader["preco_venda"].ToString();
                                txtImposto.Text = reader["imposto"].ToString();
                               
                                itemCount++;
                                UpdateItemCount();

                                // Atualizar a quantidade
                                int quantidadeAtual = Convert.ToInt32(reader["quantidade"]);
                                int produtoId = Convert.ToInt32(reader["id"]); // Armazenar o ID do produto

                                if (quantidadeAtual > 0)
                                {
                                    quantidadeAtual--; // Diminuir a quantidade

                                    // Fechar o DataReader antes de atualizar
                                    reader.Close();

                                    // Executar o UPDATE
                                    string updateSql = "UPDATE produto SET quantidade = @novaQuantidade WHERE id = @idDoProduto";
                                    using (MySqlCommand updateCmd = new MySqlCommand(updateSql, conn))
                                    {
                                        updateCmd.Parameters.AddWithValue("@novaQuantidade", quantidadeAtual);
                                        updateCmd.Parameters.AddWithValue("@idDoProduto", produtoId); // Usar o ID armazenado

                                        int rowsAffected = updateCmd.ExecuteNonQuery();
                                        Console.WriteLine($"{rowsAffected} linha(s) atualizada(s).");

                                        // Confirmação para o usuário
                                        if (rowsAffected > 0)
                                        {
                                           // MessageBox.Show("Quantidade atualizada com sucesso.");
                                        }
                                        else
                                        {
                                          //  MessageBox.Show("Nenhuma atualização realizada.");
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Quantidade insuficiente.");
                                }
                            }
                            else
                            {
                               // MessageBox.Show("Código de barras não encontrado.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            } // A conexão é fechada automaticamente aqui
        }



        private void ClearTextBoxes()
        {
            txtNumeracao.Clear();
            txtCodigoDeBarras.Clear();
            txtNomeProduto.Clear();
            txtCategoria.Clear();
            txtPrecoVenda.Clear();
            txtImposto.Clear();
        }

        private void UpdateQuantidade(MySqlConnection conn, string codigoDeBarras, int quantidadeAtual)
        {
            conn.Close(); // Fecha a conexão
            string updateQuery = "UPDATE produto SET quantidade = @quantidade WHERE codigo_barras = @codigo";
            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
            {
                updateCmd.Parameters.AddWithValue("@quantidade", quantidadeAtual);
                updateCmd.Parameters.AddWithValue("@codigo", codigoDeBarras);
                updateCmd.ExecuteNonQuery();
            }
        }

        private void UpdateItemCount()
        {
            // Atualiza o TextBox com a contagem atual de itens
            txtNumeracao.Text = itemCount.ToString();
        }




        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

     

        private void button6_Click(object sender, EventArgs e)
        {
         
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void txtCategoria_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNomeProduto_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGrid1_Navigate(object sender, NavigateEventArgs ne)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtImposto_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPrecoVenda_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNumeracao_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCodigoDeBarras_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Verifique se o campo de numeração não está vazio
            if (!string.IsNullOrWhiteSpace(txtNumeracao.Text))
            {
                // Obter o valor da numeração
                string numeracao = txtNumeracao.Text;

                // Acessa o DataTable que está vinculado ao DataGridView
                if (dataGrid1.DataSource != null)
                {
                    DataTable dataTable = (DataTable)dataGrid1.DataSource;

                    // Encontrar a linha que corresponde à numeração fornecida
                    var rowToDelete = dataTable.AsEnumerable()
                                               .FirstOrDefault(row => row.Field<string>("Item") == numeracao);

                    if (rowToDelete != null)
                    {
                        // Remover a linha do DataTable
                        dataTable.Rows.Remove(rowToDelete);

                        // Exibir uma mensagem informando que a linha foi excluída
                        MessageBox.Show("Linha excluída com sucesso!");
                    }
                    else
                    {
                        // Caso não encontre a linha com a numeração fornecida
                        MessageBox.Show("Nenhuma linha encontrada com a numeração fornecida.");
                    }
                }
            }
            else
            {
                // Caso o campo de numeração esteja vazio
                MessageBox.Show("Por favor, insira uma numeração válida.");
            }







        }

        // Método para montar o conteúdo do arquivo a partir de um DataTable
        private string GetDataTableContent(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();

            // Adiciona o cabeçalho da DataTable (nomes das colunas)
            sb.AppendLine(string.Join("\t", dataTable.Columns.Cast<DataColumn>()
                                        .Select(col => col.ColumnName)));

            // Adiciona os dados das linhas
            foreach (DataRow row in dataTable.Rows)
            {
                var rowData = row.ItemArray.Select(item => item?.ToString() ?? string.Empty).ToArray();
                sb.AppendLine(string.Join("\t", rowData));



            }

            // Para cada coluna do DataGridView
          

            return sb.ToString();
        }

        // Método para salvar o conteúdo no arquivo
        private void SaveToFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }



    }



 
}
    


