using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MixologyJournalApp.Droid.Security
{
    public class SecureStorageAccountStore
    {
        public const String GoogleServiceId = "Google";
        public async Task SaveAsync(MobileServiceUser account, string serviceId)
        {
            // Find existing accounts for the service
            List<MobileServiceUser> accounts = await FindAccountsForServiceAsync(serviceId);

            // Remove existing account with Id if exists
            accounts.RemoveAll(a => a.UserId == account.UserId);

            // Add account we are saving
            accounts.Add(account);

            // Serialize all the accounts to javascript
            var json = JsonConvert.SerializeObject(accounts);

            // Securely save the accounts for the given service
            await SecureStorage.SetAsync(serviceId, json);
        }

        public void Delete(string serviceId)
        {
            SecureStorage.Remove(serviceId);
        }

        public async Task<List<MobileServiceUser>> FindAccountsForServiceAsync(string serviceId)
        {
            // Get the json for accounts for the service
            var json = await SecureStorage.GetAsync(serviceId);

            try
            {
                // Try to return deserialized list of accounts
                return JsonConvert.DeserializeObject<List<MobileServiceUser>>(json);
            }
            catch { }

            // If this fails, return an empty list
            return new List<MobileServiceUser>();
        }
    }
}