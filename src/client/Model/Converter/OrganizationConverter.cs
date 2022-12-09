using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.Model;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.Model.Converter
{
    public static class OrganizationConverter
    {
        public static async void Convert(ObservableCollection<Organization> organizations)
        {
            AccountDto currentAccount = await WebClient.Get.AccountsGETAsync(2);
            OrganizationDto organizationDtoBuffer;

            for (int i = 0; i < currentAccount.UserGroups.Count; i++)
            {
                organizationDtoBuffer = await WebClient.Get.OrganizationsGETAsync(currentAccount.UserGroups.ElementAt(i));

                organizations.Add(
                    new Organization 
                    {
                        Id = organizationDtoBuffer.Id,
                        Name = organizationDtoBuffer.Name,
                        Description = organizationDtoBuffer.Description
                    });
            }
        }
        
    }
}
