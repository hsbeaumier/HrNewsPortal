using System;

namespace HrNewsPortal.Core.Models
{
    public interface IBaseFieldsForAzureTableRecord
    {
        string PartitionKey { get; set; }

        string RowKey { get; set; }

        DateTime Timestamp { get; set; }

        string ETag { get; set; }
    }
}
