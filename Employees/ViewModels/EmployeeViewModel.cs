using Employees.Services;
using System.Reflection;

namespace Employees.ViewModels
{
    public class EmployeeViewModel
    {
        public class EmployeeState
        {
            public Employee Employee { get; set; }
            public int Index { get; set; }
        }

        public readonly Stack<EmployeeState> UndoStack = new(20);
        public readonly Stack<EmployeeState> RedoStack = new(20);
        public List<Employee> Employees { get; } = GetEmployees();

        /// <summary>
        /// Get Svg Icon
        /// </summary>
        public byte[] GetIcon() 
            => GetResourceAsync("Employees.Resources.Robot.png").Result;

        /// <summary>
        /// About info
        /// </summary>
        public string GetAbout()
            => GetAboutAsync().Result;

        /// <summary>
        /// Get cryptocurrency rankings
        /// </summary>
        public static List<Employee> GetEmployees(int count = 20)
            => EmployeesService.Instance.GetEmployeesAsync(count).Result
                .Select(_ => new Employee((ulong)_.id, $"{_.first_name} {_.last_name}", _.avatar))
                .ToList();

        /// <summary>
        /// About info
        /// </summary>
        public async Task<string> GetAboutAsync()
            => string.Join(Environment.NewLine,
                $"Undo / Redo Example",
                string.Empty,
                $"Path:",
                GetAssemblyLocation(),
                string.Empty,
                $"Framework: \t{Environment.Version}",
                $"Platform:  \t{Environment.OSVersion}",
                string.Empty,
                await GetServerInfoAsync().ConfigureAwait(false));

        /// <summary>
        /// Server Info
        /// </summary>
        public static async Task<string> GetServerInfoAsync()
        {
            var info = await EmployeesService.Instance.GetInfoAsync().ConfigureAwait(false);
            return $"Message: \t{info}";
        }

        /// <summary>
        /// Get Executing Assembly Location
        /// </summary>
        public static string GetAssemblyLocation(int maxLength = 255)
        {
            var path = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
            return path.Substring(Math.Max(0, path.Length - maxLength), Math.Min(maxLength, path.Length));
        }

        /// <summary>
        /// Read resource file 
        /// </summary>
        public static async Task<byte[]> GetResourceAsync(string filename)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);
            using MemoryStream memoryStream = new();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            return memoryStream.ToArray();
        }
    }
}
