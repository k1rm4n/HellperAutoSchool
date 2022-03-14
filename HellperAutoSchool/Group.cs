using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    class Group : DataBaseConnect
    {
        DataGridView tableGroups = new DataGridView()
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
            InsertDataGroups();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteDataGroups();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditDataGroups();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LoadDataGroups();
        }

        /*СОБЫТИЯ*/

        public void AddTableGroups(Form form)
        {
            form.Controls.Add(tableGroups);
            LoadDataGroups();
            AddButtonOnGroupBox();
            form.Controls.Add(groupBox);
        }

        public void RemoveTableGroups(Form form)
        {
            form.Controls.Remove(tableGroups);
            form.Controls.Remove(btnAdd);
            form.Controls.Remove(btnDelete);
            form.Controls.Remove(btnEdit);
            form.Controls.Remove(btnUpdate);

            btnAdd.Click -= BtnAdd_Click;
            btnDelete.Click -= BtnDelete_Click;
            btnEdit.Click -= BtnEdit_Click;
            btnUpdate.Click -= BtnUpdate_Click;
        }

        void LoadDataGroups()
        {
            using (connection = new MySqlConnection(conString))
            {
                tableGroups.CellClick -= TableGroups_CellClick;
                tableGroups.CellValueChanged -= TableGroups_CellValueChanged;

                string query = $"SELECT * FROM Groups";

                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableGroups.DataSource = dataTable;

                string[] arrayNames = new string[] { "Id", "Имя группы" };

                for (int i = 0; i < tableGroups.Columns.Count; i++)
                {
                    tableGroups.Columns[i].HeaderText = arrayNames[i];
                }

                connection.Close();
            }
        }

        void InsertDataGroups()
        {
            using (connection = new MySqlConnection(conString))
            {
                int lastIndexRow = tableGroups.Rows.Count - 2;

                string query = "INSERT Groups(nameGroup) VALUES(@nameGroup)";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@nameGroup", tableGroups.Rows[lastIndexRow].Cells["nameGroup"].Value);

                try
                {
                    connection.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        LoadDataGroups();
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

        void EditDataGroups()
        {
            using (connection = new MySqlConnection(conString))
            {
                LoadDataGroups();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Groups", connection);

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

                tableGroups.CellValueChanged += TableGroups_CellValueChanged;
            }
        }

        private void TableGroups_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                string id = tableGroups[0, e.RowIndex].Value.ToString();
                string value = tableGroups[e.ColumnIndex, e.RowIndex].Value.ToString();
                string valueColumn = nameColumns[e.ColumnIndex].ToString();

                string query = $"UPDATE Groups SET {valueColumn} = '{value}' WHERE id = {id}";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();

                LoadDataGroups();
            }
        }

        void DeleteDataGroups()
        {
            tableGroups.CellClick += TableGroups_CellClick;
        }

        private void TableGroups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                try
                {
                    string id = tableGroups[0, e.RowIndex].Value.ToString();

                    MessageBox.Show($"Вы удалили индекс: {id}");

                    string query = $"DELETE FROM Groups WHERE id = { id }";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    LoadDataGroups();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
