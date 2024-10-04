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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

            // Carregar a string de conexão atual no TextBox
       //     textBox1.Text = GlobalVariables.ConnectionString;
            // Carregar a string de conexão padrão no TextBox
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
/*
            string connectionString = textBox1.Text;

            // Se o campo estiver vazio, manter a string padrão
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show("A string de conexão não pode estar vazia. Usando a string padrão.");
                connectionString = GlobalVariables.ConnectionString; // Retorna à string padrão
            }

            // Atualiza a string de conexão global
            GlobalVariables.ConnectionString = connectionString;
            MessageBox.Show("String de conexão salva com sucesso!");
            this.Close(); // Fecha o formulário de configuração
*/
        }
    }
}
