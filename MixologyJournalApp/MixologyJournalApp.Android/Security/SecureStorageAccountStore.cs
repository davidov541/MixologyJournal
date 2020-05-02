using Newtonsoft.Json;
using Plugin.GoogleClient.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MixologyJournalApp.Droid.Security
{
    public class SecureStorageAccountStore
    {
        public const String GoogleServiceId = "Google";
        public const String HasBeenSetUpId = "HasBeenSetUp";
        public async Task SaveCredentialsAsync(GoogleUser account, string serviceId)
        {
            // Find existing accounts for the service
            List<GoogleUser> accounts = await FindAccountsForServiceAsync(serviceId);

            // Remove existing account with Id if exists
            accounts.RemoveAll(a => a.Id == account.Id);

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

        public async Task<List<GoogleUser>> FindAccountsForServiceAsync(string serviceId)
        {
            // Get the json for accounts for the service
            String json = await SecureStorage.GetAsync(serviceId);

            try
            {
                // Try to return deserialized list of accounts
                return JsonConvert.DeserializeObject<List<GoogleUser>>(json);
            }
            catch { }

            // If this fails, return an empty list
            return new List<GoogleUser>();
        }

        public async Task SaveDataAsync(String data, string id)
        {
            await SecureStorage.SetAsync(id, data);
        }

        public async Task<String> LoadDataAsync(String id)
        {
            return await SecureStorage.GetAsync(id);
        }
    }
}