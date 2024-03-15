import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddBlogPost } from '../models/add-blog-post.model';
import { NgModel } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { CommonModule } from '@angular/common';
import BlogPostService from './../services/blog-post.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MarkdownModule } from 'ngx-markdown';
import { CategoryService } from './../../category/services/category.service';
import { Observable, Subscription } from 'rxjs';
import { Category } from '../../category/models/category.model';
import { ImageSelectorComponent } from 'src/app/shared/components/image-selector/image-selector.component';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';
@Component({
  selector: 'app-add-blogpost',
  standalone: false,
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css']
})
export class AddBlogpostComponent implements OnInit, OnDestroy {
  model: AddBlogPost;
  categories$?: Observable<Category[]>;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;

  constructor(private BlogPostService: BlogPostService, private router: Router,
    private categoryService: CategoryService,
    private imageService : ImageService) {

    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featuredImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date(),
      categories: []
    }
  }
  
  ngOnInit(): void {
    console.log(this.model);
    this.categories$ = this.categoryService.getAllCategories();
    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (selectedImage) =>{
        // Pi thojm URl o i njejt
        this.model.featuredImageUrl = selectedImage.url;
        this.closeImageSelector();
      }
    })
  }
  onFormSubmit(): void {
    this.BlogPostService.createBlogPost(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/blogposts');
      }
    })
  }
  openImageSelector(): void{
    this.isImageSelectorVisible = true;
    console.log("Allo");
  }
  closeImageSelector(): void{
    this.isImageSelectorVisible = false;
  }
  
  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
  }
}
