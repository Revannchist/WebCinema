import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserCreateDto } from '../../models/dto/user-create-dto';
import { UserDisplayDto } from '../../models/dto/user-display-dto';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  userForm!: FormGroup;
  isEditing = false;
  selectedUserId: number | null = null;
  submitted = false;
  users: UserDisplayDto[] = [];

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
    this.userForm = this.fb.group({
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
      validators: this.passwordMatchValidator
    });
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null : {'mismatch': true};
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

  onSubmit(): void {
    this.submitted = true;

    if (this.userForm.valid) {
      const { confirmPassword, ...userData } = this.userForm.value;
      
      if (this.isEditing && this.selectedUserId) {
        // Update existing user
        const userToUpdate = {
          ...userData,
          id: this.selectedUserId,
          roleId: 2
        };

        console.log('Sending update data:', userToUpdate);

        this.userService.updateUser(this.selectedUserId, userToUpdate).subscribe({
          next: (response) => {
            alert('User successfully updated');
            this.resetForm();
            this.loadUsers();
            this.isEditing = false;
            this.selectedUserId = null;
          },
          error: (error) => {
            console.error('Update error:', error);
            if (error.error && typeof error.error === 'string') {
              alert(error.error);
            } else if (error.error?.message) {
              alert(error.error.message);
            } else {
              alert('Error updating user. Please try again.');
            }
          }
        });
      } else {
        // Create new user
        const userToCreate = {
          ...userData,
          roleId: 2
        };

        this.userService.addUser(userToCreate).subscribe({
          next: () => {
            alert('User successfully created');
            this.resetForm();
            this.loadUsers();
          },
          error: (error) => {
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
    this.isEditing = false;
    this.selectedUserId = null;
    this.submitted = false;
  }

  getErrorMessage(controlName: string): string {
    const control = this.userForm.get(controlName);
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
    if (controlName === 'confirmPassword' && this.userForm.hasError('mismatch')) {
      return 'Passwords do not match';
    }
    return '';
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (users) => {
        this.users = users.filter(user => user.roleId === 2);
      },
      error: (error) => {
        console.error('Error loading users:', error);
      }
    });
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
}
