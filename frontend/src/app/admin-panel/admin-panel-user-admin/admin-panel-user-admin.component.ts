import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserDisplayDto } from '../../models/dto/user-display-dto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-panel-user-admin',
  templateUrl: './admin-panel-user-admin.component.html',
  styleUrls: ['./admin-panel-user-admin.component.css']
})
export class AdminPanelUserAdminComponent implements OnInit {
  adminForm: FormGroup;
  isEditing = false;
  selectedUserId: number | null = null;
  submitted = false;
  admins: UserDisplayDto[] = [];
  users: UserDisplayDto[] = [];
  allUsers: UserDisplayDto[] = [];
  usernameFilter: string = '';
  emailFilter: string = '';
  currentAdminUsername: string = '';
  pageSize: number = 3;
  currentPage: number = 1;
  totalPages: number = 1;
  totalUsers: number = 0;
  searchTerm: string = '';
  debounceTimer?: ReturnType<typeof setTimeout>;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.adminForm = this.formBuilder.group({
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

  ngOnInit(): void {
    this.loadAdmins();
    this.loadUsers();
    const currentUser = localStorage.getItem('currentUser');
    if (currentUser) {
      const userData = JSON.parse(currentUser);
      this.currentAdminUsername = userData.username;
    }
  }

  private passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null : {'mismatch': true};
  }

  loadAdmins(): void {
    this.userService.getAllUsers().subscribe({
      next: (response: UserDisplayDto[]) => {
        this.admins = response.filter((user: UserDisplayDto) => user.roleId === 1);
      },
      error: (error: any) => {
        console.error('Error loading admins:', error);
      }
    });
  }

  loadUsers(): void {
    const combinedSearchTerm = this.usernameFilter || this.emailFilter;
    this.userService.getUsersPagedAndFiltered(this.currentPage, this.pageSize, combinedSearchTerm)
      .subscribe({
        next: (response: any) => {
          this.users = response.users;
          this.totalUsers = response.totalUsers;
          this.totalPages = Math.ceil(this.totalUsers / this.pageSize);
        },
        error: (error: any) => {
          console.error('Error loading users:', error);
        }
      });
  }

  filterUsers(): void {
    if (this.debounceTimer) {
      clearTimeout(this.debounceTimer);
    }

    this.debounceTimer = setTimeout(() => {
      this.currentPage = 1; // Reset na prvu stranicu kod novog filtera
      this.loadUsers();
    }, 300); // 300ms debounce
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.adminForm.valid) {
      const { confirmPassword, password, ...userData } = this.adminForm.value;
      
      if (this.isEditing && this.selectedUserId) {
        const userToUpdate = {
          ...userData,
          id: this.selectedUserId,
          roleId: 2 // Za obične korisnike
        };

        this.userService.updateUser(this.selectedUserId, userToUpdate).subscribe({
          next: () => {
            alert('User successfully updated');
            this.resetForm();
            this.loadUsers();
          },
          error: (error) => {
            if (error.error && typeof error.error === 'string') {
              alert(error.error);
            } else {
              alert('Error updating user. Please try again.');
            }
          }
        });
      } else {
        // Postojeća logika za kreiranje novog admina
        const userToCreate = {
          ...userData,
          password,
          roleId: 1
        };

        this.userService.addUser(userToCreate).subscribe({
          next: () => {
            alert('Admin successfully created');
            this.resetForm();
            this.loadAdmins();
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

  editUserInUsersComponent(user: UserDisplayDto): void {
    this.router.navigate(['/users'], { 
      queryParams: { 
        userId: user.id 
      }
    });
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
    localStorage.removeItem('currentUserId');
    
    this.router.navigate(['/']);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadUsers();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadUsers();
    }
  }
}
