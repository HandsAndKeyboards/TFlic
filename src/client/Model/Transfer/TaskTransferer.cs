using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.Model.Transfer
{
    public static class TaskTransferer
    {
        public static async void TransferToClient(
            ObservableCollection<ViewModel.ViewModelClass.Task> tasks,
            long idOrganization,
            long idProjects,
            long idBoard,
            long idColumn)
        {
            ICollection<TaskGET> tasksDTO =
                await WebClient.Get.TasksAllAsync(idOrganization, idProjects, idBoard, idColumn);

            for (int i = 0; i < tasksDTO.Count; i++)
            {
                tasks.Add(
                    new ViewModel.ViewModelClass.Task()
                    {
                        Id = tasksDTO.ElementAt(i).Id,
                        Name = tasksDTO.ElementAt(i).Name,
                        Description = tasksDTO.ElementAt(i).Description
                    });
            }
        }

        public static async void TransferToClient()
        {

        }
    }
}
