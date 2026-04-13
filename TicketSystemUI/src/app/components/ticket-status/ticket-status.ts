import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TicketService } from '../../services/ticket';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ticket-status',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container mt-5" *ngIf="ticket" style="direction: rtl;">
  <div class="card shadow border-0">
    <div class="card-header bg-primary text-white p-4">
      <h3 class="mb-0">סטטוס פנייה: {{ ticket.id }}</h3>
    </div>
    
    <div class="card-body p-4 text-end">
      <p><strong>שם הלקוח:</strong> {{ ticket.fullName }}</p>
      <p><strong>תיאור המקרה:</strong> {{ ticket.description }}</p>
      
      <div class="alert alert-info mt-4 border-0 shadow-sm">
        <h5 class="d-flex align-items-center">
           סיכום AI של התקלה: <span class="ms-2">🤖</span>
        </h5>
        <p class="mb-0">{{ ticket.aiSummary || 'מעבד נתונים...' }}</p>
      </div>

<div *ngIf="ticket">
  <p class="text-muted small">DEBUG: שם הקובץ הוא: {{ ticket.imagePath }}</p>

  <div class="mt-4" *ngIf="ticket.imagePath">
    <img [src]="'https://localhost:7199/uploads/' + ticket.imagePath" 
         class="img-fluid rounded shadow" 
         style="max-width: 100%; height: auto;"
         (error)="handleImageError()">
  </div>
</div>

      <div class="mt-4 p-3 border rounded bg-light" *ngIf="ticket.resolution">
        <h5 class="text-primary">✅ פתרון הצוות:</h5>
        <p class="mb-0 whitespace-pre-wrap">{{ ticket.resolution }}</p>
      </div>

      <div class="mt-4 d-flex justify-content-between align-items-center">
        <span class="badge p-2 px-3" 
              [ngClass]="{'bg-success': ticket.status === 2, 'bg-warning text-dark': ticket.status === 1, 'bg-secondary': ticket.status === 0}">
          {{ getStatusName(ticket.status) }}
        </span>
      </div>
    </div>
  </div>
</div>
  `
})
export class TicketStatusComponent implements OnInit {
  ticket: any;

  constructor(
    private route: ActivatedRoute, // מאפשר לקרוא את ה-ID מה-URL
    private ticketService: TicketService
  ) { }

  ngOnInit() {
    // 1. חילוץ ה-ID מהכתובת
    const id = this.route.snapshot.paramMap.get('id');

    // 2. קריאה ל-Service להבאת הנתונים
    if (id) {
      this.ticketService.getTicketById(id).subscribe({
        next: (data) => this.ticket = data,
        error: (err) => console.error('לא נמצא טיקט', err)
      });
    }
  }

  getStatusName(status: number) {
    const statuses = ['פתוח', 'בטיפול', 'סגור'];
    return statuses[status];
  }

  openImageInNewTab(url: string) {
    window.open(url, '_blank');
  }

  // בתוך ה-class של TicketStatusComponent
  handleImageError() {
    console.error("אנגולר לא הצליח למצוא את התמונה בנתיב שסופק");
  }
}