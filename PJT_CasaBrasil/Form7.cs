using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{
    public partial class Form7 : Form
    {
        private string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;";
        private int contadorItens = 0; // Contador de itens encontrados
        private DataTable dataTable;
        private int itemCount;
        private decimal totalVenda = 0; // Para armazenar o total das vendas


        public Form7()
        {
            InitializeComponent();
            txtCodigoDeBarras.TextChanged += TxtCodigoDeBarras_TextChanged;

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


        public static class Totalcase
        {
            public static string Total { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Cria uma instância de Form2
            Form12 form2 = new Form12();
            // form2.DataToDisplay = dataTable.Copy();
        }

        private void TxtCodigoDeBarras_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigoDeBarras.Text))
            {
                // Chama o método para buscar os dados
                BuscarProduto(txtCodigoDeBarras.Text);

                // Se for a primeira inserção ou após o cancelamento, reiniciar o contador
                if (contadorItens == 0)
                {
                    dataTable.Clear(); // Limpa os itens anteriores
                }

                // Adiciona os dados dos TextBoxes ao DataTable se todos os campos estiverem preenchidos
                if (!string.IsNullOrWhiteSpace(txtNumeracao.Text) &&
                    !string.IsNullOrWhiteSpace(txtCodigoDeBarras.Text) &&
                    !string.IsNullOrWhiteSpace(txtNomeProduto.Text) &&
                    !string.IsNullOrWhiteSpace(txtCategoria.Text) &&
                    !string.IsNullOrWhiteSpace(txtPrecoVenda.Text) &&
                    !string.IsNullOrWhiteSpace(txtImposto.Text))
                {
                    // Adiciona a nova linha ao DataTable
                    dataTable.Rows.Add(
                        txtNumeracao.Text,
                        txtCodigoDeBarras.Text,
                        txtNomeProduto.Text,
                        txtCategoria.Text,
                        txtPrecoVenda.Text,
                        txtImposto.Text
                    );

                    // Incrementa o contador de itens
                    contadorItens++;

                    // Atualiza o contador de itens na interface
                    UpdateItemCount();

                    // Atualiza o total de vendas
                    AtualizarTotalVenda();

                    // Limpa os campos de texto para o próximo item
                    ClearTextBoxes();

                    // Caminho do arquivo de texto onde os dados serão salvos
                    string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";

                    try
                    {
                        // Verifica se o diretório existe, caso contrário, cria o diretório
                        string directoryPath = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Converte os dados do DataTable para um formato de texto
                        string content = GetDataTableContent(dataTable);

                        // Salva o conteúdo no arquivo
                        SaveToFile(filePath, content);
                    }
                    catch (IOException ioEx)
                    {
                        MessageBox.Show($"Erro ao salvar os dados no arquivo: {ioEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

                    string query = "SELECT id, nome_produto, categoria, preco_venda, imposto, quantidade FROM produto WHERE codigo_barras = @codigo";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigoDeBarras);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtNomeProduto.Text = reader["nome_produto"].ToString();
                                txtCategoria.Text = reader["categoria"].ToString();
                                txtPrecoVenda.Text = reader["preco_venda"].ToString();
                                txtImposto.Text = reader["imposto"].ToString();

                                itemCount++;
                                UpdateItemCount();

                                int quantidadeAtual = Convert.ToInt32(reader["quantidade"]);
                                int produtoId = Convert.ToInt32(reader["id"]);

                                if (quantidadeAtual > 0)
                                {
                                    quantidadeAtual--;
                                    reader.Close();

                                    string updateSql = "UPDATE produto SET quantidade = @novaQuantidade WHERE id = @idDoProduto";
                                    using (MySqlCommand updateCmd = new MySqlCommand(updateSql, conn))
                                    {
                                        updateCmd.Parameters.AddWithValue("@novaQuantidade", quantidadeAtual);
                                        updateCmd.Parameters.AddWithValue("@idDoProduto", produtoId);

                                        updateCmd.ExecuteNonQuery();
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
            }
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

        private void UpdateItemCount()
        {
            txtNumeracao.Text = itemCount.ToString();
        }

        private string GetDataTableContent(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Join("\t", dataTable.Columns.Cast<DataColumn>()
                                        .Select(col => col.ColumnName)));

            foreach (DataRow row in dataTable.Rows)
            {
                var rowData = row.ItemArray.Select(item => item?.ToString() ?? string.Empty).ToArray();
                sb.AppendLine(string.Join("\t", rowData));
            }

            return sb.ToString();
        }

        private void SaveToFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        private void CancelarVenda()
        {
            // Restaurar a quantidade dos produtos no banco de dados
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                foreach (DataRow row in dataTable.Rows)
                {
                    string codigoBarras = row["Código"].ToString();
                    int quantidadeVendida = 1;

                    string updateQuery = "UPDATE produto SET quantidade = quantidade + @quantidadeVendida WHERE codigo_barras = @codigo";
                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@quantidadeVendida", quantidadeVendida);
                        updateCmd.Parameters.AddWithValue("@codigo", codigoBarras);

                        updateCmd.ExecuteNonQuery();
                    }
                }
            }

            // Limpar o DataTable
            dataTable.Clear();
            contadorItens = 0;
            UpdateItemCount();
            txtNumeracao.Clear();

            // Apagar o conteúdo do arquivo de texto
            string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";
            File.WriteAllText(filePath, string.Empty);

            // Fechar o formulário atual
            this.Close();

            // Criar uma nova instância do Form7 e reabrir
            Form7 novoFormulario = new Form7();
            novoFormulario.Show();

            // Mensagem informando que a venda foi cancelada
            MessageBox.Show("Venda cancelada com sucesso!", "Cancelamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Evento do botão de cancelamento (manter o nome do evento conforme solicitado)
        private void button6_Click(object sender, EventArgs e)
        {
            CancelarVenda();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Aqui você pode adicionar o código para outras ações que o botão 5 deve executar.

            Totalcase.Total = textBox9.Text;
            MessageBox.Show("Erro ao converter valor para decimal: " + Totalcase.Total, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

      

        private void AtualizarTotalVenda()
        {
            totalVenda = 0;  // Inicializa o total da venda

            foreach (DataRow row in dataTable.Rows)
            {
                string precoTexto = row["Valor"].ToString().Trim();  // Pega o valor da coluna "Valor" e remove espaços em branco

                // Remover qualquer caractere não numérico, como vírgulas, se necessário
                precoTexto = precoTexto.Replace(",", ".");  // Substitui vírgula por ponto

                // Tente converter para decimal
                if (decimal.TryParse(precoTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal precoVenda))
                {
                    // Adiciona o valor convertido ao totalVenda
                    totalVenda += precoVenda;
                }
                else
                {
                    // Caso não seja possível converter o valor, exibe mensagem de erro
                    MessageBox.Show("Erro ao converter valor para decimal: " + precoTexto, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Verifica se o valor totalVenda é inteiro
            if (totalVenda == Math.Floor(totalVenda))
            {
                // Se for inteiro, não mostra casas decimais
                textBox9.Text = totalVenda.ToString("0");
            }
            else
            {
                // Se não for inteiro, exibe com duas casas decimais
                textBox9.Text = totalVenda.ToString("F2");
            }
            // Salva na classe estática

          

        }


        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            // Chama o método para atualizar o total de vendas
            AtualizarTotalVenda();

            
        }
    }
}
