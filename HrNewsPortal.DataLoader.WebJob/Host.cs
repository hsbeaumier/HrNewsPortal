using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Fluent;
using HrNewsPortal.Data.Repositories;
using HrNewsPortal.Models;
using HrNewsPortal.Services;
using HrNewsPortal.Services.Builders;

namespace HrNewsPortal.DataLoader.WebJob
{
    /// <summary>
    /// Host for DataLoader Web Job.
    /// Implements the <see cref="Microsoft.Extensions.Hosting.IHostedService" />
    /// </summary>
    /// <seealso cref="IHostedService" />
    public class Host : IHostedService
    {

        #region fields

        private readonly IHrNewsWebApiService _service;
        private readonly IHrNewsRepository _repo;
        private readonly HrNewsClientSettings _settings;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Host"/> class.
        /// </summary>
        /// <param name="service">
        /// HrNewsWebApiService used to retrieve HR news web API.
        /// </param>
        /// <param name="repo">
        /// HrNewsRepository used to store data into the Azure Table.
        /// </param>
        /// <param name="settings">
        /// HrNewsClientSettings used to supply limit of items to
        /// process per cycle.
        /// </param>
        public Host(IHrNewsWebApiService service, IHrNewsRepository repo, HrNewsClientSettings settings)
        {
            _service = service;
            _repo = repo;
            _settings = settings;
        }

        #endregion

        #region IHostedService

        /// <summary>
        /// Start the Host as an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>Task.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info().Message("Starting WebJob via StartAsync").Write();

            while (true)
            {
                try
                {
                    var startItemId = _repo.GetMaxItemId() + 1;
                    var maxTakeItem = _settings.MaxTakeItem;
                    var endItemId = await _service.GetMaxItemId();

                    var takeItem = Math.Min((startItemId + maxTakeItem), endItemId);
                    
                    var newItems = await _service.GetItems(startItemId, takeItem);

                    _repo.InsertItemRecords(newItems);
                }
                catch (Exception e)
                {
                    Log.Error().Exception(e).Message("Failed to load data from Hr News Web Api.").Write();
                }

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        /// <summary>
        /// Shut down the Host as an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>Task.</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info().Message("Shutting down WebJob after signal").Write();

            try
            {
                // TODO - Any closing tasks that are needed upon shutdown.
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        #endregion
    }
}
