using System;
using HrNewsPortal.Models;
using Microsoft.Extensions.Configuration;

namespace HrNewsPortal.Services.Builders
{
    public static class HrNewsClientSettingsBuilder
    {
        #region methods

        public static HrNewsClientSettings Build(IConfiguration configuration)
        {
            var hrSettings = new HrNewsClientSettings
            {
                WebApi = configuration.GetSection("HrNewClientSettings")["WebApi"],
                WebApiVersion = configuration.GetSection("HrNewClientSettings")["WebApiVersion"],
                MaxTakeItem = Convert.ToInt32(configuration.GetSection("HrNewClientSettings")["MaxTakeItem"])
            };

            return hrSettings;
        }

        #endregion
    }
}
