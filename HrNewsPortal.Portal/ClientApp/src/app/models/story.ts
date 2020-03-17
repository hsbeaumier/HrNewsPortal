import { Comment } from "./comment";

export class Story {
  // The item's unique identifier.
  public id: number;

  // The user name of the item's author.
  public by: string;

  // Creation UTC date of the item.
  public time?: Date;

  // The identifiers of the item's
  // comments, in ranked display order.
  public kids: number[];

  // The comments, in ranked display order.
  public comments: Comment[];

  // The URL of the story.
  public url: string;

  // The story's score, or the votes
  // for a pollopt (poll option).
  public score: number;
  
  // The title of the story, poll, or job.
  public title: string;
}
