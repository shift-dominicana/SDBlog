import { Component, OnInit } from '@angular/core';
import {ModalDismissReasons, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, NgForm } from '@angular/forms';
import { Tag } from 'src/app/models/Tag';
import { TagService } from 'src/app/services/tag/tag.service';

@Component({
  selector: 'app-crud-Tag',
  templateUrl: './crud-tag.component.html',
  styleUrls: ['./crud-tag.component.scss']
})
export class CrudTagComponent implements OnInit {
  TagList: any[] = []
  closeModal: string;
  saveProfileForm: FormGroup;
  message: string;
  closeResult: string;
  SelectedTag: Tag;
  constructor(
    private fb: FormBuilder,
    public tagService: TagService,
    private modalService: NgbModal) { }

  ngOnInit(): void {
    this.getTags();

    this.saveProfileForm = this.fb.group({
      Id: [''],
      name: [''],
      description: ['']
    });

    this.SelectedTag = null;
  }

  getTags() {
    this.tagService.getTags().subscribe(
      data => {
        this.TagList = data
      },
      error => {
        console.log(error)
      },
    )
  }

  updateTag(Tag: Tag){
    this.tagService.update(Tag, Tag.Id).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This Tag was updated successfully!';
      },
      error =>{
        console.log(error);
      }
    );
  }

  createTag(Tag: any){
    this.tagService.create(Tag).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This Tag was updated successfully!';
      },
      error =>{
        console.log(error);
      },
    );
  }

  deleteTag(id: any){
    this.tagService.delete(id).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This Tag was deleted successfully!';
      },
      error =>{
        console.log(error);
      }
    );
  }

  openModal(content, Tag) {
    
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
    
    if (Tag === "") 
    {
      this.saveProfileForm.patchValue({
        Id: "",
        name: "",
        description: ""
      });
      this.SelectedTag = null;
    }
    else 
    {
      this.saveProfileForm.patchValue({
        Id: Tag.id,
        name: Tag.name,
        description: Tag.description
      });
      this.SelectedTag = Tag
      this.SelectedTag.Id = Tag.Id;
    }
  }

  openConfirmDelete(content, Tag) {
    
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

    if (Tag !== "") 
    {
      this.SelectedTag = Tag;
      this.SelectedTag.Id = Tag.id;
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
    
    if (this.SelectedTag === null) 
    {
      let newTag = new Tag();
      newTag.Id = 0;
      newTag.name = this.saveProfileForm.value.name;
      newTag.description = this.saveProfileForm.value.description;
      this.createTag(JSON.stringify(newTag))
      console.log(JSON.stringify(newTag))
    }
    else
    {
      this.updateTag(this.saveProfileForm.getRawValue())
      console.log(JSON.stringify(this.saveProfileForm.getRawValue()))
    }
   }

   OnDelete(){
    this.deleteTag(this.SelectedTag.Id);
    this.SelectedTag = null;
    this.modalService.dismissAll();
   }

   
}
