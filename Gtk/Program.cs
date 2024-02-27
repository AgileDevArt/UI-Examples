using Employees.ViewModels;
using Gtk;
using System;

namespace MVVM_Gtk
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.GtkDialog.GtkDialog", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var view = new EmployeeView();
            view.Bind(new EmployeeViewModel());
            app.AddWindow(view);

            view.Show();
            Application.Run();
        }
    }
}
