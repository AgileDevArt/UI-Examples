using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Employees.ViewModels;
using static Employees.ViewModels.EmployeeViewModel;

namespace Wpf
{
    public partial class EmployeeView : Form
    {
        public EmployeeViewModel ViewModel { get; private set; }
        public void Bind(EmployeeViewModel vm)
        {
            ViewModel = vm;
            using var ms = new MemoryStream(vm.GetIcon());
            Icon = Icon.FromHandle(((Bitmap)Image.FromStream(ms)).GetHicon());
            bindSource(vm.Employees);
        }

        public EmployeeView()
        {
            InitializeComponent();
        }

        private void bindSource(List<Employee> employee)
        {
            this.dataGrid.DataSource = new BindingList<Employee>(employee);
            this.buttonUndo.Enabled = false;
            this.buttonRedo.Enabled = false;
        }

        private void bindModel(Employee employee)
        {
            this.textId.DataBindings.Clear();
            this.textName.DataBindings.Clear();
            this.textId.DataBindings.Add(nameof(TextBox.Text), employee, nameof(Employee.Id), true, DataSourceUpdateMode.OnPropertyChanged, string.Empty, "0").Parse += beforePropertyChanged;
            this.textName.DataBindings.Add(nameof(TextBox.Text), employee, nameof(Employee.Name), false, DataSourceUpdateMode.OnPropertyChanged, string.Empty).Parse += beforePropertyChanged;
        }

        private bool tryGetSelected(out DataGridViewRow row)
        {
            row = this.dataGrid.SelectedRows.Count > 0 
                ? this.dataGrid.Rows[this.dataGrid.SelectedRows[0].Index] 
                : null;

            return row != null;
        }

        private void setSelected(DataGridViewRow row)
            => this.dataGrid.CurrentCell = row.Cells[0];

        private void beforePropertyChanged(object sender, ConvertEventArgs e)
        {
            if (tryGetSelected(out DataGridViewRow selected) && selected.DataBoundItem is Employee employee)
            {
                ViewModel.RedoStack.Clear();
                this.buttonRedo.Enabled = false;

                if (!ViewModel.UndoStack.TryPeek(out EmployeeState state) || state.Index != selected.Index)
                {
                    ViewModel.UndoStack.Push(new EmployeeState { Index = selected.Index, Employee = (Employee)employee.Clone() });
                    this.buttonUndo.Enabled = true;
                }
            }
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (tryGetSelected(out DataGridViewRow selected) && selected.DataBoundItem is Employee employee)
            {
                bindModel(employee);
                if (ViewModel.UndoStack.TryPeek(out EmployeeState state) && state.Employee.Equals(this.dataGrid.Rows[state.Index]?.DataBoundItem as Employee))
                {
                    ViewModel.UndoStack.Pop();
                    this.buttonUndo.Enabled = ViewModel.UndoStack.Count > 0;
                }
            }
        }

        private void undo_Click(object sender, EventArgs e)
        {
            while (ViewModel.UndoStack.TryPop(out EmployeeState state))
            {
                DataGridViewRow row = this.dataGrid.Rows[state.Index];
                if (row?.DataBoundItem is Employee employee)
                {
                    if (!employee.Equals(state.Employee))
                    {
                        ViewModel.RedoStack.Push(new EmployeeState { Index = row.Index, Employee = (Employee)employee.Clone() });
                        this.buttonRedo.Enabled = true;

                        employee.Id = state.Employee.Id;
                        employee.Name = state.Employee.Name;
                        setSelected(row);
                        break;
                    }
                }
            }
            this.buttonUndo.Enabled = ViewModel.UndoStack.Count > 0;
        }

        private void redo_Click(object sender, EventArgs e)
        {
            while (ViewModel.RedoStack.TryPop(out EmployeeState state))
            {
                DataGridViewRow row = this.dataGrid.Rows[state.Index];
                if (row?.DataBoundItem is Employee employee)
                {
                    if (!employee.Equals(state.Employee))
                    {
                        ViewModel.UndoStack.Push(new EmployeeState { Index = row.Index, Employee = (Employee)employee.Clone() });
                        this.buttonUndo.Enabled = true;

                        employee.Id = state.Employee.Id;
                        employee.Name = state.Employee.Name;
                        setSelected(row);
                        break;
                    }
                }
            }
            this.buttonRedo.Enabled = ViewModel.RedoStack.Count > 0;
        }

        private async void about_Click(object sender, EventArgs e) 
            => MessageBox.Show(this, await getAboutAsync(), $"About {this.Text}", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void textId_KeyPress(object sender, KeyPressEventArgs e) 
            => e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);

        private async Task<string> getAboutAsync()
        {
            var oldCursor = this.Cursor;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                return await ViewModel.GetAboutAsync();
            }
            finally
            {
                this.Cursor = oldCursor;
            }
        }
    }

#if NETFRAMEWORK
    public static class Extensions
    {
        public static bool TryPeek<T>(this Stack<T> stack, out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Peek();
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public static bool TryPop<T>(this Stack<T> stack, out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Pop();
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
#endif
}
