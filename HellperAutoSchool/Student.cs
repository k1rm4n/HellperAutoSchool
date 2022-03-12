using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.IO;

namespace HellperAutoSchool
{
    class Student : DateBaseConnect
    {
        DataGridView tableStudents = new DataGridView()
        {
            Location = new Point(30, 30),
            Width = 750,
            Height = 400,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
            Font = new Font("Arial", 12)
        };

        GroupBox groupBox = new GroupBox()
        {
            Location = new Point(785, 100),
            Width = 192,
            Text = "Инструменты"
        };

        Button btnAdd = new Button()
        {
            Location = new Point(10, 20),
            Width = 25, Height = 25,
            BackgroundImage = Image.FromFile("C:/Users/lal-m/source/repos/HellperAutoSchool/HellperAutoSchool/Photo/add.png"),
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        Button btnDelete = new Button()
        {
            Location = new Point(38, 20),
            Width = 25,
            Height = 25,
            BackgroundImage = Image.FromFile("C:/Users/lal-m/source/repos/HellperAutoSchool/HellperAutoSchool/Photo/delete.png"),
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        Button btnEdit = new Button()
        {
            Location = new Point(65, 20),
            Width = 25,
            Height = 25,
            BackgroundImage = Image.FromFile("C:/Users/lal-m/source/repos/HellperAutoSchool/HellperAutoSchool/Photo/edit.png"),
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        Button btnUpdate = new Button()
        {
            Location = new Point(90, 20),
            Width = 25,
            Height = 25,
            BackgroundImage = Image.FromFile("C:/Users/lal-m/source/repos/HellperAutoSchool/HellperAutoSchool/Photo/update.png"),
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        public void AddButtonOnGroupBox()
        {
            btnAdd.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatAppearance.BorderSize = 0;
            groupBox.Controls.Add(btnAdd); 
            groupBox.Controls.Add(btnDelete);
            groupBox.Controls.Add(btnEdit);
            groupBox.Controls.Add(btnUpdate);

            btnAdd.Click += BtnAdd_Click;
        }

        public void AddTableStudents(Form form)
        {
            form.Controls.Add(tableStudents);
            LoadDataStudents();
            AddButtonOnGroupBox();
            form.Controls.Add(groupBox);
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
