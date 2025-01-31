import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserDisplayDto } from '../../models/dto/user-display-dto';

@Component({
  selector: 'app-admin-panel-user-admin',
  templateUrl: './admin-panel-user-admin.component.html',
  styleUrls: ['./admin-panel-user-admin.component.css']
})
export class AdminPanelUserAdminComponent implements OnInit {
  adminForm!: FormGroup;
  isEditing = false;
  selectedUserId: number | null = null;
  submitted = false;
  admins: UserDisplayDto[] = [];
  users: UserDisplayDto[] = [];
  allUsers: UserDisplayDto[] = [];
  usernameFilter: string = '';
  emailFilter: string = '';

  constructor(
    private fb: FormBuilder,
    private userService: UserService
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  private initializeForm(): void {
    this.adminForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required, 
        Validators.minLength(5),
        Validators.pattern(/^(?=.*[0-9])/)
      ]],
      confirmPassword: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required]
    }, {
      validator: this.passwordMatchValidator
    });
  }

  private passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null : {'mismatch': true};
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (users) => {
        this.admins = users.filter(user => user.roleId === 1);
        this.allUsers = users.filter(user => user.roleId === 2);
        this.users = [...this.allUsers];
      },
      error: (error) => {
        console.error('Error loading users:', error);
      }
    });
  }

  filterUsers(): void {
    this.users = this.allUsers.filter(user => {
      const matchUsername = user.username.toLowerCase().includes(this.usernameFilter.toLowerCase());
      const matchEmail = user.email.toLowerCase().includes(this.emailFilter.toLowerCase());
      
      if (!this.usernameFilter && !this.emailFilter) {
        return true;
      }
      
      if (this.usernameFilter && !this.emailFilter) {
        return matchUsername;
      }
      
      if (!this.usernameFilter && this.emailFilter) {
        return matchEmail;
      }
      
      return matchUsername && matchEmail;
    });
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.adminForm.valid) {
      const { confirmPassword, ...userData } = this.adminForm.value;
      
      if (this.isEditing && this.selectedUserId) {
        const userToUpdate = {
          ...userData,
          id: this.selectedUserId,
          roleId: 1
        };

        this.userService.updateUser(this.selectedUserId, userToUpdate).subscribe({
          next: () => {
            alert('Admin successfully updated');
            this.resetForm();
            this.loadUsers();
          },
          error: (error) => {
            if (error.error && typeof error.error === 'string') {
              alert(error.error);
            } else {
              alert('Error updating admin. Please try again.');
            }
          }
        });
      } else {
        const userToCreate = {
          ...userData,
          roleId: 1
        };

        this.userService.addUser(userToCreate).subscribe({
          next: () => {
            alert('Admin successfully created');
            this.resetForm();
            this.loadUsers();
          },
          error: (error) => {
            if (error.error && typeof error.error === 'string') {
              alert(error.error);
            } else {
              alert('Error creating admin. Please try again.');
            }
          }
        });
      }
    }
  }

  editUser(user: UserDisplayDto): void {
    if (user.roleId !== 1) {
      return;
    }

    this.isEditing = true;
    this.selectedUserId = user.id;
    
    this.adminForm.patchValue({
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

  deleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(id).subscribe({
        next: () => {
          alert('User successfully deleted');
          this.loadUsers();
        },
        error: (error) => {
          alert(error.error || 'Error deleting user. Please try again.');
        }
      });
    }
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.selectedUserId = null;
    this.resetForm();
  }

  resetForm(): void {
    this.adminForm.reset();
    this.isEditing = false;
    this.selectedUserId = null;
    this.submitted = false;
  }

  getErrorMessage(controlName: string): string {
    const control = this.adminForm.get(controlName);
    if (control?.errors && (control.dirty || control.touched || this.submitted)) {
      if (control.errors['required']) return 'This field is required';
      if (control.errors['email']) return 'Please enter a valid email address';
      if (control.errors['minlength']) {
        return `Minimum length is ${control.errors['minlength'].requiredLength} characters`;
      }
      if (control.errors['pattern'] && controlName === 'password') {
        return 'Password must contain at least one number';
      }
    }
    if (controlName === 'confirmPassword' && this.adminForm.hasError('mismatch')) {
      return 'Passwords do not match';
    }
    return '';
  }
}
