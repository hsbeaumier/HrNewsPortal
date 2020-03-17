import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Story } from "../models/story";
import { HrNewsDataService } from "../hr-news-service/hr-news-data.service";

@Component({
  selector: 'app-hr-news-stories',
  templateUrl: './hr-news-stories.component.html'
})
export class HrNewsStoriesComponent implements OnInit {
  public numberOfStories: number;
  public topStories: Story[];
  
  constructor(public service: HrNewsDataService, private cdRef: ChangeDetectorRef) {
    // Set Default
    this.numberOfStories = 100;
  }

  ngOnInit() {
    this.service.topStoriesFetched$.subscribe(records => {
      this.topStories = records;
      const timer: ReturnType<typeof setTimeout> = setTimeout(() => '', 1000);
    });
    
    this.service.getTopStories(this.numberOfStories);
  }

  updateTopStories() {
    this.service.getTopStories(this.numberOfStories);
  }
}
