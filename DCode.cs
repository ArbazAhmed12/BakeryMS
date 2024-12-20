using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DB1832CH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DisplayElements("ProductTb1", ProductsDGV);
            GetCategorie();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""G:\1832 database\Database1832CH.mdf"";Integrated Security=True;Connect Timeout=30");
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(0);
            DisplayElements("ProductTb1", ProductsDGV);
            GetCategorie();
        }

        private void GetCategorie()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CatId from CategoryTb1", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CatId", typeof(int));
            dt.Load(Rdr);
            CatCb.ValueMember = "CatId";
            CatCb.DataSource = dt;
            Con.Close();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
            DisplayElements("CustomerTb1", CustomersDGV);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(2);
            DisplayElements("CategoryTb1", categoryDGV);
        }

        private void label14_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(3);
            DisplayElements("ProductTb1", BProductDGV);
            DisplayElements("SalesTb1", BillingListDGV);
            GetCustomer();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(4);
            CountCustomer();
            CountProduct();
            SumAmount();
        }
        private void DisplayElements(string TName, Bunifu.UI.WinForms.BunifuDataGridView DGV)
        {
            Con.Open();
            string Query = "select * from " + TName + "";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ProdNameTb.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into ProductTb1(ProdName,ProdCat,ProdPrice,ProdQty) values(@PN,@PC,@PP,@PQ)", Con);
                    cmd.Parameters.AddWithValue("@PN", ProdNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatCb.SelectedIndex.ToString());
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", QuantityTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Added!!!;");
                    Con.Close();
                    DisplayElements("ProductTb1", ProductsDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int Key = 0;
        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProdNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            QuantityTb.Text = ProductsDGV.SelectedRows[0].Cells[2].Value.ToString();
            PriceTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString();
            CatCb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (ProdNameTb.Text == "")
            {
                Key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (ProdNameTb.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update ProductTb1 set ProdName=@PN,ProdCat=@PC,ProdPrice=@PP,ProdQty=@PQ where ProdID=@PKey", Con);
                    cmd.Parameters.AddWithValue("@PN", ProdNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", QuantityTb.Text);
                    cmd.Parameters.AddWithValue("@PKey", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Updated!!!;");
                    Con.Close();
                    DisplayElements("ProductTb1", ProductsDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from ProductTb1 where ProdId=@PKey", Con);
                    cmd.Parameters.AddWithValue("@PKey", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("product deleted!!!");
                    Con.Close();
                    DisplayElements("ProductTb1", ProductsDGV);

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void AddCustBtn_Click(object sender, EventArgs e)
        {
            if (CNameTb.Text == "" || CAddressTb.Text == "" || CPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into CustomerTb1(CustName,CustPhone,CustAddress)values(@CN,@CP,@CA)", Con);
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CAddressTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Added!!!;");
                    Con.Close();
                    DisplayElements("CustomerTb1", CustomersDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int CKey = 0;
        private void CustomersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CNameTb.Text = CustomersDGV.SelectedRows[0].Cells[1].Value.ToString();
            CPhoneTb.Text = CustomersDGV.SelectedRows[0].Cells[2].Value.ToString();
            CAddressTb.Text = CustomersDGV.SelectedRows[0].Cells[3].Value.ToString();
            CatCb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (CNameTb.Text == "")
            {
                CKey = Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DelCustBtn_Click(object sender, EventArgs e)
        {
            if (CKey == 0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTb1 where CustId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", CKey);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer deleted!!!");
                    Con.Close();
                    DisplayElements("CustomerTb1", CustomersDGV);

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditCustBtn_Click(object sender, EventArgs e)
        {
            if (CNameTb.Text == "" || CAddressTb.Text == "" || CPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTb1 set CustName=@CN,CustPhone=@CP,CustAddress=@CA where CustId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CAddressTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", CKey);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated!!!");
                    Con.Close();
                    DisplayElements("CustomerTb1", CustomersDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void AddCatBtn_Click(object sender, EventArgs e)
        {
            if (CatNameTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into CategoryTb1(CatName)values(@CN)", Con);
                    cmd.Parameters.AddWithValue("@CN", CatNameTb.Text);


                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Added!!!;");
                    Con.Close();
                    DisplayElements("CategoryTb1", categoryDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteCatBtn_Click(object sender, EventArgs e)
        {
            if (CatKey == 0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CategoryTb1 where CatId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", CatKey);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category deleted!!!");
                    Con.Close();
                    DisplayElements("CategoryTb1", categoryDGV);

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int CatKey = 0;
        private void categoryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CatNameTb.Text = categoryDGV.SelectedRows[0].Cells[1].Value.ToString();

            if (CatNameTb.Text == "")
            {
                CatKey = Convert.ToInt32(categoryDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditCatBtn_Click(object sender, EventArgs e)
        {
            if (CatNameTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CategoryTb1 set CatName=@CN where CatId=@Ckey", Con);
                    cmd.Parameters.AddWithValue("@CN", CatNameTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", CatKey);


                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category updated!!!;");
                    Con.Close();
                    DisplayElements("CategoryTb1", categoryDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int BPKey = 0;
        int Stock = 0;
        private void BProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BprodNameTb.Text = BProductDGV.SelectedRows[0].Cells[1].Value.ToString();

            BPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString();

            if (BprodNameTb.Text == "")
            {
                Key = 0;
                Stock = 0;
            }
            else
            {
                Key = Convert.ToInt32(BProductDGV.SelectedRows[0].Cells[0].Value.ToString());
                Stock = Convert.ToInt32(BProductDGV.SelectedRows[0].Cells[2].Value.ToString());
            }
        }
        int n = 0;
        int GrdTotal = 0;
        private void bunifuButton12_Click(object sender, EventArgs e)
        {
            if (BQtyTb.Text == "")
            {
                MessageBox.Show("Enter the Quantity");
            }
            else if (Convert.ToInt32(BQtyTb.Text) > Stock)
            {
                MessageBox.Show("Not Enough Space");
            }
            else
            {
                int total = Convert.ToInt32(BQtyTb.Text) * Convert.ToInt32(BPriceTb.Text);
                DataGridViewRow newrow = new DataGridViewRow();
                newrow.CreateCells(YourBillDGV);
                newrow.Cells[0].Value = n + 1;
                newrow.Cells[1].Value = BprodNameTb.Text;
                newrow.Cells[2].Value = BQtyTb.Text;
                newrow.Cells[3].Value = BPriceTb.Text;
                newrow.Cells[4].Value = total;
                YourBillDGV.Rows.Add(newrow);
                n++;
                GrdTotal = GrdTotal + total;
                GrdTotalLb1.Text = "Rs" + GrdTotal;

            }
        }
        private void GetCustomer()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CustId from CustomerTb1", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustId", typeof(int));
            dt.Load(Rdr);
            CustomerCb.ValueMember = "CustId";
            CustomerCb.DataSource = dt;
            Con.Close();
        }
        private void RefresfBtn_Click(object sender, EventArgs e)
        {
            BPriceTb.Text = "";
            BQtyTb.Text = "";
            BprodNameTb.Text = "";
        }

        private void SaveBillBtn_Click(object sender, EventArgs e)
        {
            if (CustomerCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into SalesTb1(Customer,SAmount,SDate)values(@CN,@SA,@SD)", Con);
                    cmd.Parameters.AddWithValue("@CN", CustomerCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SA", GrdTotal);
                    cmd.Parameters.AddWithValue("@SD", DateTime.Today.Date);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sales Added!!!;");
                    Con.Close();
                    DisplayElements("SalesTb1", BillingListDGV);
                    BPriceTb.Text = "";
                    BQtyTb.Text = "";
                    BprodNameTb.Text = "";
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void CountCustomer()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from CustomerTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            CustLb1.Text = dt.Rows[0][0].ToString() + " Customers";

            Con.Close();
        }
        private void CountProduct()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ProductTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ProdLb1.Text = dt.Rows[0][0].ToString() + " Items";

            Con.Close();
        }
        private void GetProduct()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select ProdQty from ProductTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            // ProdLb1.Text = dt.Rows[0][0].ToString() + " Items";

            Con.Close();
        }
        private void SumAmount()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select sum(SAmount) from SalesTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            SalesLb1.Text = "Rs " + dt.Rows[0][0].ToString();

            Con.Close();

        }

        private void label16_Click(object sender, EventArgs e)
        {
            Login Obj=  new Login();
            Obj.Show();
            this.Hide();
        }
    }
}
