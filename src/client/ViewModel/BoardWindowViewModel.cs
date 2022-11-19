using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClasses;
using TFlic.Command;
using System.Windows.Input;
using System.Windows;

namespace TFlic.ViewModel
{
    internal class BoardWindowViewModel : Base.ViewModelBase
    {
        #region Fields

        ObservableCollection<Column> columns = new();
        public ObservableCollection<Column> Columns
        {
            get => columns;
            set => Set(ref columns, value);
        }

        #endregion


        #region Commands

        #region Команда добавления колонки

        // Буферное поле
        string nameNewColumn = string.Empty;
        public string NameNewColumn
        {
            get => nameNewColumn;
            set => Set(ref nameNewColumn, value);
        }

        public ICommand AddColumnCommand { get; }

        private void OnAddColumnCommandExecuted(object p)
        {
            Columns.Add(
                new Column()
                {
                    Id = columns.Count,
                    Title = NameNewColumn,
                    Tasks = new()
                }
            );
        }

        private bool CanAddColumnCommandExecute(object p) { return true; }

        #endregion

        #region Команда добавления задачи

        // Буферные поля
        string nameTask = string.Empty;
        public string NameTask
        {
            get => nameTask;
            set => Set(ref nameTask, value);
        }

        string descriptionTask = string.Empty;
        public string DescriptionTask
        {
            get => descriptionTask;
            set => Set(ref descriptionTask, value);
        }

        public ICommand AddTaskCommand { get; }

        private void OnAddTaskCommandExecuted(object p)
        {
            Columns[0].Tasks.Add(
                new ViewModelClasses.Task()
                {
                    Name = nameTask,
                    Description = descriptionTask
                }
            );
        }

        private bool CanAddTaskCommandExecute(object p) { return true; }

        #endregion

        #endregion


        #region Constructors

        public BoardWindowViewModel()
        {
            Columns.Add(
                new Column()
                {
                    Id = columns.Count,
                    Title = "Backlog",
                    Tasks = new()
                }
            );

            AddColumnCommand =
                new RelayCommand(OnAddColumnCommandExecuted, CanAddColumnCommandExecute);

            AddTaskCommand =
                new RelayCommand(OnAddTaskCommandExecuted, CanAddTaskCommandExecute);
        }

        #endregion
    }
}