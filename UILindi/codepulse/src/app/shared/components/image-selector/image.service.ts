import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { BlogImage } from '../../Models/blog-image.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  selectedImage: BehaviorSubject<BlogImage> = new BehaviorSubject<BlogImage>({
    id: '',
    fileExtension: '',
    fileName: '',
    title: '',
    url: ''
  });
  constructor(private http: HttpClient) { }

  uploadImage(file: File, fileName: string, title: string): Observable<BlogImage> {

    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('title', title);
    return this.http.post<BlogImage>(`${environment.apiBaseUrl}/api/images`, formData);
    
  }
  getAllImages() : Observable<BlogImage[]>{
    return this.http.get<BlogImage[]>(`${environment.apiBaseUrl}/api/images`);
  }
 // Void se veq e sheh
  selectImage(image: BlogImage): void {
    this.selectedImage.next(image);
  } 
  onSelectImage(): Observable<BlogImage>{
    return this.selectedImage.asObservable()
  }
 
  deleteImage(imageId: string): Observable<BlogImage> {
    return this.http.delete<BlogImage>(`${environment.apiBaseUrl}/api/images/${imageId}`);
  }
}
