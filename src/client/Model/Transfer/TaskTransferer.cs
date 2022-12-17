using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.Model.Transfer
{
    public static class TaskTransferer
    {
        public static async void TransferToClient(
            ObservableCollection<ViewModel.ViewModelClass.Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn)
        {
            ICollection<TaskGET> tasksDTO =
                await WebClient.Get.TasksAllAsync(idOrganization, idProjects, idBoard, idColumn);

            long taskPriority;
            System.Windows.Media.Brush colorPriority;

            for (int i = 0; i < tasksDTO.Count; i++)
            {
                taskPriority = tasksDTO.ElementAt(i).Priority;

                colorPriority = taskPriority switch
                {
                    1 => System.Windows.Media.Brushes.Green,
                    2 => System.Windows.Media.Brushes.Yellow,
                    3 => System.Windows.Media.Brushes.Orange,
                    4 => System.Windows.Media.Brushes.OrangeRed,
                    5 => System.Windows.Media.Brushes.Red,
                    _ => System.Windows.Media.Brushes.Green,
                };

                AccountDto accDto = await WebClient.Get.AccountsGETAsync(tasksDTO.ElementAt(i).Id_executor);

                tasks.Add(
                    new ViewModel.ViewModelClass.Task()
                    {
                        Id = tasksDTO.ElementAt(i).Id,
                        Name = tasksDTO.ElementAt(i).Name,
                        Description = tasksDTO.ElementAt(i).Description,
                        IdColumn = tasksDTO.ElementAt(i).IdColumn,
                        ColorPriority = colorPriority,
                        DeadLine = tasksDTO.ElementAt(i).Deadline,
                        NameExecutor = accDto.Name
                    }); ;
            }
        }

        public static async void TransferToServer(
            ObservableCollection<ViewModel.ViewModelClass.Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn)
        {
            TaskDTO taskDTO = new()
            { 
                Position = 0,
                Name = tasks[tasks.Count - 1].Name,
                Description = tasks[tasks.Count - 1].Description,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Status = string.Empty,
                Id_executor = 2,
                Deadline= DateTime.Now.ToUniversalTime()
            };
            TaskGET taskGET = await WebClient.Get.TasksPOSTAsync(idOrganization, idProjects, idBoard, idColumn, taskDTO);
            tasks[tasks.Count - 1].Id = taskGET.Id;
        }

        public static async void TransferToServer(
            ObservableCollection<ViewModel.ViewModelClass.Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn,
            long idNewColumn,
            long idTask,
            int indexTasks)
        {
            Operation operation = new Operation();
            operation.Op = "replace";
            operation.Value = idNewColumn.ToString();
            operation.Path = "/ColumnId";

            TaskGET taskGET = await WebClient.Get.TasksPATCHAsync(idOrganization, idProjects, idBoard, idColumn, idTask, new List<Operation> { operation });
            tasks[indexTasks].IdColumn = taskGET.IdColumn;
        }
    }
}
