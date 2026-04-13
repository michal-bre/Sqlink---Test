import { Routes } from '@angular/router';
import { CreateTicketComponent } from './components/create-ticket/create-ticket';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard';
import { TicketStatusComponent } from './components/ticket-status/ticket-status';

export const routes: Routes = [
  { path: '', component: CreateTicketComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'ticket-status/:id', component: TicketStatusComponent }
];