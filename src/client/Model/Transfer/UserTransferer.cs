using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.Model.Transfer
{
    public static class UserTransferer
    {
        public static async void TransferToClient(ObservableCollection<Person> peoples, long idOrganization)
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

        public static async void TransferToServer(ObservableCollection<Person> peoples, long idOrganization)
        {

        }
    }
}
