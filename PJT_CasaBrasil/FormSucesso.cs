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
    public partial class FormSucesso : Form
    {
        public FormSucesso()
        {
            InitializeComponent();
            // Configurar o PictureBox com a imagem de sucesso
           
            labelMensagem.Text = "Cadastro realizado com sucesso!";
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  
    }

    
    
}
