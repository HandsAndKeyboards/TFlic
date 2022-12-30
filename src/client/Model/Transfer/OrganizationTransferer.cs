using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using TFlic.Model.Service;
using TFlic.ViewModel.ViewModelClass;
using AuthenticationManager = TFlic.Model.Authentication.AuthenticationManager;
using ThreadingTask = System.Threading.Tasks.Task;

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
        public static async ThreadingTask GetOrganizationDataFromServer(ObservableCollection<Organization> organizations, long idAccount)
        {
            ICollection<long>? idOrganizations = null;
            try
            {
                idOrganizations = await WebClient.Get.OrganizationsAllAsync(idAccount);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    idOrganizations = await WebClient.Get.OrganizationsAllAsync(idAccount);
                }
            }
            if (idOrganizations is null) { throw new NullReferenceException(); }

            for (int i = 0; i < idOrganizations.Count; i++)
            {
                OrganizationDto? organizationDtoBuffer = null;
                try
                {
                    organizationDtoBuffer = await WebClient.Get.OrganizationsGETAsync(idOrganizations.ElementAt(i)); 
                }
                catch (ApiException err)
                {
                    if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                    {
                        var account = AccountService.ReadAccountFromJsonFile();
                        AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                        organizationDtoBuffer = await WebClient.Get.OrganizationsGETAsync(idOrganizations.ElementAt(i)); 
                    }
                }
                if (organizationDtoBuffer is null) { throw new NullReferenceException(); }

                ObservableCollection<Project> projectsBuffer = new();
                await ProjectTransferer.GetProjectsDataFromServer(projectsBuffer, idOrganizations.ElementAt(i));

                ObservableCollection<Person> usersBuffer = new();
                await UserTransferer.GetUsersDataFromServer(usersBuffer, idOrganizations.ElementAt(i));

                ImmutableSortedSet<Person> userBufferSet 
                    = usersBuffer.ToImmutableSortedSet(Comparer<Person>.Create((x, y) => x.Id.CompareTo(y.Id)));
                usersBuffer = new(userBufferSet);
                
                ICollection<UserGroupDto>? userGroupsBuffer = null;
                try
                {
                    userGroupsBuffer = await WebClient.Get.UserGroupsAsync(idOrganizations.ElementAt(i));
                }
                catch (ApiException err)
                {
                    if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                    {
                        var account = AccountService.ReadAccountFromJsonFile();
                        AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                        userGroupsBuffer = await WebClient.Get.UserGroupsAsync(idOrganizations.ElementAt(i));
                    }
                }
                if (userGroupsBuffer is null) { throw new NullReferenceException(); }
                
                ObservableCollection<UserGroupDto> userGroupsResultBuffer = new();
                for (int j = 0; j < userGroupsBuffer.Count; j++)
                {
                    userGroupsResultBuffer.Add(userGroupsBuffer.ElementAt(j));
                }
                
                organizations.Add(
                    new Organization 
                    {
                        Id = organizationDtoBuffer.Id,
                        Name = organizationDtoBuffer.Name,
                        Description = organizationDtoBuffer.Description,
                        projects = projectsBuffer,
                        peoples = usersBuffer,
                        userGroups = userGroupsResultBuffer
                    });

            }
        }

        public static async ThreadingTask AddOrganizationAndPutDataToServer(ObservableCollection<Organization> organizations)
        {
            var currentAccountId = AccountService.ReadAccountFromJsonFile().Id;
            RegisterOrganizationRequestDto registerOrganizationRequestDto = new()
            {
                Name = organizations.Last().Name,
                Description = organizations.Last().Description,
                CreatorId = (long)currentAccountId
            };
            
            OrganizationDto? organizationDto = null;
            try
            {
                organizationDto = await WebClient.Get.OrganizationsPOSTAsync(registerOrganizationRequestDto);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    organizationDto = await WebClient.Get.OrganizationsPOSTAsync(registerOrganizationRequestDto);
                }
            }
            if (organizationDto is null) { throw new NullReferenceException(); }
            
            organizations.Last().Id = organizationDto.Id;
        }

        public static async ThreadingTask AddUserAndPutDataToServer(ObservableCollection<Organization> organizations, long idOrganization, int indexOrganization, string login)
        {
            AccountDto? accountDto = null;
            try
            {
                accountDto = await WebClient.Get.MembersPOSTAsync(idOrganization, login);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    accountDto = await WebClient.Get.MembersPOSTAsync(idOrganization, login);                
                }
            }
            if (accountDto is null) { throw new NullReferenceException(); }

            organizations[indexOrganization].peoples.Last().Id = accountDto.Id;
            organizations[indexOrganization].peoples.Last().Name = accountDto.Name;
        }

        public static async ThreadingTask ChangeOnfoOrganizationAndPutDataToServer(
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
            
            OrganizationDto? organizationDto = null;
            try
            {
                organizationDto = await WebClient.Get.OrganizationsPATCHAsync(idOrganization, operations);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    organizationDto = await WebClient.Get.OrganizationsPATCHAsync(idOrganization, operations);
                }
            }
            if (organizationDto is null) { throw new NullReferenceException(); }

            organizations[indexOrganization].Name = organizationDto.Name;
            organizations[indexOrganization].Description = organizationDto.Description;
        }

        public static async ThreadingTask DeleteUserAndPutDataToServer(long idOrganization, long idUser)
        {
            try
            {
                await WebClient.Get.MembersDELETEAsync(idOrganization, idUser); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    await WebClient.Get.MembersDELETEAsync(idOrganization, idUser); 
                }
            }
        }

        public static async ThreadingTask AddUserInUserGroupAndPutDataToServer(long idOrganization, int idUserGroup, long idUser)
        {
            try
            {
                await WebClient.Get.MembersPOST2Async(idOrganization, idUserGroup, idUser);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    await WebClient.Get.MembersPOST2Async(idOrganization, idUserGroup, idUser);
                }
            }
        }

        public static async ThreadingTask RemoveUserInUserGroupAndPutDataToServer(long idOrganization, int idUserGroup, long idUser)
        {
            try
            {
                await WebClient.Get.MembersDELETE2Async(idOrganization, idUserGroup, idUser);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    await WebClient.Get.MembersDELETE2Async(idOrganization, idUserGroup, idUser);
                }
            }
        }
    }
}
