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

namespace PJT_CasaBrasil
{
    public partial class Form2 : Form
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;

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
    }
}
