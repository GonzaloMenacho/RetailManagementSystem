using MetroSet_UI.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ComputerParts
{
    public partial class Form1 : MetroSetForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string sellerName = "";

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gonza\OneDrive\Documents\ccrmdb.mdf;Integrated Security=True;Connect Timeout=30");

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Login button
        private void button2_Click(object sender, EventArgs e)
        {
            if(UsernameTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Missing Username and Password");
            }
            else
            {
                if (RoleCb.SelectedIndex > -1)
                {
                    //Admin Login
                    if (RoleCb.SelectedItem.ToString() == "ADMINISTRATOR")
                    {
                        if (UsernameTb.Text == "Admin" && PasswordTb.Text == "AdminPass123")
                        {
                            ProductForm prod = new ProductForm();
                            prod.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Admin Username and/or Password");
                        }
                    }
                    //Seller Login
                    else if (RoleCb.SelectedItem.ToString() == "SELLER")
                    {
                        //Uses SQL count() function to find the unique seller username and password, if not found show incorrect messagebox
                        Con.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT count(*) FROM SellerTable WHERE SellerName = '" + UsernameTb.Text + "' and SellerPass = '" + PasswordTb.Text + "'", Con);
                        DataTable dt = new DataTable();
                        sqlDataAdapter.Fill(dt);
                        if (dt.Rows[0][0].ToString() == "1")
                        {
                            sellerName = UsernameTb.Text;
                            SellingForm sell = new SellingForm();
                            sell.Show();
                            this.Hide();
                            Con.Close();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Username/Password");
                        }
                        Con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a Role");
                }
            }
        }

        //Clear button
        private void label4_Click(object sender, EventArgs e)
        {
            UsernameTb.Text = "";
            PasswordTb.Text = "";
        }

        //Invetory form
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inventory inventory = new Inventory();
            inventory.Show();
        }

        //Escape button
        private void button5_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}