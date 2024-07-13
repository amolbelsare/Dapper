using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Repository
{
    public class EmployeeRepositoryDapper : IEmployeeRepository
    {
        private IDbConnection db;

        public EmployeeRepositoryDapper(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public Employee Add(Employee employee)
        {
            var sql = "INSERT INTO Employees(Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = db.Query<int>(sql, employee).Single();
            employee.EmployeeId = id;
            return employee;
        }

        public Employee Find(int id)
        {
            var sql = "SELECT * FROM Employees Where EmployeeID = @EmployeeId";
            return db.Query<Employee>(sql, new { @EmployeeId = id }).Single();
        }

        public List<Employee> GetAll()
        {
            var sql = "SELECT * FROM Employees";
            IEnumerable<Employee> emplist =  db.Query<Employee>(sql).ToList();
            if (emplist != null) 
            {
                return emplist.ToList();
            }
            return null;
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Employees WHERE ComapnyId = @Id";
            db.Execute(sql, new { id });
        }

        public Employee Update(Employee employee)
        {
            var sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email =@Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            db.Execute(sql, employee);
            return employee;
        }
    }
}
