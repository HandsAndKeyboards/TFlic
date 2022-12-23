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
    public static class ProjectTransferer
    {
        /// <summary>
        /// Передает файлы с сервера клиенту
        /// Также производятся некоторые преобразования, т.к. 
        /// клиент имеет свое средство представлния данных
        /// </summary>
        /// <param name="projects"> Коллекция проектов организации </param>
        /// <param name="idOrganization"> Идентификатор организации, проекты которой получает клиент </param>
        public static async ThreadingTask TransferToClient(ObservableCollection<Project> projects, long idOrganization)
        {
            ICollection<ProjectGET>? projectsDTO = null;
            try
            {
                projectsDTO = await WebClient.Get.ProjectsAllAsync(idOrganization);           
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    projectsDTO = await WebClient.Get.ProjectsAllAsync(idOrganization);                
                }
            }
            if (projectsDTO is null) { throw new NullReferenceException(); }

            for (int i = 0; i < projectsDTO.Count; i++)
            {
                ObservableCollection<Board> boardsBuffer = new();
                await BoardTransferer.TransferToClient(boardsBuffer, idOrganization, projectsDTO.ElementAt(i).Id);

                projects.Add(
                    new Project()
                    {
                        Id = projectsDTO.ElementAt(i).Id,
                        Name = projectsDTO.ElementAt(i).Name,
                        boards = boardsBuffer
                    });
            }
        }

        public static async ThreadingTask TransferToServer(ObservableCollection<Project> projects, long idOrganization)
        {
            ProjectDTO newProject = new()
            {
                Name = projects[projects.Count - 1].Name
            };
            
            ProjectGET? projectGET = null;
            try
            {
                projectGET = await WebClient.Get.ProjectsPOSTAsync(idOrganization, newProject);           
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    projectGET = await WebClient.Get.ProjectsPOSTAsync(idOrganization, newProject);                
                }
            }
            if (projectGET is null) { throw new NullReferenceException(); }
            
            projects[projects.Count - 1].Id = projectGET.Id;
        }

        public static async ThreadingTask TransferToServer(long idOrganization, long idProject)
        {
            try
            {
                await WebClient.Get.ProjectsDELETEAsync(idOrganization, idProject); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    await WebClient.Get.ProjectsDELETEAsync(idOrganization, idProject); 
                }
            }
        }
    }
}
