import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  // הכתובת של ה-Backend שלך (ודאי שהפורט נכון לפי ה-Visual Studio)
  private apiUrl = 'https://localhost:7199/api/tickets';

  constructor(private http: HttpClient) { }

  // פונקציה לשליחת טיקט חדש
  createTicket(formData: FormData): Observable<any> {
    return this.http.post(this.apiUrl, formData);
  }

  // פונקציה לקבלת טיקט לפי ID (בשביל דף הסטטוס)
  getTicketById(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  // הוספה בתוך ה-Class של TicketService

  // הבאת כל הטיקטים
  getAllTickets(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // עדכון סטטוס (קריאת PUT)
  updateTicketStatus(id: string, updateDto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, updateDto);
  }
}