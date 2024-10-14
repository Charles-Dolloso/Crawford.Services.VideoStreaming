import { Component } from '@angular/core';
import {Router} from '@angular/router'; 
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VideoService } from 'src/app/services/video.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-video-upload',
  templateUrl: './video-upload.component.html',
  styleUrls: ['./video-upload.component.scss']
})
export class VideoUploadComponent {
  videoForm: FormGroup;
  file: File | null = null;
  isLoading = false; // Spinner control

  constructor(
    private fb: FormBuilder, 
    private videoService: VideoService, 
    private snackBar: MatSnackBar,
    private router:Router
  ) {
    this.videoForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', [Validators.required, Validators.maxLength(160)]],
      categories: ['', Validators.required],
      file: ['', Validators.required]
    });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file && this.isValidFile(file)) {
      this.file = file;
    } else {
      this.snackBar.open('Invalid file type or size! Only MP4, AVI, and MOV under 100MB are allowed.', 'Close', { duration: 3000 });
    }
  }

  isValidFile(file: File): boolean {
    const validTypes = ['video/mp4', 'video/avi', 'video/quicktime'];
    const maxSizeInMB = 100;

    return validTypes.includes(file.type) && file.size / 1024 / 1024 <= maxSizeInMB;
  }

  triggerFileInput(): void {
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    fileInput?.click();
  }

  submit(): void {
    if (!this.file) {
      this.snackBar.open('Please complete the form and upload a valid video file.', 'Close', { duration: 3000 });
      return;
    }

    const formData = new FormData();
    formData.append('title', this.videoForm.get('title')?.value);
    formData.append('description', this.videoForm.get('description')?.value);
    formData.append('categories', this.videoForm.get('categories')?.value);
    formData.append('file', this.file);

    this.isLoading = true; // Show spinner

    this.videoService.uploadVideo(formData).subscribe({
      next: () => {
        this.snackBar.open('Video uploaded successfully!', 'Close', { duration: 3000 });
        this.isLoading = false; // Hide spinner
        this.videoForm.reset(); // Reset form after success
        this.file = null;
        this.router.navigate(['']); 
      },
      error: () => {
        this.snackBar.open('Failed to upload video. Please try again.', 'Close', { duration: 3000 });
        this.isLoading = false; // Hide spinner
      }
    });
  }
}