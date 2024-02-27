using Employees.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Employees.Services
{
    public class EmployeesService : IEmployeesService
    {
        const string BASE_URL = "https://random-data-api.com/api";

        private readonly HttpClient _httpClient;

        private static IEmployeesService _instance;
        public static IEmployeesService Instance => _instance ??= new EmployeesService();

        public EmployeesService()
        {
            _httpClient = new(new HttpClientHandler()
            {
                UseDefaultCredentials = true
            });
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient");
        }

        public async Task<EmployeeDto[]> GetEmployeesAsync(int count)
        {
            var url = $"{BASE_URL}/v2/users?size={count}";
            return await _httpClient.GetFromJsonAsync<EmployeeDto[]>(url).ConfigureAwait(false);
        }

        public async Task<byte[]> GetImageAsync(string url)
        {
            using var response = await _httpClient.GetStreamAsync(url).ConfigureAwait(false);
            using MemoryStream memoryStream = new();
            response.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<string> GetInfoAsync()
        {
            var url = $"{BASE_URL}/hipster/random_hipster_stuff";
            var hipsterStuff = await _httpClient.GetFromJsonAsync<HipsterStuffDto>(url).ConfigureAwait(false);
            return hipsterStuff?.sentence;
        }
    }
}
