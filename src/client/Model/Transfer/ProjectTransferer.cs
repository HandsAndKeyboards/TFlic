using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.Model.Transfer
{
    public static class ProjectTransferer
    {
        /// <summary>
        /// Передает файлы с сервера клиенту
        /// Также производятся некоторые преобразования, т.к. 
        /// клиент имеет свое средство представлния данных
        /// </summary>
        /// <param name="projects"> Коллекция проектов организации </param>
        /// <param name="idOrganization"> Идентификатор организации, проекты которой получает клиент </param>
        public static async void TransferToClient(ObservableCollection<Project> projects, long idOrganization)
        {
            ICollection<ProjectGET> projectsDTO = await WebClient.Get.ProjectsAllAsync(idOrganization);

            for (int i = 0; i < projectsDTO.Count; i++)
            {
                ObservableCollection<Board> boardsBuffer = new();
                BoardTransferer.TransferToClient(boardsBuffer, idOrganization, projectsDTO.ElementAt(i).Id);

                projects.Add(
                    new Project()
                    {
                        Id = projectsDTO.ElementAt(i).Id,
                        Name = projectsDTO.ElementAt(i).Name,
                        boards = boardsBuffer
                    });
            }
        }

        public static async void TransferToServer(ObservableCollection<Project> projects, long idOrganization)
        {
            ProjectDTO newProject = new()
            {
                Name = projects[projects.Count - 1].Name
            };
            ProjectGET projectGET = await WebClient.Get.ProjectsPOSTAsync(idOrganization, newProject);
            projects[projects.Count - 1].Id = projectGET.Id;
        }
    }
}
