using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Employees.ViewModels;
using static Employees.ViewModels.EmployeeViewModel;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Window, IValueConverter
    {
        public EmployeeViewModel ViewModel { get; private set; }
        public void Bind(EmployeeViewModel vm)
        {
            ViewModel = vm;
            Icon = ImageConverter.ToImage(vm.GetIcon());
            bindSource(vm.Employees);
        }

        public EmployeeView()
        {
            InitializeComponent();
            this.textId.SetBinding(TextBox.TextProperty,
                new Binding(nameof(Employee.Id))
                {
                    Converter = this,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger =
                    UpdateSourceTrigger.PropertyChanged
                });
            this.textName.SetBinding(TextBox.TextProperty,
                new Binding(nameof(Employee.Name))
                {
                    Converter = this,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger =
                    UpdateSourceTrigger.PropertyChanged
                });
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value;
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            beforePropertyChanged();
            return value;
        }

        private void bindSource(List<Employee> employee)
        {
            this.dataGrid.ItemsSource = new BindingList<Employee>(employee);
            this.buttonUndo.IsEnabled = false;
            this.buttonRedo.IsEnabled = false;
        }

        private void bindModel(Employee employee) 
            => this.DataContext = employee;

        private void setSelected(int index)
        {
            dataGrid.SelectedIndex = index;
            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        private void beforePropertyChanged()
        {
            if (dataGrid.SelectedItem is Employee employee)
            {
                ViewModel.RedoStack.Clear();
                this.buttonRedo.IsEnabled = false;

                if (!ViewModel.UndoStack.TryPeek(out EmployeeState state) || state.Index != dataGrid.SelectedIndex)
                {
                    ViewModel.UndoStack.Push(new EmployeeState { Index = dataGrid.SelectedIndex, Employee = (Employee)employee.Clone() });
                    this.buttonUndo.IsEnabled = true;
                }
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem is Employee employee)
            {
                bindModel(employee);
                if (ViewModel.UndoStack.TryPeek(out EmployeeState state) && state.Employee.Equals(this.dataGrid.Items[state.Index] as Employee))
                {
                    ViewModel.UndoStack.Pop();
                    this.buttonUndo.IsEnabled = ViewModel.UndoStack.Count > 0;
                }
            }
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            while (ViewModel.UndoStack.TryPop(out EmployeeState state))
            {
                if (this.dataGrid.Items[state.Index] is Employee employee)
                {
                    if (!employee.Equals(state.Employee))
                    {
                        ViewModel.RedoStack.Push(new EmployeeState { Index = state.Index, Employee = (Employee)employee.Clone() });
                        this.buttonRedo.IsEnabled = true;

                        employee.Id = state.Employee.Id;
                        employee.Name = state.Employee.Name;
                        setSelected(state.Index);
                        break;
                    }
                }
            }
            this.buttonUndo.IsEnabled = ViewModel.UndoStack.Count > 0;
        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {
            while (ViewModel.RedoStack.TryPop(out EmployeeState state))
            {
                if (this.dataGrid.Items[state.Index] is Employee employee)
                {
                    if (!employee.Equals(state.Employee))
                    {
                        ViewModel.UndoStack.Push(new EmployeeState { Index = state.Index, Employee = (Employee)employee.Clone() });
                        this.buttonUndo.IsEnabled = true;

                        employee.Id = state.Employee.Id;
                        employee.Name = state.Employee.Name;
                        setSelected(state.Index);
                        break;
                    }
                }
            }
            this.buttonRedo.IsEnabled = ViewModel.RedoStack.Count > 0;
        }

        private async void about_Click(object sender, RoutedEventArgs e)
            => MessageBox.Show(this, await getAboutAsync(), $"About {this.Title}", MessageBoxButton.OK, MessageBoxImage.Information);

        private void textId_PreviewTextInput(object sender, TextCompositionEventArgs e)
            => e.Handled = e.Text?.ToCharArray().All(_ => !char.IsControl(_) && !char.IsDigit(_)) == true;

        private void dataGrid_Loaded(object sender, RoutedEventArgs e) 
            => setSelected(0);

        private async Task<string> getAboutAsync()
        {
            var oldCursor = this.Cursor;
            try
            {
                this.Cursor = Cursors.Wait;
                return await ViewModel.GetAboutAsync();
            }
            finally
            {
                this.Cursor = oldCursor;
            }
        }
    }

    [ValueConversion(typeof(byte[]), typeof(ImageSource))]
    public class ImageConverter : IValueConverter
    {
        public static ImageConverter Instance { get; } = new ImageConverter();
        public static BitmapSource ToImage(byte[] content)
        {
            using MemoryStream memoryStream = new(content);
            var decoder = BitmapDecoder.Create(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            return decoder.Frames[0];
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is byte[] imageByteArray ? ToImage(imageByteArray) : (object)null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
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
