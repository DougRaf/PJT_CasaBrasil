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
                // Buscar produto no banco de dados
                BuscarProduto(txtCodigoDeBarras.Text);

                // Se os campos necessários estiverem preenchidos, proceder com o cálculo
                if (!string.IsNullOrWhiteSpace(txtPrecoVenda.Text) && !string.IsNullOrWhiteSpace(txtImposto.Text))
                {
                    // Converter valores para decimal
                    if (decimal.TryParse(txtPrecoVenda.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal precoVenda) &&
                        decimal.TryParse(txtImposto.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal imposto))
                    {
                        // Calcular o preço com imposto
                        decimal precoComImposto = precoVenda + (precoVenda * (imposto / 100));

                        // Adicionar ao DataTable
                        dataTable.Rows.Add(
                            txtNumeracao.Text,
                            txtCodigoDeBarras.Text,
                            txtNomeProduto.Text,
                            txtCategoria.Text,
                            precoComImposto.ToString("F2"), // Formata com 2 casas decimais
                            Math.Floor(imposto).ToString() // Remove as casas decimais do imposto
                        );

                        // Incrementar contador de itens
                        contadorItens++;
                        UpdateItemCount();

                        // Atualizar o total de vendas
                        AtualizarTotalVenda();

                        // Limpar campos para próxima entrada
                        ClearTextBoxes();

                        // Salvar no arquivo
                        string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";
                        try
                        {
                            string content = GetDataTableContent(dataTable);
                            SaveToFile(filePath, content);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao salvar os dados no arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro ao converter valores de preço ou imposto.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string file = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\total.txt";

            File.WriteAllText(filePath, string.Empty);
            File.WriteAllText(file, string.Empty);

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
    totalVenda = 0; // Inicializa o total da venda

    foreach (DataRow row in dataTable.Rows)
    {
        string precoTexto = row["Valor"].ToString().Trim(); // Pega o valor da coluna "Valor" e remove espaços em branco

        // Remover qualquer caractere não numérico, como vírgulas, se necessário
        precoTexto = precoTexto.Replace(",", "."); // Substitui vírgula por ponto

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

    // Atualiza o texto exibido no textBox9
    textBox9.Text = totalVenda.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
}


        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            // Chama o método para atualizar o total de vendas
            AtualizarTotalVenda();


            // Salva o conteúdo do textBox9 em um arquivo txt
            SalvarTextoEmArquivo();


        }

        // Método para salvar o texto do textBox9 em um arquivo txt
        private void SalvarTextoEmArquivo()
        {
            try
            {
                // O caminho onde o arquivo será salvo. Pode ser ajustado conforme necessário.
                string caminhoArquivo = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\total.txt";

                // Verifica se o diretório existe, caso contrário, cria o diretório
                string diretorio = Path.GetDirectoryName(caminhoArquivo);
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                // Usando StreamWriter para salvar o texto no arquivo
                using (StreamWriter sw = new StreamWriter(caminhoArquivo))
                {
                    // Escreve o conteúdo do TextBox no arquivo
                    sw.Write(textBox9.Text);
                }

                // Mensagem de sucesso (opcional)
                // MessageBox.Show("Arquivo salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro caso algo dê errado
                MessageBox.Show("Erro ao salvar o arquivo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
