import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from 'src/app/models/User';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-crud-user',
  templateUrl: './crud-user.component.html',
  styleUrls: ['./crud-user.component.scss']
})
export class CrudUserComponent implements OnInit {
  UserList: any[] = []
  closeModal: string;
  saveProfileForm: FormGroup;
  message: string;
  closeResult: string;
  selectedUser: User;
  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    private modalService: NgbModal) { }

  ngOnInit(): void {
    this.getCategories();

    this.saveProfileForm = this.fb.group({
      Id: [''],
      userName: [''],
      password: [''],
      email: [''],
      firstName: [''],
      lastName: [''],
      dateOfBirth: [''],
      isAdmin: ['']
    });

    this.selectedUser = null;
  }

  getCategories() {
    this.userService.getUsers().subscribe(
      data => {
        this.UserList = data
      },
      error => {
        console.log(error)
      },
    )
  }

  updateUser(user: User){
    this.userService.update(user, user.id).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This user was updated successfully!';
      },
      error =>{
        console.log(error);
      }
    );
  }

  createUser(category: any){
    this.userService.create(category).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This user was updated successfully!';
      },
      error =>{
        console.log(error);
      },
    );
  }

  deleteUser(id: any){
    this.userService.delete(id).subscribe(
      response =>{
        console.log(response);
        window.location.reload();
        this.message = response.message ? response.message : 'This user was deleted successfully!';
      },
      error =>{
        console.log(error);
      }
    );
  }

  openModal(content, user) {
    
    this.modalService.open(content, {
      centered: true,
      backdrop: 'static',
      size:'md',
      ariaLabelledBy: 'modal-basic-title'
    })
      .result.then((result) => {
        this.closeResult = `Closed with: ${result}`
      }, (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`
      })
    
    if (!user) 
    {
      this.saveProfileForm.patchValue({
        id: "",
        userName: "",
        password: "",
        email: "",
        firstName: "",
        lastName: "",
        dateOfBirth: "",
        isAdmin: ""
      });
      this.selectedUser = null;
    }
    else 
    {
      this.saveProfileForm.patchValue({
        Id: user.id,
        userName: user.userName,
        password: user.password,
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        dateOfBirth: user.dateOfBirth,
        isAdmin: user.isAdmin
      });
      this.selectedUser = user
      this.selectedUser.id = user.id;
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
      this.selectedUser = category;
      this.selectedUser.id = category.id;
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
    
    if (this.selectedUser === null) 
    {
      let newUser = new User();
      newUser.id = 0;
      newUser.firstName = this.saveProfileForm.value.name;
      newUser.lastName = this.saveProfileForm.value.description;
      this.createUser(JSON.stringify(newUser))
      console.log(JSON.stringify(newUser))
    }
    else
    {
      this.updateUser(this.saveProfileForm.getRawValue())
      console.log(JSON.stringify(this.saveProfileForm.getRawValue()))
    }
   }

   OnDelete(){
    this.deleteUser(this.selectedUser.id);
    this.selectedUser = null;
    this.modalService.dismissAll();
   }

   
}
