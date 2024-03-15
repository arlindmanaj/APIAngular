import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import BlogPostService from '../services/blog-post.service';
import { CommonModule } from '@angular/common';
import { NgIf } from '@angular/common';
import { NgModel } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';
import { ImageSelectorComponent } from 'src/app/shared/components/image-selector/image-selector.component';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';
@Component({
  selector: 'app-edit-blogpost',
  standalone: false,
  templateUrl: './edit-blogpost.component.html',
  styleUrl: './edit-blogpost.component.css'
})
export class EditBlogpostComponent implements OnInit, OnDestroy{
  id?: string | null;
  routeSubscription?: Subscription;
  updateBlogPostSubscription?: Subscription;
  getBlogPostSubscription?  : Subscription;
  deleteBlogPostSubscription? : Subscription;
  model?: BlogPost;
  selectedCategories?: string[];
  categories$?: Observable<Category[]>;
  isImageSelectorVisible: boolean = false;
  imageSelectSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private router: Router,
    private imageService: ImageService){

  }
 
  ngOnInit(): void {
   this.categories$ =  this.categoryService.getAllCategories()
   this.routeSubscription  = this.route.paramMap.subscribe({
    next: (params) => {
      this.id = params.get('id');

      // Get Blogpost from api n Service
      if(this.id){
        this.getBlogPostSubscription = this.blogPostService.getBlogPostbyId(this.id).subscribe({
          next: (response) => {
            this.model = response;
            this.selectedCategories = response.categories.map(x => x.id);
          }
        });
       
      };

      this.imageSelectSubscription = this.imageService.onSelectImage().subscribe({
        next:(response) => {
          if(this.model){
            this.model.featuredImageUrl = response.url;
            this.isImageSelectorVisible = false;
          }
        }
      })
      
    }
   })
  }
  onFormSubmit(): void{
    // COnvert model to request
    if(this.model && this.id) {
      var updateBlogPost:  UpdateBlogPost = {
        author: this.model.author,
        content: this.model.content,
        shortDescription: this.model.shortDescription,
        featuredImageUrl: this.model.featuredImageUrl,
        isVisible: this.model.isVisible,
        publishedDate: this.model.publishedDate,
        title: this.model.title,
        urlHandle: this.model.urlHandle,
        categories: this.selectedCategories ?? []
      };
      this.updateBlogPostSubscription = this.blogPostService.updateBlogPost(this.id, updateBlogPost).subscribe({
        next: (response) => {
          // E krijon ni variabel called router ku n response ka me .navigateByUrl te linki qe ja qet
          this.router.navigateByUrl('/admin/blogposts');
        }
      })
    }
  }
  onDelete(): void {
    if(this.id){
      // e thirr service edhe e fshin
      this.blogPostService.deleteBlogPost(this.id).subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/blogposts');
        }
      })
    }
  }
  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getBlogPostSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
    this.deleteBlogPostSubscription?.unsubscribe();
    this.imageSelectSubscription?.unsubscribe();
  }
  openImageSelector(): void{
    this.isImageSelectorVisible = true;
  }
  closeImageSelector(): void{
    this.isImageSelectorVisible = false;
  }
}
