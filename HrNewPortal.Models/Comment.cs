using System;

namespace HrNewsPortal.Models
{
    public class Comment
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
        public DateTime? Time  { get; set; }

        /// <summary>
        /// The comment story or poll text.
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// The comment's parent which is relevant to the story.
        /// </summary>
        public Story ParentStory { get; set; }

        /// <summary>
        /// The comment's parent which is another comment
        /// </summary>
        public Comment ParentComment { get; set; }

        /// <summary>
        /// The comments, in ranked display order.
        /// </summary>
        public Comment[] Kids { get; set; }
    }
}
