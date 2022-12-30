using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TFlic.Model;

namespace TFlic.ViewModel.ViewModelClass
{
    public class Organization : INotifyPropertyChanged
    {
        #region Overriding INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region Organization's fields and properties

        public long Id { get; set; }
        string name = string.Empty;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        string description = string.Empty;
        public string Description
        {
            get => description;
            set => Set(ref description, value);
        }

        public ObservableCollection<Project> projects { get; set; } = new();
        public ObservableCollection<Person> peoples { get; set; } = new();
        public ObservableCollection<UserGroupDto> userGroups { get; set; } = new();
        #endregion

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
