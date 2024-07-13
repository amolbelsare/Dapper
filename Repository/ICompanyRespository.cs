using DapperDemo.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace DapperDemo.Repository
{
    public interface ICompanyRespository
    {
        Company Find(int id);
        List<Company> GetAll();
        Company Add(Company company);
        Company Update(Company company);
        void Remove(int id);
    }
}
