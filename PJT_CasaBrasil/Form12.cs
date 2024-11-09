using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;

namespace PJT_CasaBrasil
{
    public partial class Form12 : Form
    {
        // Propriedade para receber dados de Form1
        public string DataToDisplay { get; set; }
        private DataTable dataTable;
        private Timer timer;

        public Form12()
        {
            InitializeComponent();

            // Configura o Timer para chamar o evento a cada 1 segundo (1000 ms)
            timer = new Timer();
            timer.Interval = 1000; // 1000 ms = 1 segundo
            timer.Tick += Timer_Tick; // Evento quando o tempo expirar

        }

    

        private void Form12_Load(object sender, EventArgs e)
        {
            txtCodigoDeBarras.Text = DataToDisplay;
            // Obtém todos os monitores conectados
            Screen[] screens = Screen.AllScreens;

            // Verifica se existe pelo menos dois monitores
            if (screens.Length > 0)
            {
                // Define a localização do formulário no segundo monitor
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(screens[0].Bounds.X, screens[0].Bounds.Y);
                this.Size = new Size(screens[0].Bounds.Width, screens[0].Bounds.Height); // Ajusta o tamanho
                this.WindowState = FormWindowState.Maximized; // Maximiza o formulário
                // this.FormBorderStyle = FormBorderStyle.None; // Remove a borda do formulário
            }
            else
            {
                MessageBox.Show("O segundo monitor não está disponível. O formulário não será exibido.");
                this.Close(); // Fecha o formulário se o segundo monitor não existir
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Tenta mover o formulário para o segundo monitor
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 0)
            {
                this.Location = new Point(screens[0].Bounds.X, screens[0].Bounds.Y);
                this.Size = new Size(screens[0].Bounds.Width, screens[0].Bounds.Height);
            }
            else
            {
                MessageBox.Show("Este aplicativo só pode ser executado no segundo monitor.");
                this.Close();
            }


            // Caminho do arquivo de texto
            string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";

            try
            {
                // Carregar dados do arquivo de texto no DataTable
                dataTable = LoadDataFromFile(filePath);

                // Exibir os dados no DataGridView
                dataGrid1.DataSource = dataTable;

                // Iniciar o Timer
                timer.Start();
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Erro ao carregar os dados do arquivo: {ioEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para carregar dados de um arquivo TXT e inseri-los em um DataTable
        private DataTable LoadDataFromFile(string filePath)
        {
            DataTable dataTable = new DataTable();

            try
            {
                // Lê todas as linhas do arquivo de texto
                var lines = File.ReadAllLines(filePath);

                if (lines.Length > 0)
                {
                    // A primeira linha contém os nomes das colunas (cabeçalhos)
                    var columnHeaders = lines[0].Split('\t'); // Assume que a tabulação foi usada como delimitador
                    foreach (var columnHeader in columnHeaders)
                    {
                        dataTable.Columns.Add(columnHeader); // Cria uma coluna para cada cabeçalho
                    }

                    // As linhas seguintes contêm os dados
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var rowValues = lines[i].Split('\t'); // Divide a linha pelos tabuladores
                        dataTable.Rows.Add(rowValues); // Adiciona a linha ao DataTable
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o arquivo: {ex.Message}", "Erro ao carregar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;  // Rethrow para que o tratamento no Form1_Load também seja acionado
            }

            return dataTable;
        }

        // Evento Tick do Timer: atualiza o DataTable a cada 1 segundo
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Caminho do arquivo de texto
            string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";

            try
            {
                // Recarregar dados do arquivo de texto
                dataTable = LoadDataFromFile(filePath);

                // Atualizar o DataGridView com os novos dados
                dataGrid1.DataSource = dataTable;

                // Mostrar o último dado inserido nos TextBoxes
                ShowLastInsertedData();
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Erro ao carregar os dados do arquivo: {ioEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    }

        // Método para mostrar o último dado inserido nos TextBoxes
        private void ShowLastInsertedData()
        {
            // Verifique se há pelo menos uma linha no DataTable
            if (dataTable.Rows.Count > 0)
            {
                // Pega a última linha do DataTable
                DataRow lastRow = dataTable.Rows[dataTable.Rows.Count - 1];

                // Atualiza os TextBoxes com os dados da última linha
                txtNumeracao.Text = lastRow["Item"].ToString(); // Substitua com o nome real da coluna
                txtCodigoDeBarras.Text = lastRow["Código"].ToString(); // Substitua com o nome real da coluna
                txtNomeProduto.Text = lastRow["Produto"].ToString(); // Substitua com o nome real da coluna
                txtCategoria.Text = lastRow["Descrição"].ToString(); // Substitua com o nome real da coluna
                txtPrecoVenda.Text = lastRow["Valor"].ToString(); // Substitua com o nome real da coluna
                txtImposto.Text = lastRow["Taxa"].ToString();
            }

            else
            {
                // Caso o DataTable esteja vazio, limpa os TextBoxes
                txtNumeracao.Clear();
                txtCodigoDeBarras.Clear();
                txtNomeProduto.Clear();
                txtCategoria.Clear();
                txtPrecoVenda.Clear();
                txtImposto.Clear();
            } 
        }
    }
}

