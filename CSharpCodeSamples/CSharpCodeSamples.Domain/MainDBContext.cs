namespace CSharpCodeSamples.Domain
{
    using System.Data.Entity;
    using EFModels;
    using EFModels.Mapping;

    public class MainDBContext : DbContext
    {
        static MainDBContext() {
            Database.SetInitializer<MainDBContext>(null);
        }

        public MainDBContext(string conn)
            : base(conn)
        {}

        public DbSet<SalesRepresentative> Orders          { get; set; }
        public DbSet<SalesRepresentative> Representatives { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new SalesRepresentativeMap());
        }
    }
}
