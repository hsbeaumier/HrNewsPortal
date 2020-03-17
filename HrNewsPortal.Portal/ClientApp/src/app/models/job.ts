export class Job {
  // The item's unique identifier.
  public id: number;

  // The user name of the item's author.
  public by: string;

  // Creation UTC date of the item.
  public time?: Date;

  // The comment story or poll text.
  public text: string;

  // The URL of the story.
  public url: string;

  // The story's score, or the votes
  // for a pollopt (poll option).
  public score: number;

  // The title of the story, poll, or job.
  public title: string;
}
