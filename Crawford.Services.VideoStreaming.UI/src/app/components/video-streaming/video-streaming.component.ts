import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { VideoService } from 'src/app/services/video.service';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-video-streaming',
  templateUrl: './video-streaming.component.html',
  styleUrls: ['./video-streaming.component.scss']
})
export class VideoStreamingComponent implements OnInit {
  videoUrl: string | null = null;
  videoId: Guid | null = null;

  constructor(
    private route: ActivatedRoute,
    private videoService: VideoService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.videoId = params['id'];
      if (this.videoId) {
        this.videoService.streamVideo(this.videoId).subscribe((url: string) => {
          this.videoUrl = url;
        });
      }
    });
  }
}
