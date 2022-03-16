using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    class Teacher : DataBaseConnect
    {
        DataGridView tableTeacher = new DataGridView()
        {
            Location = new Point(15, 50),
            Width = 750,
            Height = 400,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
            Font = new Font("Arial", 12)
        };

        GroupBox groupBox = new GroupBox()
        {
            Location = new Point(15, 0),
            Width = 122,
            Height = 55,
            Text = "Инструменты"
        };

        Button btnAdd = new Button()
        {
            Location = new Point(10, 20),
            Width = 25,
            Height = 25,
            BackgroundImage = Resource.add,
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        Button btnDelete = new Button()
        {
            Location = new Point(38, 20),
            Width = 25,
            Height = 25,
            BackgroundImage = Resource.delete,
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        Button btnEdit = new Button()
        {
            Location = new Point(65, 20),
            Width = 25,
            Height = 25,
            BackgroundImage = Resource.edit,
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        Button btnUpdate = new Button()
        {
            Location = new Point(90, 20),
            Width = 25,
            Height = 25,
            BackgroundImage = Resource.update,
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat
        };

        void AddButtonOnGroupBox()
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
            btnDelete.Click += BtnDelete_Click;
            btnEdit.Click += BtnEdit_Click;
            btnUpdate.Click += BtnUpdate_Click;
        }


        /*СОБЫТИЯ*/

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            InsertDataTeachers();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteDataTeachers();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditDataTeachers();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LoadDataTeachers();
        }

        /*СОБЫТИЯ*/

        public void AddTableTeachers(Form form)
        {
            form.Controls.Add(tableTeacher);
            LoadDataTeachers();
            AddButtonOnGroupBox();
            form.Controls.Add(groupBox);
        }

        public void RemoveTableTeachers(Form form)
        {
            form.Controls.Remove(tableTeacher);
            form.Controls.Remove(btnAdd);
            form.Controls.Remove(btnDelete);
            form.Controls.Remove(btnEdit);
            form.Controls.Remove(btnUpdate);

            btnAdd.Click -= BtnAdd_Click;
            btnDelete.Click -= BtnDelete_Click;
            btnEdit.Click -= BtnEdit_Click;
            btnUpdate.Click -= BtnUpdate_Click;
        }

        void LoadDataTeachers()
        {
            using (connection = new MySqlConnection(conString))
            {
                tableTeacher.CellClick -= TableTeachers_CellClick;
                tableTeacher.CellValueChanged -= TableTeachers_CellValueChanged;

                string query = $"SELECT * FROM Teachers";

                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableTeacher.DataSource = dataTable;

                string[] arrayNames = new string[] { "Id", "Фамилия", "Имя", "Код группы" };

                for (int i = 0; i < tableTeacher.Columns.Count; i++)
                {
                    tableTeacher.Columns[i].HeaderText = arrayNames[i];
                }

                connection.Close();
            }
        }

        void InsertDataTeachers()
        {
            using (connection = new MySqlConnection(conString))
            {
                int lastIndexRow = tableTeacher.Rows.Count - 2;

                string query = "INSERT Teachers(lastname, firstname, id_group) VALUES(@lastname, @firstname, @id_group)";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@lastname", tableTeacher.Rows[lastIndexRow].Cells["lastname"].Value);
                cmd.Parameters.AddWithValue("@firstname", tableTeacher.Rows[lastIndexRow].Cells["firstname"].Value);
                cmd.Parameters.AddWithValue("@id_group", tableTeacher.Rows[lastIndexRow].Cells["id_group"].Value);

                try
                {
                    connection.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        LoadDataTeachers();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    connection.Close();
                }
            }
        }

        ArrayList nameColumns;

        void EditDataTeachers()
        {
            using (connection = new MySqlConnection(conString))
            {
                LoadDataTeachers();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Teachers", connection);

                connection.Open();

                MySqlDataReader reader = cmd.ExecuteReader();


                nameColumns = new ArrayList();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        nameColumns.Add(reader.GetName(i));
                    }
                }

                connection.Close();

                tableTeacher.CellValueChanged += TableTeachers_CellValueChanged;
            }
        }

        private void TableTeachers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                string id = tableTeacher[0, e.RowIndex].Value.ToString();
                string value = tableTeacher[e.ColumnIndex, e.RowIndex].Value.ToString();
                string valueColumn = nameColumns[e.ColumnIndex].ToString();

                string query = $"UPDATE Teachers SET {valueColumn} = '{value}' WHERE id = {id}";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();

                LoadDataTeachers();
            }
        }

        void DeleteDataTeachers()
        {
            tableTeacher.CellClick += TableTeachers_CellClick;
        }

        private void TableTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                try
                {
                    string id = tableTeacher[0, e.RowIndex].Value.ToString();

                    MessageBox.Show($"Вы удалили индекс: {id}");

                    string query = $"DELETE FROM Teachers WHERE id = { id }";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    LoadDataTeachers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

