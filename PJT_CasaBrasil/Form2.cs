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


namespace PJT_CasaBrasil
{
    public partial class Form2 : Form
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
     //   private PrintDocument printDocument;

        private string printerName = "XP-58"; // Nome da impressora

        public Form2()
        {
            InitializeComponent();
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            button1.MouseEnter += Button2_MouseEnter;
            button1.MouseLeave += Button2_MouseLeave;
            button2.MouseEnter += Button2_MouseEnter;
            button2.MouseLeave += Button2_MouseLeave;
            button3.MouseEnter += Button2_MouseEnter;
            button3.MouseLeave += Button2_MouseLeave;
            button4.MouseEnter += Button2_MouseEnter;
            button4.MouseLeave += Button2_MouseLeave;
            button5.MouseEnter += Button2_MouseEnter;
            button5.MouseLeave += Button2_MouseLeave;
            button6.MouseEnter += Button2_MouseEnter;
            button6.MouseLeave += Button2_MouseLeave;         
            button7.MouseEnter += Button2_MouseEnter;
            button7.MouseLeave += Button2_MouseLeave;
            button8.MouseEnter += Button2_MouseEnter;
            button8.MouseLeave += Button2_MouseLeave;


          //  printDocument = new PrintDocument();
         //   printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            // Nome da impressora
          //  printerName = "XP-58"; // Substitua pelo nome exato da impressora configurada

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
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
           

            // Reproduza o áudio
            waveOutDevice.Play();
        }

        private void StopAudio()
        {
            // Pare a reprodução do áudio

            waveOutDevice?.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);

            waveOutDevice.Play();
            Form4 form5 = new Form4();
            form5.Show();
        
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            waveOutDevice.Dispose();
            audioFileReader.Dispose();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();


            try
            {
                Form7 form7 = new Form7();
                form7.Show();

                // Adicione controles ao mainForm conforme necessário                
            }
            catch (Exception ex)
            {
                // Tratar exceção e exibir uma mensagem para o usuário
                MessageBox.Show($"Ocorreu um erro ao abrir o formulário principal: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void button8_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("C:\\Users\\esqui\\OneDrive\\Área de Trabalho\\PJT_CasaBrasil\\PJT_CasaBrasil\\Resources\\hover.mp3"); // Substitua com o caminho relativo ao arquivo
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

        private void button9_Click(object sender, EventArgs e)
        {


            try
            {
                PrintDirectly("Olá, Mundo!\n");
                MessageBox.Show("Documento enviado para impressão e gaveta aberta.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void PrintDirectly(string textToPrint)
        {
            using (PrintDocument printDocument = new PrintDocument())
            {
                printDocument.PrinterSettings.PrinterName = printerName;
                printDocument.PrintPage += (sender, e) =>
                {
                    Font printFont = new Font("Arial", 12);
                    e.Graphics.DrawString(textToPrint, printFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
                };

                printDocument.Print(); // Imprime diretamente sem abrir o gerenciador de impressão
            }
        }
      

        private void SendCommandToPrinter(byte[] command)
        {
            var printerHelper = new RawPrinterHelper();

            bool success = printerHelper.SendBytesToPrinter(printerName, command);

            if (!success)
            {
                // Se a função retornar false, lança uma exceção com uma mensagem detalhada.
                throw new InvalidOperationException("Falha ao enviar comandos para a impressora.");
            }
        }


    }

    public class RawPrinterHelper
    {
        [System.Runtime.InteropServices.DllImport("winspool.drv", EntryPoint = "OpenPrinter", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern bool OpenPrinter(string printerName, out IntPtr hPrinter, IntPtr pDefault);

        [System.Runtime.InteropServices.DllImport("winspool.drv", EntryPoint = "WritePrinter", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern bool WritePrinter(IntPtr hPrinter, byte[] pBytes, int dwCount, out int dwWritten);

        [System.Runtime.InteropServices.DllImport("winspool.drv", EntryPoint = "ClosePrinter", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        public bool SendBytesToPrinter(string printerName, byte[] bytes)
        {
            IntPtr hPrinter = new IntPtr(0);
            bool success = false;

            if (OpenPrinter(printerName, out hPrinter, IntPtr.Zero))
            {
                int dwWritten = 0;
                success = WritePrinter(hPrinter, bytes, bytes.Length, out dwWritten);
                ClosePrinter(hPrinter);
            }

            return success;
        }



    }


    
}

