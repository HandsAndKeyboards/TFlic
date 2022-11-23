using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClass;
using TFlic.Command;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace TFlic.ViewModel
{
    internal class BoardWindowViewModel : Base.ViewModelBase
    {
        #region Fields
        // Некоторые поля, помещены в регионы с командами
        // С которыми они используются

        ObservableCollection<Column> columns = new();
        public ObservableCollection<Column> Columns
        {
            get => columns;
            set => Set(ref columns, value);
        }

        int idcounter = -1;

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

        #region Команды добавления и изменения задачи

        #region Буферные поля
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

        string nameExecutor = string.Empty;
        public string NameExecutor
        {
            get => nameExecutor;
            set => Set(ref nameExecutor, value);
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
        public DateTime Deadline
        {
            get => deadline;
            set => Set(ref deadline, value);
        }
        #endregion

        #region Команда добавления задачи 
        public ICommand AddTaskCommand { get; }
        private void OnAddTaskCommandExecuted(object p)
        {
            Columns[0].Tasks.Add(
                new ViewModelClass.Task()
                {
                    Id = ++idcounter,
                    IdColumn = 0,
                    Name = nameTask,
                    Description = descriptionTask,
                    ColorPriority = colorPriority,
                    ExecutionTime = executionTime,
                    DeadLine = deadline,
                    NameExecutor = nameExecutor
                }
            );
        }

        private bool CanAddTaskCommandExecute(object p) { return true; }
        #endregion

        #region Команда изменения задачи

        public ICommand ChangeTaskCommand { get; }
        private void OnChangeTaskCommandExecuted(object p)
        {
            int idTask = Convert.ToInt32(idTaskBuffer);
            int idColumn = Convert.ToInt32(idColumnBuffer);
            int taskIndex = SearchIndexTask(idColumn, idTask);

            columns[idColumn].Tasks[taskIndex].Name = NameTask;
            columns[idColumn].Tasks[taskIndex].Description = DescriptionTask;
            columns[idColumn].Tasks[taskIndex].ColorPriority = ColorPriority;
            columns[idColumn].Tasks[taskIndex].ExecutionTime = ExecutionTime;
            columns[idColumn].Tasks[taskIndex].DeadLine = Deadline;
            columns[idColumn].Tasks[taskIndex].NameExecutor = NameExecutor;
        }

        private bool CanChangeTaskCommandExecute(object p) 
        { 
            return (MessageBoxResult)p == MessageBoxResult.Yes; 
        }

        #endregion

        #endregion

        #region Команды перемещения задачи

        // Буферные поля 
        string idTaskBuffer = string.Empty;
        public string IdTaskBuffer
        {
            get => idTaskBuffer;
            set => Set(ref idTaskBuffer, value);
        }

        string idColumnBuffer = string.Empty;
        public string IdColumnBuffer
        {
            get => idColumnBuffer;
            set => Set(ref idColumnBuffer, value);
        }

        #region Команда перемещения задачи в следующую колонку

        public ICommand MoveTaskToNextColumnCommand { get; }

        private void OnMoveTaskToNextColumnExecuted(object p)
        {
            int idTask = Convert.ToInt32(idTaskBuffer);
            int idColumn = Convert.ToInt32(idColumnBuffer);
            int taskIndex = SearchIndexTask(idColumn, idTask);

            // Добавли задачу в следующую колонку и удалили из текущей
            columns[idColumn + 1].Tasks.Add(columns[idColumn].Tasks[taskIndex]);
            columns[idColumn].Tasks.RemoveAt(taskIndex);

            // Нашли индекс задачи в колонке, в которую она была добавлена
            // И прибавили к ее id единицу (id колонки = index колонки в нашем случае)
            taskIndex = SearchIndexTask(idColumn + 1, idTask);
            columns[idColumn + 1].Tasks[taskIndex].IdColumn++;
        }

        private bool CanMoveTaskToNextColumnExecute(object p) 
        {
            bool result = true;

            if (!int.TryParse(idColumnBuffer, out int idColumn)
                || !int.TryParse(idTaskBuffer, out _)) 
                result = false;
            if (idColumn >= columns.Count - 1)
                result = false;

            return result;
        }

        #endregion

        #region Команда перемещения задачи в предыдущую колонку

        public ICommand MoveTaskToPrevColumnCommand { get; }

        private void OnMoveTaskToPrevColumnExecuted(object p)
        {
            int idTask = Convert.ToInt32(idTaskBuffer);
            int idColumn = Convert.ToInt32(idColumnBuffer);
            int taskIndex = SearchIndexTask(idColumn, idTask);

            // Перемещаем задачу в предыдущую колонку и удаляем ее из текущей
            columns[idColumn - 1].Tasks.Add(columns[idColumn].Tasks[taskIndex]);
            columns[idColumn].Tasks.RemoveAt(taskIndex);

            // Нашли индекс задачи в колонке, в которую она была добавлена
            // И вычли из ее id единицу (id колонки = index колонки в нашем случае)
            taskIndex = SearchIndexTask(idColumn - 1, idTask);
            columns[idColumn - 1].Tasks[taskIndex].IdColumn--;
        }

        private bool CanMoveTaskToPrevColumnExecute(object p)
        {
            bool result = true;

            if (!int.TryParse(idColumnBuffer, out int idColumn)
                || !int.TryParse(idTaskBuffer, out _))
                result = false;
            if (columns.Count - 1 < idColumn || idColumn <= 0)
                result = false;

            return result;
        }

        #endregion

        #endregion

        #region Команда удаления задачи

        public ICommand DeleteTaskCommand { get; }

        private void OnDeleteTaskCommandExecuted(object p)
        {
            int idTask = Convert.ToInt32(idTaskBuffer);
            int idColumn = Convert.ToInt32(idColumnBuffer);
            int taskIndex = SearchIndexTask(idColumn, idTask);

            columns[idColumn].Tasks.RemoveAt(taskIndex);
        }

        private bool CanDeleteTaskCommandExecute(object p) 
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes;
        }

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

            MoveTaskToNextColumnCommand =
                new RelayCommand(OnMoveTaskToNextColumnExecuted, CanMoveTaskToNextColumnExecute);

            MoveTaskToPrevColumnCommand =
                new RelayCommand(OnMoveTaskToPrevColumnExecuted, CanMoveTaskToPrevColumnExecute);

            ChangeTaskCommand =
                new RelayCommand(OnChangeTaskCommandExecuted, CanChangeTaskCommandExecute);

            DeleteTaskCommand =
                new RelayCommand(OnDeleteTaskCommandExecuted, CanDeleteTaskCommandExecute);
        }

        #endregion


        #region Methods

        private int SearchIndexTask(int idColumn, int idTask)
        {
            int result = 0;

            for (int i = 0; i < columns[idColumn].Tasks.Count; i++)
                if (columns[idColumn].Tasks[i].Id == idTask)
                    result = i;

            return result;
        }

        #endregion
    }
}