import { Injectable } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category.model';
import { environment } from 'src/environments/environment';
import { UpdateCategoryRequest } from './../models/update-category-request.model';
@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  addCategory(model: AddCategoryRequest): Observable<void> {
    //Krijum environment per shkak se nese ka shum veta tu punu me ni projekt, mos mu marr me Url t ndryshem e 100 her me shkru po veq e krijon ni environment 
    // me atributin apiUrlBase (ose qka do) me linkun e pershtatshem.
    return this.http.post<void>(`${environment.apiBaseUrl}/api/categories`, model);

  }
  //  Osht array puna qe ka 3 atribute (name id url) 
  getAllCategories(): Observable<Category[]> {

    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/Categories`)

  }

  getCategoryById(id: string): Observable<Category> {

    return this.http.get<Category>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }

  updateCategory(id: string, updateCategoryRequest: UpdateCategoryRequest): Observable<Category> {
    return this.http.put<Category>(`${environment.apiBaseUrl}/api/categories/${id}`, updateCategoryRequest);


  }
  deleteCategory(id: string): Observable<Category> {

    return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories/${id}`)
  }
}
