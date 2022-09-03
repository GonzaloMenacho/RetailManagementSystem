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
    public partial class CategoryForm : Form
    {
        public CategoryForm()
        {
            InitializeComponent();
        }
        
        //defines sql connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gonza\OneDrive\Documents\ccrmdb.mdf;Integrated Security=True;Connect Timeout=30");

        //Categories' add button
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "INSERT INTO CategoryTable VALUES(" + CatIDTb.Text + ",'" + CatNameTb.Text + "','" + CatDescTb.Text + "')";
                SqlCommand command = new SqlCommand(query,Con);
                command.ExecuteNonQuery();
                MessageBox.Show("Category Info Added Succesfully ✔");
                Con.Close();
                populate();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        //Populates CategoryDVG with a SQL select query through a sqlDataAdapter which fills a new dataSet
        private void populate()
        {
            Con.Open();
            string query = "SELECT CatID AS ID, CatName AS 'Category Name', " +
                "CatDesc As 'Category Description' FROM CategoryTable";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query,Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            CategoryDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        //Populates Catgegories' text boxes with DGV selection
        private void CategoryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CatIDTb.Text = CategoryDGV.SelectedRows[0].Cells[0].Value.ToString();
            CatNameTb.Text = CategoryDGV.SelectedRows[0].Cells[1].Value.ToString();
            CatDescTb.Text = CategoryDGV.SelectedRows[0].Cells[2].Value.ToString();
        }

        //Populate DGV on load
        private void CategoryForm_Load(object sender, EventArgs e)
        {
            populate();
        }

        //Delete Category
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (CatIDTb.Text == "")
                {
                    MessageBox.Show("Select a Category to Delete");
                }
                else
                {
                    Con.Open();
                    string query = "DELETE FROM CategoryTable WHERE CatID = " + CatIDTb.Text + "";
                    SqlCommand command = new SqlCommand(query, Con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Category Deleted Succesfully ✔");
                    Con.Close();
                    populate();
                    CatIDTb.Text = "";
                    CatNameTb.Text = "";
                    CatDescTb.Text = "";
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Edit Category
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (CatIDTb.Text == "" || CatNameTb.Text == "" || CatDescTb.Text == "")
                {
                    MessageBox.Show("Missing Info");
                }
                else
                {
                    Con.Open();
                    string query = "UPDATE CategoryTable SET CatName = '" + CatNameTb.Text + "', " +
                        "CatDesc = '" + CatDescTb.Text + "' WHERE CatID = " + CatIDTb.Text + ";";
                    SqlCommand command = new SqlCommand(query, Con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Category Updated Succesfully ✔");
                    Con.Close();
                    populate();
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Load components form
        private void button2_Click(object sender, EventArgs e)
        {
            ProductForm prod = new ProductForm();
            prod.Show();
            this.Hide();
        }

        //Load sellers form
        private void button1_Click(object sender, EventArgs e)
        {
            SellerForm sell = new SellerForm();
            sell.Show();
            this.Hide();
        }

        //Load login form
        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form login = new Form1();
            login.Show();
        }

        //Escape button
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
