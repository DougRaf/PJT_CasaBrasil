using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
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
        private decimal totalVenda = 0; // Para armazenar o total das vendas
                                      
        private decimal valorPago = 0;


        public Form7()
        {
            InitializeComponent();
            txtCodigoDeBarras.TextChanged += TxtCodigoDeBarras_TextChanged;

            InitializeDataTable();
            dataGrid1.DataSource = dataTable;

            itemCount = 0; // Inicializa o contador
            UpdateItemCount();
            txtNumeracao.Clear();
            txtCodigoDeBarras.Focus();
            txtNumeracao.ReadOnly = true;
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
            // Iniciar a impressão de forma assíncrona 
            PrintAsync();

            // Quando for inserir os dados no banco
            InsertDataIntoDatabase();
        }

        private async void PrintAsync()
        {
            await Task.Run(() => ImprimirNotaFiscal());
        }

        private async void PrintAsyncCredito()
        {
            await Task.Run(() => ImprimirNotaFiscalCredito());
        }

        private async void PrintAsyncPrazo()
        {
            await Task.Run(() => ImprimirNotaFiscalCredito());
        }

        private void ImprimirNotaFiscalPrazo()
        {
            try
            {
                // Define a impressora e o método de impressão
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(prazo);
                pd.PrinterSettings.PrinterName = "XP-58";  // Defina a impressora desejada

                // Imprime sem abrir a caixa de diálogo
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar imprimir: " + ex.Message);
            }
        }


        private void ImprimirNotaFiscal()
        {
            try
            {
                // Define a impressora e o método de impressão
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(PrintPage);
                pd.PrinterSettings.PrinterName = "XP-58";  // Defina a impressora desejada

                // Imprime sem abrir a caixa de diálogo
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar imprimir: " + ex.Message);
            }
        }

        private void ImprimirNotaFiscalCredito()
        {
            try
            {
                // Define a impressora e o método de impressão
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(credito);
                pd.PrinterSettings.PrinterName = "XP-58";  // Defina a impressora desejada

                // Imprime sem abrir a caixa de diálogo
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar imprimir: " + ex.Message);
            }
        }

    

        // Método para capturar o valor pago antes de imprimir
        private void CapturarValorPago()
        {
            // Exemplo de captura: caixa de entrada de texto
            string entradaUsuario = txtCobrar.Text; // Supondo que o TextBox chama-se textBoxValorPago
            if (!decimal.TryParse(entradaUsuario, out valorPago) || valorPago < 0)
            {
                MessageBox.Show("Insira um valor válido para o pagamento.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                valorPago = 0;
            }
        }
        private void PrintPage(object sender, PrintPageEventArgs e)
{
    if (dataTable.Rows.Count == 0)
    {
        MessageBox.Show("Não há dados para serem impressos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    // Fontes
    Font fonteCabecalho = new Font("Arial", 8, FontStyle.Bold);
    Font fonteItens = new Font("Arial", 6, FontStyle.Bold);
    Font fonteFooter = new Font("Arial", 6, FontStyle.Bold);

    // Cor preta personalizada
    Brush corPretaDensa = new SolidBrush(Color.Black);

    // Margens e largura útil
    float margem = 5;
    float larguraUtil = e.PageBounds.Width - 2 * margem;

    // Posição inicial
    float posY = margem;

    // Função para reforçar texto
    void DrawTextReinforced(Graphics g, string texto, Font fonte, Brush cor, PointF pos)
    {
        g.DrawString(texto, fonte, cor, pos);
    }

    // Desenha a imagem
    Image imagemOriginal = Image.FromFile("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\logo.png");
    Image imagemEscurecida = DarkenImage(imagemOriginal);

    int tamanhoImagem = 80;
    float posXImagem = (e.PageBounds.Width - tamanhoImagem) / 2;
    e.Graphics.DrawImage(imagemEscurecida, new RectangleF(posXImagem, posY, tamanhoImagem, tamanhoImagem));

    posY += tamanhoImagem + 5;

    // Data e hora no formato japonês (AAAA-MM-DD HH:mm)
    string dataHoraOperacao = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
    float larguraDataHora = e.Graphics.MeasureString(dataHoraOperacao, fonteCabecalho).Width;
    float posXDataHora = (e.PageBounds.Width - larguraDataHora) / 2;
    DrawTextReinforced(e.Graphics, dataHoraOperacao, fonteCabecalho, corPretaDensa, new PointF(posXDataHora, posY));

    posY += e.Graphics.MeasureString(dataHoraOperacao, fonteCabecalho).Height + 5;

    // Desenha o cabeçalho
    string textoNotaFiscal = "Nota Fiscal de Compra!";
    float larguraTextoNotaFiscal = e.Graphics.MeasureString(textoNotaFiscal, fonteCabecalho).Width;
    float posXTextoNotaFiscal = (e.PageBounds.Width - larguraTextoNotaFiscal) / 2;
    DrawTextReinforced(e.Graphics, textoNotaFiscal, fonteCabecalho, corPretaDensa, new PointF(posXTextoNotaFiscal, posY));

    posY += e.Graphics.MeasureString(textoNotaFiscal, fonteCabecalho).Height + 2;

    // Inicializando variáveis para somar os impostos
    decimal imposto8Porcento = 0;
    decimal imposto10Porcento = 0;
    decimal totalSemImposto = 0;

    // Adiciona os itens
    string textoItens = "------------------------\nItens Inclusos\n------------------------\n";
    for (int i = 0; i < dataTable.Rows.Count; i++)
    {
        var item = dataTable.Rows[i];
        string nomeProduto = item["Produto"].ToString();
        string valor = item["Valor"] as string ?? "0";
        string taxa = item["Taxa"] as string ?? "0";

        if (nomeProduto.Length > 3)
        {
            nomeProduto = nomeProduto.Substring(0, 3) + "...";
        }

        decimal valorDecimal = decimal.TryParse(valor, out var v) ? v : 0;
        decimal taxaDecimal = decimal.TryParse(taxa, out var t) ? t : 0;
        decimal valorSemTaxa = Math.Round(valorDecimal / (1 + (taxaDecimal / 100)), 0);
        decimal impostoProduto = Math.Round(valorSemTaxa * (taxaDecimal / 100), 0);

        if (taxaDecimal == 8)
        {
            imposto8Porcento += impostoProduto;
        }
        else if (taxaDecimal == 10)
        {
            imposto10Porcento += impostoProduto;
        }

        textoItens += $"{i + 1}- {nomeProduto} - ¥{valorSemTaxa}   (Imposto {taxaDecimal}%: ¥{impostoProduto})\n";
        totalSemImposto += valorSemTaxa;
    }

    textoItens += "------------------------\n";
    textoItens += $"Imposto 8%: ¥{imposto8Porcento:N0}\n";
    textoItens += $"Imposto 10%: ¥{imposto10Porcento:N0}\n";

    decimal totalComImposto = totalSemImposto + imposto8Porcento + imposto10Porcento;

    textoItens += "------------------------\n";
    textoItens += $"Imposto Total: ¥{(imposto8Porcento + imposto10Porcento):N0}\n";
    textoItens += $"Total: ¥{totalComImposto:N0}\n";

    // Pega o valor pago do TextBox txtCobrar.Text
    decimal valorPago = 0;
    if (decimal.TryParse(txtCobrar.Text, out valorPago))
    {
        // Calcula o troco
        decimal troco = valorPago - totalComImposto;
        textoItens += "------------------------\n";
        textoItens += $"Valor Pago: ¥{valorPago:N0}\n";
        textoItens += $"Troco: ¥{(troco >= 0 ? troco : 0):N0}\n"; // Evita valores negativos
    }
    else
    {
        // Caso o valor no TextBox não seja um número válido
        textoItens += "Valor Pago: INVÁLIDO\n";
        textoItens += "Troco: INVÁLIDO\n";
    }

    textoItens += "------------------------\n";
    textoItens += "Obrigado pela Preferência\n";

    DrawTextReinforced(e.Graphics, textoItens, fonteItens, corPretaDensa, new PointF(margem, posY));
    posY += e.Graphics.MeasureString(textoItens, fonteItens).Height + 2;

    posY += 18;

    float linhaY = posY;
    e.Graphics.DrawLine(new Pen(corPretaDensa), margem, linhaY, e.PageBounds.Width - margem, linhaY);

    posY = linhaY + 2;

    // Footer
    string textoFooter = "Desenvolvido por DGSSISTEMAS® 2024";
    string linkFooter = "www.dgssistemas.net";

    float larguraTextoFooter = e.Graphics.MeasureString(textoFooter, fonteFooter).Width;
    float larguraLinkFooter = e.Graphics.MeasureString(linkFooter, fonteFooter).Width;

    float posXTextoFooter = (e.PageBounds.Width - larguraTextoFooter) / 2;
    float posXLinkFooter = (e.PageBounds.Width - larguraLinkFooter) / 2;

    DrawTextReinforced(e.Graphics, textoFooter, fonteFooter, corPretaDensa, new PointF(posXTextoFooter, posY));
    posY += e.Graphics.MeasureString(textoFooter, fonteFooter).Height + 2;

    DrawTextReinforced(e.Graphics, linkFooter, fonteFooter, corPretaDensa, new PointF(posXLinkFooter, posY));
    posY += e.Graphics.MeasureString(linkFooter, fonteFooter).Height;

    float linhaFooterY = posY + 2;
    e.Graphics.DrawLine(new Pen(corPretaDensa), margem, linhaFooterY, e.PageBounds.Width - margem, linhaFooterY);

    // Salvar no arquivo
    string caminhoArquivo = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\vendas.txt";
    StringBuilder conteudoArquivo = new StringBuilder();

    for (int i = 0; i < dataTable.Rows.Count; i++)
    {
        var item = dataTable.Rows[i];
        string nomeProduto = item["Produto"].ToString();
        string codigoProduto = item["Código"].ToString();
        string formaPagamento = "Dinheiro";
        string taxa = item["Taxa"] as string ?? "0";
        string valor = item["Valor"] as string ?? "0";

        decimal valorDecimal = decimal.TryParse(valor, out var v) ? v : 0;
        decimal taxaDecimal = decimal.TryParse(taxa, out var t) ? t : 0;
        decimal valorSemTaxa = Math.Round(valorDecimal / (1 + (taxaDecimal / 100)), 0);
        decimal impostoProduto = Math.Round(valorSemTaxa * (taxaDecimal / 100), 0);

        string dataHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        conteudoArquivo.AppendLine(
            $"{nomeProduto};{codigoProduto};{formaPagamento};{(int)valorDecimal};{taxaDecimal};{valorSemTaxa};{impostoProduto};{dataHora}");
    }

    // Salvar no arquivo txt
    File.WriteAllText(caminhoArquivo, conteudoArquivo.ToString());
}


        private void InsertDataIntoDatabase()
        {
            string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;"; // String de conexão com o banco de dados
            string caminhoArquivo = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\vendas.txt";
            string caminhoArquivoLimpar = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt"; // Caminho do arquivo a ser limpo

            try
            {
                // Verifica se o arquivo de vendas existe
                if (!File.Exists(caminhoArquivo))
                {
                    MessageBox.Show("O arquivo vendas.txt não foi encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lê todas as linhas do arquivo de vendas
                string[] linhas = File.ReadAllLines(caminhoArquivo);

                // Verifica se o arquivo de vendas está vazio
                if (linhas.Length == 0)
                {
                    MessageBox.Show("O arquivo vendas.txt está vazio. Não há dados para inserir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (string linha in linhas)
                    {
                        // Ignora linhas vazias
                        if (string.IsNullOrWhiteSpace(linha))
                            continue;

                        // Divide os valores da linha pelo delimitador ';'
                        string[] valores = linha.Split(';');
                        if (valores.Length != 8)
                        {
                            MessageBox.Show($"Formato inválido na linha: {linha}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        // Atribui os valores às variáveis com os novos nomes
                        string nomeProduto = valores[0]; // nomeProduto
                        string codigoBarras = valores[1]; // codigoBarras
                        string pagamento = valores[2]; // pagamento
                        decimal impostoPorcentagem = decimal.Parse(valores[3]); // impostoPorcentagem
                        decimal impostoValor = decimal.Parse(valores[4]); // impostoValor
                        decimal totalSemImposto = decimal.Parse(valores[5]); // totalSemImposto
                        decimal totalComImposto = decimal.Parse(valores[6]); // totalComImposto
                        DateTime dataHoraOperacao = DateTime.Parse(valores[7]); // dataHoraOperacao

                        // Query SQL para inserção
                        string query = @"INSERT INTO vendas (nomeProduto, codigoBarras, pagamento, impostoPorcentagem, impostoValor, totalSemImposto, totalComImposto, dataHoraOperacao)
                                 VALUES (@nomeProduto, @codigoBarras, @pagamento, @impostoPorcentagem, @impostoValor, @totalSemImposto, @totalComImposto, @dataHoraOperacao);";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            // Adiciona os parâmetros
                            cmd.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                            cmd.Parameters.AddWithValue("@codigoBarras", codigoBarras);
                            cmd.Parameters.AddWithValue("@pagamento", pagamento);
                            cmd.Parameters.AddWithValue("@impostoPorcentagem", impostoPorcentagem);
                            cmd.Parameters.AddWithValue("@impostoValor", impostoValor);
                            cmd.Parameters.AddWithValue("@totalSemImposto", totalSemImposto);
                            cmd.Parameters.AddWithValue("@totalComImposto", totalComImposto);
                            cmd.Parameters.AddWithValue("@dataHoraOperacao", dataHoraOperacao);

                            // Executa o comando
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Limpa o conteúdo do arquivo vendas.txt após a inserção dos dados
                    File.WriteAllText(caminhoArquivo, string.Empty);

                    // Limpa o conteúdo do arquivo arquivo.txt
                    File.WriteAllText(caminhoArquivoLimpar, string.Empty);

                    // Exibe a mensagem de sucesso
                    ShowMessageOnSecondMonitor("Compra e pagamento feito com sucesso !!!");

                    // Limpa os dados do formulário 
                    LimparFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir os dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowMessageOnSecondMonitor(string message)
        {
            // Verifica se há mais de um monitor conectado
            if (Screen.AllScreens.Length > 1)
            {
                // Obtém o segundo monitor
                var secondMonitor = Screen.AllScreens[1];

                // Cria um formulário temporário para mostrar a mensagem no segundo monitor
                Form messageForm = new Form
                {
                    StartPosition = FormStartPosition.Manual,
                    Size = new System.Drawing.Size(400, 200),
                    Text = "Mensagem de Sucesso",
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    ControlBox = false,
                    ShowInTaskbar = false, // Não aparece na barra de tarefas
                    Location = secondMonitor.Bounds.Location
                };

                // Centraliza o formulário na tela do segundo monitor
                messageForm.Location = new System.Drawing.Point(
                    secondMonitor.Bounds.Left + (secondMonitor.Bounds.Width - messageForm.Width) / 2,
                    secondMonitor.Bounds.Top + (secondMonitor.Bounds.Height - messageForm.Height) / 2
                );

                // Cria um label para exibir a mensagem
                Label label = new Label
                {
                    Text = message,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                    Padding = new Padding(20)
                };

                // Adiciona o label ao formulário
                messageForm.Controls.Add(label);

                // Cria o botão "OK"
                Button okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Size = new System.Drawing.Size(75, 30)
                };

                // Calcula a posição para centralizar o botão
                okButton.Location = new System.Drawing.Point(
                    (messageForm.Width - okButton.Width) / 2,
                    messageForm.Height - okButton.Height - 20
                );

                // Adiciona o botão "OK" ao formulário
                messageForm.Controls.Add(okButton);
                messageForm.AcceptButton = okButton;

                // Cria o temporizador para fechar o formulário após 5 segundos
                Timer timer = new Timer
                {
                    Interval = 5000 // 5000ms = 5 segundos
                };

                timer.Tick += (sender, e) =>
                {
                    messageForm.Close();  // Fecha o formulário
                    timer.Stop(); // Para o temporizador
                };

                timer.Start(); // Inicia o temporizador

                // Exibe o formulário
                messageForm.ShowDialog();
            }
            else
            {
                // Caso não haja segundo monitor, exibe a mensagem na tela principal
                MessageBox.Show(message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }




        private void LimparFormulario()
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
        

            // Mensagem informando que a venda foi cancelada MessageBox.Show("Venda cancelada com sucesso!", "Cancelamento", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

       


        private Image DarkenImage(Image originalImage)
        {
            // Cria uma nova imagem com o mesmo tamanho da original
            Bitmap darkenedImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Cria um objeto Graphics para aplicar o filtro de escurecimento
            using (Graphics g = Graphics.FromImage(darkenedImage))
            {
                // Define um filtro para escurecer a imagem
                ColorMatrix matrix = new ColorMatrix(new float[][]
                {
            new float[] {0.05f, 0, 0, 0, 0}, // Red (bem escuro)
            new float[] {0, 0.05f, 0, 0, 0}, // Green (bem escuro)
            new float[] {0, 0, 0.05f, 0, 0}, // Blue (bem escuro)
            new float[] {0, 0, 0, 1, 0},     // Alpha (não muda)
            new float[] {0, 0, 0, 0, 1}      // Branco (não muda)
                });

                // Cria o objeto ImageAttributes para aplicar a matriz de cor
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix);

                // Desenha a imagem original, mas com o filtro de escurecimento
                g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                0, 0, originalImage.Width, originalImage.Height,
                                GraphicsUnit.Pixel, attributes);
            }

            return darkenedImage;
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
                        // Coloca o foco no campo 'txtCodigoDeBarras'
                        txtCodigoDeBarras.Focus();
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
                                    return;
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
            txtCodigoDeBarras.Focus();
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
            // Habilita o campo para entrada de numeração
            txtNumeracao.ReadOnly = false;

            // Exibe uma mensagem para o usuário inserir o número do item
            MessageBox.Show("Digite o número do item que deseja remover no campo apropriado e pressione 'OK'.", "Remover Item", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Verifica se o campo txtNumeracao foi preenchido
            if (!int.TryParse(txtNumeracao.Text, out int numeroItem) || numeroItem <= 0 || numeroItem > dataTable.Rows.Count)
            {
                MessageBox.Show("Número inválido. Por favor, insira um número de item válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirma a remoção do item
            DialogResult confirmacao = MessageBox.Show($"Deseja realmente remover o item número {numeroItem}?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacao == DialogResult.Yes)
            {
                // Obtém o índice do item a ser removido
                int rowIndex = numeroItem - 1;

                // Obtém o código do produto para restaurar a quantidade no banco
                string codigoBarras = dataTable.Rows[rowIndex]["Código"].ToString();

                // Remove a linha do DataTable
                dataTable.Rows.RemoveAt(rowIndex);

                // Atualiza o contador de itens
                contadorItens--;
                UpdateItemCount();

                // Atualiza o total da venda
                AtualizarTotalVenda();

                // Limpa o campo de entrada e desabilita novamente
                txtNumeracao.Clear();
                txtNumeracao.ReadOnly = true;

                // Restaura a quantidade do produto no banco de dados
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE produto SET quantidade = quantidade + 1 WHERE codigo_barras = @codigo";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@codigo", codigoBarras);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao atualizar o banco de dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Mensagem de sucesso
                MessageBox.Show("Item removido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCodigoDeBarras.Focus();
            }
            else
            {
                // Cancela a remoção e desabilita o campo novamente
                txtNumeracao.Clear();
                txtNumeracao.ReadOnly = true;
            }
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

        private void credito(object sender, PrintPageEventArgs e)
        {
            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para serem impressos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Fontes
            Font fonteCabecalho = new Font("Arial", 8, FontStyle.Bold);
            Font fonteItens = new Font("Arial", 6, FontStyle.Bold);
            Font fonteFooter = new Font("Arial", 6, FontStyle.Bold);

            // Cor preta personalizada
            Brush corPretaDensa = new SolidBrush(Color.Black);

            // Margens e largura útil
            float margem = 5;
            float larguraUtil = e.PageBounds.Width - 2 * margem;

            // Posição inicial
            float posY = margem;

            // Função para reforçar texto
            void DrawTextReinforced(Graphics g, string texto, Font fonte, Brush cor, PointF pos)
            {
                g.DrawString(texto, fonte, cor, pos);
            }

            // Desenha a imagem
            Image imagemOriginal = Image.FromFile("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\logo.png");
            Image imagemEscurecida = DarkenImage(imagemOriginal);

            int tamanhoImagem = 80;
            float posXImagem = (e.PageBounds.Width - tamanhoImagem) / 2;
            e.Graphics.DrawImage(imagemEscurecida, new RectangleF(posXImagem, posY, tamanhoImagem, tamanhoImagem));

            posY += tamanhoImagem + 5;

            // Data e hora no formato japonês (AAAA-MM-DD HH:mm)
            string dataHoraOperacao = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            float larguraDataHora = e.Graphics.MeasureString(dataHoraOperacao, fonteCabecalho).Width;
            float posXDataHora = (e.PageBounds.Width - larguraDataHora) / 2;
            DrawTextReinforced(e.Graphics, dataHoraOperacao, fonteCabecalho, corPretaDensa, new PointF(posXDataHora, posY));

            posY += e.Graphics.MeasureString(dataHoraOperacao, fonteCabecalho).Height + 5;

            // Desenha o cabeçalho
            string textoNotaFiscal = "Nota Fiscal de Compra!";
            float larguraTextoNotaFiscal = e.Graphics.MeasureString(textoNotaFiscal, fonteCabecalho).Width;
            float posXTextoNotaFiscal = (e.PageBounds.Width - larguraTextoNotaFiscal) / 2;
            DrawTextReinforced(e.Graphics, textoNotaFiscal, fonteCabecalho, corPretaDensa, new PointF(posXTextoNotaFiscal, posY));

            posY += e.Graphics.MeasureString(textoNotaFiscal, fonteCabecalho).Height + 2;

            // Inicializando variáveis para somar os impostos
            decimal imposto8Porcento = 0;
            decimal imposto10Porcento = 0;
            decimal totalSemImposto = 0;

            // Adiciona os itens
            string textoItens = "------------------------\nItens Inclusos\n------------------------\n";
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var item = dataTable.Rows[i];
                string nomeProduto = item["Produto"].ToString();
                string valor = item["Valor"] as string ?? "0";
                string taxa = item["Taxa"] as string ?? "0";

                if (nomeProduto.Length > 3)
                {
                    nomeProduto = nomeProduto.Substring(0, 3) + "...";
                }

                decimal valorDecimal = decimal.TryParse(valor, out var v) ? v : 0;
                decimal taxaDecimal = decimal.TryParse(taxa, out var t) ? t : 0;
                decimal valorSemTaxa = Math.Round(valorDecimal / (1 + (taxaDecimal / 100)), 0);
                decimal impostoProduto = Math.Round(valorSemTaxa * (taxaDecimal / 100), 0);

                if (taxaDecimal == 8)
                {
                    imposto8Porcento += impostoProduto;
                }
                else if (taxaDecimal == 10)
                {
                    imposto10Porcento += impostoProduto;
                }

                textoItens += $"{i + 1}- {nomeProduto} - ¥{valorSemTaxa}   (Imposto {taxaDecimal}%: ¥{impostoProduto})\n";
                totalSemImposto += valorSemTaxa;
            }

            textoItens += "------------------------\n";
            textoItens += $"Imposto 8%: ¥{imposto8Porcento:N0}\n";
            textoItens += $"Imposto 10%: ¥{imposto10Porcento:N0}\n";

            decimal totalComImposto = totalSemImposto + imposto8Porcento + imposto10Porcento;

            textoItens += "------------------------\n";
            textoItens += $"Imposto Total: ¥{(imposto8Porcento + imposto10Porcento):N0}\n";
            textoItens += $"Total: ¥{totalComImposto:N0}\n";
            textoItens += "------------------------\n";
            textoItens += "Obrigado pela Preferência\n";

            DrawTextReinforced(e.Graphics, textoItens, fonteItens, corPretaDensa, new PointF(margem, posY));
            posY += e.Graphics.MeasureString(textoItens, fonteItens).Height + 2;

            posY += 18;

            float linhaY = posY;
            e.Graphics.DrawLine(new Pen(corPretaDensa), margem, linhaY, e.PageBounds.Width - margem, linhaY);

            posY = linhaY + 2;

            // Footer
            string textoFooter = "Desenvolvido por DGSSISTEMAS® 2024";
            string linkFooter = "www.dgssistemas.net";

            float larguraTextoFooter = e.Graphics.MeasureString(textoFooter, fonteFooter).Width;
            float larguraLinkFooter = e.Graphics.MeasureString(linkFooter, fonteFooter).Width;

            float posXTextoFooter = (e.PageBounds.Width - larguraTextoFooter) / 2;
            float posXLinkFooter = (e.PageBounds.Width - larguraLinkFooter) / 2;

            DrawTextReinforced(e.Graphics, textoFooter, fonteFooter, corPretaDensa, new PointF(posXTextoFooter, posY));
            posY += e.Graphics.MeasureString(textoFooter, fonteFooter).Height + 2;

            DrawTextReinforced(e.Graphics, linkFooter, fonteFooter, corPretaDensa, new PointF(posXLinkFooter, posY));
            posY += e.Graphics.MeasureString(linkFooter, fonteFooter).Height;

            float linhaFooterY = posY + 2;
            e.Graphics.DrawLine(new Pen(corPretaDensa), margem, linhaFooterY, e.PageBounds.Width - margem, linhaFooterY);

            // Salvar no arquivo
            string caminhoArquivo = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\vendas.txt";
            StringBuilder conteudoArquivo = new StringBuilder();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var item = dataTable.Rows[i];
                string nomeProduto = item["Produto"].ToString();
                string codigoProduto = item["Código"].ToString();
                string formaPagamento = "Credito";
                string taxa = item["Taxa"] as string ?? "0";
                string valor = item["Valor"] as string ?? "0";

                decimal valorDecimal = decimal.TryParse(valor, out var v) ? v : 0;
                decimal taxaDecimal = decimal.TryParse(taxa, out var t) ? t : 0;
                decimal valorSemTaxa = Math.Round(valorDecimal / (1 + (taxaDecimal / 100)), 0);
                decimal impostoProduto = Math.Round(valorSemTaxa * (taxaDecimal / 100), 0);

                string dataHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                conteudoArquivo.AppendLine(
                    $"{nomeProduto};{codigoProduto};{formaPagamento};{(int)taxaDecimal};{(int)impostoProduto};{(int)valorSemTaxa};{(int)valorDecimal};{dataHora}"
                );
            }

            try
            {
                File.WriteAllText(caminhoArquivo, conteudoArquivo.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void prazo(object sender, PrintPageEventArgs e)
        {
            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para serem impressos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Fontes
            Font fonteCabecalho = new Font("Arial", 8, FontStyle.Bold);
            Font fonteItens = new Font("Arial", 6, FontStyle.Bold);
            Font fonteFooter = new Font("Arial", 6, FontStyle.Bold);

            // Cor preta personalizada
            Brush corPretaDensa = new SolidBrush(Color.Black);

            // Margens e largura útil
            float margem = 5;
            float larguraUtil = e.PageBounds.Width - 2 * margem;

            // Posição inicial
            float posY = margem;

            // Função para reforçar texto
            void DrawTextReinforced(Graphics g, string texto, Font fonte, Brush cor, PointF pos)
            {
                g.DrawString(texto, fonte, cor, pos);
            }

            // Desenha a imagem
            Image imagemOriginal = Image.FromFile("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Img\\logo.png");
            Image imagemEscurecida = DarkenImage(imagemOriginal);

            int tamanhoImagem = 80;
            float posXImagem = (e.PageBounds.Width - tamanhoImagem) / 2;
            e.Graphics.DrawImage(imagemEscurecida, new RectangleF(posXImagem, posY, tamanhoImagem, tamanhoImagem));

            posY += tamanhoImagem + 5;

            // Data e hora no formato japonês (AAAA-MM-DD HH:mm)
            string dataHoraOperacao = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            float larguraDataHora = e.Graphics.MeasureString(dataHoraOperacao, fonteCabecalho).Width;
            float posXDataHora = (e.PageBounds.Width - larguraDataHora) / 2;
            DrawTextReinforced(e.Graphics, dataHoraOperacao, fonteCabecalho, corPretaDensa, new PointF(posXDataHora, posY));

            posY += e.Graphics.MeasureString(dataHoraOperacao, fonteCabecalho).Height + 5;

            // Desenha o cabeçalho
            string textoNotaFiscal = "Nota Fiscal de Compra!";
            float larguraTextoNotaFiscal = e.Graphics.MeasureString(textoNotaFiscal, fonteCabecalho).Width;
            float posXTextoNotaFiscal = (e.PageBounds.Width - larguraTextoNotaFiscal) / 2;
            DrawTextReinforced(e.Graphics, textoNotaFiscal, fonteCabecalho, corPretaDensa, new PointF(posXTextoNotaFiscal, posY));

            posY += e.Graphics.MeasureString(textoNotaFiscal, fonteCabecalho).Height + 2;

            // Inicializando variáveis para somar os impostos
            decimal imposto8Porcento = 0;
            decimal imposto10Porcento = 0;
            decimal totalSemImposto = 0;

            // Adiciona os itens
            string textoItens = "------------------------\nItens Inclusos\n------------------------\n";
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var item = dataTable.Rows[i];
                string nomeProduto = item["Produto"].ToString();
                string valor = item["Valor"] as string ?? "0";
                string taxa = item["Taxa"] as string ?? "0";

                if (nomeProduto.Length > 3)
                {
                    nomeProduto = nomeProduto.Substring(0, 3) + "...";
                }

                decimal valorDecimal = decimal.TryParse(valor, out var v) ? v : 0;
                decimal taxaDecimal = decimal.TryParse(taxa, out var t) ? t : 0;
                decimal valorSemTaxa = Math.Round(valorDecimal / (1 + (taxaDecimal / 100)), 0);
                decimal impostoProduto = Math.Round(valorSemTaxa * (taxaDecimal / 100), 0);

                if (taxaDecimal == 8)
                {
                    imposto8Porcento += impostoProduto;
                }
                else if (taxaDecimal == 10)
                {
                    imposto10Porcento += impostoProduto;
                }

                textoItens += $"{i + 1}- {nomeProduto} - ¥{valorSemTaxa}   (Imposto {taxaDecimal}%: ¥{impostoProduto})\n";
                totalSemImposto += valorSemTaxa;
            }

            textoItens += "------------------------\n";
            textoItens += $"Imposto 8%: ¥{imposto8Porcento:N0}\n";
            textoItens += $"Imposto 10%: ¥{imposto10Porcento:N0}\n";

            decimal totalComImposto = totalSemImposto + imposto8Porcento + imposto10Porcento;

            textoItens += "------------------------\n";
            textoItens += $"Imposto Total: ¥{(imposto8Porcento + imposto10Porcento):N0}\n";
            textoItens += $"Total: ¥{totalComImposto:N0}\n";
            textoItens += "------------------------\n";
            textoItens += "Obrigado pela Preferência\n";

            DrawTextReinforced(e.Graphics, textoItens, fonteItens, corPretaDensa, new PointF(margem, posY));
            posY += e.Graphics.MeasureString(textoItens, fonteItens).Height + 2;

            posY += 18;

            float linhaY = posY;
            e.Graphics.DrawLine(new Pen(corPretaDensa), margem, linhaY, e.PageBounds.Width - margem, linhaY);

            posY = linhaY + 2;

            // Footer
            string textoFooter = "Desenvolvido por DGSSISTEMAS® 2024";
            string linkFooter = "www.dgssistemas.net";

            float larguraTextoFooter = e.Graphics.MeasureString(textoFooter, fonteFooter).Width;
            float larguraLinkFooter = e.Graphics.MeasureString(linkFooter, fonteFooter).Width;

            float posXTextoFooter = (e.PageBounds.Width - larguraTextoFooter) / 2;
            float posXLinkFooter = (e.PageBounds.Width - larguraLinkFooter) / 2;

            DrawTextReinforced(e.Graphics, textoFooter, fonteFooter, corPretaDensa, new PointF(posXTextoFooter, posY));
            posY += e.Graphics.MeasureString(textoFooter, fonteFooter).Height + 2;

            DrawTextReinforced(e.Graphics, linkFooter, fonteFooter, corPretaDensa, new PointF(posXLinkFooter, posY));
            posY += e.Graphics.MeasureString(linkFooter, fonteFooter).Height;

            float linhaFooterY = posY + 2;
            e.Graphics.DrawLine(new Pen(corPretaDensa), margem, linhaFooterY, e.PageBounds.Width - margem, linhaFooterY);

            // Salvar no arquivo
            string caminhoArquivo = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\vendas.txt";
            StringBuilder conteudoArquivo = new StringBuilder();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var item = dataTable.Rows[i];
                string nomeProduto = item["Produto"].ToString();
                string codigoProduto = item["Código"].ToString();
                string formaPagamento = "Prazo";
                string taxa = item["Taxa"] as string ?? "0";
                string valor = item["Valor"] as string ?? "0";

                decimal valorDecimal = decimal.TryParse(valor, out var v) ? v : 0;
                decimal taxaDecimal = decimal.TryParse(taxa, out var t) ? t : 0;
                decimal valorSemTaxa = Math.Round(valorDecimal / (1 + (taxaDecimal / 100)), 0);
                decimal impostoProduto = Math.Round(valorSemTaxa * (taxaDecimal / 100), 0);

                string dataHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                conteudoArquivo.AppendLine(
                    $"{nomeProduto};{codigoProduto};{formaPagamento};{(int)taxaDecimal};{(int)impostoProduto};{(int)valorSemTaxa};{(int)valorDecimal};{dataHora}"
                );
            }

            try
            {
                File.WriteAllText(caminhoArquivo, conteudoArquivo.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void button7_Click(object sender, EventArgs e)
        {

            // Iniciar a impressão de forma assíncrona 
            PrintAsyncCredito();

            // Quando for inserir os dados no banco
            InsertDataIntoDatabase();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Iniciar a impressão de forma assíncrona 
            PrintAsyncPrazo();

            // Quando for inserir os dados no banco
            InsertDataIntoDatabase();

        }

        private void txtCodigoDeBarras_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();
        }
    }
}
