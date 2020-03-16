using System;
using HrNewsPortal.Core.Extensions;
using Newtonsoft.Json;

namespace HrNewsPortal.Models
{
    /// <summary>
    /// Items represent stories, comments, jobs, polls, ...
    /// from HR News Web API.
    /// </summary>
    [JsonObject]
    public class Item
    {
        /// <summary>
        /// The item's unique identifier.
        /// </summary>
        [JsonProperty(PropertyName="id")]
        public int Id { get; set; }

        /// <summary>
        /// True if the item has been deleted.
        /// </summary>
        [JsonProperty(PropertyName= "deleted")]
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
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// The user name of the item's author.
        /// </summary>
        [JsonProperty(PropertyName = "by")]
        public string By { get; set; }

        /// <summary>
        /// Creation date of the item, in Unix Time. 
        /// </summary>
        /// <remarks>
        /// Do not use this value directly.  Use UtcTime.
        /// </remarks>
        [JsonProperty(PropertyName = "time")]
        public int Time { get; set; }

        /// <summary>
        /// UTC date time representation of the item.
        /// </summary>
        /// <remarks>
        /// Use this value.
        /// </remarks>
        public DateTime? UtcTime => Time.GetDateTime();

        /// <summary>
        /// The comment story or poll text.
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// True if the item is dead.
        /// </summary>
        [JsonProperty(PropertyName = "dead")]
        public bool Dead { get; set; }

        /// <summary>
        /// The comment's parent:  either
        /// another comment or the relevant story.
        /// </summary>
        [JsonProperty(PropertyName = "parent")]
        public int Parent { get; set; }

        /// <summary>
        /// The pollopt's (poll option's)
        /// associated poll.
        /// </summary>
        [JsonProperty(PropertyName = "poll")]
        public int Poll { get; set; }

        /// <summary>
        /// The identifiers of the item's
        /// comments, in ranked display order.
        /// </summary>
        [JsonProperty(PropertyName = "kids")]
        public int[] Kids { get; set; }

        /// <summary>
        /// The URL of the story.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// The story's score, or the votes
        /// for a pollopt (poll option).
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }

        /// <summary>
        /// The title of the story, poll, or job.
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// A list of related pollopts (Poll Options).
        /// </summary>
        /// <remarks>
        /// Come in display order.
        /// </remarks>
        [JsonProperty(PropertyName = "parts")]
        public int[] Parts { get; set; }

        /// <summary>
        /// In the case of stories or polls,
        /// the total comment count.
        /// </summary>
        [JsonProperty(PropertyName = "descendants")]
        public int Descendants { get; set; }
    }
}
