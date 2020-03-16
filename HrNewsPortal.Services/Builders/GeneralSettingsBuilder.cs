using HrNewsPortal.Models;
using Microsoft.Extensions.Configuration;

namespace HrNewsPortal.Services.Builders
{
    public static class GeneralSettingsBuilder
    {
        #region methods

        public static GeneralSettings Build(IConfiguration configuration)
        {
            var settings = new GeneralSettings
            {
                AzureStorageConnectionString = configuration.GetSection("GeneralSettings")["AzureStorageConnectionString"],
            };

            return settings;
        }

        #endregion
    }
}
