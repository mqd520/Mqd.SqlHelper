﻿using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static class Tool
    {
        private static PropertyInfo[] Mapping(PropertyInfo[] pi, DataColumnCollection cols)
        {
            PropertyInfo[] result = new PropertyInfo[cols.Count];
            for (int i = 0; i < cols.Count; i++)
            {
                int existIndex = -1;
                for (int j = 0; j < pi.Length; j++)
                {
                    if (cols[i].ColumnName.ToLower() == pi[j].Name.ToLower())
                    {
                        existIndex = j;
                        break;
                    }
                }
                if (existIndex > -1)
                {
                    result[i] = pi[existIndex];
                }
            }
            return result;
        }

        /// <summary>
        /// 将DataTable转换成相应的实体类型
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dt">要转换的DataTable</param>
        /// <returns>转换后的实体数据集合</returns>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            PropertyInfo[] pi = typeof(T).GetProperties();
            PropertyInfo[] piMap = Mapping(pi, dt.Columns);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T model = new T();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (piMap[j] != null && !Convert.IsDBNull(dt.Rows[i][j]))
                    {
                        piMap[j].SetValue(model, Convert.ChangeType(dt.Rows[i][j], piMap[j].PropertyType));
                    }
                }
                list.Add(model);
            }
            return list;
        }
    }
}
