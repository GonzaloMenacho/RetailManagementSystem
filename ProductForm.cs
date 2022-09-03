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

namespace ComputerParts
{
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gonza\OneDrive\Documents\ccrmdb.mdf;Integrated Security=True;Connect Timeout=30");

        //Binds combobox with database
        private void fillCombo()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("SELECT CatName FROM CategoryTable", Con);
            SqlDataReader reader;
            reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CatName", typeof(String));
            dt.Load(reader);
            CatCb.ValueMember = "catName";
            CatCb.DataSource = dt;
            SearchCb.ValueMember = "catName";
            SearchCb.DataSource = dt;
            Con.Close();

        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            fillCombo();
            populate();
        }

        //Add button
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "INSERT INTO ComponentsTable VALUES(" + CompID.Text + ",'" + CompName.Text + "'," + CompQty.Text + "," + CompPrice.Text + ",'" + CatCb.SelectedValue.ToString() + "')";
                SqlCommand command = new SqlCommand(query, Con);
                command.ExecuteNonQuery();
                MessageBox.Show("Component Added Succesfully ✔");
                Con.Close();
                populate();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        //Populates CompDGV with a SQL select query through a sqlDataAdapter which fills a new dataSet
        private void populate()
        {
            Con.Open();
            string query = "SELECT CompID AS ID, CompName AS 'Component Name', CompQty AS 'Component Qty', CompPrice AS 'Component Price', CompCat AS 'Component Catagory' FROM ComponentsTable";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            CompDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        //Populates Components' text boxes with DGV selection
        private void CompDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CompID.Text = CompDGV.SelectedRows[0].Cells[0].Value.ToString();
            CompName.Text = CompDGV.SelectedRows[0].Cells[1].Value.ToString();
            CompQty.Text = CompDGV.SelectedRows[0].Cells[2].Value.ToString();
            CompPrice.Text = CompDGV.SelectedRows[0].Cells[3].Value.ToString();
            CatCb.SelectedValue = CompDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        //Edit Component
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (CompID.Text == "" || CompName.Text == "" || CompQty.Text == "" || CompPrice.Text == "" || CatCb.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Missing Info");
                }
                else
                {
                    Con.Open();
                    string query = "UPDATE ComponentsTable SET CompName = '" + CompName.Text + "', " + "CompQty = " + CompQty.Text + ", CompPrice = " + CompPrice.Text + ", CompCat = '" + CatCb.SelectedValue.ToString() + "' WHERE CompID = " + CompID.Text + ";";
                    SqlCommand command = new SqlCommand(query, Con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Component Updated Succesfully ✔");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Delete Component
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (CompID.Text == "")
                {
                    MessageBox.Show("Select a Category to Delete");
                }
                else
                {
                    Con.Open();
                    string query = "DELETE FROM ComponentsTable WHERE CompID = " + CompID.Text + "";
                    SqlCommand command = new SqlCommand(query, Con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Component Deleted Succesfully ✔");
                    Con.Close();
                    populate();
                    CompID.Text = "";
                    CompName.Text = "";
                    CompQty.Text = "";
                    CompPrice.Text = "";
                    CatCb.SelectedValue = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Refresh button (categories combobox)
        private void button8_Click(object sender, EventArgs e)
        {
            populate();
        }

        //Changes what items are shown based on what is selected on the categories combo box
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Con.Open();
            string query = "SELECT CompID AS ID, CompName AS 'Component Name', CompQty AS 'Component Qty', CompPrice AS 'Component Price', CompCat AS 'Component Catagory' FROM ComponentsTable WHERE CompCat = '" + SearchCb.SelectedValue.ToString() + "';";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            CompDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        
        //Form switches
        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form login = new Form1();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SellerForm sell = new SellerForm();
            sell.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CategoryForm cat = new CategoryForm();
            cat.Show();
            this.Hide();
        }

        //Exit app
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
