using Gtk;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Employees.ViewModels;
using static Employees.ViewModels.EmployeeViewModel;
using UI = Gtk.Builder.ObjectAttribute;

namespace MVVM_Gtk
{
    public class EmployeeView : Window
    {
        #region UI
        [UI] private Entry textId = null;
        [UI] private Entry textName = null;
        [UI] private Button buttonUndo = null;
        [UI] private Button buttonRedo = null;
        [UI] private Button buttonAbout = null;
        [UI] private TreeView dataGrid = null;
        [UI] private TreeViewColumn colImage = null;
        [UI] private TreeViewColumn colId = null;
        [UI] private TreeViewColumn colName = null;
        [UI] private CellRendererPixbuf crImage = null;
        [UI] private CellRendererText crId = null;
        [UI] private CellRendererText crName = null;
        [UI] private ScrolledWindow scrolledWindow = null;
        #endregion

        public EmployeeViewModel ViewModel { get; private set; }
        public void Bind(EmployeeViewModel vm)
        {
            ViewModel = vm;
            Icon = new Gdk.Pixbuf(vm.GetIcon(), 16, 16);
            bindSource(vm.Employees);
        }

        public EmployeeView() : this(new Builder($"{nameof(EmployeeView)}.glade")) { }

        private EmployeeView(Builder builder) : base(builder.GetRawOwnedObject(nameof(EmployeeView)))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;

            buttonUndo.Sensitive = false;
            buttonRedo.Sensitive = false;
            buttonUndo.Clicked += ButtonUndo_Clicked;
            buttonRedo.Clicked += ButtonRedo_Clicked;
            buttonAbout.Clicked += ButtonAbout_Clicked;

            textId.TextInserted += TextId_TextInserted;
            textId.Changed += TextId_Changed;
            textName.Changed += TextName_Changed;

            dataGrid.Selection.Changed += Selection_Changed;

            colImage.SetCellDataFunc(crImage, new TreeCellDataFunc(RenderImage));
            colId.SetCellDataFunc(crId, new TreeCellDataFunc(RenderId));
            colName.SetCellDataFunc(crName, new TreeCellDataFunc(RenderName));
        }

        private void bindSource(List<Employee> employee)
        {
            var store = new ListStore(typeof(Employee));
            foreach (var emp in employee)
                store.AppendValues(emp);

            dataGrid.Model = store;
            if (dataGrid.Model.GetIter(out var iter, TreePath.NewFirst()))
                setSelected(iter);
        }

        private void bindModel(Employee employee)
        {
            this.textName.Changed -= TextName_Changed;
            this.textId.Changed -= TextId_Changed;
            this.textName.Text = employee.Name;
            this.textId.Text = employee.Id.ToString();
            this.textName.Changed += TextName_Changed;
            this.textId.Changed += TextId_Changed;
        }

        private void RenderImage(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            var buffer = ((Employee)model.GetValue(iter, 0)).Image;
            ((CellRendererPixbuf)cell).Pixbuf = new Gdk.Pixbuf(buffer, 32, 32);
        }

        private void RenderId(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter) 
            => ((CellRendererText)cell).Text = ((Employee)model.GetValue(iter, 0)).Id.ToString();

        private void RenderName(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter) 
            => ((CellRendererText)cell).Text = ((Employee)model.GetValue(iter, 0)).Name;

        void TextId_Changed(object sender, EventArgs e)
            => ChangeEmployee(e => e.Id = ulong.TryParse(textId.Text, out var id) ? id : 0);

        void TextName_Changed(object sender, EventArgs e)
            => ChangeEmployee(e => e.Name = textName.Text);

        private void ChangeEmployee(Action<Employee> change)
        {
            if (dataGrid.Selection.GetSelected(out TreeIter iter) && dataGrid.Model.GetValue(iter, 0) is Employee employee)
            {
                ViewModel.RedoStack.Clear();
                this.buttonRedo.Sensitive = false;

                var selectedPath = dataGrid.Model.GetPath(iter);
                if (!ViewModel.UndoStack.TryPeek(out EmployeeState state) || state.Index.ToTreePath().Compare(selectedPath) != 0)
                {
                    ViewModel.UndoStack.Push(new EmployeeState { Index = selectedPath.ToRowIndex(), Employee = (Employee)employee.Clone() });
                    this.buttonUndo.Sensitive = true;
                }
                change.Invoke(employee);
                dataGrid.Model.EmitRowChanged(selectedPath, iter);
            }
        }

