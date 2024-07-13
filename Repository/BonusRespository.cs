using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;
using System.Data;

namespace DapperDemo.Repository
{
    public class BonusRespository : IBonusRespository
    {
        private IDbConnection db;

        public BonusRespository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company GetCompanyWithAddresses(int id)
        {
            var p = new 
            {
                CompanyId = id
            };

            var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId;"
                      + "SELECT * FROM Employees WHERE CompanyId = @CompanyId; ";

            Company company;
            using (var lists = db.QueryMultiple(sql,p))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }
            return company;
        }

        public List<Employee> GetEmployeeWithCompany(int companyId)
        {
            var sql = "SELECT E.*, C.* FROM Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId";
            if(companyId != 0)
            {
                sql += " WHERE E.CompanyId = @CompanyId ";
            }

            var employee = db.Query<Employee, Company, Employee>(sql, (e, c) =>
            {
                e.Company = c;
                return e;
            }, new { CompanyId = companyId },splitOn: "CompanyId");

            return employee.ToList();          
        }
    }
}
