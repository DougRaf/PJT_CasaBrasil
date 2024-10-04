using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PJT_CasaBrasil
{
    public partial class Form10 : Form
    {

        private string connectionString = "Server=localhost;Database=casabrasil;Uid=root;Pwd=root;";

        public Form10()
        {
            InitializeComponent();

            textBox9.MaxLength = 8;

            // Configurar comprimento máximo para o TextBox de confirmação de senha
            textBox10.MaxLength = 8;

            // Inicializar os CheckBox
            checkBox1.CheckedChanged += CheckBox_CheckedChanged;
            checkBox2.CheckedChanged += CheckBox_CheckedChanged;

            // Inicialmente desabilitar os controles de acesso
            ToggleAcessoControls(false);

        }

        private void ToggleAcessoControls(bool enable)
        {
            textBox9.Enabled = enable;
            textBox8.Enabled = enable;
            textBox10.Enabled = enable;
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == checkBox1)
            {
                if (checkBox1.Checked)
                {
                    checkBox2.Checked = false;
                    ToggleAcessoControls(true);
                }
                else
                {
                    ToggleAcessoControls(false);
                }
            }
            else if (sender == checkBox2)
            {
                if (checkBox2.Checked)
                {
                    checkBox1.Checked = false;
                    ToggleAcessoControls(false);
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Obtendo dados dos controles
            string nomeCompleto = textBox1.Text;
            string endereco = textBox2.Text;
            string dataNascimento = textBox3.Text;
            string estadoCivil = textBox4.Text; // Atualizado para TextBox
            string email = textBox5.Text;
            string telefone = textBox6.Text;
            string documento = textBox7.Text;
            string usuario = textBox8.Text;
            string senha = textBox9.Text;
            string senhaConfirmada = textBox10.Text; // Senha confirmada

            // Verificar se todos os campos obrigatórios estão preenchidos
            if (string.IsNullOrWhiteSpace(nomeCompleto) ||
                string.IsNullOrWhiteSpace(endereco) ||
                string.IsNullOrWhiteSpace(estadoCivil) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(telefone) ||
                string.IsNullOrWhiteSpace(documento) ||
                (checkBox1.Checked && (string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(senhaConfirmada))))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                return;
            }

            // Verificar se a senha confirmada é igual à senha
            if (checkBox1.Checked && senha != senhaConfirmada)
            {
                MessageBox.Show("A senha e a confirmação de senha não coincidem.");
                return;
            }

            // Verificar o comprimento da senha
            if (checkBox1.Checked && (senha.Length < 8 || senha.Length > 8))
            {
                MessageBox.Show("A senha deve ter exatamente 8 caracteres.");
                return;
            }

            // Gerar o hash MD5 da senha
            string senhaHash = GenerateMD5Hash(senha);


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Verificar se o nome já existe
                    string queryCheckNome = "SELECT COUNT(*) FROM funcionarios WHERE nome_completo = @nomeCompleto";
                    MySqlCommand cmdCheckNome = new MySqlCommand(queryCheckNome, conn);
                    cmdCheckNome.Parameters.AddWithValue("@nomeCompleto", nomeCompleto);
                    int count = Convert.ToInt32(cmdCheckNome.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Já existe um funcionário com este nome.");
                        return;
                    }


                    if (checkBox2.Checked)
                    {
                        // Inserir dados na tabela 'funcionarios'
                        string queryFuncionario = "INSERT INTO funcionarios (nome_completo, endereco, data_nascimento, estado_civil, email, telefone, documento, acesso_id) VALUES (@nomeCompleto, @endereco, @dataNascimento, @estadoCivil, @email, @telefone, @documento, NULL)";
                        MySqlCommand cmdFuncionario = new MySqlCommand(queryFuncionario, conn);
                        cmdFuncionario.Parameters.AddWithValue("@nomeCompleto", nomeCompleto);
                        cmdFuncionario.Parameters.AddWithValue("@endereco", endereco);
                        cmdFuncionario.Parameters.AddWithValue("@dataNascimento", dataNascimento);
                        cmdFuncionario.Parameters.AddWithValue("@estadoCivil", estadoCivil); // Atualizado para TextBox
                        cmdFuncionario.Parameters.AddWithValue("@email", email);
                        cmdFuncionario.Parameters.AddWithValue("@telefone", telefone);
                        cmdFuncionario.Parameters.AddWithValue("@documento", documento);
                        cmdFuncionario.Parameters.AddWithValue("@acessoId", DBNull.Value); // Garantindo que acesso_id seja NULL
                        cmdFuncionario.ExecuteNonQuery();
                    }
                    else if (checkBox1.Checked)
                    {

                        // Verificar se o usuário já existe na tabela 'acessos'
                        string queryCheckUsuario = "SELECT COUNT(*) FROM acessos WHERE usuario = @usuario";
                        MySqlCommand cmdCheckUsuario = new MySqlCommand(queryCheckUsuario, conn);
                        cmdCheckUsuario.Parameters.AddWithValue("@usuario", usuario);
                        int countUsuario = Convert.ToInt32(cmdCheckUsuario.ExecuteScalar());

                        if (countUsuario > 0)
                        {
                            MessageBox.Show("Já existe um usuário com este nome de acesso.");
                            return;
                        }


                        // Inserir dados na tabela 'acessos'
                        string queryAcesso = "INSERT INTO acessos (usuario, senha) VALUES (@usuario, @senha)";
                        MySqlCommand cmdAcesso = new MySqlCommand(queryAcesso, conn);
                        cmdAcesso.Parameters.AddWithValue("@usuario", usuario);
                        cmdAcesso.Parameters.AddWithValue("@senha", senhaHash);
                        cmdAcesso.ExecuteNonQuery();

                        long acessoId = cmdAcesso.LastInsertedId;

                        // Inserir dados na tabela 'funcionarios'
                        string queryFuncionario = "INSERT INTO funcionarios (nome_completo, endereco, data_nascimento, estado_civil, email, telefone, documento, acesso_id) VALUES (@nomeCompleto, @endereco, @dataNascimento, @estadoCivil, @email, @telefone, @documento, @acessoId)";
                        MySqlCommand cmdFuncionario = new MySqlCommand(queryFuncionario, conn);
                        cmdFuncionario.Parameters.AddWithValue("@nomeCompleto", nomeCompleto);
                        cmdFuncionario.Parameters.AddWithValue("@endereco", endereco);
                        cmdFuncionario.Parameters.AddWithValue("@dataNascimento", dataNascimento);
                        cmdFuncionario.Parameters.AddWithValue("@estadoCivil", estadoCivil); // Atualizado para TextBox
                        cmdFuncionario.Parameters.AddWithValue("@email", email);
                        cmdFuncionario.Parameters.AddWithValue("@telefone", telefone);
                        cmdFuncionario.Parameters.AddWithValue("@documento", documento);
                        cmdFuncionario.Parameters.AddWithValue("@acessoId", acessoId);
                        cmdFuncionario.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Selecione uma das opções de acesso.");
                        return;
                    }

                    // Mostrar o formulário de sucesso
                    FormSucesso formSucesso = new FormSucesso();
                    formSucesso.ShowDialog();

                    // Limpar todos os TextBoxes
                    textBox1.Clear(); // Nome Completo
                    textBox2.Clear(); // Endereço
                    textBox3.Clear(); // Estado Civil
                    textBox4.Clear(); // E-mail
                    textBox5.Clear(); // Telefone
                    textBox6.Clear(); // Documento
                    textBox7.Clear(); // data
                    textBox8.Clear(); // Usuário
                    textBox9.Clear(); // Senha
                    textBox10.Clear(); // Senha Confirmada


                    // Desmarcar os CheckBoxes
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;

                    // Desabilitar controles de acesso
                    ToggleAcessoControls(false);
                }


                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }


            }
        }

        private string GenerateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Limpar todos os TextBoxes
            textBox1.Clear(); // Nome Completo
            textBox2.Clear(); // Endereço
            textBox3.Clear(); // Estado Civil
            textBox4.Clear(); // E-mail
            textBox5.Clear(); // Telefone
            textBox6.Clear(); // Documento
            textBox7.Clear(); // data
            textBox8.Clear(); // Usuário
            textBox9.Clear(); // Senha
            textBox10.Clear(); // Senha Confirmada

         
            // Desmarcar os CheckBoxes
            checkBox1.Checked = false;
            checkBox2.Checked = false;

            // Desabilitar controles de acesso
            ToggleAcessoControls(false);

        }
    }
}
