using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using TFlic.ViewModel.ViewModelClass;

using ThreadingTask = System.Threading.Tasks.Task;

namespace TFlic.Model.Transfer
{
    public static class TaskTransferer
    {
        public static async ThreadingTask TransferToClient(
            ObservableCollection<Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn)
        {
            ICollection<TaskGET> tasksDTO =
                await WebClient.Get.TasksAllAsync(idOrganization, idProjects, idBoard, idColumn);

            long taskPriority;
            Brush colorPriority;

            for (int i = 0; i < tasksDTO.Count; i++)
            {
                taskPriority = tasksDTO.ElementAt(i).Priority;

                colorPriority = taskPriority switch
                {
                    1 => new SolidColorBrush(Color.FromRgb(130, 255, 130)),
                    2 => new SolidColorBrush(Color.FromRgb(210, 255, 230)),
                    3 => new SolidColorBrush(Color.FromRgb(255, 255, 130)),
                    4 => new SolidColorBrush(Color.FromRgb(255, 200, 130)),
                    5 => new SolidColorBrush(Color.FromRgb(255, 150, 130)),
                    _ => new SolidColorBrush(Color.FromRgb(130, 255, 130)),
                };

                AccountDto accDto = await WebClient.Get.AnonymousGET2Async(tasksDTO.ElementAt(i).Id_executor);

                tasks.Add(
                    new Task()
                    {
                        Id = tasksDTO.ElementAt(i).Id,
                        Name = tasksDTO.ElementAt(i).Name,
                        Description = tasksDTO.ElementAt(i).Description,
                        IdColumn = tasksDTO.ElementAt(i).IdColumn,
                        ColorPriority = colorPriority,
                        Priority = taskPriority,
                        ExecutionTime = tasksDTO.ElementAt(i).EstimatedTime,
                        DeadLine = tasksDTO.ElementAt(i).Deadline,
                        IdExecutor = tasksDTO.ElementAt(i).Id_executor,
                        NameExecutor = accDto.Name,
                        LoginExecutor = accDto.Login
                    });
            }
        }

        public static async ThreadingTask TransferToServer(
            ObservableCollection<Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn)
        {
            AccountDto accountDto = await WebClient.Get.AnonymousGETAsync(tasks.Last().LoginExecutor);

            TaskDTO taskDTO = new()
            { 
                Position = 0,
                Name = tasks.Last().Name,
                Description = tasks.Last().Description,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Priority = (int)tasks.Last().Priority,
                EstimatedTime = tasks.Last().ExecutionTime,
                Status = string.Empty,
                Id_executor = accountDto.Id,
                Deadline = tasks.Last().DeadLine.ToUniversalTime()
            };
            TaskGET taskGET = await WebClient.Get.TasksPOSTAsync(idOrganization, idProjects, idBoard, idColumn, taskDTO);
            tasks.Last().Id = taskGET.Id;
            tasks.Last().NameExecutor = accountDto.Name;
            tasks.Last().IdColumn = taskGET.IdColumn;

        }

        public static async ThreadingTask TransferToServer(
            ObservableCollection<Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn,
            long idNewColumn,
            long idTask,
            int indexTasks)
        {
            Operation operation = new()
            {
                Op = "replace",
                Value = idNewColumn.ToString(),
                Path = "/ColumnId"
            };

            TaskGET taskGET = await WebClient.Get.TasksPATCHAsync(idOrganization, idProjects, idBoard, idColumn, idTask, new List<Operation> { operation });
            tasks[indexTasks].IdColumn = taskGET.IdColumn;
        }

        public static async ThreadingTask TransferToServer(
            ObservableCollection<Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn,
            long idTask,
            int indexTask)
        {
            AccountDto accountDto = await WebClient.Get.AnonymousGETAsync(tasks[indexTask].LoginExecutor);

            Operation replaceNameOperation = new()
            {
                Op = "replace",
                Value = tasks[indexTask].Name,
                Path = "/Name"
            };
            Operation replaceDescriptionOperation = new()
            {
                Op = "replace",
                Value = tasks[indexTask].Description,
                Path = "/Description"
            };
            Operation replacePriorityOperation = new()
            {
                Op = "replace",
                Value = tasks[indexTask].Priority.ToString(),
                Path = "/Priority"
            };
            Operation replaceExecutionTimeOperation = new()
            {
                Op = "replace",
                Value = tasks[indexTask].ExecutionTime.ToString(),
                Path = "/EstimatedTime"
            };
            
            // Принимаемый тип даты 2022-12-18T13:36:22.141Z
            DateTime dt = tasks[indexTask].DeadLine;
            string deadlineString = $"{dt.Year}-{dt.Month}-{dt.Day}T00:00:00.000Z";

            Operation replaceDeadlineOperation = new()
            {
                Op = "replace",
                Value = deadlineString,
                Path = "/Deadline"
            };
            Operation replaceExecutorOperation = new()
            {
                Op = "replace",
                Value = accountDto.Id.ToString(),
                Path = "/ExecutorId"
            };

            List<Operation> operations = new()
            {
                replaceNameOperation,
                replaceDescriptionOperation,
                replacePriorityOperation,
                replaceExecutionTimeOperation,
                replaceDeadlineOperation,
                replaceExecutorOperation
            };

            TaskGET taskGET = await WebClient.Get.TasksPATCHAsync(idOrganization, idProjects, idBoard, idColumn, idTask, operations);

            tasks[indexTask].NameExecutor = accountDto.Name;
        }

        public static async ThreadingTask TransferToServer(
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn,
            long idTask)
        {
            await WebClient.Get.TasksDELETEAsync(idOrganization, idProjects, idBoard, idColumn, idTask);
        }
    }
}
