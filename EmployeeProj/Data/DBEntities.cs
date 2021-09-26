using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using EmployeeProj.Model;
namespace EmployeeProj.Data
{
    public class DBEntities: DbContext
    {
        public DBEntities() : base("HRContext")
        {
        }
        public DbSet<EmployeeRequestModel> EMPLOYEE{ get; set; }     
        public DbSet<DepartmentModel> DEPARTMENT { get; set; }
        public DbSet<SkillModel> skills { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeRequestModel>().ToTable("EMPLOYEE");
            modelBuilder.Entity<DepartmentModel>().ToTable("DEPARTMENT");
            modelBuilder.Entity<SkillModel>().ToTable("skills");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}