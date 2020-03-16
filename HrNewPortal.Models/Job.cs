using System;

namespace HrNewsPortal.Models
{
    public class Job
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
    }
}
