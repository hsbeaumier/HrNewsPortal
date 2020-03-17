import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { HrNewsStoriesComponent } from "./hr-news-stories/hr-news-stories.component";
import { HrNewsStorySearchComponent } from "./hr-news-story-search/hr-news-story-search.component"
import { HrNewsCommentsComponent } from "./hr-news-comments/hr-news-comments.component";
import { HrNewsDataService } from "./hr-news-service/hr-news-data.service";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HrNewsStoriesComponent,
    HrNewsCommentsComponent,
    HrNewsStorySearchComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'news-stories', component: HrNewsStoriesComponent },
      { path: 'news-story-search', component: HrNewsStorySearchComponent }
    ])
  ],
  providers: [HrNewsDataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
