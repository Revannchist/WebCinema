import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../auth.service';

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
    private router: Router,
    private authService: AuthService
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
      console.log('Attempting login with:', loginData);

      this.userService.login(loginData).subscribe({
        next: (response) => {
          console.log('Login response:', response);
          setTimeout(() => {
            const role = this.authService.getUserRole();
            console.log('User role after login:', role);
            console.log('Response roleId:', response.roleId);
            
            if (role === 'Admin') {
              console.log('User is admin, navigating to admin panel');
              localStorage.setItem('currentUser', JSON.stringify(response));
              localStorage.setItem('isAdmin', 'true');
              alert('Successfully logged in as Admin');
              this.router.navigate(['/admin']);
            } else {
              console.log('User is regular user, navigating to home');
              localStorage.setItem('currentUser', JSON.stringify(response));
              localStorage.setItem('currentUserId', response.id.toString());
              alert('Successfully logged in');
              this.router.navigate(['/home']);
            }
          }, 100);
        },
        error: (error) => {
          console.error('Login error:', error);
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
