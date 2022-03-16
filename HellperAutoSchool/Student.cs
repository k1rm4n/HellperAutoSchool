using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    class Student : DataBaseConnect
    {
        DataGridView tableStudents = new DataGridView()
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
            Width = 122, Height = 55,
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
            InsertDataStudents();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteDataStudents();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditDataStudents();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LoadDataStudents();
        }

        /*СОБЫТИЯ*/

        public void AddTableStudents(Form form)
        {
            form.Controls.Add(tableStudents);
            LoadDataStudents();
            AddButtonOnGroupBox();
            form.Controls.Add(groupBox);
        }

        public void RemoveTableStudents(Form form)
        {
            form.Controls.Remove(tableStudents);
            form.Controls.Remove(btnAdd);
            form.Controls.Remove(btnDelete);
            form.Controls.Remove(btnEdit);
            form.Controls.Remove(btnUpdate);
            form.Controls.Remove(groupBox);

            btnAdd.Click -= BtnAdd_Click;
            btnDelete.Click -= BtnDelete_Click;
            btnEdit.Click -= BtnEdit_Click;
            btnUpdate.Click -= BtnUpdate_Click;
        }

        void LoadDataStudents()
        {
            using (connection = new MySqlConnection(conString))
            {
                tableStudents.CellClick -= TableStudents_CellClick;
                tableStudents.CellValueChanged -= TableStudents_CellValueChanged;

                string query = $"SELECT * FROM Students";

                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableStudents.DataSource = dataTable;

                string[] arrayNames = new string[] { "Id", "Фамилия", "Имя", "Категория", "Код интруктора", "Код учителя" };

                for (int i = 0; i < tableStudents.Columns.Count; i++)
                {
                    tableStudents.Columns[i].HeaderText = arrayNames[i];
                }

                connection.Close();
            } 
        }

        void InsertDataStudents()
        {
            using (connection = new MySqlConnection(conString))
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

        ArrayList nameColumns;

        void EditDataStudents()
        {
            using (connection = new MySqlConnection(conString))
            {
                LoadDataStudents();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Students", connection);

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

                tableStudents.CellValueChanged += TableStudents_CellValueChanged;
            }
        }

        private void TableStudents_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                string id = tableStudents[0, e.RowIndex].Value.ToString();
                string value = tableStudents[e.ColumnIndex, e.RowIndex].Value.ToString();
                string valueColumn = nameColumns[e.ColumnIndex].ToString();

                string query = $"UPDATE Students SET {valueColumn} = '{value}' WHERE id = {id}";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();

                LoadDataStudents();
            }
            
        }

        void DeleteDataStudents()
        {
            tableStudents.CellClick += TableStudents_CellClick;
        }

        private void TableStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                try
                {
                    string id = tableStudents[0, e.RowIndex].Value.ToString();

                    MessageBox.Show($"Вы удалили индекс: {id}");

                    string query = $"DELETE FROM Students WHERE id = { id }";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    LoadDataStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
