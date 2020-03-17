import { Component, OnInit, Input } from '@angular/core';
import { Comment } from '../models/comment';
import { HrNewsDataService } from "../hr-news-service/hr-news-data.service";
import { Story } from "../models/story";
import { CommentView } from "../models/comment-view";
import { Poll } from "../models/poll";

@Component({
  selector: 'app-hr-news-comments',
  templateUrl: './hr-news-comments.component.html'
})
export class HrNewsCommentsComponent implements OnInit {

  @Input() public story: Story;
  @Input() public comment: Comment;
  @Input() public poll: Poll;
  @Input() public index: number;

  public lastCommentIdLoaded: number = 0;

  constructor(public service: HrNewsDataService) { }

  ngOnInit() {
    if (this.story != null) {
      this.service.loadMoreCommentsFetched$.subscribe(response => {
        if (response.index === this.index) {
          if (this.lastCommentIdLoaded > 0) {
            this.story.comments.concat(response.comments);
          } else {
            this.story.comments = response.comments;
          }

          if (this.story.comments == null || this.story.comments == undefined || this.story.comments.length === 0) {
            alert("There are no comments.");
          }

          this.lastCommentIdLoaded = this.story.comments[this.story.comments.length - 1].id;
          const timer: ReturnType<typeof setTimeout> = setTimeout(() => '', 1000);
        }
      });
    }
    if (this.comment != null) {
      this.service.loadMoreCommentsFetched$.subscribe(response => {
        if (response.index === this.index) {
          if (this.lastCommentIdLoaded > 0) {
            this.comment.comments.concat(response.comments);
          } else {
            this.comment.comments = response.comments;
          }

          if (this.comment.comments == null || this.comment.comments == undefined || this.comment.comments.length === 0) {
            alert("There are no comments.");
          }

          this.lastCommentIdLoaded = this.comment.comments[this.comment.comments.length - 1].id;
          const timer: ReturnType<typeof setTimeout> = setTimeout(() => '', 1000);
        }
      });
    }
    if (this.poll != null) {
      this.service.loadMoreCommentsFetched$.subscribe(response => {
        if (response.index === this.index) {
          if (this.lastCommentIdLoaded > 0) {
            this.poll.comments.concat(response.comments);
          } else {
            this.poll.comments = response.comments;
          }

          if (this.poll.comments == null || this.poll.comments == undefined || this.poll.comments.length === 0) {
            alert("There are no comments.");
          }

          this.lastCommentIdLoaded = this.poll.comments[this.poll.comments.length - 1].id;
          const timer: ReturnType<typeof setTimeout> = setTimeout(() => '', 1000);
        }
      });
    }
  }

  loadMoreComments(ordinalIndex: number, numberOfCommentsToLoad: number) {
    if (this.story != null) {
      this.service.loadMoreComments(ordinalIndex, this.story.id, "story", this.lastCommentIdLoaded, numberOfCommentsToLoad);
    }
    if (this.comment != null) {
      this.service.loadMoreComments(ordinalIndex, this.comment.id, "comment", this.lastCommentIdLoaded, numberOfCommentsToLoad);
    }
    if (this.poll != null) {
      this.service.loadMoreComments(ordinalIndex, this.poll.id, "poll", this.lastCommentIdLoaded, numberOfCommentsToLoad);
    }
  }
}
