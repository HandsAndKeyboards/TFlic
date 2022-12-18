using System;
using System.Collections.ObjectModel;
using TFlic.ViewModel.ViewModelClass;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using TFlic.ViewModel.Command;
using TFlic.Model.Transfer;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Markup;

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

        long idOrganization = 0;
        public long IdOrganization
        {
            get => idOrganization;
            set => Set(ref idOrganization, value);
        }

        long idProject = 0;
        public long IdProject
        {
            get => idProject;   
            set => Set(ref idProject, value);
        }

        long idBoard = 0;
        public long IdBoard
        {
            get => idBoard;
            set => Set(ref idBoard, value);
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
                    Title = NameNewColumn,
                    Tasks = new()
                }
            );
            try
            {
                ColumnTransferer.TransferToServer(Columns, idOrganization, idProject, idBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanAddColumnCommandExecute(object p) { return true; }

        #endregion

        #region Команда переименования колонки 

        public ICommand RenameColumnCommand { get; }

        private void OnRenameColumnCommandExecuted(object p)
        {
            long idColumn = Convert.ToInt32(idColumnBuffer);
            int indexColumn = SearchIndexColumn(idColumn);

            try
            {
                ColumnTransferer.TransferToServer(columns, idOrganization, idProject, idBoard, idColumn, nameNewColumn, indexColumn); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanRenameColumnCommandExecute(object p) 
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes;
        }

        #endregion

        #region Команда удаления колонки

        public ICommand DeleteColumnCommand { get; }

        private void OnDeleteColumnCommandExecuted(object p)
        {
            long idColumn = Convert.ToInt64(idColumnBuffer);
            int indexColumn = SearchIndexColumn(idColumn);

            Columns.RemoveAt(indexColumn);
            ColumnTransferer.TransferToServer(IdOrganization, IdProject, IdBoard, idColumn);
        }

        private bool CanDeleteColumnCommandExecute(object p) 
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes;
        }

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

        string login;
        public string Login
        {
            get => login;
            set => Set(ref login, value);
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

        long priority = 1;
        public long Priority
        {
            get => priority;
            set => Set(ref priority, value);
        }

        int executionTime;
        public int ExecutionTime
        {
            get => executionTime;
            set => Set(ref executionTime, value);
        }

        DateTime deadline = DateTime.UtcNow;
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
                new Task()
                {
                    Name = nameTask,
                    Description = descriptionTask,
                    ColorPriority = colorPriority,
                    Priority = priority,
                    ExecutionTime = executionTime,
                    DeadLine = deadline,
                    LoginExecutor = login
                });
            try
            {
                TaskTransferer.TransferToServer(Columns[0].Tasks, idOrganization, idProject, idBoard, Columns[0].Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanAddTaskCommandExecute(object p) 
        { 
            return columns.Count != 0; 
        }
        #endregion

        #region Команда изменения задачи

        public ICommand ChangeTaskCommand { get; }
        private void OnChangeTaskCommandExecuted(object p)
        {
            long idTask = Convert.ToInt32(idTaskBuffer);
            long idColumn = Convert.ToInt32(idColumnBuffer);
            int indexColumn = SearchIndexColumn(idColumn);
            int taskIndex = SearchIndexTask(indexColumn, idTask);

            columns[indexColumn].Tasks[taskIndex].Name = NameTask;
            columns[indexColumn].Tasks[taskIndex].Description = DescriptionTask;
            columns[indexColumn].Tasks[taskIndex].ColorPriority = ColorPriority;
            columns[indexColumn].Tasks[taskIndex].ExecutionTime = ExecutionTime;
            columns[indexColumn].Tasks[taskIndex].DeadLine = Deadline;
            columns[indexColumn].Tasks[taskIndex].LoginExecutor = Login;

            try
            {
                TaskTransferer.TransferToServer(columns[indexColumn].Tasks, IdOrganization, IdProject, IdBoard, idColumn, idTask, taskIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
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
            long idTask = Convert.ToInt64(idTaskBuffer);
            long idColumn = Convert.ToInt64(idColumnBuffer);
            int columnIndex = SearchIndexColumn(idColumn);
            int taskIndex = SearchIndexTask(columnIndex, idTask);

            columns[columnIndex + 1].Tasks.Add(columns[columnIndex].Tasks[taskIndex]);
            columns[columnIndex].Tasks.RemoveAt(taskIndex);

            taskIndex = SearchIndexTask(columnIndex + 1, idTask);
            try
            {
                TaskTransferer.TransferToServer(columns[columnIndex + 1].Tasks, idOrganization, idProject, idBoard, idColumn, columns[columnIndex + 1].Id, idTask, taskIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanMoveTaskToNextColumnExecute(object p) 
        {
            bool result = true;

            if (!int.TryParse(idColumnBuffer, out int idColumn)
                || !int.TryParse(idTaskBuffer, out _)) 
                result = false;
            if (SearchIndexColumn(idColumn) >= columns.Count - 1)
                result = false;

            return result;
        }

        #endregion

        #region Команда перемещения задачи в предыдущую колонку

        public ICommand MoveTaskToPrevColumnCommand { get; }

        private void OnMoveTaskToPrevColumnExecuted(object p)
        {
            long idTask = Convert.ToInt64(idTaskBuffer);
            long idColumn = Convert.ToInt64(idColumnBuffer);
            int columnIndex = SearchIndexColumn(idColumn);
            int taskIndex = SearchIndexTask(columnIndex, idTask);

            columns[columnIndex - 1].Tasks.Add(columns[columnIndex].Tasks[taskIndex]);
            columns[columnIndex].Tasks.RemoveAt(taskIndex);

            taskIndex = SearchIndexTask(columnIndex - 1, idTask);
            try
            {
                TaskTransferer.TransferToServer(columns[columnIndex - 1].Tasks, idOrganization, idProject, idBoard, idColumn, columns[columnIndex - 1].Id, idTask, taskIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanMoveTaskToPrevColumnExecute(object p)
        {
            bool result = true;

            if (!int.TryParse(idColumnBuffer, out int idColumn)
                || !int.TryParse(idTaskBuffer, out _))
                result = false;
            if (SearchIndexColumn(idColumn) <= 0)
                result = false;

            return result;
        }

        #endregion

        #endregion

        #region Команда удаления задачи

        public ICommand DeleteTaskCommand { get; }

        private void OnDeleteTaskCommandExecuted(object p)
        {
            long idTask = Convert.ToInt64(idTaskBuffer);
            long idColumn = Convert.ToInt64(idColumnBuffer);
            int indexColumn = SearchIndexColumn(idColumn);
            int taskIndex = SearchIndexTask(indexColumn, idTask);

            columns[indexColumn].Tasks.RemoveAt(taskIndex);
            TaskTransferer.TransferToServer(IdOrganization, IdProject, IdBoard, idColumn, idTask);
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
            /*Columns.Add(
                new Column()
                {
                    Id = columns.Count,
                    Title = "Backlog",
                    Tasks = new()
                }
            );*/

            AddColumnCommand =
                new RelayCommand(OnAddColumnCommandExecuted, CanAddColumnCommandExecute);

            RenameColumnCommand =
                new RelayCommand(OnRenameColumnCommandExecuted, CanRenameColumnCommandExecute);

            DeleteColumnCommand =
                new RelayCommand(OnDeleteColumnCommandExecuted, CanDeleteColumnCommandExecute);

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

        private int SearchIndexTask(int indexColumn, long idTask)
        {
            int result = 0;

            for (int i = 0; i < columns[indexColumn].Tasks.Count; i++)
                if (columns[indexColumn].Tasks[i].Id == idTask)
                    result = i;

            return result;
        }

        private int SearchIndexColumn(long idColumn)
        {
            int result = 0;

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Id == idColumn)
                    result = i;
            }

            return result;
        }

        #endregion
    }
}