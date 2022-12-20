using System.Collections.Generic;

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
            ICollection<Sprint> sprintsDto = await WebClient.Get.SprintsAsync(IdOrganization, idProject);

            foreach(Sprint sprint in sprintsDto)
            {
                sprints.Add(sprint);
            }
        }
    }
}