        private void setSelected(TreeIter newIter)
        {
            if (!dataGrid.Selection.IterIsSelected(newIter))
            {
                this.dataGrid.Selection.SelectIter(newIter);
            }
            else if (dataGrid.Model.GetValue(newIter, 0) is Employee employee)
            {
                bindModel(employee); //already selected, just bind again
            }
            this.dataGrid.Selection.TreeView.ScrollToCell(dataGrid.Model.GetPath(newIter), null, false, 0, 0); //make visible
        }

        private void Selection_Changed(object sender, EventArgs e)
        {
            if (dataGrid.Selection.GetSelected(out TreeIter iter) && dataGrid.Model.GetValue(iter, 0) is Employee employee)
            {
                bindModel(employee);
                if (ViewModel.UndoStack.TryPeek(out EmployeeState state) &&
                    dataGrid.Model.GetIter(out var stateIter, state.Index.ToTreePath()) &&
                    state.Employee.Equals(this.dataGrid.Model.GetValue(stateIter, 0) as Employee))
                {
                    ViewModel.UndoStack.Pop();
                    this.buttonUndo.Sensitive = ViewModel.UndoStack.Count > 0;
                }
            }
        }

        private void ButtonUndo_Clicked(object sender, EventArgs e)
        {
            while (ViewModel.UndoStack.TryPop(out EmployeeState state))
            {
                if (dataGrid.Model.GetIter(out var iter, state.Index.ToTreePath()) && 
                    dataGrid.Model.GetValue(iter, 0) is Employee employee)
                {
                    if (!employee.Equals(state.Employee))
                    {
                        ViewModel.RedoStack.Push(new EmployeeState { Index = dataGrid.Model.GetPath(iter).ToRowIndex(), Employee = (Employee)employee.Clone() });
                        this.buttonRedo.Sensitive = true;

                        employee.Id = state.Employee.Id;
                        employee.Name = state.Employee.Name;
                        dataGrid.Model.EmitRowChanged(state.Index.ToTreePath(), iter);
                        setSelected(iter);
                        break;
                    }
                }
            }
            this.buttonUndo.Sensitive = ViewModel.UndoStack.Count > 0;
        }

        private void ButtonRedo_Clicked(object sender, EventArgs e)
        {
            while (ViewModel.RedoStack.TryPop(out EmployeeState state))
            {
                if (dataGrid.Model.GetIter(out var iter, state.Index.ToTreePath()) &&
                    dataGrid.Model.GetValue(iter, 0) is Employee employee)
                {
                    if (!employee.Equals(state.Employee))
                    {
                        ViewModel.UndoStack.Push(new EmployeeState { Index = dataGrid.Model.GetPath(iter).ToRowIndex(), Employee = (Employee)employee.Clone() });
                        this.buttonUndo.Sensitive = true;

                        employee.Id = state.Employee.Id;
                        employee.Name = state.Employee.Name;
                        dataGrid.Model.EmitRowChanged(state.Index.ToTreePath(), iter);
                        setSelected(iter);
                        break;
                    }
                }
            }
            this.buttonRedo.Sensitive = ViewModel.RedoStack.Count > 0;
        }

        private async void ButtonAbout_Clicked(object sender, EventArgs e)
        {
            using var dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, await getAboutAsync())
            {
                Title = $"About {this.Title}",
                Icon = this.Icon
            };
            dialog.Run();
        }

        private void TextId_TextInserted(object o, TextInsertedArgs args)
        {
            if (!ulong.TryParse(args.NewText, out _))
            {
                this.textId.DeleteText(args.Position - args.NewTextLength, args.Position);
                args.Position -= args.NewTextLength;
            }
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private async Task<string> getAboutAsync()
        {
            var oldCursor = Window.Cursor;
            try
            {
                Window.Cursor = new Gdk.Cursor(Window.Display, Gdk.CursorType.Watch);
                return await ViewModel.GetAboutAsync();
            }
            finally
            {
                Window.Cursor = oldCursor;
            }
        }
    }

    public static class TreePathExtensions
    {
        public static TreePath ToTreePath(this int rowIndex) => new TreePath(new[] { rowIndex });
        public static int ToRowIndex(this TreePath treePath) => treePath.Indices[0];
    }
}
