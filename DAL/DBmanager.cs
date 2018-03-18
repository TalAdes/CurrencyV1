using Entities;
using System.Data.Entity;


namespace DAL
{
    public class DBmanager : DbContext
    {
        public DBmanager() : base()
        {
            Database.SetInitializer<DBmanager>(new DropCreateDatabaseIfModelChanges<DBmanager>());
        }
        

        public DbSet<Coin> Coins { get; set; }
        public DbSet<HistoricalCoin> HistoricalCoins { get; set; }
    }
}
