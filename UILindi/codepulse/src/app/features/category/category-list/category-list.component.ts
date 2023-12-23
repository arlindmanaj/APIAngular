import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';
import { Observable } from 'rxjs';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { RouterLink } from '@angular/router';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],



})
export class CategoryListComponent implements OnInit {

  categories$?: Observable<Category[]> | undefined;

  constructor(private categoryService: CategoryService) {

  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();


  }
}
