import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Story } from "../models/story";
import { HrNewsDataService } from "../hr-news-service/hr-news-data.service";

@Component({
  selector: 'app-hr-news-story-search',
  templateUrl: './hr-news-story-search.component.html'
})
export class HrNewsStorySearchComponent implements OnInit {
  public byCriteria: string;
  public timeCriteria: string;
  public urlCriteria: string;
  public scoreCriteria: number;

  public stories: Story[];

  constructor(public service: HrNewsDataService, private cdRef: ChangeDetectorRef) { }

  ngOnInit() {
    this.service.storiesSearchedFetched$.subscribe(records => {
      this.stories = records;
      const timer: ReturnType<typeof setTimeout> = setTimeout(() => '', 1000);
    });
  }

  searchForStories() {
    let score: string = null;
    if (this.scoreCriteria != null) {
      score = this.scoreCriteria.toString();
    }
    this.service.searchForStories(this.byCriteria, this.timeCriteria, this.urlCriteria, score);
  }
}
