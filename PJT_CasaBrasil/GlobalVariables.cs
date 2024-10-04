using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJT_CasaBrasil
{
    public static class GlobalVariables
    {
        /* String de conexão padrão inicial
        private static string defaultConnectionString = "Server=localhost;Database=seu_banco;User ID=seu_usuario;Password=sua_senha;";

        // Propriedades
        public static string ConnectionString { get; set; } = defaultConnectionString;
    
        // Método para obter a string de conexão padrão
        public static string GetDefaultConnectionString()
        {
            return defaultConnectionString;
        }

        // Método para obter uma nova conexão
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);

        }*/


        public static string ConnectionString { get; set; }

        public static void LoadConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
