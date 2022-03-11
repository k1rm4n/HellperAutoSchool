using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace HellperAutoSchool
{
    class Student : DateBaseConnect
    {
        static int x = 860;

        DataGridView tableStudents = new DataGridView()
        {
            Location = new Point(30, 30),
            Width = 800,
            Height = 400,
            BackgroundColor = Color.GreenYellow,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
            Font = new Font("Arial", 12)
        };

        Button btnAdd = new Button()
        {
            Location = new Point(x, 100),
            Text = "Add"
        };

        public void AddTableStudents(Form form)
        {
            form.Controls.Add(tableStudents);
            LoadDataStudents();
            form.Controls.Add(btnAdd);

            btnAdd.Click += BtnAdd_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            InsertDataStudents();
        }

        public void RemoveTableStudents(Form form)
        {
            form.Controls.Remove(tableStudents);
        }

        public void LoadDataStudents()
        {
            string query = $"SELECT * FROM Students";

            connection.Open();

            DataTable dataTable = new DataTable();

            Adapter(query).Fill(dataTable);

            tableStudents.DataSource = dataTable;

            string[] arrayNames = new string[] { "Id", "Фамилия", "Имя", "Категория", "Индекс интруктора", "Индекс учителя"};

            for (int i = 0; i < tableStudents.Columns.Count; i++)
            {
                tableStudents.Columns[i].HeaderText = arrayNames[i];
            }

            connection.Close();
        }

        public void InsertDataStudents()
        {
            int lastIndexRow = tableStudents.Rows.Count - 2;

            string query = "INSERT Students(lastname, firstname, category, id_instruct, id_teacher) VALUES(@lastname, @firstname, @category, @id_instruct, @id_teacher)";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@lastname", tableStudents.Rows[lastIndexRow].Cells["lastname"].Value);
            cmd.Parameters.AddWithValue("@firstname", tableStudents.Rows[lastIndexRow].Cells["firstname"].Value);
            cmd.Parameters.AddWithValue("@category", tableStudents.Rows[lastIndexRow].Cells["category"].Value);
            cmd.Parameters.AddWithValue("@id_instruct", tableStudents.Rows[lastIndexRow].Cells["id_instruct"].Value);
            cmd.Parameters.AddWithValue("@id_teacher", tableStudents.Rows[lastIndexRow].Cells["id_teacher"].Value);

            try
            {
                connection.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    LoadDataStudents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }
    }
}
