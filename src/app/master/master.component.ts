import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-master',
  imports: [],
  templateUrl: './master.component.html',
  styleUrl: './master.component.css'
})
export class MasterComponent {
  constructor(private router: Router) {}

  navigateStudents() {
    this.router.navigate(['/students']);
  }

  navigateClasses() {
    this.router.navigate(['/classes']);
  }

  logout() {
    localStorage.removeItem('authToken');
    this.router.navigate(['/']);
  }
}
