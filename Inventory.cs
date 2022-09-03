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
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gonza\OneDrive\Documents\ccrmdb.mdf;Integrated Security=True;Connect Timeout=30");

        private void fillCombo()
        {
            //Binds combobox with database
            Con.Open();
            SqlCommand command = new SqlCommand("SELECT CatName FROM CategoryTable", Con);
            SqlDataReader reader;
            reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CatName", typeof(String));
            dt.Load(reader);
            SearchCb.ValueMember = "catName";
            SearchCb.DataSource = dt;
            Con.Close();

        }

        private void populate()
        {
            Con.Open();
            string query = "SELECT CompName AS 'Component Name', CompQty AS 'Component Qty', CompPrice AS 'Component Price ($)', CompCat AS 'Component Catagory' FROM ComponentsTable";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            InventoryDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            populate();
            fillCombo();
        }


        private void SearchCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Con.Open();
            string query = "SELECT CompName AS 'Component Name', CompQty AS 'Component Qty', CompPrice AS 'Component Price ($)', CompCat AS 'Component Catagory' FROM ComponentsTable WHERE CompCat = '" + SearchCb.SelectedValue.ToString() + "';";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            InventoryDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Con.Open();
            string query = "SELECT CompName AS 'Component Name', CompQty AS 'Component Qty', CompPrice AS 'Component Price ($)', CompCat AS 'Component Catagory' FROM ComponentsTable WHERE CompName LIKE '%" + Searchlbl.Text + "%';";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            InventoryDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form login = new Form1();
            login.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
