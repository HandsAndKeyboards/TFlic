﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TFlic.ViewModel.ViewModelClass
{
    public class Column : INotifyPropertyChanged
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

        #region Column's fields and properties
        public long Id { get; set; }

        string title = string.Empty;
        public string Title 
        {
            get => title;
            set => Set(ref title, value);
        }

        public ObservableCollection<Task> Tasks { get; set; } = new();
        #endregion
    }
}
