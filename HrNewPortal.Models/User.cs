using System;

namespace HrNewsPortal.Models
{
    public class User
    {
        /// <summary>
        /// The user's unique username.
        /// </summary>
        /// <remarks>
        /// Case-sensitive.  Required.
        /// </remarks>
        public string Id { get; set; }

        /// <summary>
        /// Delay in minutes between a comment's
        /// creation and its visibility to other users.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Creation UTC date of the user.
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// The user's karma.
        /// </summary>
        public int Karma { get; set; }

        /// <summary>
        /// The user's optional self-description.  HTML. 
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// List of the user's stories, polls, and comments.
        /// </summary>
        public Item[] Submitted { get; set; }
    }
}
