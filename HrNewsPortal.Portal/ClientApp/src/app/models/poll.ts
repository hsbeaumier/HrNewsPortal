import { PollOption } from "./poll-option";
import { Comment } from "./comment";

export class Poll {
  // The item's unique identifier.
  public id: number;

  // The user name of the item's author.
  public by: string;

  // Creation UTC date of the item.
  public time?: Date;

  // The comment story or poll text.
  public text: string;

  // The identifiers of the item's
  // comments, in ranked display order.
  public kids: number[];

  // The comments, in ranked display order.
  public comments: Comment[];

  // The story's score, or the votes
  // for a pollopt (poll option).
  public score: number;

  // The title of the story, poll, or job.
  public title: string;

  /// A list of related Poll Options ids.
  /// Come in display order.
  public parts: number[];

  // A list of related Poll Options.
  // Come in display order.
  public pollOptions: PollOption[];

  // In the case of stories or polls,
  // the total comment count.
  public descendant: number;
}
