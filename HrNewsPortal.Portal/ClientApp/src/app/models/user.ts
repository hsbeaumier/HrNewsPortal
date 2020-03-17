import { Item } from "./item";

export class User {
  // The user's unique username.
  // Case-sensitive.  Required.
  public id: string;

  // Delay in minutes between a comment's
  // creation and its visibility to other users.
  public delay: number;

  // Creation UTC date of the user.
  public created?: Date;

  // The user's karma.
  public karma: number;

  // The user's optional self-description.  HTML. 
  public about: string;

  // List of the user's stories, polls, and comments.
  public submitted: Item[];
}
