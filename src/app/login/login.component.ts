import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  // create login form
  loginForm = new FormGroup({
      username: new FormControl<string>(''),
      password: new FormControl<string>('')
    })

  constructor(private router: Router, private http: HttpClient) { }
  
  onLoginFormSubmit()
  {
    const username = this.loginForm.value.username;
  const password = this.loginForm.value.password;
  // hardcoded check
  if (username === 'admin' && password === 'admin123') {
    this.router.navigate(['/master']);
  } else {
    alert('Invalid login');
  }
  }

  login() {
    const body = {
      username: this.loginForm.value.username,
      password: this.loginForm.value.password
    };
  
    this.http.post<any>('https://localhost:7211/Auth/GetAccessToken', body)
      .subscribe({
        next: (res) => {
          const token = res.token;
  
          localStorage.setItem('authToken', token);
  
          this.router.navigate(['/master']);
        },
        error: () => alert('Invalid login')
      });
  }
  
}
