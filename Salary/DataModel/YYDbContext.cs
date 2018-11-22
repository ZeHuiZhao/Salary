namespace Salary.DataModel.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Entity;

    public partial class YYDbContext : DbContext
    {
        public YYDbContext()
            : base("name=YYDbContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<YY_Activity> YY_Activity { get; set; }

        public DbSet<YY_Company> YY_Company { get; set; }

        public DbSet<YY_CompanyContact> YY_CompanyContact { get; set; }

        public DbSet<YY_ContactRecord> YY_ContactRecord { get; set; }

        public DbSet<YY_Enroll> YY_Enroll { get; set; }

        public DbSet<YY_EnrollAudit> YY_EnrollAudit { get; set; }

        public DbSet<YY_Function> YY_Function { get; set; }

        public DbSet<YY_Material> YY_Material { get; set; }

        public DbSet<YY_MaterialBrower> YY_MaterialBrower { get; set; }

        public DbSet<YY_MaterialChannel> YY_MaterialChannel { get; set; }

        public DbSet<YY_MaterialShare> YY_MaterialShare { get; set; }

        public DbSet<YY_MaterialType> YY_MaterialType { get; set; }

        public DbSet<YY_Role> YY_Role { get; set; }

        public DbSet<YY_Userinfo> YY_Userinfo { get; set; }
        




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
