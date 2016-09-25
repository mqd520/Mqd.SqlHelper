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
            DbParameter[] paras = new DbParameter[] { 
                new System.Data.SqlClient.SqlParameter{
                     ParameterName="@OrderID",
                      Value="10248"
                }
            };
            DataTable dt = _db.GetTable("select * from Orders where OrderID=@OrderID", paras);
            Tool.FillListView(dt, listView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.FileStream fs = System.IO.File.Open(@"D:\download\狼来了.png",
                System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[1];
            System.Data.SqlClient.SqlParameter para = new System.Data.SqlClient.SqlParameter();
            para.DbType = (DbType)SqlDbType.Binary;
            para.Size = buffer.Length;
            para.Value = buffer;
            para.ParameterName = "@Picture";
            paras[0] = para;
            string sql = "insert into Categories(CategoryName,Description,Picture) values('狼来了','狼来了',@Picture)";
            int n = _db.ExecuteNonQuery(sql, paras);
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
            DbParameter[] paras = new DbParameter[] { 
                new System.Data.SqlClient.SqlParameter{
                     ParameterName="@OrderID",
                      Value="10248"
                }
            };
            DataTable dt = _db.ExecuteStoreProcedure("CustOrdersDetail", paras);
            Tool.FillListView(dt, listView1);
        }
    }
}
