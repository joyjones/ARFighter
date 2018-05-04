using Chloe.Infrastructure;
using Chloe.MySql;
using MySql.Data.MySqlClient;
using System.Data;

namespace SkyARFighter.Common
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        public MySqlConnectionFactory(string conn)
        {
            ConnectionString = conn;
        }
        public string ConnectionString
        {
            get;
            private set;
        }
        public MySqlContext CreateContext()
        {
            return new MySqlContext(new MySqlConnectionFactory(ConnectionString));
        }
        public IDbConnection CreateConnection()
        {
            IDbConnection conn = new MySqlConnection(ConnectionString);
            return new ChloeMySqlConnection(conn);
        }
    }
}
