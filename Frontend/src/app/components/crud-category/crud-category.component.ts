import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/services/category/category.service';
import {ModalDismissReasons, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import { Category } from 'src/app/models/Category';
import { FormGroup, FormBuilder, NgForm } from '@angular/forms';

@Component({
  selector: 'app-crud-category',
  templateUrl: './crud-category.component.html',
  styleUrls: ['./crud-category.component.scss']
})
export class CrudCategoryComponent implements OnInit {
  CategoryList: any[] = []
  closeModal: string;
  saveProfileForm: FormGroup;
  message: string;
  closeResult: string;
  SelectedCategory: Category;
  constructor(
    private fb: FormBuilder,
    public categoryService: CategoryService,
    private modalService: NgbModal) { }

  ngOnInit(): void {
    this.getCategories();

    this.saveProfileForm = this.fb.group({
      id: [''],
      name: [''],
      description: ['']
    });

    this.SelectedCategory = null;
  }

  getCategories() {
    this.categoryService.getCategories().subscribe(
      data => {
        this.CategoryList = data
      },
      error => {
        console.log(error)
      },
    )
  }

  updateCategory(category: Category){
    this.categoryService.update(category).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This category was updated successfully!';
      },
      error =>{
        console.log(error);
      }
    );
  }

  createCategory(category: any){
    this.categoryService.create(category).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This category was updated successfully!';
      },
      error =>{
        console.log(error);
      },
    );
  }

  deleteCategory(id: any){
    this.categoryService.delete(id).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This category was deleted successfully!';
      },
      error =>{
        console.log(error);
      }
    );
  }

  openModal(content, category) {
    
    this.modalService.open(content, {
      centered: true,
      backdrop: 'static',
      size:'xl',
      ariaLabelledBy: 'modal-basic-title'
    })
      .result.then((result) => {
        this.closeResult = `Closed with: ${result}`
      }, (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`
      })
    
    if (category === "") 
    {
      this.saveProfileForm.patchValue({
        id: "",
        name: "",
        description: ""
      });
      this.SelectedCategory = null;
    }
    else 
    {
      this.saveProfileForm.patchValue({
        id: category.id,
        name: category.name,
        description: category.description
      });
      this.SelectedCategory = category;
    }
  }

  openConfirmDelete(content, category) {
    
    this.modalService.open(content, {
      centered: true,
      backdrop: 'static',
      size:'sm',
      ariaLabelledBy: 'modal-basic-title'
    })
      .result.then((result) => {
        this.closeResult = `Closed with: ${result}`
      }, (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`
      })

    if (category !== "") 
    {
      this.SelectedCategory = category;
    }
  }
  
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  closeModalApp() {
    this.modalService.dismissAll();
  }

  onSubmit() {
    this.modalService.dismissAll();
    
    if (this.SelectedCategory === null) 
    {
      let newCategory = new Category();
      newCategory.Id = 0;
      newCategory.CreatedBy = 1;
      newCategory.CreatedDate = new Date();
      newCategory.ModifiedBy = 1;
      newCategory.ModifiedDate = new Date();
      newCategory.isDeleted = false;
      newCategory.Id = this.saveProfileForm.value.id; 
      newCategory.name = this.saveProfileForm.value.name;
      newCategory.description = this.saveProfileForm.value.description;
      this.createCategory(JSON.stringify(newCategory))
    }
    else
    {
      this.updateCategory(this.saveProfileForm.getRawValue())
    }
   }

   OnDelete(){
    this.deleteCategory(this.SelectedCategory.id);
    this.SelectedCategory = null;
    this.modalService.dismissAll();
   }

   
}
