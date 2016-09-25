using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace Mqd.SqlHelper.Demo
{
    public class Tool
    {
        public static void FillListView(DataTable dt, ListView lv)
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            lv.Columns.Clear();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                lv.Columns.Add(dt.Columns[i].ColumnName);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] arr = new string[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = dt.Rows[i][j].ToString();
                }
                lv.Items.Add(new ListViewItem(arr));
            }
            lv.EndUpdate();
        }

        public static void FillListView<T>(IList<T> list, ListView lv)
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            lv.Columns.Clear();
            System.Type t = typeof(T);
            t.GetProperties();
            PropertyInfo[] pi = t.GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                lv.Columns.Add(pi[i].Name);
            }
            foreach (T item in list)
            {
                string[] arr = new string[pi.Length];
                for (int j = 0; j < pi.Length; j++)
                {
                    object val = pi[j].GetValue(item);
                    if (val != null)
                    {
                        arr[j] = val.ToString();
                    }
                    else
                    {
                        arr[j] = "";
                    }
                }
                lv.Items.Add(new ListViewItem(arr));
            }
            lv.EndUpdate();
        }
    }
}
