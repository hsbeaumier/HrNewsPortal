using System;

namespace HrNewsPortal.Models
{
    public class Poll
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
        /// The item's comments, in ranked display order.
        /// </summary>
        public Comment[] Kids { get; set; }

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
        /// A list of related Poll Options.
        /// </summary>
        /// <remarks>
        /// Come in display order.
        /// </remarks>
        public PollOption[] Parts { get; set; }

        /// <summary>
        /// In the case of stories or polls,
        /// the total comment count.
        /// </summary>
        public int Descendant { get; set; }
    }
}
