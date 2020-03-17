import { Story } from "./story";
import { Comment } from "./comment";
import { Poll } from "./poll";

export class CommentView {
  public story: Story;
  public comment: Comment;
  public poll: Poll;
  public lastCommentIdLoaded: number;
  public entryForStory: boolean;
  public entryForComment: boolean;
  public entryForPoll: boolean;
}
