using System;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, System.EventArgs e)
        {

        }

        private void label6_Click(object sender, System.EventArgs e)
        {

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
