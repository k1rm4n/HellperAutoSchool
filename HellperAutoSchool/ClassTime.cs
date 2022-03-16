using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace HellperAutoSchool
{
    class ClassTime : DataBaseConnect
    {
        DataGridView tableClassTime = new DataGridView()
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
            InsertDataClassTime();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteDataClassTime();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditDataClassTime();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LoadDataClassTime();
        }

        /*СОБЫТИЯ*/

        public void AddTableClassTime(Form form)
        {
            form.Controls.Add(tableClassTime);
            LoadDataClassTime();
            AddButtonOnGroupBox();
            form.Controls.Add(groupBox);
        }

        public void RemoveTableClassTime(Form form)
        {
            form.Controls.Remove(tableClassTime);
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

        void LoadDataClassTime()
        {
            using (connection = new MySqlConnection(conString))
            {
                tableClassTime.CellClick -= TableClassTime_CellClick;
                tableClassTime.CellValueChanged -= TableClassTime_CellValueChanged;

                string query = $"SELECT * FROM ClassTime";

                connection.Open();

                DataTable dataTable = new DataTable();

                Adapter(query).Fill(dataTable);

                tableClassTime.DataSource = dataTable;

                string[] arrayNames = new string[] { "Id", "Дата и время занятий", "Код группы" };

                for (int i = 0; i < tableClassTime.Columns.Count; i++)
                {
                    tableClassTime.Columns[i].HeaderText = arrayNames[i];
                }

                tableClassTime.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

                connection.Close();
            }
        }

        void InsertDataClassTime()
        {
            using (connection = new MySqlConnection(conString))
            {
                int lastIndexRow = tableClassTime.Rows.Count - 2;

                string query = "INSERT ClassTime(timeData, id_group) VALUES(@timeData, @id_group)";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@timeData", tableClassTime.Rows[lastIndexRow].Cells["timeData"].Value);
                cmd.Parameters.AddWithValue("@id_group", tableClassTime.Rows[lastIndexRow].Cells["id_group"].Value);

                try
                {
                    connection.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        LoadDataClassTime();
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

        void EditDataClassTime()
        {
            using (connection = new MySqlConnection(conString))
            {
                LoadDataClassTime();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM ClassTime", connection);

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

                tableClassTime.CellValueChanged += TableClassTime_CellValueChanged;
            }
        }

        private void TableClassTime_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                string id = tableClassTime[0, e.RowIndex].Value.ToString();
                string value = tableClassTime[e.ColumnIndex, e.RowIndex].Value.ToString();
                string valueColumn = nameColumns[e.ColumnIndex].ToString();

                string query;

                if (valueColumn == "timeData")
                {
                    DateTime DTvalue = Convert.ToDateTime(value);

                    string formatDT = DTvalue.ToString("yyyy-MM-dd HH:mm:ss");
                    query = $"UPDATE ClassTime SET {valueColumn} = '{formatDT}' WHERE id = {id}";
                }
                else
                {
                    query = $"UPDATE ClassTime SET {valueColumn} = '{value}' WHERE id = {id}";
                }


                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();

                LoadDataClassTime();
            }
        }

        void DeleteDataClassTime()
        {
            tableClassTime.CellClick += TableClassTime_CellClick;
        }

        private void TableClassTime_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (connection = new MySqlConnection(conString))
            {
                try
                {
                    string id = tableClassTime[0, e.RowIndex].Value.ToString();

                    MessageBox.Show($"Вы удалили индекс: {id}");

                    string query = $"DELETE FROM ClassTime WHERE id = { id }";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    LoadDataClassTime();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
