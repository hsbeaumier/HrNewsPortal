import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Story } from "../models/story";
import { Comment } from '../models/comment';
import { Subject } from 'rxjs/internal/Subject'
import { LoadMoreCommentsResponse } from "../models/load-more-comments-response";

@Injectable({
  providedIn: 'root'
})
export class HrNewsDataService {
  public topStoriesFetchedSource = new Subject<Story[]>();
  public topStoriesFetched$ = this.topStoriesFetchedSource.asObservable();

  public storiesSearchedFetchedSource = new Subject<Story[]>();
  public storiesSearchedFetched$ = this.storiesSearchedFetchedSource.asObservable();

  public loadMoreCommentsFetchedSource = new Subject<LoadMoreCommentsResponse>();
  public loadMoreCommentsFetched$ = this.loadMoreCommentsFetchedSource.asObservable();
  
  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) { }

  getTopStories(topStories: number) {
    this.http.get<Story[]>(this.baseUrl + `gettopstories/${topStories}`).subscribe(data => {
      this.topStoriesFetchedSource.next(data);
    }, error => console.error(error));
  }

  loadMoreComments(index: number, itemId: number, type: string, lastCommentIdLoaded: number, numberToLoad: number) {
    this.http.get<Comment[]>(this.baseUrl + `loadmorecomments/?itemId=${itemId}&type=${type}&lastCommentIdLoaded=${lastCommentIdLoaded}&numberToLoad=${numberToLoad}`).subscribe(data => {
      let response = new LoadMoreCommentsResponse();
      response.comments = data;
      response.index = index;
      this.loadMoreCommentsFetchedSource.next(response);
    }, error => console.error(error));
  }

  searchForStories(byCriteria: string, timeCriteria: string, urlCriteria: string, scoreCriteria: string) {
    let queryString: string = "";
    if (byCriteria != null && byCriteria != undefined) {
      if (this.isNullOrWhitespace(queryString)) {
        queryString += `by=${byCriteria}`;
      } else {
        queryString += `&by=${byCriteria}`;
      }
    }
    if (timeCriteria != null && timeCriteria != undefined) {
      if (this.isNullOrWhitespace(queryString)) {
        queryString += `time=${timeCriteria}`;
      } else {
        queryString += `&time=${timeCriteria}`;
      }
    }
    if (urlCriteria != null && urlCriteria != undefined) {
      if (this.isNullOrWhitespace(queryString)) {
        queryString += `url=${urlCriteria}`;
      } else {
        queryString += `&url=${urlCriteria}`;
      }
    }
    if (scoreCriteria != null && scoreCriteria != undefined) {
      if (this.isNullOrWhitespace(queryString)) {
        queryString += `score=${scoreCriteria}`;
      } else {
        queryString += `&score=${scoreCriteria}`;
      }
    }
    this.http.get<Story[]>(this.baseUrl + `searchforstories?${queryString}`).subscribe(data => {
      this.storiesSearchedFetchedSource.next(data);
    }, error => console.error(error));
  }

  isNullOrWhitespace(input: string): boolean {
    if (input == null) {
      return true;
    }

    if (input.trim() === "") {
      return true;
    }
  }
}
