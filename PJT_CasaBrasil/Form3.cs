using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PJT_CasaBrasil.Form2;

namespace PJT_CasaBrasil
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form3_Load);
            textBox1.Text = UserSession.Username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Dados para o banco de dados
                string nome = textBox1.Text; // Por exemplo, usando o nome do usuário
                string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Data e hora atual
                string troco = textBox2.Text; // Pega o valor do troco no TextBox2

                // Verifica se o campo troco está vazio
                if (string.IsNullOrEmpty(troco))
                {
                    // Exibe uma mensagem de erro e interrompe o fluxo
                    MessageBox.Show("Por favor, insira o valor do troco.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Interrompe o fluxo, não continua com o código abaixo
                }

                // Defina a string de conexão com seu banco de dados MySQL
                string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;";

                // Crie a conexão com o banco de dados
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Comando SQL para inserir dados na tabela abre_caixa
                    string query = "INSERT INTO abre_caixa (nome, data, troco) VALUES (@nome, @data, @troco)";

                    // Crie o comando com parâmetros para evitar SQL Injection
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@data", data);
                        cmd.Parameters.AddWithValue("@troco", troco);

                        // Execute o comando
                        cmd.ExecuteNonQuery();
                    }
                }

                // Mostrar mensagem de sucesso
                MessageBox.Show("Dados inseridos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abre o Form7 e esconde o Form3
                Form7 form7 = new Form7();
                form7.Show();
                this.Hide();

                // Verifica se há pelo menos 2 monitores conectados
                if (Screen.AllScreens.Length > 1)
                {
                    // Pega o segundo monitor (índice 1)
                    Screen secondScreen = Screen.AllScreens[1];

                    // Cria uma nova instância de Form12
                    Form12 form12 = new Form12();

                    // Define a posição do Form12 no monitor secundário
                    form12.StartPosition = FormStartPosition.Manual;

                    // Define a localização no segundo monitor
                    form12.Location = secondScreen.Bounds.Location;

                    // Define o tamanho do Form12 para preencher o monitor secundário
                    form12.Size = secondScreen.Bounds.Size;

                    // Exibe o Form12 em modo maximizado
                    form12.WindowState = FormWindowState.Maximized;

                    // Exibe o formulário
                    form12.Show();
                }
                else
                {
                    MessageBox.Show("Não há múltiplos monitores conectados.");
                }
            }
            catch (Exception ex)
            {
                // Tratar exceção e exibir uma mensagem para o usuário
                MessageBox.Show($"Ocorreu um erro ao abrir o formulário principal: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Form3_Load(object sender, EventArgs e)
        {
            maskedTextBox1.Mask = "00/00/0000 00:00";

            // Atualiza o conteúdo do MaskedTextBox com a data e hora atuais
            maskedTextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            maskedTextBox1.ReadOnly = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
