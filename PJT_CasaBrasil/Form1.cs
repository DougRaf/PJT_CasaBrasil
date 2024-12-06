using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PJT_CasaBrasil
{
    public partial class Form1 : Form
    {
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            // Verifica se o valor no banco é "1" antes de iniciar o sistema
            VerificarBancoDeDados();

            InitializeProgressBar();
        }

        // Função para verificar o banco de dados antes de iniciar o sistema
        private void VerificarBancoDeDados()
        {
            string connString = "Server=68.178.145.20;Database=casabrasil;Uid=dgssistemasJP;Pwd=Douglas@2025;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    // Definir a consulta SQL
                    string query = "SELECT ativo FROM casabrasil LIMIT 1";

                    // Executar o comando SQL
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Obter o valor do banco de dados
                    object result = cmd.ExecuteScalar(); // ExecuteScalar retorna o valor da primeira célula da primeira linha

                    // Verificar se o resultado é nulo e exibir o valor
                    if (result != null)
                    {
                        string valor = result.ToString();
                        Console.WriteLine("Valor retornado: " + valor);

                        // Verificar se o valor retornado é "1"
                        if (valor == "1")
                        {
                            // Bloquear o fluxo (parar a execução do método)
                            MessageBox.Show("Valor é 1. Parando execução e matando os processos...");

                            // Matar os processos (cuidado com isso, pois pode finalizar processos importantes)
                            var processes = Process.GetProcesses();
                            foreach (var process in processes)
                            {
                                try
                                {
                                    // Exemplo de matar processos que têm o nome "notepad" (substitua conforme necessário)
                                    if (process.ProcessName.ToLower() == "notepad")
                                    {
                                        process.Kill();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Tratar exceções caso não seja possível matar algum processo
                                    MessageBox.Show("Erro ao matar processo: " + ex.Message);
                                }
                            }

                            // Fecha o formulário, interrompendo a execução
                            this.Close();
                            return; // Impede que o código continue
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nenhum valor encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar banco de dados: " + ex.Message);
                this.Close(); // Fecha o formulário em caso de erro na verificação
            }
        }

        private void InitializeProgressBar()
        {
            // Configurar o Timer
            timer = new Timer();
            timer.Interval = 100; // Intervalo de 100 ms
            timer.Tick += Timer_Tick;
            timer.Start();

            // Iniciar os serviços em segundo plano
            Task.Run(() =>
            {
                try
                {
                    // Verificar e iniciar o MAMP
                    string mampPath = @"C:\MAMP\MAMP.exe"; // Ajuste o caminho conforme necessário
                    if (System.IO.File.Exists(mampPath))
                    {
                        // Iniciar MAMP de forma silenciosa
                        StartProcess(mampPath, "-start"); // Adicionar parâmetros se necessário para iniciar sem janela
                    }
                    else
                    {
                        Console.WriteLine("O arquivo de inicialização do MAMP não foi encontrado em: " + mampPath);
                    }

                    // Iniciar o MySQL usando o mysqld diretamente
                    string mysqlPath = @"C:\MAMP\bin\mysql\bin\mysqld.exe"; // Caminho correto para o mysqld
                    if (System.IO.File.Exists(mysqlPath))
                    {
                        // Iniciar o MySQL de forma silenciosa
                        StartProcess(mysqlPath, "--console"); // Usando --console para evitar a janela do console
                    }
                    else
                    {
                        Console.WriteLine("O executável mysqld não foi encontrado em: " + mysqlPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro na inicialização dos serviços: " + ex.Message);
                }
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Atualizar a barra de progresso e o label
            if (progressBar.Value < progressBar.Maximum)
            {
                progressBar.Value += 1; // Aumenta o valor da barra de progresso
                lblPercentage.Text = $"{progressBar.Value}%"; // Atualiza o texto do label
            }
            else
            {
                timer.Stop(); // Para o timer quando a barra atinge 100%
                this.Close(); // Fecha o formulário de preloader
            }
        }

        private void StartProcess(string executablePath, string arguments = "")
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = arguments, // Passa argumentos se necessário
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true // Modo silencioso
                };

                var process = Process.Start(processStartInfo);
                if (process != null)
                {
                    process.WaitForExit();
                    // Se necessário, verificar a saída do processo
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    Console.WriteLine($"Output: {output}");
                    Console.WriteLine($"Error: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao executar o processo: " + ex.Message);
            }
        }
    }
}
