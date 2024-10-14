import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { VideoUploadComponent } from './components/video-upload/video-upload.component';
import { VideoStreamingComponent } from './components/video-streaming/video-streaming.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'video-upload', component: VideoUploadComponent },
  { path: 'video-stream/:id', component: VideoStreamingComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }