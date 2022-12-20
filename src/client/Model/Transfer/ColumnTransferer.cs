using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TFlic.ViewModel.ViewModelClass;

using ThreadingTask = System.Threading.Tasks.Task;

namespace TFlic.Model.Transfer
{
    static class ColumnTransferer
    {
        /// <summary>
        /// Передает файлы с сервера клиенту
        /// Также производятся некоторые преобразования, т.к. 
        /// клиент имеет свое средство представлния данных
        /// </summary>
        /// <param name="columns"> Коллекция колонок </param>
        /// <param name="idOrganization">  </param>
        /// <param name="idProjects"></param>
        /// <param name="idBoard"></param>
        public static async ThreadingTask TransferToClient(
            ObservableCollection<Column> columns,
            long idOrganization,
            long idProjects,
            long idBoard)
        {
            ICollection<ColumnGET> columnsDTO = 
                await WebClient.Get.ColumnsAllAsync(idOrganization, idProjects, idBoard);

            for (int i = 0; i < columnsDTO.Count; i++)
            {
                ObservableCollection<Task> tasksBuffer = new();
                await TaskTransferer.TransferToClient(
                    tasksBuffer, idOrganization, idProjects, idBoard, columnsDTO.ElementAt(i).Id);

                columns.Add(
                    new Column()
                    {
                        Id = columnsDTO.ElementAt(i).Id,
                        Title = columnsDTO.ElementAt(i).Name,
                        Tasks = tasksBuffer
                    });
            }
        }

        public static async ThreadingTask TransferToServer(
            ObservableCollection<Column> columns,
            long idOrganization,
            long idProjects,
            long idBoard)
        {
            ColumnDTO newColumn = new()
            {
                Name = columns.Last().Title,
                Position = 0,
                LimitOfTask = 0,
            };
            ColumnGET columnGET
                = await WebClient.Get.ColumnsPOSTAsync(idOrganization, idProjects, idBoard, newColumn);
            columns.Last().Id = columnGET.Id;
        }

        public static async ThreadingTask TransferToServer(
            ObservableCollection<Column> columns,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn,
            string newName,
            int indexColumn)
        {
            Operation replaceNameOperation = new()
            {
                Op = "replace",
                Value = newName,
                Path = "/Name"
            };

            ColumnGET columnGET = 
                await WebClient.Get.ColumnsPATCHAsync(
                    idOrganization, 
                    idProjects, 
                    idBoard, 
                    idColumn, 
                    new List<Operation>() { replaceNameOperation });

            columns[indexColumn].Title = newName;
        }

        public static async ThreadingTask TransferToServer(
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn)
        {
            await WebClient.Get.ColumnsDELETEAsync(idOrganization, idProjects, idBoard, idColumn);
        }
    }
}
