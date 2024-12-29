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
    public partial class Form14 : Form
    {
       

        public Form14()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form14_Load);
            textBox1.Text = UserSession.Username;
       

        }

        private void Form14_Load(object sender, EventArgs e)
        {
            maskedTextBox1.Mask = "00/00/0000 00:00";

            // Atualiza o conteúdo do MaskedTextBox com a data e hora atuais
            maskedTextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            maskedTextBox1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Confirmação do fechamento do caixa
                DialogResult confirmResult = MessageBox.Show(
                    "Tem certeza de que deseja fechar o caixa?",
                    "Confirmação de Fechamento",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // Se o usuário clicar em "Não", interrompe o fluxo
                if (confirmResult == DialogResult.No)
                {
                    return;
                }

                // Dados para o banco de dados
                string nome = textBox1.Text; // Por exemplo, usando o nome do usuário
                string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Data e hora atual
                string troco = textBox1.Text; // Pega o valor do troco no TextBox2

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

                    // Comando SQL para inserir dados na tabela fechamento_caixa
                    string query = "INSERT INTO fechamento_caixa (nome, data) VALUES (@nome, @data)";

                    // Crie o comando com parâmetros para evitar SQL Injection
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@data", data);

                        // Execute o comando
                        cmd.ExecuteNonQuery();
                    }
                }

                // Mostrar mensagem de sucesso
                MessageBox.Show("Caixa fechado com sucesso!", "Fechamento", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
               

               Form5 form5 = new Form5();
               form5.Show();


            }
            catch (Exception ex)
            {
                // Tratar exceção e exibir uma mensagem para o usuário
                MessageBox.Show($"Ocorreu um erro ao fechar o caixa: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
