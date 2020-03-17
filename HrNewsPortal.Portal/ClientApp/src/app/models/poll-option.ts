import { Poll } from "./poll";

export class PollOption {
  // The item's unique identifier.
  public id: number;

  // The user name of the item's author.
  public by: string;

  // Creation UTC date of the item.
  public time?: Date;

  // The comment story or poll text.
  public text: string;

  // The poll option's associated poll identifer.
  public pollId: number;

  // The poll option's associated poll.
  public poll: Poll;

  // The story's score, or the votes
  // for a poll option.
  public score: number;
}
