using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TFlic.ViewModel.ViewModelClass
{
    internal class Sprint
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

        #region Fields and properties
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
