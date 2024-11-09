using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{

    public partial class Form2 : Form
    {
        private string placeholderText1 = "Digite seu usuário..."; // Texto do placeholder
        private string placeholderText2 = "Digite sua senha..."; // Texto do placeholder
        private object panelContainer;
        private string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;";



        public Form2()
        {
            InitializeComponent();
           

            // Configure o TextBox
            ConfigurePlaceholder(textBox1, placeholderText1);
            ConfigurePlaceholder(textBox2, placeholderText2);
            this.ActiveControl = null; // Remove o foco de qualquer controle
        }

        private void CloseLoginForm()
        {
            this.Hide(); // Oculta o formulário
            this.Dispose(); // Libera recursos
        }

        public static class UserSession
        {
            public static string Username { get; set; }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
                   
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id, senha FROM acessos WHERE usuario = @username";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", textBox1.Text);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32("id");
                            string hashedPassword = reader.GetString("senha");

                            if (ComputeMd5Hash(textBox2.Text) == hashedPassword)
                            {
                                UserSession.Username = textBox1.Text; // Salva na classe estática

                                // Login bem-sucedido
                                Form9 form9 = new Form9(userId);
                                form9.Show();
                                this.Hide();
                             
                            }
                            else
                            {
                                MessageBox.Show("Senha Incorreta! Tente outra vez!!!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Sai do método se houver campos vazios
                            }
                        }
                        else
                        {
                            MessageBox.Show("Usuário não encontrado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private string ComputeMd5Hash(string rawData)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }





   
                     
      

         
       

        private void Form3_Load(object sender, EventArgs e)
        {
            // Configure os TextBox no evento Load do formulário
            ConfigurePlaceholder(textBox1, placeholderText1);
            ConfigurePlaceholder(textBox2, placeholderText2);
            
        }



        private void ConfigurePlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
            textBox.Font = new Font(textBox.Font, FontStyle.Italic);

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    textBox.Font = new Font(textBox.Font, FontStyle.Regular);
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                    textBox.Font = new Font(textBox.Font, FontStyle.Italic);
                }
            };


        }

        private void Form3_Activated(object sender, EventArgs e)
        {
            this.ActiveControl = null; // Remove o foco de qualquer controle
        }

        private void button2_Click(object sender, EventArgs e)
        {
         

              // Login bem-sucedido
                                Form5 form5 = new Form5();
                                form5.Show();


        }
    }
}


