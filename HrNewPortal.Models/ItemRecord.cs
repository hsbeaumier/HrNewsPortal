using System;
using HrNewsPortal.Core.Models;

namespace HrNewsPortal.Models
{
    /// <summary>
    /// ItemRecord represent stories, comments, jobs, polls, ...
    /// from the Azure Table
    /// </summary>
    public class ItemRecord : IBaseFieldsForAzureTableRecord
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
        /// The is the Id field's value.
        /// </remarks>
        public string RowKey { get; set; }

        /// <summary>
        /// Time stamp of the Azure record.
        /// </summary>
        public DateTime Timestamp { get; set; }

        public string ETag { get; set; }

        /// <summary>
        /// The item's unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// True if the item has been deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// The type of item.
        /// </summary>
        /// <remarks>
        /// Examples:
        /// job
        /// story
        /// comment
        /// poll
        /// pollopt
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// The user name of the item's author.
        /// </summary>
        public string By { get; set; }
        
        /// <summary>
        /// UTC date time representation of the item.
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// The comment story or poll text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// True if the item is dead.
        /// </summary>
        public bool Dead { get; set; }

        /// <summary>
        /// The comment's parent:  either
        /// another comment or the relevant story.
        /// </summary>
        public int Parent { get; set; }

        /// <summary>
        /// The pollopt's (poll option's)
        /// associated poll.
        /// </summary>
        public int Poll { get; set; }

        /// <summary>
        /// The identifiers of the item's
        /// comments, in ranked display order.
        /// </summary>
        public string Kids { get; set; }

        /// <summary>
        /// The URL of the story.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The story's score, or the votes
        /// for a pollopt (poll option).
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The title of the story, poll, or job.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A list of related pollopts (Poll Options).
        /// </summary>
        /// <remarks>
        /// Come in display order.
        /// </remarks>
        public string Parts { get; set; }

        /// <summary>
        /// In the case of stories or polls,
        /// the total comment count.
        /// </summary>
        public int Descendants { get; set; }
    }
}
