import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { MasterComponent } from './master/master.component';
import { StudentsComponent } from './students/students.component';
import { ClassesComponent } from './classes/classes.component';


export const routes: Routes = [
    { path: '', component: LoginComponent }, 
    { path: 'master', component: MasterComponent }, 
  { path: 'students', component: StudentsComponent },
  { path: 'classes', component: ClassesComponent }, 
];
