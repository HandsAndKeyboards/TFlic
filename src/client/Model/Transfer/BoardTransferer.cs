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
    public static class BoardTransferer
    {
        /// <summary>
        /// Передает файлы с сервера клиенту
        /// Также производятся некоторые преобразования, т.к. 
        /// клиент имеет свое средство представлния данных
        /// </summary>
        /// <param name="boards"> Коллекция досок проекта </param>
        /// <param name="idOrganization"> 
        /// Идентификатор организации, которая содержит необходимый нам проект 
        /// с досками
        /// </param>
        /// <param name="idProject"> Идентификатор проекта, доски которого получает клиент</param>
        public static async ThreadingTask TransferToClient(
            ObservableCollection<Board> boards, 
            long idOrganization, 
            long idProject)
        {
            ICollection<BoardGET>? boardsDTO = null;
            try
            {
                boardsDTO = await WebClient.Get.BoardsAllAsync(idOrganization, idProject); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    boardsDTO = await WebClient.Get.BoardsAllAsync(idOrganization, idProject); 
                }
            }
            if (boardsDTO is null) { throw new NullReferenceException(); }
            
            for (int i = 0; i < boardsDTO.Count; i++)
            {
                ObservableCollection<Column> columnsBuffer = new();
                await ColumnTransferer.TransferToClient(
                    columnsBuffer, idOrganization, idProject, boardsDTO.ElementAt(i).Id);

                boards.Add(
                    new Board
                    {
                        Id = boardsDTO.ElementAt(i).Id,
                        Name = boardsDTO.ElementAt(i).Name,
                        columns = columnsBuffer
                    });
            }
        }

        public static async ThreadingTask TransferToServer(
            ObservableCollection<Board> boards, 
            long idOrganization,
            long idProject)
        {
            BoardDTO newBoard = new()
            {
                Name = boards.ElementAt(boards.Count - 1).Name,
            };
            
            BoardGET? boardGET = null;
            try
            {
                boardGET = await WebClient.Get.BoardsPOSTAsync(idOrganization, idProject, newBoard);; 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    boardGET = await WebClient.Get.BoardsPOSTAsync(idOrganization, idProject, newBoard); 
                }
            }
            if (boardGET is null) { throw new NullReferenceException(); }
            
            boards[boards.Count - 1].Id = boardGET.Id;

            ColumnGET? startColumns = null;
            try
            {
                startColumns = await WebClient.Get.ColumnsGETAsync(idOrganization, idProject, boardGET.Id, boardGET.Columns.First()); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    startColumns = await WebClient.Get.ColumnsGETAsync(idOrganization, idProject, boardGET.Id, boardGET.Columns.First()); 
                }
            }
            if (startColumns is null) { throw new NullReferenceException(); }

            Column backlog = new();
            backlog.Id = startColumns.Id;
            backlog.Title = startColumns.Name;

            boards.Last().columns.Add(backlog);
        }

        public static async ThreadingTask TransferToServer(
            long idOrganization,
            long idProject,
            long idBoard)
        {
            try
            {
                await WebClient.Get.BoardsDELETEAsync(idOrganization, idProject, idBoard); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    await WebClient.Get.BoardsDELETEAsync(idOrganization, idProject, idBoard); 
                }
            }
        }
    }
}
