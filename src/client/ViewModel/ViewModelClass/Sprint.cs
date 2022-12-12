using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TFlic.Model;

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

        DateTime beginDate = DateTime.MinValue;
        public DateTime BeginDate
        {
            get => beginDate;
            set => Set(ref beginDate, value);
        }

        DateTime endDate = DateTime.MaxValue;
        public DateTime EndDate
        {
            get => endDate;
            set => Set(ref endDate, value);
        }

        ulong duration = 0;
        public ulong Duration
        {
            get => duration;
            set => Set(ref duration, value);
        }

        public uint number = 0;
        public uint Number
        {
            get => number;
            set => Set(ref number, value);
        }
        #endregion

        #region Methods
        public Sprint()
        {
            name = "";
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
