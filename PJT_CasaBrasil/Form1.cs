using System;
using System.Diagnostics;
using System.Windows.Forms;


namespace PJT_CasaBrasil
{
    public partial class Form1 : Form
    {
        private Timer timer;
        public Form1()
        {
            InitializeComponent();
            InitializeProgressBar();

        }

        private void InitializeProgressBar()
        {
            // Configurar o Timer
            timer = new Timer();
            timer.Interval = 100; // Intervalo de 100 ms
            timer.Tick += Timer_Tick;
            timer.Start();
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

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Caminho para o executável do MAMP
                string mampPath = @"C:\MAMP\MAMP.exe";

                // Configuração do processo
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = mampPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                // Inicializando o processo
                Process process = Process.Start(startInfo);

                if (process != null && !process.HasExited)
                {
                    MessageBox.Show("MAMP Server iniciado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao iniciar o MAMP Server:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

