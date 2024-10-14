// components/home/home.component.ts
import { Component, OnInit } from '@angular/core';
import { VideoService } from 'src/app/services/video.service';
import { Video } from 'src/app/models/video.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  videos: Video[] = [];

  constructor(private videoService: VideoService) { }

  ngOnInit(): void {
    // Load video list on component init
    this.videoService.getVideos().subscribe(
      (response) => { this.videos = response; },
      (error) => { console.log(error); });
  }
}