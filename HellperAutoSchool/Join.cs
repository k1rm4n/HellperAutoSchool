using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    class Join : DataBaseConnect
    {
        DataGridView tableJoin = new DataGridView()
        {
            Location = new Point(15, 50),
            Width = 750,
            Height = 400,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
            Font = new Font("Arial", 12)
        };

        public void AddTableJoins(Form form)
        {
            form.Controls.Add(tableJoin);
            /*ShowTableStudents();
            ShowTableClassTime();
            ShowTableInstructors();*/
        }

        public void ClearTable()
        {
            tableJoin.Controls.Clear();
        }

        public void ShowTableStudents()
        {
            using (connection = new MySqlConnection(conString))
            {
                string query = $"SELECT s.lastname, s.firstname, s.category, t.lastname, t.firstname, g.nameGroup FROM Students AS s " +
                    $"LEFT JOIN Teachers AS t ON t.id = s.id_teacher " +
                    $"INNER JOIN Groups AS g ON g.id = t.id_group";


                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableJoin.DataSource = dataTable;

                string[] arrayNames = new string[] { "Фамилия ученика", "Имя ученика", "Категория", "Фамилия учителя", "Имя учителя", "Номер группы", "Время занятий" };

                for (int i = 0; i < tableJoin.Columns.Count; i++)
                {
                    tableJoin.Columns[i].HeaderText = arrayNames[i];
                }

                connection.Close();
            }
        }

        public void ShowTableClassTime()
        {
            using (connection = new MySqlConnection(conString))
            {
                string query = $"SELECT g.nameGroup, ct.timeData FROM ClassTime AS ct " +
                    $"LEFT JOIN Groups AS g ON g.id = ct.id_group"; 


                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableJoin.DataSource = dataTable;

                string[] arrayNames = new string[] { "Название группы", "Время занятий" };

                for (int i = 0; i < tableJoin.Columns.Count; i++)
                {
                    tableJoin.Columns[i].HeaderText = arrayNames[i];
                }

                connection.Close();
            }
        }

        public void ShowTableInstructors()
        {
            using (connection = new MySqlConnection(conString))
            {
                string query = $"SELECT i.lastname, i.firstname, s.lastname, s.firstname, " +
                    $"s.category, c.mark, c.model, c.color FROM Students AS s " +
                    $"LEFT JOIN Instructors AS i ON i.id = s.id_instruct " +
                    $"LEFT JOIN Cars AS c ON c.id = i.id_car"; 


                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableJoin.DataSource = dataTable;

                string[] arrayNames = new string[] { "Фамилия инструктора", "Имя инструктора", 
                    "Фамилия ученика", "Имя ученика", "Категория",
                    "Марка машины", "Модель машины", "Цвет"
                };

                for (int i = 0; i < tableJoin.Columns.Count; i++)
                {
                    tableJoin.Columns[i].HeaderText = arrayNames[i];
                }

                connection.Close();
            }
        }
    }
}
