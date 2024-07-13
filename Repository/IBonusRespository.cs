using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IBonusRespository
    {
        List<Employee> GetEmployeeWithCompany(int id);
        Company GetCompanyWithAddresses(int id);
    }
}
