using Chloe.MySql;
using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public interface ITickObject
    {
        void Tick(TimeSpan elapsedTime);
    }

    public abstract class GameObject<T> : ITickObject
        where T : DataInfo
    {
        public GameObject()
        {
            Info = Activator.CreateInstance(typeof(T)) as T;
        }
        public GameObject(T info)
        {
            Info = info;
        }
        public T Info
        {
            get; protected set;
        }
        public long Id { get; set; } = DataInfo.AutoGenerateID;

        public virtual void Load()
        {
        }
        public virtual void Tick(TimeSpan elapsedTime)
        {
        }
        public static void LoadDB(MySqlContext ctx)
        {
            ctx.Query<T>().ToList().ForEach(info =>
            {
                records[info.Id] = info;
            });
        }
        public static T GetDataInfo(long id)
        {
            records.TryGetValue(id, out T info);
            return info;
        }
        public static void AddModelInfo(T info)
        {
            records[info.Id] = info;
        }
        public static int RecordsCount
        {
            get { return records.Count; }
        }
        public static IEnumerable<T> Records
        {
            get { return records.Values; }
        }
        protected static Dictionary<long, T> records = new Dictionary<long, T>();
    }
}
