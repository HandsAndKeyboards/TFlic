using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClass;

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
        public static async void TransferToClient(
            ObservableCollection<Board> boards, 
            long idOrganization, 
            long idProject)
        {
            ICollection<BoardGET> boardsDTO = await WebClient.Get.BoardsAllAsync(idOrganization, idProject);

            for (int i = 0; i < boardsDTO.Count; i++)
            {
                ObservableCollection<Column> columnsBuffer = new();
                ColumnTransferer.TransferToClient(
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

        public static async void TransferToServer(
            ObservableCollection<Board> boards, 
            long idOrganization,
            long idProject)
        {
            BoardDTO newBoard = new()
            {
                Name = boards.ElementAt(boards.Count - 1).Name,
            };
            BoardGET boardGET = await WebClient.Get.BoardsPOSTAsync(idOrganization, idProject, newBoard);
            boards[boards.Count - 1].Id = boardGET.Id;

            ColumnGET startColumns 
                = await WebClient.Get.ColumnsGETAsync(idOrganization, idProject, boardGET.Id, boardGET.Columns.First());

            Column backlog = new();
            backlog.Id = startColumns.Id;
            backlog.Title = startColumns.Name;

            boards.Last().columns.Add(backlog);
        }
    }
}
