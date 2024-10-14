import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Video } from '../models/video.model';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root'
})
export class VideoService {
  private apiUrl = 'https://localhost:7220/api/video';  // Base URL for backend API

  constructor(private http: HttpClient) { }

  // 1. Fetch all videos (GET request)
  getVideos(): Observable<Video[]> {
    return this.http.get<Video[]>(this.apiUrl);
  }

  // 2. Upload video (POST request with FormData)
  uploadVideo(videoData: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/upload`, videoData, { responseType: 'text'});
  }

  // 3. Stream video by id (GET request to stream video)
  streamVideo(videoId: Guid): Observable<any> {
    return this.http.get(`${this.apiUrl}/stream/${videoId}`, { responseType: 'text' });
  }
}