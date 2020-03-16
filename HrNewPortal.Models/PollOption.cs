using System;

namespace HrNewsPortal.Models
{
    public class PollOption
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
        /// The comment story or poll text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The poll option's associated poll identifer.
        /// </summary>
        public int PollId { get; set; }

        /// <summary>
        /// The poll option's associated poll.
        /// </summary>
        public Poll Poll { get; set; }
        
        /// <summary>
        /// The story's score, or the votes
        /// for a poll option.
        /// </summary>
        public int Score { get; set; }
    }
}
