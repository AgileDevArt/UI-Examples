using Employees.Models;

namespace Employees.Services
{
    public interface IEmployeesService
    {
        Task<EmployeeDto[]> GetEmployeesAsync(int page);
        Task<byte[]> GetImageAsync(string url);
        Task<string> GetInfoAsync();
    }
}