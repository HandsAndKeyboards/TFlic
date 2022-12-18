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
using TFlic.Model.Service;

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

                ObservableCollection<Person> usersBuffer = new();
                UserTransferer.TransferToClient(usersBuffer, idOrganizations.ElementAt(i));

                organizations.Add(
                    new Organization 
                    {
                        Id = organizationDtoBuffer.Id,
                        Name = organizationDtoBuffer.Name,
                        Description = organizationDtoBuffer.Description,
                        projects = projectsBuffer,
                        peoples = usersBuffer
                    });

            }
        }

        public static async void TransferToServer(ObservableCollection<Organization> organizations)
        {
            var currentAccountId = AccountService.ReadAccountFromJsonFile().Id;
            RegisterOrganizationRequestDto registerOrganizationRequestDto = new()
            {
                Name = organizations.Last().Name,
                Description = organizations.Last().Description,
                CreatorId = (long)currentAccountId
            };
            OrganizationDto organizationDto = await WebClient.Get.OrganizationsPOSTAsync(registerOrganizationRequestDto);
            organizations.Last().Id = organizationDto.Id;
        }

        public static async void TransferToServer(ObservableCollection<Organization> organizations, long idOrganization, int indexOrganization, string login)
        {
            AccountDto accountDto = await WebClient.Get.MembersPOSTAsync(idOrganization, login);

            organizations[indexOrganization].peoples.Last().Id = accountDto.Id;
            organizations[indexOrganization].peoples.Last().Name = accountDto.Name;
        }

        public static async void TransferToServer(
            ObservableCollection<Organization> organizations, 
            long idOrganization,
            int indexOrganization, 
            string newName,
            string newDescription)
        {
            Operation replaceNameOperation = new();
            replaceNameOperation.Op = "replace";
            replaceNameOperation.Value = newName;
            replaceNameOperation.Path = "/Name";

            Operation replaceDescriptionOperation = new();
            replaceDescriptionOperation.Op = "replace";
            replaceDescriptionOperation.Value = newDescription;
            replaceDescriptionOperation.Path = "/Description";

            List<Operation> operations = new()
            { 
                replaceNameOperation,
                replaceDescriptionOperation
            };

            OrganizationDto organizationDto = await WebClient.Get.OrganizationsPATCHAsync(idOrganization, operations);

            organizations[indexOrganization].Name = organizationDto.Name;
            organizations[indexOrganization].Description = organizationDto.Description;
        }

        public static async void TransferToServer(long idOrganization, long idUser)
        {
            await WebClient.Get.MembersDELETEAsync(idOrganization, idUser);
        }
    }
}
