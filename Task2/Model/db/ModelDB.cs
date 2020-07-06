namespace Task2.Model.db
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.ObjectModel;

    public partial class ModelDB : DbContext
    {
        public ModelDB()
            : base("name=ModelDB")
        {
        }

        public virtual DbSet<Parameter> Parameters { get; set; }
        public virtual DbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parameter>()
                .Property(e => e.RequiredValue)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Parameter>()
                .Property(e => e.MeasuredValue)
                .HasPrecision(18, 3);
        }
    }

    public partial class ModelDB : IDB
    {
        public ObservableCollection<Parameter> GetParameters()
        {
            this.Parameters.Load();
            return this.Parameters.Local;
        }

        public ObservableCollection<Test> GetTests()
        {
            this.Tests.Load();
            return this.Tests.Local;
        }

        void IDB.SaveChanges()
        {
            this.SaveChanges();
        }
    }
}
