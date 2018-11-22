namespace Salary_MVC.Data
{
    using System.Data.Entity;
    using Common;
    using DataModel;

    public partial class GZDbContext : DbContext
    {
        public GZDbContext()
            : base("name=GZDbContext")
        {
        }

        public virtual DbSet<AbnormalAppeal> AbnormalAppeal { get; set; }
        public virtual DbSet<AnnualLeave> AnnualLeave { get; set; }
        public virtual DbSet<Leave> Leave { get; set; }
        public virtual DbSet<OutsideApply> OutsideApply { get; set; }
        public virtual DbSet<Schedul> Schedul { get; set; }
        public virtual DbSet<SchedulAttendLocation> SchedulAttendLocation { get; set; }
        public virtual DbSet<SignData> SignData { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserWages> UserWages { get; set; }
        public virtual DbSet<WagesCode> WagesCode { get; set; }
        public virtual DbSet<GZ_Attendance> GZ_Attendance { get; set; }
        public virtual DbSet<GZ_AttendanceDetail> GZ_AttendanceDetail { get; set; }
        public virtual DbSet<GZ_MonthlySalaryDetail> GZ_MonthlySalaryDetail { get; set; }
        public virtual DbSet<GZ_Attachment> GZ_Attachment { get; set; }
        public virtual DbSet<GZ_EmployeeSalary> GZ_EmployeeSalary { get; set; }
        public virtual DbSet<GZ_HouseMoneyDetail> GZ_HouseMoneyDetail { get; set; }
        public virtual DbSet<GZ_HouseMoneyMaster> GZ_HouseMoneyMaster { get; set; }
        public virtual DbSet<GZ_ShortSalary> GZ_ShortSalary { get; set; }
        public virtual DbSet<GZ_Bonus> GZ_Bonus { get; set; }
        public virtual DbSet<GZ_Employee> GZ_Employee { get; set; }
        public virtual DbSet<GZ_FinancialUnit> GZ_FinancialUnit { get; set; }
        public virtual DbSet<GZ_Function> GZ_Function { get; set; }
        public virtual DbSet<GZ_UpdateHistory> GZ_UpdateHistory { get; set; }
        public virtual DbSet<GZ_FunctionGroup> GZ_FunctionGroup { get; set; }
        public virtual DbSet<GZ_FunctionGroupRight> GZ_FunctionGroupRight { get; set; }
        public virtual DbSet<GZ_ApproveLog> GZ_ApproveLog { get; set; }
        public virtual DbSet<GZ_MonthlySalaryMaster> GZ_MonthlySalaryMaster { get; set; }
        public virtual DbSet<GZ_SMS> GZ_SMS { get; set; }
        public virtual DbSet<GZ_SocialMoneyDetailGZ> GZ_SocialMoneyDetailGZ { get; set; }
        public virtual DbSet<GZ_SocialMoneyDetailSZ> GZ_SocialMoneyDetailSZ { get; set; }
        public virtual DbSet<GZ_SocialMoneyMasterGZ> GZ_SocialMoneyMasterGZ { get; set; }
        public virtual DbSet<GZ_SocialMoneyMasterSZ> GZ_SocialMoneyMasterSZ { get; set; }
        public virtual DbSet<GZ_User> GZ_User { get; set; }
        public virtual DbSet<GZ_UserFunctionRight> GZ_UserFunctionRight { get; set; }
        public virtual DbSet<GZ_UserRole> GZ_UserRole { get; set; }
        public virtual DbSet<GZ_Company> GZ_Company { get; set; }
        public virtual DbSet<GZ_Department> GZ_Department { get; set; }
        public virtual DbSet<GZ_Role> GZ_Role { get; set; }
        



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
            base.OnModelCreating(modelBuilder);
        }
    }
}
