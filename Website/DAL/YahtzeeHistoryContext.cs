using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace Octogami.Yahtzee.Website.DAL
{
    public class YahtzeeHistoryContext : HistoryContext
    {
        public YahtzeeHistoryContext(DbConnection dbConnection, string defaultSchema)
            : base(dbConnection, defaultSchema)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().ToTable(tableName: "__MigrationHistory", schemaName: "yz");
        }
    }

    public class ModelConfiguration : DbConfiguration
    {
        public ModelConfiguration()
        {
            this.SetHistoryContext("System.Data.SqlClient",
                (connection, defaultSchema) => new YahtzeeHistoryContext(connection, defaultSchema));
        }
    }
}