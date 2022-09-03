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
    public partial class SellerForm : Form
    {
        public SellerForm()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gonza\OneDrive\Documents\ccrmdb.mdf;Integrated Security=True;Connect Timeout=30");

        //Populates Sellers' text boxes with DGV selection
        private void SellerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SellerID.Text = SellerDGV.SelectedRows[0].Cells[0].Value.ToString();
            SellerName.Text = SellerDGV.SelectedRows[0].Cells[1].Value.ToString();
            SellerPhone.Text = SellerDGV.SelectedRows[0].Cells[2].Value.ToString();
            SellerAge.Text = SellerDGV.SelectedRows[0].Cells[3].Value.ToString();
            SellerEmail.Text = SellerDGV.SelectedRows[0].Cells[4].Value.ToString();
            SellerPass.Text = SellerDGV.SelectedRows[0].Cells[5].Value.ToString();
        }

        //Add Seller
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "INSERT INTO SellerTable VALUES(" + SellerID.Text + ",'" + SellerName.Text + "','" + SellerPhone.Text + "'," + SellerAge.Text + ",'" + SellerEmail.Text + "','" + SellerPass.Text + "')";
                SqlCommand command = new SqlCommand(query, Con);
                command.ExecuteNonQuery();
                MessageBox.Show("Seller Added Succesfully ✔");
                Con.Close();
                populate();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        //Populates SellerDVG with a SQL select query through a sqlDataAdapter which fills a new dataSet
        private void populate()
        {
            Con.Open();
            string query = "SELECT SellerID AS ID, SellerName AS Name, SellerPhone AS 'Phone Number', SellerAge AS Age, SellerEmail AS Email, SellerPass AS Password FROM SellerTable";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            SellerDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        private void SellerForm_Load(object sender, EventArgs e)
        {
            populate();
        }

        //Delete Seller
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (SellerID.Text == "")
                {
                    MessageBox.Show("Select a Seller to Delete");
                }
                else
                {
                    Con.Open();
                    string query = "DELETE FROM SellerTable WHERE SellerID = " + SellerID.Text + "";
                    SqlCommand command = new SqlCommand(query, Con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Seller Deleted Succesfully ✔");
                    Con.Close();
                    populate();
                    SellerID.Text = "";
                    SellerName.Text = "";
                    SellerPhone.Text = "";
                    SellerAge.Text = "";
                    SellerEmail.Text = "";
                    SellerPass.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Edit Seller
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (SellerID.Text == "" || SellerName.Text == "" || SellerPhone.Text == "" || SellerAge.Text == "" || SellerEmail.Text == "" || SellerPass.Text == "")
                {
                    MessageBox.Show("Missing Info");
                }
                else
                {
                    Con.Open();
                    string query = "UPDATE SellerTable SET SellerName = '" + SellerName.Text + "', SellerPhone = " + SellerPhone.Text + ", SellerAge = " + SellerAge.Text + ", SellerEmail = '" + SellerEmail.Text + "', SellerPass = '" + SellerPass.Text + "' WHERE SellerID = " + SellerID.Text + ";";
                    SqlCommand command = new SqlCommand(query, Con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Seller Updated Succesfully ✔");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Load forms
        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form login = new Form1();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProductForm prod = new ProductForm();
            prod.Show();
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
