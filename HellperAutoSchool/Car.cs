using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    class Car : DataBaseConnect
    {
        DataGridView tableCars = new DataGridView()
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
            InsertDataCars();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteDataCars();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditDataCars();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LoadDataCars();
        }

        /*СОБЫТИЯ*/

        public void AddTableCars(Form form)
        {
            form.Controls.Add(tableCars);
            LoadDataCars();
            AddButtonOnGroupBox();
            form.Controls.Add(groupBox);
        }

        public void RemoveTableCars(Form form)
        {
            form.Controls.Remove(tableCars);
            form.Controls.Remove(btnAdd);
            form.Controls.Remove(btnDelete);
            form.Controls.Remove(btnEdit);
            form.Controls.Remove(btnUpdate);

            btnAdd.Click -= BtnAdd_Click;
            btnDelete.Click -= BtnDelete_Click;
            btnEdit.Click -= BtnEdit_Click;
            btnUpdate.Click -= BtnUpdate_Click;
        }

        void LoadDataCars()
        {
            using (connection = new MySqlConnection(conString))
            {
                tableCars.CellClick -= TableCars_CellClick;
                tableCars.CellValueChanged -= TableCars_CellValueChanged;

                string query = $"SELECT * FROM Cars";

                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableCars.DataSource = dataTable;

                string[] arrayNames = new string[] { "Id", "Фамилия", "Имя", "Код автомобиля", "Код студента" };

                for (int i = 0; i < tableCars.Columns.Count; i++)
                {
                    tableCars.Columns[i].HeaderText = arrayNames[i];
                }

                connection.Close();
            }
        }

        void InsertDataCars()
        {
            using (connection = new MySqlConnection(conString))
            {
                int lastIndexRow = tableCars.Rows.Count - 2;

                string query = "INSERT Cars(mark, model, color) VALUES(@mark, @model, @color)";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@mark", tableCars.Rows[lastIndexRow].Cells["mark"].Value);
                cmd.Parameters.AddWithValue("@model", tableCars.Rows[lastIndexRow].Cells["model"].Value);
                cmd.Parameters.AddWithValue("@color", tableCars.Rows[lastIndexRow].Cells["color"].Value);

                try
                {
                    connection.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        LoadDataCars();
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

        void EditDataCars()
        {
            using (connection = new MySqlConnection(conString))
            {
                LoadDataCars();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Cars", connection);

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

                tableCars.CellValueChanged += TableCars_CellValueChanged;
            }
        }

        private void TableCars_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                string id = tableCars[0, e.RowIndex].Value.ToString();
                string value = tableCars[e.ColumnIndex, e.RowIndex].Value.ToString();
                string valueColumn = nameColumns[e.ColumnIndex].ToString();

                string query = $"UPDATE Cars SET {valueColumn} = '{value}' WHERE id = {id}";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();

                LoadDataCars();
            }
        }

        void DeleteDataCars()
        {
            tableCars.CellClick += TableCars_CellClick;
        }

        private void TableCars_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                try
                {
                    string id = tableCars[0, e.RowIndex].Value.ToString();

                    MessageBox.Show($"Вы удалили индекс: {id}");

                    string query = $"DELETE FROM Cars WHERE id = { id }";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    LoadDataCars();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
