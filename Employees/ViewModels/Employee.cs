using Employees.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Employees.ViewModels
{
    public class Employee : INotifyPropertyChanged, ICloneable, IEquatable<Employee>
    {
        private Task<byte[]> _imageTask;
        private string _imageUrl;
        private byte[] _image;
        public byte[] Image
        {
            get => _image ??= _imageTask.Result;
            set
            {
                if (_image == value)
                    return;

                _image = value;
                RaisePropertyChange();
            }
        }

        private ulong _id;
        public ulong Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;

                _id = value;
                RaisePropertyChange();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;

                _name = value;
                RaisePropertyChange();
            }
        }

        public Employee(ulong id, string name, string imageUrl, byte[] image = null)
        {
            _id = id;
            _name = name;
            _imageUrl = Regex.Replace(imageUrl, "size=\\d+x\\d+", "size=32x32");
            if (image != null)
                _image = image;
            else _imageTask = EmployeesService.Instance.GetImageAsync(_imageUrl);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChange([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public object Clone() => new Employee(_id, _name, _imageUrl, _image);

        public bool Equals(Employee other)
            => other != null &&
               _id.Equals(other._id) &&
               _name.Equals(other._name);
    }
}
