using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using TFlic.Model.Service;
using TFlic.ViewModel.ViewModelClass;
using AuthenticationManager = TFlic.Model.Authentication.AuthenticationManager;
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
            ICollection<ColumnGET>? columnsDTO = null;
            try
            {
                columnsDTO = await WebClient.Get.ColumnsAllAsync(idOrganization, idProjects, idBoard); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    columnsDTO = await WebClient.Get.ColumnsAllAsync(idOrganization, idProjects, idBoard); 
                }
            }
            if (columnsDTO is null) { throw new NullReferenceException(); }

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
            
            ColumnGET? columnGET = null;
            try
            {
                columnGET = await WebClient.Get.ColumnsPOSTAsync(idOrganization, idProjects, idBoard, newColumn); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    columnGET = await WebClient.Get.ColumnsPOSTAsync(idOrganization, idProjects, idBoard, newColumn); 
                }
            }
            if (columnGET is null) { throw new NullReferenceException(); }
            
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
            
            var requestBody = new List<Operation>() {replaceNameOperation}; 
            try
            {
                await WebClient.Get.ColumnsPATCHAsync(idOrganization, idProjects, idBoard, idColumn, requestBody); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    await WebClient.Get.ColumnsPATCHAsync(idOrganization, idProjects, idBoard, idColumn, requestBody); 
                }
            }

            columns[indexColumn].Title = newName;
        }

        public static async ThreadingTask TransferToServer(
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn)
        {
            try
            {
                await WebClient.Get.ColumnsDELETEAsync(idOrganization, idProjects, idBoard, idColumn); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    await WebClient.Get.ColumnsDELETEAsync(idOrganization, idProjects, idBoard, idColumn); 
                }
            }
        }
    }
}
