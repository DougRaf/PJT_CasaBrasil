using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{

    public partial class Form15 : Form
    {
        private string connectionString = "Server=localhost; Database=casabrasil; Uid=root; Pwd=root;";
        public Form15()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox2.Text;
            string nomeUsuario = textBox1.Text;


            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Por favor, insira o e-mail.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost; Database=casabrasil; Uid=root; Pwd=root;"))
                {
                    conn.Open();
                    string query = "SELECT senha FROM acessos WHERE usuario = @usuario";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", nomeUsuario);
                     

                        var senha = cmd.ExecuteScalar();

                        if (senha != null)
                        {
                            EnviarEmail(email, senha.ToString());
                            MessageBox.Show("Senha enviada para o e-mail!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Usuário ou e-mail não encontrado. Verifique e tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void EnviarEmail(string emailDestino, string senha)
        {
            try
            {
                // Configuração do cliente SMTP
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); // Usando TLS
                smtp.Credentials = new NetworkCredential("lauricasabrasil@gmail.com", "244565"); // Use a senha de aplicativo gerada
                smtp.EnableSsl = true; // Habilita SSL para segurança

                // Criar a mensagem de e-mail
                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress("lauricasabrasil@gmail.com"); // E-mail do remetente
                mensagem.To.Add(emailDestino);
                mensagem.Subject = "Recuperação de Senha";
                mensagem.Body = $"Olá,\n\nSua senha é: {senha}\n\nPor favor, mantenha sua senha segura.";

                // Enviar o e-mail
                smtp.Send(mensagem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar o e-mail: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

