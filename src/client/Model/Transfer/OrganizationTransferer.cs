using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.Model;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;
using TFlic.Model.Transfer;

namespace TFlic.Model.Transfer
{
    public static class OrganizationTransferer
    {
        /// <summary>
        /// Передает файлы с сервера клиенту
        /// Также производятся некоторые преобразования, т.к. 
        /// клиент имеет свое средство представлния данных
        /// </summary>
        /// <param name="organizations"> Коллекция организаций пользователя </param>
        /// <param name="idAccount"> Идентификатор пользователя, проекты которого получает клиент </param>
        public static async void TransferToClient(ObservableCollection<Organization> organizations, long idAccount)
        {
            ICollection<long> idOrganizations = await WebClient.Get.OrganizationsAllAsync(idAccount);
            OrganizationDto organizationDtoBuffer;

            for (int i = 0; i < idOrganizations.Count; i++)
            {
                organizationDtoBuffer = await WebClient.Get.OrganizationsGETAsync(idOrganizations.ElementAt(i));
                ObservableCollection<Project> projectsBuffer = new();
                ProjectTransferer.TransferToClient(projectsBuffer, idOrganizations.ElementAt(i));

                organizations.Add(
                    new Organization 
                    {
                        Id = organizationDtoBuffer.Id,
                        Name = organizationDtoBuffer.Name,
                        Description = organizationDtoBuffer.Description,
                        projects = projectsBuffer
                    });
            }
        }

        public static async void TransferToServer(ObservableCollection<Organization> organizations)
        {

        }
    }
}
