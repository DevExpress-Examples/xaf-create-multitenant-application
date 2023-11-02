using DevExpress.DevAV;
using Microsoft.EntityFrameworkCore;

namespace OutlookInspired.Win.Tests.Import{
	public class DevAvDb : DevAVDb{
		public DevAvDb(string connectionStringOrName) : base(connectionStringOrName){
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder){
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Employee>()
				.HasMany(e => e.AssignedEmployeeTasks)
				.WithMany(et => et.AssignedEmployees)
				.UsingEntity<Dictionary<string, object>>("EmployeeEmployeeTasks", 
					j => j.HasOne<EmployeeTask>().WithMany().HasForeignKey("EmployeeTask_Id"),
					j => j.HasOne<Employee>().WithMany().HasForeignKey("Employee_Id")
				);
		}
	}
}