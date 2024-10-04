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

        private void Form3_Load(object sender, EventArgs e)
        {
            maskedTextBox1.Mask = "00/00/0000 00:00";

            // Atualiza o conteúdo do MaskedTextBox com a data e hora atuais
            maskedTextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            maskedTextBox1.ReadOnly = true;

        }


    }
}
