using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queues.Rabbit.Shared
{
    public class DAClient
    {
        internal string mySqlConnectionString = "server=localhost;port=3306;userid=root;password={pwd};database={dbName};";

        public bool ConnectionToDatabase(Client cliente)
        {
            using (var connection = new MySqlConnection(mySqlConnectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = $"INSERT INTO Clientes (NOMBRE,APELLIDO,UNIQUEID) VALUES('{cliente.Name}','{cliente.LastName}','{cliente.uniqueId}');";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public Client ObtenerRegistroCreado(string uniqueID)
        {
            var client = new Client();

            using (var connection = new MySqlConnection(mySqlConnectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM Clientes WHERE UNIQUEID='{uniqueID}';";
                cmd.CommandType = System.Data.CommandType.Text;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    client.uniqueId = reader["UNIQUEID"] != null ? reader["UNIQUEID"].ToString() : "";
                    client.Name = reader["NOMBRE"] != null ? reader["NOMBRE"].ToString() : "";
                    client.LastName = reader["APELLIDO"] != null ? reader["APELLIDO"].ToString() : "";
                }
            }

            return client;
        }
    }
}
