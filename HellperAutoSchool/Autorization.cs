using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    public partial class Autorization : Form
    {
        DataBaseConnect dataConnect = new DataBaseConnect();
        

        public Autorization()
        {
            InitializeComponent();
        }

        private void BTNInput_Click(object sender, EventArgs e)
        {
            using (dataConnect.connection = new MySqlConnection(dataConnect.conString))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE login=@login and pass=@pass", dataConnect.connection);

                dataConnect.connection.Open();

                cmd.Parameters.AddWithValue("@login", TBLogin.Text);
                cmd.Parameters.AddWithValue("@pass", TBPass.Text);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows == true)
                {
                    if (TBLogin.Text == "admin")
                    {
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        Hide();
                    }
                    else
                    {
                        UserForm userForm = new UserForm();
                        userForm.Show();
                        Hide();
                    }                    
                    
                    MessageBox.Show("Авторизация прошла успешно!");
                }
                else
                {
                    MessageBox.Show("Логин или пароль не вереный!\nПроверить корректность введенных параметров!");
                }

                dataConnect.connection.Close();
            }
        }

        private void BTNReg_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            Hide();
        }
    }
}
