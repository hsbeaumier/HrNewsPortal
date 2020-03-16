using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace HrNewsPortal.Core.TableStorage
{
    public static class CloudStorageAccountExtensions
    {
        /// <summary>
        /// Parses connectionstring using standard connectionstring settings or custom SAS string: account-name@endpoint-suffix?sas-token
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static CloudStorageAccount Parse(string connectionString)
        {
            CloudStorageAccount storageAccount;

            if (connectionString.Contains("?"))
            {
                //SAS

                if (connectionString.StartsWith("?"))
                    throw new InvalidOperationException(
                        "SAS connectionstring must be in format account-name@endpoint-suffix?sas-token");

                var additionalInfo = connectionString.Split('?')[0];

                string account;
                string suffix = null;

                if (additionalInfo.Contains("@"))
                {
                    var accountItems = additionalInfo.Split('@');

                    account = accountItems[0];
                    suffix = accountItems[1];
                }
                else
                {
                    account = additionalInfo;
                }

                connectionString = connectionString.Substring(connectionString.IndexOf('?'));

                // ReSharper disable once InconsistentNaming
                var accountSAS = new StorageCredentials(connectionString);
                storageAccount = new CloudStorageAccount(accountSAS, account, suffix, true);
            }
            else
            {
                storageAccount = CloudStorageAccount.Parse(connectionString);
            }

            return storageAccount;
        }
    }
}
