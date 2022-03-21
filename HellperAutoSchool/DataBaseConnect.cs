using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace HellperAutoSchool
{
    class DataBaseConnect
    {
        public string conString = "server=dev.gameoxford.ru;user=krotov;database=krotov;password=2V5xK@t!a;charset=utf8";

        public MySqlConnection connection;

        public MySqlDataAdapter Adapter(string query)
        {
            return new MySqlDataAdapter(query, connection);
        }
    }
}
