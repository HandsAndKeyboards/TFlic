using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TFlic.ViewModel.ViewModelClass
{
    public class Task : INotifyPropertyChanged
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

        #region Tasks's fields and properties

        public long Id { get; set; }
        public int IdColumn { get; set; }

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

        Brush colorPriority;
        public Brush ColorPriority 
        {
            get => colorPriority;
            set => Set(ref colorPriority, value);
        }

        int executionTime;
        public int ExecutionTime 
        {
            get => executionTime;
            set => Set(ref executionTime, value);
        }

        DateTime deadline;
        public DateTime DeadLine 
        {
            get => deadline;
            set => Set(ref deadline, value);
        }

        string nameExecutor = string.Empty;
        public string NameExecutor 
        {
            get => nameExecutor;
            set => Set(ref nameExecutor, value);
        }

        #endregion
    }
}
