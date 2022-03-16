using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    class Instructor : DataBaseConnect
    {
        DataGridView tableInstructors = new DataGridView()
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
            InsertDataInstructors();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteDataInstructors();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditDataInstructors();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LoadDataInstructors();
        }

        /*СОБЫТИЯ*/

        public void AddTableInstructors(Form form)
        {
            form.Controls.Add(tableInstructors);
            LoadDataInstructors();
            AddButtonOnGroupBox();
            form.Controls.Add(groupBox);
        }

        public void RemoveTableInstructors(Form form)
        {
            form.Controls.Remove(tableInstructors);
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

        void LoadDataInstructors()
        {
            using (connection = new MySqlConnection(conString))
            {
                tableInstructors.CellClick -= TableInstructors_CellClick;
                tableInstructors.CellValueChanged -= TableInstructors_CellValueChanged;

                string query = $"SELECT * FROM Instructors";

                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableInstructors.DataSource = dataTable;

                string[] arrayNames = new string[] { "Id", "Фамилия", "Имя", "Код автомобиля", "Код студента" };

                for (int i = 0; i < tableInstructors.Columns.Count; i++)
                {
                    tableInstructors.Columns[i].HeaderText = arrayNames[i];
                }
                
                connection.Close();
            }
        }

        void InsertDataInstructors()
        {
            using (connection = new MySqlConnection(conString))
            {
                int lastIndexRow = tableInstructors.Rows.Count - 2;

                string query = "INSERT Instructors(lastname, firstname, id_car, id_student) VALUES(@lastname, @firstname, @id_car, @id_student)";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@lastname", tableInstructors.Rows[lastIndexRow].Cells["lastname"].Value);
                cmd.Parameters.AddWithValue("@firstname", tableInstructors.Rows[lastIndexRow].Cells["firstname"].Value);
                cmd.Parameters.AddWithValue("@id_car", tableInstructors.Rows[lastIndexRow].Cells["id_car"].Value);
                cmd.Parameters.AddWithValue("@id_student", tableInstructors.Rows[lastIndexRow].Cells["id_student"].Value);

                try
                {
                    connection.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        LoadDataInstructors();
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

        void EditDataInstructors()
        {
            using (connection = new MySqlConnection(conString))
            {
                LoadDataInstructors();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Instructors", connection);

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

                tableInstructors.CellValueChanged += TableInstructors_CellValueChanged; 
            }
        }

        private void TableInstructors_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                string id = tableInstructors[0, e.RowIndex].Value.ToString();
                string value = tableInstructors[e.ColumnIndex, e.RowIndex].Value.ToString();
                string valueColumn = nameColumns[e.ColumnIndex].ToString();

                string query = $"UPDATE Instructors SET {valueColumn} = '{value}' WHERE id = {id}";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();

                LoadDataInstructors();
            }
        }

        void DeleteDataInstructors()
        {
            tableInstructors.CellClick += TableInstructors_CellClick;
        }

        private void TableInstructors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                try
                {
                    string id = tableInstructors[0, e.RowIndex].Value.ToString();

                    MessageBox.Show($"Вы удалили индекс: {id}");

                    string query = $"DELETE FROM Instructors WHERE id = { id }";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    LoadDataInstructors();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
