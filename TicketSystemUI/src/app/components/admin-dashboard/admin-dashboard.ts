import { Component, OnInit } from '@angular/core';
import { TicketService } from '../../services/ticket'; // ודאי שהנתיב ל-Service מדויק
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.html'
})
export class AdminDashboardComponent implements OnInit {
  tickets: any[] = [];
  
  // משתנים לניהול המודל (החלון הקופץ)
  selectedTicket: any = null;
  solutionText: string = '';
  selectedStatus: number = 1;

  constructor(private ticketService: TicketService) {}

  ngOnInit() {
    this.loadTickets();
  }

  loadTickets() {
    this.ticketService.getAllTickets().subscribe({
      next: (data) => {
        this.tickets = data;
      },
      error: (err) => console.error('שגיאה בטעינת טיקטים:', err)
    });
  }

  // פונקציה עזר להצגת שם הסטטוס בטבלה
  getStatusName(status: number): string {
    const statuses = ['פתוח', 'בטיפול', 'סגור'];
    return statuses[status] || 'לא ידוע';
  }

  // פתיחת המודל והזנת הנתונים של הטיקט שנבחר
  openEditModal(ticket: any) {
    this.selectedTicket = ticket;
    this.solutionText = ticket.resolution || ''; 
    this.selectedStatus = ticket.status === 0 ? 1 : ticket.status;
    
    // הצגת המודל (Bootstrap Manual Trigger)
    const modalElement = document.getElementById('ticketModal');
    if (modalElement) {
      modalElement.classList.add('show');
      modalElement.style.display = 'block';
      document.body.classList.add('modal-open');
      
      // הוספת רקע כהה (Backdrop)
      const backdrop = document.createElement('div');
      backdrop.className = 'modal-backdrop fade show';
      backdrop.id = 'modalBackdrop';
      document.body.appendChild(backdrop);
    }
  }

  // סגירת המודל וניקוי שאריות
  closeModal() {
    this.selectedTicket = null;
    const modalElement = document.getElementById('ticketModal');
    if (modalElement) {
      modalElement.classList.remove('show');
      modalElement.style.display = 'none';
      document.body.classList.remove('modal-open');
      
      const backdrop = document.getElementById('modalBackdrop');
      if (backdrop) backdrop.remove();
    }
  }

  // שמירת הפתרון ושליחה ל-Backend
  saveSolution() {
    if (!this.selectedTicket) return;

    const updateData = {
      status: Number(this.selectedStatus),
      resolution: this.solutionText
    };

    this.ticketService.updateTicketStatus(this.selectedTicket.id, updateData).subscribe({
      next: () => {
        alert('הפתרון נשמר והודעה נשלחה ללקוח!');
        this.closeModal();
        this.loadTickets(); // רענון רשימת הטיקטים בטבלה
      },
      error: (err) => {
        console.error('Error updating ticket:', err);
        alert('שגיאה בעדכון הפתרון. בדקי את ה-Console');
      }
    });
  }
}