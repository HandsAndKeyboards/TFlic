using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClass;

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
        public static async void TransferToClient(
            ObservableCollection<Column> columns,
            long idOrganization,
            long idProjects,
            long idBoard)
        {
            ICollection<ColumnGET> columnsDTO = 
                await WebClient.Get.ColumnsAllAsync(idOrganization, idProjects, idBoard);

            for (int i = 0; i < columnsDTO.Count; i++)
            {
                ObservableCollection<ViewModel.ViewModelClass.Task> tasksBuffer = new();
                TaskTransferer.TransferToClient(
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

        public static async void TransferToServer(
            ObservableCollection<Column> columns,
            long idOrganization,
            long idProjects,
            long idBoard)
        {
            ColumnDTO newColumn = new()
            {
                Name = columns[columns.Count - 1].Title,
                Position = 0,
                LimitOfTask = 0,
            };
            ColumnGET columnGET
                = await WebClient.Get.ColumnsPOSTAsync(idOrganization, idProjects, idBoard, newColumn);
            columns[columns.Count - 1].Id = columnGET.Id;
        }
    }
}
