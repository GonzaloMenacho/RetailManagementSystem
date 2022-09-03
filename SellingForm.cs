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
    public partial class SellingForm : Form
    {
        public SellingForm()
        {
            InitializeComponent();
        }

        int grdTotal = 0, n = 0;

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gonza\OneDrive\Documents\ccrmdb.mdf;Integrated Security=True;Connect Timeout=30");

        //Populates CompDVG1 with a SQL select query through a sqlDataAdapter which fills a new dataSet
        private void populate()
        {
            Con.Open();
            string query = "SELECT CompName AS 'Component Name', CompPrice AS 'Price' FROM ComponentsTable";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            CompDGV1.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        //Populates BillsDGV with a SQL select query through a sqlDataAdapter which fills a new dataSet
        private void populateBills()
        {
            Con.Open();
            string query = "SELECT BillID AS 'ID', SellerName AS 'Seller Name', BillDate AS 'Bill Date', TotalAmt AS 'Total' FROM BillTable";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            BillsDGV.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        //On form load
        private void SellingForm_Load(object sender, EventArgs e)
        {
            populate();
            populateBills();
            fillCombo();
            SellerNamelbl.Text = Form1.sellerName;
        }

        //Populates Selling text boxes with DGV selection
        private void ProdDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CompName.Text = CompDGV1.SelectedRows[0].Cells[0].Value.ToString();
            CompPrice.Text = CompDGV1.SelectedRows[0].Cells[1].Value.ToString();

        }

        //Add date to panel
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Datelbl.Text = DateTime.Now.ToString();
        }

        //Add button (SALES REPORTS)
        private void button4_Click(object sender, EventArgs e)
        {

            if (BillID.Text == "")
            {
                MessageBox.Show("Missing Bill ID");
            }

            else
            {
                try
                {
                    Con.Open();
                    string query = "INSERT INTO BillTable VALUES(" + BillID.Text + ",'" + SellerNamelbl.Text + "','" + Datelbl.Text + "'," + Amountlbl.Text + ")";
                    SqlCommand command = new SqlCommand(query, Con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Oder Added Succesfully ✔");
                    Con.Close();
                    populateBills();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        //Print
        private void button6_Click(object sender, EventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        //Contents of what is to be printed
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("CENTURYCOMPUTERCOMPONENTS", new Font("Microsoft YaHei UI", 25, FontStyle.Bold),Brushes.Navy,new Point(100));
            e.Graphics.DrawString("Bill ID: " + BillsDGV.SelectedRows[0].Cells[0].Value.ToString(), new Font("Microsoft YaHei UI", 20, FontStyle.Bold), Brushes.SlateBlue, new Point(70,70));
            e.Graphics.DrawString("Seller Name: " + BillsDGV.SelectedRows[0].Cells[1].Value.ToString(), new Font("Microsoft YaHei UI", 20, FontStyle.Bold), Brushes.SlateBlue, new Point(70, 100));
            e.Graphics.DrawString("Date: " + BillsDGV.SelectedRows[0].Cells[2].Value.ToString(), new Font("Microsoft YaHei UI", 20, FontStyle.Bold), Brushes.SlateBlue, new Point(70, 130));
            e.Graphics.DrawString("Total Amount: $" + BillsDGV.SelectedRows[0].Cells[3].Value.ToString(), new Font("Microsoft YaHei UI", 20, FontStyle.Bold), Brushes.SlateBlue, new Point(70, 160));
            e.Graphics.DrawString("Gonzalo Menacho", new Font("Microsoft YaHei UI", 20, FontStyle.Italic), Brushes.Black, new Point(100, 220));
        }

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

        //Refresh
        private void button8_Click(object sender, EventArgs e)
        {
            populate();
        }

        //Changes what items are shown based on what is selected on the categories combo box
        private void SearchCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Con.Open();
            string query = "SELECT CompName AS 'Component Name', CompPrice AS 'Price' FROM ComponentsTable WHERE CompCat = '" + SearchCb.SelectedValue.ToString() + "';";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sqlDataAdapter);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            CompDGV1.DataSource = dataSet.Tables[0];
            Con.Close();
        }

        //Return to login
        private void label8_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form login = new Form1();
            login.Show();
        }

        //Exit app
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Add button (Component)
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (CompName.Text == "" || CompQty.Text == "")
            {
                MessageBox.Show("Missing Info");
            }
            else
            {
                int total = Convert.ToInt32(CompPrice.Text) * Convert.ToInt32(CompQty.Text);
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(OrderDGV);
                row.Cells[0].Value = n + 1;
                row.Cells[1].Value = CompName.Text;
                row.Cells[2].Value = CompPrice.Text;
                row.Cells[3].Value = CompQty.Text;
                row.Cells[4].Value = Convert.ToInt32(CompPrice.Text) * Convert.ToInt32(CompQty.Text);
                OrderDGV.Rows.Add(row);
                n++;
                grdTotal += total;
                Amountlbl.Text = "" + grdTotal;
            }
        }
    }
}
