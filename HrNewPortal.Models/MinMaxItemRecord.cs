using System;
using HrNewsPortal.Core.Models;

namespace HrNewsPortal.Models
{
    /// <summary>
    /// MinMaxItemRecord is the min and max Item identifier (RowKey)
    /// for a Type (PartitionKey).
    /// </summary>
    public class MinMaxItemRecord : IBaseFieldsForAzureTableRecord
    {
        /// <summary>
        /// PartitionKey of the Azure record.
        /// </summary>
        /// <remarks>
        /// The is the Type field's value.
        /// </remarks>
        public string PartitionKey { get; set; }

        /// <summary>
        /// RowKey of the Azure record.
        /// </summary>
        /// <remarks>
        /// This will just be zero.
        /// </remarks>
        public string RowKey { get; set; }

        /// <summary>
        /// Time stamp of the Azure record.
        /// </summary>
        public DateTime Timestamp { get; set; }

        public string ETag { get; set; }

        public int MinItemId { get; set; }

        public int MaxItemId { get; set; }

        public int SnapShotRecordCount { get; set; }
    }
}
