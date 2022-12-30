using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TFlic.ViewModel.ViewModelClass
{
    public class Person : INotifyPropertyChanged
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

        #region Person's fields and properties

        public long Id;

        string name = string.Empty;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
