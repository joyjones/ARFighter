﻿using Chloe.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    public abstract class DataInfo
    {
        public DataInfo()
        {
        }
        [Column("id"), JsonProperty("id")]
        public long Id
        {
            get; set;
        } = AutoGenerateID;
        [NotMapped, JsonIgnore]
        public bool IsNew
        {
            get; set;
        } = true;

        public virtual void Save(MySqlConnectionFactory fac)
        {
            using (var ctx = fac.CreateContext())
            {
                if (IsNew)
                {
                    ctx.Insert(this);
                    IsNew = false;
                }
                else
                {
                    string sql = string.Empty;

                    var arrayFieldInfo = new List<FieldInfo>(GetType().GetFields());
                    var ps = new List<Chloe.DbParam>();

                    string sets = "";
                    if (GetType().GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() is TableAttribute tAttr)
                    {
                        sql = $"update {tAttr.Name} set ";
                        foreach (var fi in arrayFieldInfo)
                        {
                            if (fi != null)
                            {
                                if (sets.Length > 0)
                                    sets += ",";
                                if (fi.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() is ColumnAttribute columnAttr)
                                {
                                    sets += columnAttr.Name + "=?" + columnAttr.Name;
                                    ps.Add(new Chloe.DbParam("?" + columnAttr.Name, fi.GetValue(this), fi.GetType()));
                                }
                            }
                        }

                        sql += sets;
                        sql += $" where id={Id}";

                        ctx.Session.ExecuteNonQuery(sql, ps.ToArray());
                    }
                }
            }
        }
        public bool Delete(MySqlConnectionFactory fac)
        {
            int c = 0;
            using (var ctx = fac.CreateContext())
            {
                if (GetType().GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() is TableAttribute tAttr)
                {
                    var sql = $"delete from {tAttr.Name} where id={Id}";
                    c += ctx.Session.ExecuteNonQuery(sql);
                }
            }
            return c > 0;
        }
        public static long AutoGenerateID
        {
            get { return long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + CommonMethods.Rander.Next(100000).ToString().PadLeft(5, '0')); }
        }
    }
}
