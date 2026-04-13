import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TicketService } from '../../services/ticket';

@Component({
  selector: 'app-create-ticket',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container mt-5" style="direction: rtl;">
      <div class="card shadow p-4">
        <h2 class="text-center mb-4">פתיחת קריאת שירות חדשה</h2>
        
        <form (ngSubmit)="onSubmit()">
          <div class="mb-3">
            <label class="form-label">שם מלא:</label>
            <input type="text" [(ngModel)]="ticket.fullName" name="fullName" class="form-control" required>
          </div>

          <div class="mb-3">
            <label class="form-label">אימייל:</label>
            <input type="email" [(ngModel)]="ticket.email" name="email" class="form-control" required>
          </div>

          <div class="mb-3">
            <label class="form-label">תיאור התקלה:</label>
            <textarea [(ngModel)]="ticket.description" name="description" class="form-control" rows="3" required></textarea>
          </div>

          <div class="mb-3">
            <label class="form-label">העלאת תמונה:</label>
            <input type="file" (change)="onFileSelected($event)" class="form-control" accept="image/*">
          </div>

          <button type="submit" [disabled]="isSubmitting" class="btn btn-primary w-100">
            {{ isSubmitting ? 'שולח...' : 'פתח טיקט' }}
          </button>
        </form>
      </div>
    </div>
    <footer class="mt-5 py-3 text-center border-top">
  <small class="text-muted">צוות תמיכה? <a routerLink="/admin">התחבר כאן</a></small>
</footer>
  `,
  styles: []
})
export class CreateTicketComponent {
  // אתחול האובייקט - קריטי כדי שהשדות יופיעו!
  ticket = {
    fullName: '',
    email: '',
    description: ''
  };

  selectedFile: File | null = null;
  isSubmitting = false;

  constructor(private ticketService: TicketService) { }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onSubmit() {
    this.isSubmitting = true;
    const formData = new FormData();

    // שליחת נתונים עם מפתחות תואמים ל-C# (אותיות גדולות)
    formData.append('FullName', this.ticket.fullName);
    formData.append('Email', this.ticket.email);
    formData.append('Description', this.ticket.description);

    if (this.selectedFile) {
      formData.append('image', this.selectedFile);
    }

    this.ticketService.createTicket(formData).subscribe({
      next: (res) => {
        alert('הטיקט נפתח בהצלחה!');
        this.isSubmitting = false;
        // איפוס הטופס
        this.ticket = { fullName: '', email: '', description: '' };
      },
      error: (err) => {
        console.error('SERVER ERROR:', err.error);
        this.isSubmitting = false;
      }
    });
  }
}