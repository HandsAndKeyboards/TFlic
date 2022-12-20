using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TFlic.ViewModel.ViewModelClass;

using ThreadingTask = System.Threading.Tasks.Task;

namespace TFlic.Model.Transfer
{
    public static class UserTransferer
    {
        public static async ThreadingTask TransferToClient(ObservableCollection<Person> peoples, long idOrganization)
        {
            ICollection<AccountDto> accountsDto = await WebClient.Get.MembersAllAsync(idOrganization);

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

        public static async ThreadingTask TransferToServer(ObservableCollection<Person> peoples, long idOrganization)
        {

        }
    }
}
