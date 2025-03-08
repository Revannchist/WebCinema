import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.loginForm.valid) {
      const loginData = this.loginForm.value;

      this.userService.login(loginData).subscribe({
        next: (response) => {
          if (response.roleId === 1) { // Admin
            localStorage.setItem('currentUser', JSON.stringify(response));
            localStorage.setItem('isAdmin', 'true');
            alert('Successfully logged in as Admin');
            this.router.navigate(['/admin']);
          } else { // Regular user
            localStorage.setItem('currentUser', JSON.stringify(response));
            localStorage.setItem('currentUserId', response.id.toString());
            alert('Successfully logged in');
            this.router.navigate(['/movie-list']);
          }
        },
        error: (error) => {
          if (error.message === 'User not found') {
            alert('User not found. Please check your username.');
          } else if (error.message === 'Invalid password') {
            alert('Incorrect password. Please try again.');
          } else {
            alert('Login failed. Please try again.');
          }
        }
      });
    }
  }

  goToCreateAccount(): void {
    sessionStorage.setItem('fromLoginCreate', 'true');
    this.router.navigate(['/users'], { queryParams: { mode: 'create' } });
  }
}
