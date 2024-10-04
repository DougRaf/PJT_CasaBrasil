using System;
using System.Windows.Forms;


namespace PJT_CasaBrasil
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


                  // Criar e exibir o preloader
            using (Form1 preloader = new Form1())
            {
                preloader.ShowDialog(); // Exibe o preloader de forma modal
            }

            // Criar e exibir o formulário principal após o fechamento do preloader
            try
            {
           

                // Adicione controles ao mainForm conforme necessário
                Application.Run(new Form2());
            }
            catch (Exception ex)
            {
                // Tratar exceção e exibir uma mensagem para o usuário
                MessageBox.Show($"Ocorreu um erro ao abrir o formulário principal: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}





