using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PJT_CasaBrasil
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            
        }

        private void Form7_Load_1(object sender, EventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Criar e exibir o formulário principal após o fechamento do preloader
          
                Form2 form2 = new Form2();
                form2.ShowDialog();

                // Adicione controles ao mainForm conforme necessário

      
        }
    }
}
