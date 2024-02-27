using Employees.ViewModels;
using System;
using System.Windows.Forms;

namespace Wpf
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var view = new EmployeeView();
            view.Bind(new EmployeeViewModel());
            Application.Run(view);
        }
    }
}
