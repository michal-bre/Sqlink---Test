import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router'; // הייבוא הזה קריטיimport { CreateTicketComponent } from './components/create-ticket/create-ticket'; // הייבוא של הקומפוננטה שלך
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard';
import { CreateTicketComponent } from './components/create-ticket/create-ticket';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
     CreateTicketComponent, 
     AdminDashboardComponent,
    RouterOutlet, RouterLink,
     RouterLinkActive, CommonModule], // הוסיפי את CreateTicketComponent כאן!
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent {
  title = 'TicketSystemUI';
}