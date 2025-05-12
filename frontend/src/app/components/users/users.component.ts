import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserCreateDto } from '../../models/dto/user-create-dto';
import { UserDisplayDto } from '../../models/dto/user-display-dto';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { debounceTime, distinctUntilChanged, switchMap, map, first, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  submitted = false;
  isEditing = false;
  selectedUserId: number | null = null;
  currentUserRole: number = 0;
  isAdmin: boolean = false;
  existingPassword: string = '';
  userForm!: FormGroup;
  pageSize: number = 3;
  currentPage: number = 1;
  totalPages: number = 1;
  totalUsers: number = 0;
  password: string = '';
  public isCreateMode: boolean = true;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslateService
  ) {
    this.translate.use('en');
    this.initializeEmptyForm();
    this.route.queryParams.subscribe(params => {
      this.isCreateMode = params['mode'] === 'create';
    });
  }

  private initializeEmptyForm(): void {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(5), Validators.pattern(/^(?=.*[0-9])/)]],
      confirmPassword: ['', Validators.required]
    }, {
      validator: this.passwordMatchValidator
    });
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const userId = params['userId'];
      if (userId) {
        this.userService.getUserById(userId).subscribe({
          next: (user: UserDisplayDto) => {
            this.users = [user];
            this.editUser(user);
          },
          error: (error) => {
            console.error('Error loading specific user:', error);
          }
        });
      } else {
        const currentUser = localStorage.getItem('currentUser');
        if (currentUser) {
          const userData = JSON.parse(currentUser);
          this.currentUserRole = userData.roleId;
          this.isAdmin = userData.roleId === 1;
        }

        const isFromLoginCreate = sessionStorage.getItem('fromLoginCreate');
        if (isFromLoginCreate) {
          this.clearEverything();
          sessionStorage.removeItem('fromLoginCreate');
        } else {
          const currentUserId = localStorage.getItem('currentUserId');
          if (currentUserId) {
            this.setupEditModeIfNeeded();
          }
        }
        if (!this.isCreateMode) {
          this.loadUsers();
        }
      }
    });

    if (this.isCreateMode) {
      // Remove the problematic valueChanges
    }
  }
  
  private clearEverything(): void {
    this.users = [];
    this.isEditing = false;
    this.selectedUserId = null;
    this.existingPassword = '';
    localStorage.removeItem('currentUserId');
    this.initializeEmptyForm();
  }

  passwordMatchValidator(g: AbstractControl): ValidationErrors | null {
    const password = g.get('password');
    const confirmPassword = g.get('confirmPassword');
    
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { passwordMismatch: true };
    }
    return null;
  }

  setupEditModeIfNeeded(): void {
    const currentUserId = localStorage.getItem('currentUserId');
    if (currentUserId) {
      this.userService.getAllUsers().subscribe({
        next: (users) => {
          const userToEdit = users.find(u => u.id.toString() === currentUserId);
          if (userToEdit) {
            this.isEditing = true;
            this.selectedUserId = userToEdit.id;
            this.existingPassword = userToEdit.password;
            
            const formData: any = {
              username: userToEdit.username,
              email: userToEdit.email,
              firstName: userToEdit.firstName,
              lastName: userToEdit.lastName,
              dateOfBirth: new Date(userToEdit.dateOfBirth).toISOString().split('T')[0]
            };

            if (!this.isAdmin) {
              formData.password = '';
              formData.confirmPassword = '';
            }

            this.userForm.patchValue(formData);
            this.users = [userToEdit];
          }
        },
        error: (error) => {
          console.error('Error loading user for edit:', error);
        }
      });
    }
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.userForm.valid) {
      const formValue = this.userForm.value;
      
      if (this.isEditing && this.selectedUserId) {
        const updateData: any = {
          id: this.selectedUserId,
          username: formValue.username,
          email: formValue.email,
          firstName: formValue.firstName,
          lastName: formValue.lastName,
          dateOfBirth: formValue.dateOfBirth,
          roleId: 2
        };

        if (this.isAdmin) {
          updateData.password = this.existingPassword;
        } else {
          updateData.password = formValue.password || this.existingPassword;
        }

        this.userService.updateUser(this.selectedUserId, updateData).subscribe({
          next: (response: any) => {
            alert('User successfully updated');
            this.users = [response];
            this.existingPassword = updateData.password;
            localStorage.setItem('token', response.token);
          },
          error: (error: any) => {
            console.error('Update error:', error);
            if (error.error && typeof error.error === 'string') {
              alert(error.error);
            } else {
              alert('Error updating user. Please try again.');
            }
          }
        });
      } else {
        const newUser = {
          ...formValue,
          roleId: 2
        };
        delete newUser.confirmPassword;

        this.userService.createUser(newUser).subscribe({
          next: (response: any) => {
            alert('User successfully created');
            this.users = [response];
            this.existingPassword = formValue.password;
            this.selectedUserId = response.id;
            this.resetForm();
            localStorage.setItem('token', response.token);
          },
          error: (error: any) => {
            if (error.error && typeof error.error === 'string') {
              alert(error.error);
            } else {
              alert('Error creating user. Please try again.');
            }
          }
        });
      }
    }
  }

  resetForm(): void {
    this.userForm.reset();
    this.submitted = false;
  }

  editUser(user: UserDisplayDto): void {
    this.isEditing = true;
    this.selectedUserId = user.id;
    
    this.userForm.patchValue({
      username: user.username,
      email: user.email,
      firstName: user.firstName,
      lastName: user.lastName,
      dateOfBirth: new Date(user.dateOfBirth).toISOString().split('T')[0],
      password: user.password,
      confirmPassword: user.password
    });

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.selectedUserId = null;
    this.resetForm();
  }

  getErrorMessage(controlName: string): string {
    const control = this.userForm.get(controlName);
    if (control?.errors && (control.dirty || control.touched || this.submitted)) {
      if (control.errors['required']) 
        return 'USER_REGISTRATION.VALIDATION.REQUIRED';
      if (control.errors['email']) 
        return 'USER_REGISTRATION.VALIDATION.EMAIL_INVALID';
      if (control.errors['minlength']) 
        return 'USER_REGISTRATION.VALIDATION.PASSWORD_LENGTH';
      if (control.errors['pattern'] && controlName === 'password') 
        return 'USER_REGISTRATION.VALIDATION.PASSWORD_NUMBER';
      if (control.errors['usernameTaken']) 
        return 'USER_REGISTRATION.VALIDATION.USERNAME_TAKEN';
    }
    if (controlName === 'confirmPassword' && this.userForm.hasError('mismatch')) {
      return 'USER_REGISTRATION.VALIDATION.PASSWORDS_MATCH';
    }
    return '';
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.totalUsers = users.length;
        this.totalPages = 1;
        this.currentPage = 1;
      },
      error: (error) => {
        console.error('Error loading users:', error);
      }
    });
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.loadUsers();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.loadUsers();
    }
  }

  deleteUser(userId: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(userId).subscribe({
        next: (response: any) => {
          alert('User successfully deleted');
          window.location.reload();
        },
        error: (error: any) => {
          console.error('Delete error:', error);
          if (error.error && typeof error.error === 'string') {
            alert(error.error);
          } else {
            alert('Error deleting user. Please try again.');
          }
        }
      });
    }
  }

  onPasswordInput(event: any) {
    this.password = event.target.value;
  }
}
