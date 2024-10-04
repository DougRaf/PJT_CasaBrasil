using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using System.Drawing.Printing;
using System.IO;
using PJT_CasaBrasil.Properties;
namespace PJT_CasaBrasil
{

    public partial class Form9 : Form
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        //   private PrintDocument printDocument;

        private string printerName = "XP-58"; // Nome da impressora

        private NotifyIcon notifyIcon1;
        private int userId;

        public Form9(int userId)
        {

              InitializeComponent();//          C:\\Users\\User\\Desktop\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3
            this.userId = userId;
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            button1.MouseEnter += Button2_MouseEnter;
            button1.MouseLeave += Button2_MouseLeave;
            button2.MouseEnter += Button2_MouseEnter;
            button2.MouseLeave += Button2_MouseLeave;
            button3.MouseEnter += Button2_MouseEnter;
            button3.MouseLeave += Button2_MouseLeave;
            button4.MouseEnter += Button2_MouseEnter;
            button4.MouseLeave += Button2_MouseLeave;
          
            button6.MouseEnter += Button2_MouseEnter;
            button6.MouseLeave += Button2_MouseLeave;
            button7.MouseEnter += Button2_MouseEnter;
            button7.MouseLeave += Button2_MouseLeave;
            button8.MouseEnter += Button2_MouseEnter;
            button8.MouseLeave += Button2_MouseLeave;

            // Inicializa o NotifyIcon
            notifyIcon1 = new NotifyIcon
            {
                Icon = new Icon("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\casabrasil.ico"), // Certifique-se de que o caminho do ícone está correto
                Text = "Texto do ícone",
                Visible = true
            };




            // Adiciona um manipulador de eventos de clique duplo
            notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;

            // Inicializa o menu de contexto se necessário
            var contextMenu = new ContextMenuStrip();//C:\\Users\\User\\Desktop\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\casabrasil.ico
            var abrirMenuItem = new ToolStripMenuItem("Abrir", Image.FromFile("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\casabrasil.ico")); // Caminho para o ícone
            var sairMenuItem = new ToolStripMenuItem("Sair", Image.FromFile("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\casabrasil.ico")); // Caminho para o ícone
            contextMenu.Items.Add(abrirMenuItem);
            contextMenu.Items.Add(sairMenuItem);

            notifyIcon1.ContextMenuStrip = contextMenu;

            abrirMenuItem.Click += (sender, e) =>
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            };

            sairMenuItem.Click += (sender, e) => Application.Exit();
        }

        public Form9()
        {
        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            this.Hide(); // Oculta a janela principal
        }

        private void Button2_MouseEnter(object sender, EventArgs e)
        {
            // Inicialize e reproduza o áudio quando o mouse entra
            PlayAudio();
        }

        private void Button2_MouseLeave(object sender, EventArgs e)
        {
            // Pare a reprodução do áudio quando o mouse sai
            StopAudio();
        }

        private void PlayAudio()
        {
            // Libere recursos antigos se ainda existirem
            waveOutDevice?.Stop();
            waveOutDevice?.Dispose();
            audioFileReader?.Dispose();

            // Inicialize o player e o leitor de arquivo de áudio
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);


            // Reproduza o áudio
            waveOutDevice.Play();
        }

        private void StopAudio()
        {
            // Pare a reprodução do áudio

            waveOutDevice?.Stop();
        }




        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {





        }

        private void button5_Click(object sender, EventArgs e)
        {
            
                Form5 form5 = new Form5();
                form5.Show();




            }

        private void button8_Click(object sender, EventArgs e)
        {
            Form11 form11 = new Form11();
            form11.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();


            try
            {
                Form3 form3 = new Form3();
                form3.Show();

                // Adicione controles ao mainForm conforme necessário                
            }
            catch (Exception ex)
            {
                // Tratar exceção e exibir uma mensagem para o usuário
                MessageBox.Show($"Ocorreu um erro ao abrir o formulário principal: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Form10 form10 = new Form10();
                form10.Show();

                // Adicione controles ao mainForm conforme necessário                
            }
            catch (Exception ex)
            {
                // Tratar exceção e exibir uma mensagem para o usuário
                MessageBox.Show($"Ocorreu um erro ao abrir o formulário principal: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
