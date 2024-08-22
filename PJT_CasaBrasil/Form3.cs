using System;
using System.Drawing;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{

    public partial class Form3 : Form
    {
        private string placeholderText1 = "Digite seu usuário..."; // Texto do placeholder
        private string placeholderText2 = "Digite sua senha..."; // Texto do placeholder

        public Form3()
        {
            InitializeComponent();         

            // Configure o TextBox
            ConfigurePlaceholder(textBox1, placeholderText1);
            ConfigurePlaceholder(textBox2, placeholderText2);
            this.ActiveControl = null; // Remove o foco de qualquer controle
        }

    


        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
                form2.Show();

                // Adicione controles ao mainForm conforme necessário                
            }
            catch (Exception ex)
            {
                // Tratar exceção e exibir uma mensagem para o usuário
                MessageBox.Show($"Ocorreu um erro ao abrir o formulário principal: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}


