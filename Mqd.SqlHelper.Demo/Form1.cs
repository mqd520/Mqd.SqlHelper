using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Reflection;
using System.IO;
using Mqd.SqlHelper.Demo.Properties;
using Mqd.SqlHelper;
using Mqd.SqlHelper.Entity;

namespace Mqd.SqlHelper.Demo
{
    public partial class Form1 : Form
    {
        private readonly Db _db = new Db();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = _db.GetTable("select * from Orders where OrderID=@OrderID", new DbParameter[]{
                _db.CreateParameter("OrderID","10248")
            });
            Tool.FillListView(dt, listView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            Resources.Langlaile.Save(ms, Resources.Langlaile.RawFormat);
            byte[] buffer = new byte[ms.Length];
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            string sql = "insert into Categories(CategoryName,Description,Picture) values(@CategoryName,@Description,@Picture)";
            int n = _db.ExecuteNonQuery(sql, new DbParameter[]{
                _db.CreateParameter("@CategoryName","狼来了"),
                _db.CreateParameter("@Description","狼来了"),
                _db.CreateParameter("@Picture",buffer)
            });
        }

        private void fun1()
        {
            List<Categories> list = _db.GetTable("select * from Categories").ToList<Categories>();
            list.Reverse();
            string root = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var item in list)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(item.Picture);
                try
                {
                    Image img = Image.FromStream(ms);
                    string ext = GetImageExtension(img.RawFormat);
                    img.Save(root + item.CategoryName + ext);
                    img.Dispose();
                    ms.Close();
                }
                catch (Exception)
                {

                }
            }
        }

        private string GetImageExtension(System.Drawing.Imaging.ImageFormat format)
        {
            string ext = ".jpg";
            System.Reflection.PropertyInfo[] pi = typeof(System.Drawing.Imaging.ImageFormat).GetProperties();
            foreach (System.Reflection.PropertyInfo item in pi)
            {
                if (item.PropertyType == typeof(System.Drawing.Imaging.ImageFormat) &&
                    format.Guid.ToString() == ((System.Drawing.Imaging.ImageFormat)item.GetValue(format)).Guid.ToString())
                {
                    ext = item.Name.ToLower();
                    break;
                }
            }
            return "." + ext;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "update Categories set CategoryName=N'狼没来' where CategoryID=9";
            int n = _db.ExecuteNonQuery(sql);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = _db.GetTable("select * from [Current Product List]");
            Tool.FillListView(dt, listView1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = _db.ExecuteStoreProcedure("CustOrdersDetail", new DbParameter[]{
                _db.CreateParameter("@OrderID","10248")
            });
            Tool.FillListView(dt, listView1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int result = _db.Insert<Customers>(new Customers
            {
                CustomerID = "hcdus",
                CompanyName = "CompanyName:djksjdksjdksj",
                Address = "sdsdsds",
                Country = "sdsdsds"
            });
            Console.WriteLine(result);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sql = "delete from Categories where CategoryID=@CategoryID";
            int n = _db.ExecuteNonQuery(sql, new DbParameter[]{
                _db.CreateParameter("@CategoryID",14)
            });
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DbParameter[] paras = new DbParameter[]{
                _db.CreateParameter("@customerID",type:DbType.String,value:"AROUT"),
                _db.CreateParameter("@count",type:DbType.Int32,direction:ParameterDirection.Output)
            };
            DataTable dt = _db.ExecuteStoreProcedure("Customer1", paras);
            Tool.FillListView(dt, listView1);
            Console.WriteLine(paras[1].Value);
        }
    }
}
