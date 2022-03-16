using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HellperAutoSchool
{
    public partial class UserForm : Form
    {
        Join join = new Join();

        public UserForm()
        {
            InitializeComponent();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            join.AddTableJoins(this);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comboBoxState = comboBox1.Items[comboBox1.SelectedIndex].ToString();

            if (comboBoxState == "Группы")
            {
                join.ClearTable();
                join.ShowTableStudents();
            }
            else if (comboBoxState == "Расписание занятий")
            {
                join.ClearTable();
                join.ShowTableClassTime();
            }
            else if (comboBoxState == "Инструктора")
            {
                join.ClearTable();
                join.ShowTableInstructors();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Hide();
            Autorization auto = new Autorization();
            auto.Show();
        }
    }
}
