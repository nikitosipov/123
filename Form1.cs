using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsForms
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;
        private SqlCommandBuilder sqlBuilder2 = null;

        private SqlDataAdapter sqlDataAdapter = null;
        private SqlDataAdapter sqlDataAdapter2 = null;

        private DataSet dataSet = null;


        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()  // загрузка из БД при запуске программы
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM Table1", sqlConnection); // добавляет в таблицу кнопку 
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Table1");

                dataGridView1.DataSource = dataSet.Tables["Table1"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkCell;
                }

                sqlDataAdapter2 = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM Table2", sqlConnection); // добавляет в таблицу кнопку 
                sqlBuilder2 = new SqlCommandBuilder(sqlDataAdapter2);

                sqlBuilder2.GetInsertCommand();
                sqlBuilder2.GetUpdateCommand();
                sqlBuilder2.GetDeleteCommand();

              //  dataSet = new DataSet();
                sqlDataAdapter2.Fill(dataSet, "Table2");

                dataGridView2.DataSource = dataSet.Tables["Table2"];

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView2[3, i] = linkCell;
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     
        private void ReloadData()   //обновление БД
        {
            try
            {
                dataSet.Tables["Table1"].Clear();

                sqlDataAdapter.Fill(dataSet, "Table1");

                dataGridView1.DataSource = dataSet.Tables["Table1"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkCell;
                }
  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData2()   //обновление БД
        {
            try
            {
                
                dataSet.Tables["Table2"].Clear();

                sqlDataAdapter2.Fill(dataSet, "Table2");

                dataGridView2.DataSource = dataSet.Tables["Table2"];

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView2[3, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)   // загзака БД в форму
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\10181386\WindowsFormsBOOKS\WindowsFormsBOOKS\Database1.mdf;Integrated Security=True");

            sqlConnection.Open();

            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)  // обновить БД и грид (кнопка сверху формы)
        {
            LoadData();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)   //удаление в строке
        {
            try
            {
                if (e.ColumnIndex == 7)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["Table1"].Rows[rowIndex].Delete();


                            sqlDataAdapter.Update(dataSet, "Table1");
                        }
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)   // удаление в строке
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    string task = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView2.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["Table2"].Rows[rowIndex].Delete();


                            sqlDataAdapter2.Update(dataSet, "Table2");
                        }
                    }

                    ReloadData2();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void buttonAdd_Click(object sender, EventArgs e)     //добавить в БД
        {
          dataSet.Tables["Table1"].Columns["id_t"].AutoIncrement = true;
          dataSet.Tables["Table1"].Columns["id_t"].AutoIncrementSeed = 1;
          dataSet.Tables["Table1"].Columns["id_t"].AutoIncrementStep = 1;

            DataRow row = dataSet.Tables["Table1"].NewRow();

            row.SetField("Adres", textBoxNomer.Text);
            row.SetField("Dom", numericUpDown1.Text);
            row.SetField("Etaj", numericUpDown2.Text);
            row.SetField("Komnat", numericUpDown4.Text);        
            row.SetField("Metraj", numericUpDown3.Text);
            row.SetField("Cena", textBoxCena.Text);

            dataSet.Tables["Table1"].Rows.Add(row);
            sqlDataAdapter.Update(dataSet, "Table1");
            ReloadData();

        }

        private void button2_Click(object sender, EventArgs e)  // добавить заказчика
        {
            dataSet.Tables["Table2"].Columns["Id_b"].AutoIncrement = true;
            dataSet.Tables["Table2"].Columns["Id_b"].AutoIncrementSeed = 1;
            dataSet.Tables["Table2"].Columns["Id_b"].AutoIncrementStep = 1;

            DataRow row = dataSet.Tables["Table2"].NewRow();

            row.SetField("Status", comboBox1.Text);
            row.SetField("CenaNado", textBox3.Text);

            dataSet.Tables["Table2"].Rows.Add(row);
            sqlDataAdapter2.Update(dataSet, "Table2");
            ReloadData2();
        }

        private void textBoxSearchNumber_TextChanged(object sender, EventArgs e)  // поиск по названию
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Cena LIKE '%{textBoxSearchNumber.Text}%'";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"CenaNado LIKE '%{textBox1.Text}%'";
        }



       

        private void textBoxQuantity_KeyPress(object sender, KeyPressEventArgs e) // ввод только чисел в коли-во штафов
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }


        


        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }



        
    }
}
