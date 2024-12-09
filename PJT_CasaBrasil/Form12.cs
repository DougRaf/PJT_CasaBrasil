using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{
    public partial class Form12 : Form
    {
        public string DataToDisplay { get; set; }
        private DataTable dataTable;
        private Timer timer;
        private FileSystemWatcher _fileWatcher;

        public Form12()
        {
            InitializeComponent();

            // Configura o Timer
            timer = new Timer
            {
                Interval = 1000 // 1 segundo
            };
            timer.Tick += Timer_Tick;

            // Configura o FileSystemWatcher
            ConfigurarWatcher();
        }

            private void DispararMessageBox()
    {
        // Criar uma nova instância do Form7
        Form7 form7 = new Form7();
        

    }

        private void Form12_Load(object sender, EventArgs e)
        {
            txtCodigoDeBarras.Text = DataToDisplay;

            // Configura o formulário no monitor principal
            ConfigurarFormularioNoMonitor();

            // Caminho do arquivo de texto
            string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";


            try
            {
                // Carrega dados do arquivo
                dataTable = LoadDataFromFile(filePath);

                // Exibe os dados no DataGridView
                dataGrid1.DataSource = dataTable;

                // Inicia o Timer
                timer.Start();
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Erro ao carregar os dados do arquivo: {ioEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormularioNoMonitor()
        {
            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(screens[0].Bounds.X, screens[0].Bounds.Y);
                this.Size = new Size(screens[0].Bounds.Width, screens[0].Bounds.Height);
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                MessageBox.Show("O segundo monitor não está disponível. O formulário não será exibido.");
                this.Close();
            }
        }

        private DataTable LoadDataFromFile(string filePath)
        {
            DataTable dt = new DataTable();

            var lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                var headers = lines[0].Split('\t');
                foreach (var header in headers)
                {
                    dt.Columns.Add(header);
                }

                for (int i = 1; i < lines.Length; i++)
                {
                    var rowValues = lines[i].Split('\t');
                    dt.Rows.Add(rowValues);
                }
            }

            return dt;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\arquivo.txt";

            try
            {
                dataTable = LoadDataFromFile(filePath);
                dataGrid1.DataSource = dataTable;
                MostrarUltimaLinha();
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Erro ao carregar os dados do arquivo: {ioEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarUltimaLinha()
        {
            if (dataTable.Rows.Count > 0)
            {
                DataRow lastRow = dataTable.Rows[dataTable.Rows.Count - 1];
                txtNumeracao.Text = lastRow["Item"].ToString();
                txtCodigoDeBarras.Text = lastRow["Código"].ToString();
                txtNomeProduto.Text = lastRow["Produto"].ToString();
                txtCategoria.Text = lastRow["Descrição"].ToString();
                txtPrecoVenda.Text = lastRow["Valor"].ToString();
                txtImposto.Text = lastRow["Taxa"].ToString();
            }
            else
            {
                txtNumeracao.Clear();
                txtCodigoDeBarras.Clear();
                txtNomeProduto.Clear();
                txtCategoria.Clear();
                txtPrecoVenda.Clear();
                txtImposto.Clear();
            }
        }

        private void ConfigurarWatcher()
        {
            string filePath = @"C:\PJT_CasaBrasil\PJT_CasaBrasil\Resources\total.txt";

            _fileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(filePath),
                Filter = Path.GetFileName(filePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };

            _fileWatcher.Changed += (s, e) =>
            {
                System.Threading.Thread.Sleep(100);
                this.Invoke(new Action(() => AtualizarTxtTotal(e.FullPath)));
            };

            _fileWatcher.EnableRaisingEvents = true;
        }

        private void AtualizarTxtTotal(string caminhoArquivo)
        {
            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    string conteudo = File.ReadAllText(caminhoArquivo);
                    txtTotal.Text = conteudo;
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Erro ao acessar o arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _fileWatcher?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
