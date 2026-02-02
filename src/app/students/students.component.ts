import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Student } from '../../models/student.model';

@Component({
  selector: 'app-students',
  imports: [CommonModule, FormsModule],
  templateUrl: './students.component.html',
  styleUrl: './students.component.css'
})
export class StudentsComponent implements OnInit {
  apiUrl = 'https://localhost:7211/Students/GetAll';
  apiAdd = 'https://localhost:7211/Students/AddStudent';

  students: Student[] = [];
  filteredStudents: Student[] = [];

  //constructor(private router: Router) {}
  constructor(private router: Router, private http: HttpClient) {}

  newStudent: Student = { studentId: 0, firstName: '', lastName: '', classIds: '' };
  editMode = false;
  // search
  searchText = '';
  // pagination
  pageSize = 3;
  currentPage = 1;
  // sorting
  sortColumn: keyof Student = 'firstName';
  sortAsc = true;

  ngOnInit() {
    this.loadStudents();
  }
  loadStudents() {
    this.http.get<Student[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.students = data;
        this.applyAll();
      },
      error: (err) => {
        console.error('Error loading students', err);
      }
    });
  }
  applyAll() {
    this.search();
    this.sort();
  }
  search() {
    const text = this.searchText.toLowerCase();
    this.filteredStudents = this.students.filter(s =>
      s.firstName.toLowerCase().includes(text) ||
      s.firstName.toLowerCase().includes(text)
    );
    this.currentPage = 1;
  }
  sortBy(column: keyof Student) {
    if (this.sortColumn === column) {
      this.sortAsc = !this.sortAsc;
    } else {
      this.sortColumn = column;
      this.sortAsc = true;
    }
    this.sort();
  }
  sort() {
    this.filteredStudents.sort((a, b) => {
      const v1 = a[this.sortColumn];
      const v2 = b[this.sortColumn];
      return this.sortAsc
        ? String(v1).localeCompare(String(v2))
        : String(v2).localeCompare(String(v1));
    });
  }
  // PAGINATION
  get pagedStudents() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredStudents.slice(start, start + this.pageSize);
  }
  get totalPages() {
    return Math.ceil(this.filteredStudents.length / this.pageSize);
  }
  nextPage() {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }
  prevPage() {
    if (this.currentPage > 1) this.currentPage--;
  }

  addStudent() {
    this.http.post<Student>(`${this.apiAdd}/AddStudent`, this.newStudent)
      .subscribe({
        next: (res) => {
          alert('Student added successfully');
          this.loadStudents(); 
          
          this.newStudent = { studentId: 0, firstName: '', lastName: '', classIds: '' };
        },
        error: (err) => console.error('Add failed', err)
      });
  }

  goBack() {
    this.router.navigate(['/master']);
  }
}
