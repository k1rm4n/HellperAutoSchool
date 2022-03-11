using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace HellperAutoSchool
{
    class DateBaseConnect
    {
        static string conString = "server=87.249.53.69;user=krotov;database=krotov;password=D8YHK6ga!;charset=utf8";

        public MySqlConnection connection = new MySqlConnection(conString);

        public MySqlDataAdapter Adapter(string query)
        {
            return new MySqlDataAdapter(query, connection);
        }

        public MySqlCommand Command(string query)
        {
            return new MySqlCommand(query, connection);
        }
    }
}
