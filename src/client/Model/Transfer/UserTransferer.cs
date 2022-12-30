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
    public static class UserTransferer
    {
        public static async ThreadingTask GetUsersDataFromServer(ObservableCollection<Person> peoples, long idOrganization)
        {
            ICollection<AccountDto>? accountsDto = null;
            try
            {
                accountsDto = await WebClient.Get.MembersAllAsync(idOrganization);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    accountsDto = await WebClient.Get.MembersAllAsync(idOrganization);
                }
            }
            if (accountsDto is null) { throw new NullReferenceException(); }
            
            for (int i = 0; i < accountsDto.Count; i++)
            {
                peoples.Add(
                    new Person()
                    { 
                        Id = accountsDto.ElementAt(i).Id,
                        Name = accountsDto.ElementAt(i).Name
                    });
            }
        }
    }
}
