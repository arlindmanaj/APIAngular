import { Component } from '@angular/core';
import { ImageService } from './image.service';
import { OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BlogImage } from '../../Models/blog-image.model';
import { NgOptimizedImage } from '@angular/common'
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-image-selector',
  //imports: [],
  templateUrl: './image-selector.component.html',
  styleUrl: './image-selector.component.css'
})
export class ImageSelectorComponent implements OnInit {

  private file?: File;
  fileName: string = '';
  title: string = '';
  images$?: Observable<BlogImage[]>;
  selectedImage?: BlogImage;
  
  constructor(private imageService: ImageService, private route: ActivatedRoute,
    
    private router: Router) {

  }
  ngOnInit(): void {
    this.getImages();
  }

  onFileUploadChange(event: Event): void {
    const element = event.currentTarget as HTMLInputElement;
    this.file = element.files?.[0];


  }

  uploadImage(): void {
    if (this.file && this.fileName !== '' && this.title !== '') {
      // Image service me bo uploadin
      this.imageService.uploadImage(this.file, this.fileName, this.title)
        .subscribe({
          next: (response) => {
           
            this.getImages();
          }
        });
    }
  }
  selectImage(image: BlogImage){
    this.selectedImage = image;
  }
  
    deleteImageOLD() {
    
      if(this.selectedImage)
      this.imageService.deleteImage(this.selectedImage).subscribe({
        next: (response) => {
          this.router.navigateByUrl('admin/images')
        }
      })
    }

    deleteImage(imageId: number) {
    // Assuming you have a deleteImage method in your image service
    this.imageService.deleteImage(imageId).subscribe(() => {
      // Optionally, you can refresh the images list after deletion
      this.images$ = this.imageService.getImages();
    }, error => {
      console.error('Error deleting image:', error);
      // Handle error, show message, etc.
    });
  }
  

  private getImages(){
    this.images$ = this.imageService.getAllImages();

  }

}
