using System;

namespace HrNewsPortal.Models
{
    public class Story
    {
        /// <summary>
        /// The item's unique identifier.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The user name of the item's author.
        /// </summary>
        public string By { get; set; }

        /// <summary>
        /// Creation UTC date of the item.
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// The identifiers of the item's
        /// comments, in ranked display order.
        /// </summary>
        public int[] Kids { get; set; }

        /// <summary>
        /// The comments, in ranked display order.
        /// </summary>
        public Comment[] Comments { get; set; }

        /// <summary>
        /// The URL of the story.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The story's score, or the votes
        /// for a pollopt (poll option).
        /// </summary>
        public int Score { get; set; }
    }
}
