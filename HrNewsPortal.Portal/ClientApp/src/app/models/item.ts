export class Item {
  // The item's unique identifier.
  public id: number;

  // True if the item has been deleted.
  public deleted: boolean;

  // The type of item.
  // Examples:
  // job
  // story
  // comment
  // poll
  // pollopt
  public type: string;

  // The user name of the item's author.
  public by: string;
  
  // Creation date of the item, in Unix Time. 
  // Do not use this value directly.  Use UtcTime.
  public time: number;

  // UTC date time representation of the item.
  // Use this value.
  public utcTime?: Date;

  // The comment story or poll text.
  public text: string;

  // True if the item is dead.
  public dead: boolean;

  // The comment's parent:  either
  // another comment or the relevant story.
  public parent: number;

  // The pollopt's (poll option's)
  // associated poll.
  public poll: number;

  // The identifiers of the item's
  // comments, in ranked display order.
  public kids: number[];

  // The URL of the story.
  public url: string;

  // The story's score, or the votes
  // for a pollopt (poll option).
  public score: number;

  // The title of the story, poll, or job.
  public title: string;

  // A list of related pollopts (Poll Options).
  // Come in display order.
  public parts: number[];
  
  // In the case of stories or polls,
  // the total comment count.
  public descendants: number;
}
