

using DB_Module.SQL_Models;

namespace DB_Module
{
    public class SQL : DbContext
    {
        public static string ConnectionString { get; set; }

        public SQL() : base(new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options)
        {
        }

        public SQL(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tábla> Táblák { get; set; }
        //DBSet-ek
    }
}
