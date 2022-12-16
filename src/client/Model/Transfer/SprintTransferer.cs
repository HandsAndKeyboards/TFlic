using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.Model.Transfer
{
    public static class SprintTransferer
    {
        /*
         Добавить в апи запрос по конкретному спринту 
        */
        public static async System.Threading.Tasks.Task TransferToClient(ICollection<Model.Sprint> sprints, long IdOrganization, long idProject)
        {
            ICollection<Model.Sprint> sprintsDto = await WebClient.Get.SprintsAsync(IdOrganization, idProject);

            foreach(Sprint sprint in sprintsDto)
            {
                sprints.Add(sprint);
            }
        }
    }
}
