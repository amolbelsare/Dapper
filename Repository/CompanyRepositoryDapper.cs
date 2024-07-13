﻿using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System.Data;
using System.Net;

namespace DapperDemo.Repository
{
    public class CompanyRepositoryDapper : ICompanyRespository
    {
        private IDbConnection db;

        public CompanyRepositoryDapper(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {
            var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int);";
            //var id = db.Query<int>(sql, new 
            //{ 
            //   company.Name, 
            //   company.Address, 
            //   company.City, 
            //   company.State, 
            //   company.PostalCode 
            //}).Single();

            var id = db.Query<int>(sql, company).Single();

            company.CompanyId = id;
            return company;
        }

        public Company Find(int id)
        {
            var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return db.Query<Company>(sql, new { @CompanyId = id }).Single();
           
        }

        public List<Company> GetAll()
        {
            var sql = "SELECT * FROM Companies";          
            return db.Query<Company>(sql).ToList();        
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            db.Execute(sql, new{ @Id = id });
        }

        public Company Update(Company company)
        {
            var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State," +
                      "PostalCode = @PostalCode WHERE CompanyId =@CompanyId";
            db.Execute(sql, company);
            return company;
        }
    }
}
