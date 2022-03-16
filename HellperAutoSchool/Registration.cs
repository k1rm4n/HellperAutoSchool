using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    public partial class Registration : Form
    {
        DataBaseConnect baseConnect = new DataBaseConnect();

        public Registration()
        {
            InitializeComponent();
        }

        private void BTNReg_Click(object sender, EventArgs e)
        {
            using (baseConnect.connection = new MySqlConnection(baseConnect.conString))
            {
                string query = $"INSERT users(login, pass) VALUES('{TBLogin.Text}', '{TBPass.Text}')";

                MySqlCommand cmd = new MySqlCommand(query, baseConnect.connection);

                baseConnect.connection.Open();

                cmd.ExecuteNonQuery();

                baseConnect.connection.Close();

                TBLogin.Text = "";
                TBPass.Text = "";

                Autorization auto = new Autorization();
                Hide();
                auto.Show();

                MessageBox.Show("Регистрация прошла успешно!");
            }
        }
    }
}
