import { Story } from "./story";

export class Comment {
  // The item's unique identifier.
  public id: number;

  // The user name of the item's author.
  public by: string;

  // Creation UTC date of the item.
  public time?: Date;

  // The comment story or poll text.
  public text: string;

  // The comment's parent identifier.
  public parent: number;

  // The comment's parent which is relevant to the story.
  public parentStory: Story;

  // The comment's parent which is another comment.
  public parentComment: Comment;

  // The identifiers of the item's
  // comments, in ranked display order.
  public kids: number[];

  // The comments, in ranked display order.
  public comments: Comment[];
}
