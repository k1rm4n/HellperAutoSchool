using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace HellperAutoSchool
{
    class DataBaseConnect
    {
        protected static string conString = "server=87.249.53.69;user=krotov;database=krotov;password=D8YHK6ga!;charset=utf8";

        protected MySqlConnection connection;

        protected MySqlDataAdapter Adapter(string query)
        {
            return new MySqlDataAdapter(query, connection);
        }
    }
}
