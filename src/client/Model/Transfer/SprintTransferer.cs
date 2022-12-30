using System;
using System.Collections.Generic;
using System.Net;
using TFlic.Model.Service;
using AuthenticationManager = TFlic.Model.Authentication.AuthenticationManager;
using ThreadingTask = System.Threading.Tasks.Task;

namespace TFlic.Model.Transfer
{
    public static class SprintTransferer
    {
        /*
         Добавить в апи запрос по конкретному спринту 
        */
        public static async ThreadingTask TransferToClient(ICollection<Sprint> sprints, long IdOrganization, long idProject)
        {
            ICollection<Sprint>? sprintsDto = null;
            try
            {
                sprintsDto = await WebClient.Get.SprintsAsync(IdOrganization, idProject);            
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    sprintsDto = await WebClient.Get.SprintsAsync(IdOrganization, idProject);                
                }
            }
            if (sprintsDto is null) { throw new NullReferenceException(); }

            foreach(Sprint sprint in sprintsDto)
            {
                sprints.Add(sprint);
            }
        }
    }
}
